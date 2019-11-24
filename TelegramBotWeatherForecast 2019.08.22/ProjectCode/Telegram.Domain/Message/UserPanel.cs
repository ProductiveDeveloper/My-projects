using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Sender;
using Telegram.Sender.Core;
using Telegram.Storage.Core;
using Telegram.Domain.UserItem.Core;
using Telegram.Bot.Storage.InMemory;
using Telegram.Domain.MessageBar;
using Parse.WeatherForecast;
using System.Threading.Tasks;
using Telegram.Domain.Reminders;

namespace Telegram.Domain.Message
{
    class UserPanel : UserPanelBar
    {
        private readonly DomainUserItem _userItem;
        private readonly ISender _sender;
        private readonly Reminder _reminder;

        public UserPanel(DomainUserItem userItem, ISender sender , Reminder reminder)
        {
            _userItem = userItem;
            _sender = sender;
            _reminder = reminder;
        }

        public DomainUserItem UserPanelMessage(InMemoryStorage storage)
        {
            switch (_userItem.DomainStatus)
            {
                case DomainStatusEnum.User_Greeting:
                    _sender.SendMessage(_userItem.ChatId, $"Привет , {_userItem.FirstName} .", MainWindow);
                    _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                    break;
                case DomainStatusEnum.User_MainWindow:
                    if (_userItem.Message.Equals("Погода \U00002600") || _userItem.Message.ToLower().Equals("погода"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Введите город :", null);
                        _userItem.DomainStatus = DomainStatusEnum.User_Weather_InputCity;
                    }
                    else if (_userItem.Message.Equals("О боте \U0001F916") || _userItem.Message.ToLower().Equals("о боте"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Данный бот является примером работы <ProductiveDeveloper> разработчика.\n" +
                            $"Технологии :\n" +
                            $"C#,\n" +
                            $"Парсер данных. Сайт: https://pogoda.mail.ru,\n"+
                            "SqlServer,\n" +
                            "Telegram Api,\n" +
                            "Yandex translate Api.", MainWindow);
                    }
                    else if (_userItem.Message.Equals("Напоминание") || _userItem.Message.Equals("Напоминание \U000023F0"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.User_Reminder_InputDays;
                        _sender.SendMessage(_userItem.ChatId, "Выберите количество дней через которое будет отправляться прогноз погоды :", ReminderInputDays);
                    }
                    else if (_userItem.Message.Equals("/newsletterreset"))
                    {
                        _reminder.ReminderRemove(_userItem.ChatId);
                        _sender.SendMessage(_userItem.ChatId ,"Рассылка погоды отключена. \U00002705" , MainWindow);
                    }
                    else
                    {
                        _sender.SendMessage(_userItem.ChatId, "Я не знаю такой команды . \U0000274C", MainWindow);
                    }
                    break;
                case DomainStatusEnum.User_Reminder_InputDays:
                    if (Int32.TryParse(_userItem.Message , out int _countDaysReminder) && _countDaysReminder != 0)
                    {
                        _userItem.User_Reminder_CountDays = _countDaysReminder;
                        _sender.SendMessage(_userItem.ChatId, "Введите город для прогноза погоды :", null);
                        _userItem.DomainStatus = DomainStatusEnum.User_Reminder_InputCity;
                    }
                    else if (_userItem.Message.Equals("Отмена"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                        _sender.SendMessage(_userItem.ChatId, "Отмена. \U0001F519", MainWindow);
                    }
                    else
                    {
                        _sender.SendMessage(_userItem.ChatId, "Ввеедите число , а именно количество дней.", null);
                    }
                    break;
                case DomainStatusEnum.User_Reminder_InputCity:
                    Parse.WeatherForecast.Parsing parsingReminder = new Parse.WeatherForecast.Parsing();
                    WeatherForecast weatherForecastReminder = parsingReminder.ParseWeatherForecast(_userItem.Message).Result;
                    if (weatherForecastReminder.Days.Count.Equals(14))
                    {
                        _userItem.User_Reminder_City = _userItem.Message;
                        _userItem.ReminderWeatherForecast = weatherForecastReminder;
                        _sender.SendMessage(_userItem.ChatId, $"Рассылка погоды \U00002705\nКаждые {_userItem.User_Reminder_CountDays} (дня) \U000023F0\nГород : {_userItem.User_Reminder_City}", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                        ReminderWeatherForecast reminder = new ReminderWeatherForecast()
                        {
                            ChatId = _userItem.ChatId,
                            City = _userItem.User_Reminder_City,
                            DateOfRegistrationReminder = DateTimeOffset.Now,
                            IntervalDays = _userItem.User_Reminder_CountDays
                        };
                        _reminder.ReminderAdd(reminder);
                    }
                    else
                    {
                          _sender.SendMessage(_userItem.ChatId, "Хмм... Что-то пошло не так.\nВозможно вы ввели неправильно город. \U0000274C", MainWindow);
                          _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                    }
                        break;
                case DomainStatusEnum.User_Weather_InputCity:
                    Parse.WeatherForecast.Parsing parsing = new Parse.WeatherForecast.Parsing();
                    WeatherForecast weatherForecast = parsing.ParseWeatherForecast(_userItem.Message).Result;
                    if (!weatherForecast.Days.Count.Equals(14))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Хмм... Что-то пошло не так.\nВозможно вы ввели неправильно город. \U0000274C", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                    }
                    else if (weatherForecast.Days.Count.Equals(14))
                    {
                        _userItem.weatherForecast = weatherForecast;
                        _sender.SendMessage(_userItem.ChatId, "Введите количество дней для прогноза погоды : \n", WeatherForecastInputDays);
                        _userItem.DomainStatus = DomainStatusEnum.User_Weather_IputCountDays;
                    }
                    break;
                case DomainStatusEnum.User_Weather_IputCountDays:
                    if (int.TryParse(_userItem.Message, out int _countDays) &&
                        _countDays <= _userItem.weatherForecast.Days.Count &&
                        _countDays != 0)
                    {
                        string result = "\U0001F316 Утро \U0001F31D День \U0001F317 Вечер \U0001F31A Ночь\n" +
                            "\U0001F525 Температура\n" +
                            "\U0001F555 Давление\n" +
                            "\U0001F4A7 Влажность\n" +
                            "\U0001F4C8 Вероятность дождя\n\n\n";
                        for (int i = 0; i < _countDays; i++)
                        {
                            string a;
                            a = $"{_userItem.weatherForecast.Days[i].Day.DateTime}\n\n" +
                            $"          \U0001F316            \U0001F31D         \U0001F317            \U0001F31A\n\n" +
                            $"\U0001F525     {_userItem.weatherForecast.Days[i].Morning.Temperature}\t     {_userItem.weatherForecast.Days[i].Day.Temperature}\t       {_userItem.weatherForecast.Days[i].Evening.Temperature}\t\t    {_userItem.weatherForecast.Days[i].Night.Temperature}\n\n" +
                            $"\U0001F555     {_userItem.weatherForecast.Days[i].Morning.Pressure}       {_userItem.weatherForecast.Days[i].Day.Pressure}          {_userItem.weatherForecast.Days[i].Evening.Pressure}       {_userItem.weatherForecast.Days[i].Night.Pressure} (мм)\n\n" +
                            $"\U0001F4A7     {_userItem.weatherForecast.Days[i].Morning.Humidity}\t      {_userItem.weatherForecast.Days[i].Day.Humidity}\t       {_userItem.weatherForecast.Days[i].Evening.Humidity}\t     {_userItem.weatherForecast.Days[i].Night.Humidity}\n\n" +
                            $"\U0001F4C8      {_userItem.weatherForecast.Days[i].Morning.ChanceOfPrecipitation}\t      {_userItem.weatherForecast.Days[i].Day.ChanceOfPrecipitation}\t        {_userItem.weatherForecast.Days[i].Evening.ChanceOfPrecipitation}\t      {_userItem.weatherForecast.Days[i].Night.ChanceOfPrecipitation}\n\n\n";
                            result = result + a;
                        }
                        _sender.SendMessage(_userItem.ChatId, result, MainWindow);

                        _userItem.CountDays = 0;
                        _userItem.weatherForecast = null;
                        _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                    }
                    else if (_userItem.Message.Equals("Отмена"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.User_MainWindow;
                        _sender.SendMessage(_userItem.ChatId, "Отмена. \U0001F519", MainWindow);
                    }
                    else
                    {
                        _sender.SendMessage(_userItem.ChatId, "Максимальное количество дней для прогноза - 14.", null);
                    }
                    break;
            }

            return _userItem;
        }
    }
}

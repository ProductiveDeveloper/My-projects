using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Sender.Core;

namespace Telegram.Domain.Reminders
{
    public class Reminder
    {
        private List<ReminderWeatherForecast> RemindersList { get; set; } = new List<ReminderWeatherForecast>();

        private ISender sender { get; set; }

        public Reminder(ISender sender)
        {
            this.sender = sender;
        }

        public void ReminderAdd(ReminderWeatherForecast reminderWeatherForecast)
        {
            RemindersList.Add(reminderWeatherForecast);
        }

        public void ReminderRemove(long ChatId)
        {
            for (int i = 0; i < RemindersList.Count; i++)
            {
                if (RemindersList[i].ChatId == ChatId)
                    RemindersList.RemoveAt(i);
            }
        }

        public void RemindersUpdate()
        {
            DateTimeOffset DateNow = DateTimeOffset.Now;
            Parse.WeatherForecast.Parsing parsing = new Parse.WeatherForecast.Parsing();
            foreach (var item in RemindersList)
            {
                if (item.DateOfRegistrationReminder.Day + item.IntervalDays == DateNow.Day)
                {
                    string result = $"{item.City}\nПогода на сегодня | завтра \U00002709\nЧтобы отключить рассылку погоды /newsletterreset \n\n" + "\U0001F316 Утро \U0001F31D День \U0001F317 Вечер \U0001F31A Ночь\n" +
                            "\U0001F525 Температура\n" +
                            "\U0001F555 Давление\n" +
                            "\U0001F4A7 Влажность\n" +
                            "\U0001F4C8 Вероятность дождя\n\n\n";
                    Parse.WeatherForecast.WeatherForecast weatherForecast = parsing.ParseWeatherForecast(item.City).Result;
                    for(int i = 0; i < 2; i++)
                    {
                        result += $"{weatherForecast.Days[i].Day.DateTime}\n\n"+"          \U0001F316            \U0001F31D         \U0001F317            \U0001F31A\n\n" +
                            $"\U0001F525     {weatherForecast.Days[i].Morning.Temperature}\t     {weatherForecast.Days[i].Day.Temperature}\t       {weatherForecast.Days[i].Evening.Temperature}\t\t    {weatherForecast.Days[i].Night.Temperature}\n\n" +
                            $"\U0001F555     {weatherForecast.Days[i].Morning.Pressure}       {weatherForecast.Days[i].Day.Pressure}          {weatherForecast.Days[i].Evening.Pressure}       {weatherForecast.Days[i].Night.Pressure} (мм)\n\n" +
                            $"\U0001F4A7     {weatherForecast.Days[i].Morning.Humidity}\t      {weatherForecast.Days[i].Day.Humidity}\t       {weatherForecast.Days[i].Evening.Humidity}\t     {weatherForecast.Days[i].Night.Humidity}\n\n" +
                            $"\U0001F4C8      {weatherForecast.Days[i].Morning.ChanceOfPrecipitation}\t      {weatherForecast.Days[i].Day.ChanceOfPrecipitation}\t        {weatherForecast.Days[i].Evening.ChanceOfPrecipitation}\t      {weatherForecast.Days[i].Night.ChanceOfPrecipitation}\n\n";
                    }
                    item.DateOfRegistrationReminder = DateNow;
                    sender.SendMessage(item.ChatId, result , null);
                }
            }
        }
    }
}

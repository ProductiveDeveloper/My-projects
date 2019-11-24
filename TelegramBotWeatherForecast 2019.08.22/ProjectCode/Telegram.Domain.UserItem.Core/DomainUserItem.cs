using Parse.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Storage.Core;
using Telegram.Bot.Types;

namespace Telegram.Domain.UserItem.Core
{
    public class DomainUserItem : IDomainUserItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long ChatId { get; set; }

        public string FirstName { get; set; }

        public StatusEnum Status { get; set; }

        public DomainStatusEnum DomainStatus { get; set; }

        public string Message { get; set; }

        public DateTimeOffset DateOfRegistration { get; set; }

        public WeatherForecast weatherForecast { get; set; }

        public int CountDays { get; set; }

        public string Admin_MessageSend { get; set; }

        public string Admin_InputName { get; set; }

        public string PhoneNumber { get; set; }

        public string Admin_InputNumber { get; set; }

        public int User_Reminder_CountDays { get; set; }

        public string User_Reminder_City { get; set; }

        public WeatherForecast ReminderWeatherForecast { get; set; }

        public Telegram.Bot.Types.PhotoSize[] Admin_Photo { get; set; }

        public Telegram.Bot.Types.PhotoSize[] Photo { get; set; }

        public string Caption { get; set; }

        public string Admin_Caption { get; set; }
    }
}

using Parse.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Storage.Core;

namespace Telegram.Domain.UserItem.Core
{
    public interface IDomainUserItem : IUserItem
    {
        DomainStatusEnum DomainStatus { get; set; }
        
        //User
        WeatherForecast weatherForecast { get; set; }

        WeatherForecast ReminderWeatherForecast { get; set; }

        int CountDays { get; set; }

        //Admin
        string Admin_MessageSend { get; set; }

        string Admin_InputName { get; set; }

        string Admin_InputNumber { get; set; }

        string PhoneNumber { get; set; }

        int User_Reminder_CountDays { get; set; }

        string User_Reminder_City { get; set; }

        Telegram.Bot.Types.PhotoSize[] Admin_Photo { get; set; }

        Telegram.Bot.Types.PhotoSize[] Photo { get; set; }

        string Caption { get; set; }

        string Admin_Caption { get; set; }
    }
}

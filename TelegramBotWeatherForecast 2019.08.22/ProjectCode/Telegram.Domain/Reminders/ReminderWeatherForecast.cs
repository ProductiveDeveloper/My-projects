using Parse.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Domain
{
    public class ReminderWeatherForecast
    {
        public DateTimeOffset DateOfRegistrationReminder { get; set; }

        public int IntervalDays { get; set; }

        public string City { get; set; }

        public long ChatId { get; set; }
    }
}

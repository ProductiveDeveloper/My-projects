using System;
using System.Collections.Generic;
using System.Text;

namespace Parse.WeatherForecast
{
    public class TimesOfDay
    {
        public Weather Morning { get; set; }

        public Weather Day { get; set; }

        public Weather Evening { get; set; }

        public Weather Night { get; set; }
    }
}

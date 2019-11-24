using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using System.Linq;
using System.Threading;
using Translator;

namespace Parse.WeatherForecast
{
    public class Parsing
    {
        public async Task<WeatherForecast> ParseWeatherForecast(string City)
        {
            YandexTranslator yandexTranslator = new YandexTranslator();
            string translateCity = yandexTranslator.Translate(City, "ru-en");

            if (City.Equals("Москва") || City.Equals("Moscow"))
                translateCity = "Moskva";

            var WeatherForecastresult = new WeatherForecast();
            var config = Configuration.Default.WithDefaultLoader();
            var address = @"https://pogoda.mail.ru/prognoz/" + $"{translateCity}" + "/14dney/";

            var document = await BrowsingContext.New(config).OpenAsync(address);

            //DayDate
            var _dayDate = document.QuerySelectorAll("div").Where(item => item.ClassName == "day__date").ToList();

            //Pressure
            var _pressure = document.QuerySelectorAll("div").Where(item => item.ClassName == "day__additional").ToList();
            _pressure = SpaceRemove(_pressure);
            //Temperature
            var _temperature = document.QuerySelectorAll("div").Where(item => item.ClassName == "day__temperature ").ToList();
            //DateTime
            var _dateTime = document.QuerySelectorAll("div").Where(item => item.ClassName == "heading heading_minor heading_line" || item.ClassName == "heading heading_minor heading_line text-red").ToList();
            _dateTime = SpaceRemoveDateTime(_dateTime);

            List<Weather> dayDateWeatherForecastsArr = ParsePressure(_pressure);

            WeatherForecastresult = ParseDayDateWeatherForecast(dayDateWeatherForecastsArr, _dayDate, _temperature, _dateTime);


            return WeatherForecastresult;
        }


        List<AngleSharp.Dom.IElement> SpaceRemove(List<AngleSharp.Dom.IElement> arr)
        {
            var result = new List<AngleSharp.Dom.IElement>(arr);
            for (int i = 0; i < result.Count; i++)
            {
                List<char> charArr = new List<char>(result[i].TextContent.ToArray<Char>());
                for (int p = 0; p < 10; p++)
                {
                    for (int c = 0; c < charArr.Count; c++)
                    {
                        if (charArr[c] == '\n' || charArr[c] == '\t' || charArr[c] == 'м' || charArr[c] == ' ')
                        {
                            charArr.RemoveAt(c);
                        }
                    }
                }
                result[i].TextContent = string.Join("", charArr.ToArray());
            }
            return result;
        }

        List<AngleSharp.Dom.IElement> SpaceRemoveDateTime(List<AngleSharp.Dom.IElement> arr)
        {
            var result = new List<AngleSharp.Dom.IElement>(arr);
            for (int i = 0; i < result.Count; i++)
            {
                List<char> charArr = new List<char>(result[i].TextContent.ToArray<Char>());
                for (int p = 0; p < 10; p++)
                {
                    for (int c = 0; c < charArr.Count; c++)
                    {
                        if (charArr[c] == '\n' || charArr[c] == '\t' || charArr[c] == 'м')
                        {
                            charArr.RemoveAt(c);
                        }
                    }
                }
                result[i].TextContent = string.Join("", charArr.ToArray());
            }
            return result;
        }

        List<Weather> ParsePressure(List<AngleSharp.Dom.IElement> pressure)
        {
            var result = new List<Weather>();

            int arr = 0;
            int i = 0;
            try
            {
                while (i != pressure.Count)
                {
                    result.Add(new Weather());
                    result[arr].Pressure = pressure[i].TextContent;
                    i++;
                    result[arr].Humidity = pressure[i].TextContent;
                    i++;
                    result[arr].Wind = pressure[i].TextContent;
                    i++;
                    result[arr].ChanceOfPrecipitation = pressure[i].TextContent;
                    i++;
                    arr++;
                }
            }
            catch
            {
            }

            return result;
        }

        WeatherForecast ParseDayDateWeatherForecast(List<Weather> dayDateWeatherForecastsArr,
           List<AngleSharp.Dom.IElement> dayDate,
           List<AngleSharp.Dom.IElement> Temperature,
           List<AngleSharp.Dom.IElement> DateTime)
        {
            var result = new WeatherForecast();
            TimesOfDay dateTime = new TimesOfDay();

            int i = 0;
            int d = dayDate.Count;
            int t = Temperature.Count;
            int DayTimeFull = 1;
            int dateTimeIndex = 0;
            try
            {
                while (d != 0)
                {
                    var dayDateWeatherForecasts = new Weather();
                    dayDateWeatherForecasts.Pressure = dayDateWeatherForecastsArr[i].Pressure;
                    dayDateWeatherForecasts.Humidity = dayDateWeatherForecastsArr[i].Humidity;
                    dayDateWeatherForecasts.Wind = dayDateWeatherForecastsArr[i].Wind;
                    dayDateWeatherForecasts.ChanceOfPrecipitation = dayDateWeatherForecastsArr[i].ChanceOfPrecipitation;
                    dayDateWeatherForecasts.DayDate = dayDate[i].TextContent;
                    dayDateWeatherForecasts.Temperature = Temperature[i].TextContent;
                    dayDateWeatherForecasts.DateTime = DateTime[dateTimeIndex].TextContent;
                    i++;
                    d--;
                    if (DayTimeFull == 1)
                    {
                        dateTime.Night = dayDateWeatherForecasts;
                    }
                    if (DayTimeFull == 2)
                    {
                        dateTime.Morning = dayDateWeatherForecasts;
                    }
                    if (DayTimeFull == 3)
                    {
                        dateTime.Day = dayDateWeatherForecasts;
                    }
                    if (DayTimeFull == 4)
                    {
                        dateTime.Evening = dayDateWeatherForecasts;
                        DayTimeFull = 1;
                        dateTimeIndex++;
                        result.Days.Add(dateTime);
                        dateTime = new TimesOfDay();
                        continue;
                    }
                    DayTimeFull++;
                }
            }
            catch
            {

            }

            return result;
        }
    }
}
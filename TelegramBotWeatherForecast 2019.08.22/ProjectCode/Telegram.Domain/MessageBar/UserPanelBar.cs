using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Domain.MessageBar
{
    public class UserPanelBar
    {
        public static ReplyKeyboardMarkup MainWindow = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Погода \U00002600"),
                    new KeyboardButton("Напоминание \U000023F0")
                },
                new[]
                {
                    new KeyboardButton("О боте \U0001F916"),
                },
            },
            OneTimeKeyboard = true,
            ResizeKeyboard = true,
        };

        public static ReplyKeyboardMarkup ReminderInputDays = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("1"),
                    new KeyboardButton("2"),
                    new KeyboardButton("3"),
                    new KeyboardButton("4"),
                    new KeyboardButton("5"),
                    new KeyboardButton("6"),
                    new KeyboardButton("7")
                },
                new[]
                {
                    new KeyboardButton("Отмена"),
                },
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
        };

        public static ReplyKeyboardMarkup WeatherForecastInputDays = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("1"),
                    new KeyboardButton("2"),
                    new KeyboardButton("3"),
                    new KeyboardButton("4"),
                    new KeyboardButton("5"),
                    new KeyboardButton("6"),
                    new KeyboardButton("7")
                },
                new[]
                {
                    new KeyboardButton("8"),
                    new KeyboardButton("9"),
                    new KeyboardButton("10"),
                    new KeyboardButton("11"),
                    new KeyboardButton("12"),
                    new KeyboardButton("13"),
                    new KeyboardButton("14")
                }
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true,
        };
    }
}

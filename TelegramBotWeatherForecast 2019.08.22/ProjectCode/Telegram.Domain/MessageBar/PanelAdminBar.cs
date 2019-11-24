using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Domain.MessageBar
{
    public class PanelAdminBar
    {
        public static ReplyKeyboardMarkup MainWindow = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Отправить сообщение \U00002709"),
                    new KeyboardButton("Статус пользователя \U00002755")
                },
            },
            ResizeKeyboard = true
        };

        static public InlineKeyboardMarkup MessageSend = new InlineKeyboardMarkup(
        new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
        {
              new []
              {
                   InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Всем" , "All"),
                   InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Пользователю" , "User")
              },
        });

        public static ReplyKeyboardMarkup MessageSendAll = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Изменить сообщение"),
                    new KeyboardButton("Отправить себе")
                },
                new[]
                {
                    new KeyboardButton("Отправить")
                },
                new[]
                {
                    new KeyboardButton("Отмена")
                }
            },
            ResizeKeyboard = true
            
        };

        static public InlineKeyboardMarkup RepairStatusBar = new InlineKeyboardMarkup(
            new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
            {
                new []
                {
                   InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Mut" , "Mut"),
                   InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("User" , "User")
                },
                new[]
                {
                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Отмена" , "Back")
                }                
            });
           

    }
}

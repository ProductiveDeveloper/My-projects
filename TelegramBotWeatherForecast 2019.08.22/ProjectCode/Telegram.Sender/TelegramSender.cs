using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Sender.Core;

namespace Telegram.Sender
{
    public class TelegramSender : ISender
    {
        private TelegramBotClient botClient;

        public TelegramSender(TelegramBotClient telegramBotClient)
        {
            botClient = telegramBotClient;
        }

        public async void SendMessage(long ChatId, string message, IReplyMarkup replyKeyboard = null)
        {
            await botClient.SendTextMessageAsync(
               ChatId,
               message,
               Telegram.Bot.Types.Enums.ParseMode.Default,
               false,
               false,
               0,
               replyKeyboard);
        }
        public async void SendPhoto(long ChatId, Telegram.Bot.Types.PhotoSize[] photo, string caption  , IReplyMarkup Markup)
        {
             await botClient.SendPhotoAsync(ChatId, photo[0].FileId , caption: caption , replyMarkup: Markup);
        }
    }
}

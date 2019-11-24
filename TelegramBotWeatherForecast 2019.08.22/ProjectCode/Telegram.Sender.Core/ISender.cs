using System;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Sender.Core
{
    public interface ISender
    {
        void SendMessage(long chatId, string message, IReplyMarkup replyKeyboard);

        void SendPhoto(long ChatId, Telegram.Bot.Types.PhotoSize[] photo , string caption , IReplyMarkup Markup);
    }
}

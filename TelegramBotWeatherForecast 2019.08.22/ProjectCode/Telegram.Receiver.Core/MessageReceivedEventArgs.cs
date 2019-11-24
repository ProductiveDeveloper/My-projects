using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Receiver.Core
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }

        public long ChatId { get; set; }

        public string FirstName { get; set; }

        public Telegram.Bot.Types.PhotoSize[] Photo { get; set; }

        public string Caption { get; set; }

        public MessageReceivedEventArgs(long ChatId, string message , string firstname , Telegram.Bot.Types.PhotoSize[] photo , string caption)
        {
            this.ChatId = ChatId;
            Message = message;
            FirstName = firstname;
            Photo = photo;
            Caption = caption;
        }
    }
}

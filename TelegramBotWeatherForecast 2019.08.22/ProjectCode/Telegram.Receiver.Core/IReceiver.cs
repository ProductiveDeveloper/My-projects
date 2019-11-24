using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram.Receiver.Core
{
    public interface IReceiver
    {
        void Run();

        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        TelegramBotClient botClient { get; set; }
    }
}

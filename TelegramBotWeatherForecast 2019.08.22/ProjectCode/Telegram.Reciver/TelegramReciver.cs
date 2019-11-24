using System;
using System.Net;
using Telegram.Receiver.Core;
using Telegram.Bot;
using MihaZupan;

namespace Telegram.Receiver
{
    public class TelegramReceiver : IReceiver
    {
        public TelegramBotClient botClient { get; set; }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public TelegramReceiver(string token)
        {
            HttpToSocks5Proxy _proxy = new HttpToSocks5Proxy("181.215.129.234", 65234, "Productivedevelopercode", "N8f4BeX");

            botClient = new TelegramBotClient(token, _proxy);
        }

        public string GetHelloFromBot()
        {
            global::Telegram.Bot.Types.User user =
                botClient.GetMeAsync().Result;

            return $"Hello from user {user.Id}." +
                $"My name is {user.FirstName} {user.LastName}";
        }

        public void Run()
        {
            botClient.OnMessage += BotClient_OnMessage;
            botClient.StartReceiving();
        }

        private void BotClient_OnMessage(
            object sender,
            global::Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == global::Telegram.Bot.Types.Enums.MessageType.Text || e.Message.Type == Bot.Types.Enums.MessageType.Photo)
            {
                MessageReceived(
                   this,
                   new MessageReceivedEventArgs(
                       e.Message.Chat.Id,
                       e.Message.Text,
                       e.Message.Chat.FirstName,
                       e.Message.Photo,
                       e.Message.Caption));
            }
        }

        protected virtual void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(sender, e);
        }
    }
}

using System;
using System.Collections.Generic;
using Telegram.Bot.Storage.Sql;
using Telegram.Storage.Core;
using System.IO;
using Telegram.Bot.Storage.File;
using System.Threading;
using Telegram.Receiver;
using Telegram.Receiver.Core;
using Telegram.Sender;
using Telegram.Domain;
using Telegram.Bot.Storage.InMemory;
using Telegram.Domain.Reminders;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {            
            FileStorage fileStorage = new FileStorage(AppDomain.CurrentDomain.BaseDirectory);          

            Telegram.Bot.Storage.InMemory.InMemoryStorage inMemoryStorage = new Telegram.Bot.Storage.InMemory.InMemoryStorage();

            Telegram.Receiver.TelegramReceiver reciver = new Telegram.Receiver.TelegramReceiver("658409803:AAGD9lg4y7_j976T3o--Gm1p78dQi8JakMA");
            Telegram.Sender.TelegramSender telegramSender = new Telegram.Sender.TelegramSender(reciver.botClient);
            Telegram.Domain.Reminders.Reminder reminder = new Telegram.Domain.Reminders.Reminder(telegramSender);


            Telegram.Domain.BotDomain botDomain = new Telegram.Domain.BotDomain(telegramSender, reciver, fileStorage, inMemoryStorage, reminder);

            System.Console.WriteLine(reciver.GetHelloFromBot());
            reciver.Run();

            while (true)
            {
                System.Threading.Thread.Sleep(1800000);
                reminder.RemindersUpdate();
            }
            
        }
    }
}

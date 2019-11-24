
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


namespace BuildingLibrariesForTelegramsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString =
    "Server=ProductiveDeveloperCode.database.windows.net;Database=TelegramDB;User Id=ProductiveDeveloper;Password=fDx-Vbt-Lvr-7Vg;";

            Telegram.Bot.Storage.Sql.SqlStorage sqlStorage = new Telegram.Bot.Storage.Sql.SqlStorage(connectionString);

            Telegram.Bot.Storage.InMemory.InMemoryStorage inMemoryStorage = new Telegram.Bot.Storage.InMemory.InMemoryStorage();

            Telegram.Receiver.TelegramReceiver reciver = new Telegram.Receiver.TelegramReceiver("984957183:AAECP59aHpYez0iBzmw_QBMb-sxMRgO5vpM");
            Telegram.Sender.TelegramSender telegramSender = new Telegram.Sender.TelegramSender(reciver.botClient);
            Telegram.Domain.Reminders.Reminder reminder = new Telegram.Domain.Reminders.Reminder(telegramSender);


            Telegram.Domain.BotDomain botDomain = new Telegram.Domain.BotDomain(telegramSender, reciver, sqlStorage, inMemoryStorage , reminder);

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

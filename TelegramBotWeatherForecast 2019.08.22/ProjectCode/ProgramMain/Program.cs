using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Storage.InMemory;
using Telegram.Bot.Storage.Sql;
using Telegram.Domain;
using Telegram.Domain.Reminders;
using Telegram.Receiver;
using Telegram.Sender;

namespace ProgramMain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string connectionString =
    "Server=ProductiveDeveloperCode.database.windows.net;Database=TelegramDB;User Id=ProductiveDeveloper;Password=fDx-Vbt-Lvr-7Vg;";

            SqlStorage sqlStorage = new SqlStorage(connectionString);

            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            TelegramReceiver reciver = new TelegramReceiver("984957183:AAECP59aHpYez0iBzmw_QBMb-sxMRgO5vpM");
            TelegramSender telegramSender = new TelegramSender(reciver.botClient);
            Reminder reminder = new Reminder(telegramSender);


            BotDomain botDomain = new BotDomain(telegramSender, reciver, sqlStorage, inMemoryStorage, reminder);

            Console.WriteLine(reciver.GetHelloFromBot());
            reciver.Run();

            while (true)
            {
                Thread.Sleep(1800000);
                reminder.RemindersUpdate();
            }
        }
    }
}

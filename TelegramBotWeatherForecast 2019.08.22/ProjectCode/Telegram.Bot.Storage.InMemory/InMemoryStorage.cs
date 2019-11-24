using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Storage.Core;
using Telegram.Domain.UserItem.Core;

namespace Telegram.Bot.Storage.InMemory
{
    public class InMemoryStorage 
    {
        internal readonly Dictionary<Guid , IDomainUserItem> UserStorageInMemory;

        public InMemoryStorage ()
        {
            UserStorageInMemory = new Dictionary<Guid, IDomainUserItem>();
        }

        public int Count => UserStorageInMemory.Count;

        public void AddUser(IDomainUserItem user)
        {
            UserStorageInMemory.Add(user.Id , user);
        }

        public IDomainUserItem GetUser(Guid Id)
        {
            foreach (var item in UserStorageInMemory)
            {
                if (item.Key == Id)
                    return item.Value;
            }
            return null;
        }

        public List<IDomainUserItem> GetUsers()
        {
            List<IDomainUserItem> result = new List<IDomainUserItem>();
            foreach (var item in UserStorageInMemory)
            {
                result.Add(item.Value);
            }
            return result;
        }

        public void RemoveUser(Guid Id)
        {
            UserStorageInMemory.Remove(Id);
        }

        public void UpdateStatus(IDomainUserItem user)
        {
            foreach (var item in UserStorageInMemory)
            {
                if (user.Id == item.Key)
                {
                    item.Value.Status = user.Status;
                    item.Value.DomainStatus = user.DomainStatus;
                    item.Value.weatherForecast = user.weatherForecast;
                    item.Value.CountDays = user.CountDays;
                    item.Value.Admin_MessageSend = user.Admin_MessageSend;
                    item.Value.Admin_InputName = user.Admin_InputName;
                    item.Value.Admin_InputNumber = user.Admin_InputNumber;
                    item.Value.User_Reminder_CountDays = user.User_Reminder_CountDays;
                    item.Value.User_Reminder_City = user.User_Reminder_City;
                    item.Value.ReminderWeatherForecast = user.ReminderWeatherForecast;
                    item.Value.Admin_Photo = user.Admin_Photo;
                    item.Value.Admin_Caption = user.Admin_Caption;
                }
            }
        }

        public void Clear()
        {
            UserStorageInMemory.Clear();
        }

        public IDomainUserItem GetUser(long ChatId)
        {
            foreach (var item in UserStorageInMemory)
            {
                if (item.Value.ChatId == ChatId)
                    return item.Value;
            }
            return null;
        }
    }
}

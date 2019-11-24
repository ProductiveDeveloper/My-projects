using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Storage.InMemory;
using Telegram.Domain.Message;
using Telegram.Domain.Reminders;
using Telegram.Domain.UserItem.Core;
using Telegram.Receiver.Core;
using Telegram.Sender.Core;
using Telegram.Storage.Core;

namespace Telegram.Domain
{
    public class BotDomain
    {
        private readonly ISender _sender;
        private readonly IReceiver _receiver;
        private readonly IUserStorage _storage;
        private readonly InMemoryStorage _storageInMemory;
        private readonly Reminder _reminder;

        private UserPanel userPanel;
        private PanelAdmin panelAdmin;

        public BotDomain(ISender sender, IReceiver receiver, IUserStorage storage, InMemoryStorage storageInMemory , Reminder reminder)
        {
            _sender = sender;
            _receiver = receiver;
            _storage = storage;
            _storageInMemory = storageInMemory;
            _reminder = reminder;

            List<Telegram.Storage.Core.UserItem> userItems = _storage.GetUsers();
            foreach (var item in userItems)
            {
                DomainStatusEnum domainStatusEnum = DomainStatusEnum.User_Greeting;
                if (item.Status.Equals(StatusEnum.Admin))
                    domainStatusEnum = DomainStatusEnum.Admin_Greeteng_Notification;
                _storageInMemory.AddUser(new DomainUserItem()
                {
                    ChatId = item.ChatId,
                    DateOfRegistration = item.DateOfRegistration,
                    FirstName = item.FirstName,
                    Id = item.Id,
                    Message = item.Message,
                    Status = item.Status,
                    DomainStatus = domainStatusEnum
                });
            }

            _receiver.MessageReceived += Receiver_MeesageReciver;
        }

        private void Receiver_MeesageReciver(object sender, MessageReceivedEventArgs e)
        {
            IDomainUserItem item = _storageInMemory.GetUser(e.ChatId);
            DomainUserItem userItem;

            if (item == null)
            {
                Telegram.Storage.Core.UserItem newUser = new Telegram.Storage.Core.UserItem()
                {
                    ChatId = Convert.ToInt64(e.ChatId),
                    DateOfRegistration = DateTimeOffset.Now,
                    FirstName = e.FirstName,
                    Id = Guid.NewGuid(),
                    Message = e.Message,
                    Status = StatusEnum.User, //StatusEnum.User
                };

                _storage.AddUser(newUser);

                userItem = new DomainUserItem()
                {
                    ChatId = newUser.ChatId,
                    DateOfRegistration = newUser.DateOfRegistration,
                    FirstName = newUser.FirstName,
                    Id = newUser.Id,
                    Message = newUser.Message,
                    Status = newUser.Status, //newUser.Status,
                    DomainStatus = DomainStatusEnum.Admin_Greeteng_Notification,//DomainStatusEnum.User_Greeting
                };
                _storageInMemory.AddUser(userItem);
            }
            else
            {
                DomainStatusEnum domainStatus = item.DomainStatus;
                if (item.DomainStatus.Equals(DomainStatusEnum.Null))
                {
                    if (item.Status.Equals(StatusEnum.Admin))
                        domainStatus = DomainStatusEnum.Admin_Greeteng_Notification;
                    else if (item.Status.Equals(StatusEnum.User))
                        domainStatus = DomainStatusEnum.User_Greeting;
                }
                userItem = new DomainUserItem()
                {
                    ChatId = item.ChatId,
                    DateOfRegistration = item.DateOfRegistration,
                    FirstName = item.FirstName,
                    Id = item.Id,
                    Message = e.Message,
                    Status = item.Status,//ITEM.STATUS
                    DomainStatus = domainStatus,//item.DomainStatus
                    weatherForecast = item.weatherForecast,
                    Admin_MessageSend = item.Admin_MessageSend,
                    Admin_InputName = item.Admin_InputName,
                    CountDays = item.CountDays,
                    User_Reminder_CountDays = item.User_Reminder_CountDays,
                    User_Reminder_City = item.User_Reminder_City,
                    ReminderWeatherForecast = item.ReminderWeatherForecast,
                    Admin_Photo = item.Admin_Photo,
                    Photo = e.Photo,
                    Caption = e.Caption,
                    Admin_Caption = item.Admin_Caption
                };
            }

            userPanel = new UserPanel(userItem, _sender , _reminder);
            panelAdmin = new PanelAdmin(userItem, _sender , _storage);

            if (userItem.Status.Equals(StatusEnum.User))
                userItem = userPanel.UserPanelMessage(_storageInMemory);
            else if (userItem.Status.Equals(StatusEnum.Admin))
                userItem = panelAdmin.PanelAdminMessage(_storageInMemory);
            else
                _sender.SendMessage(userItem.ChatId, "Хмм... Что-то пошло не так.", null);

            if (userItem != item)
                _storageInMemory.UpdateStatus(userItem);         
        }
    }
}

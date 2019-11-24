using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Sender.Core;
using Telegram.Storage.Core;
using Telegram.Domain.UserItem.Core;
using Telegram.Bot.Storage.InMemory;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Domain.MessageBar;

namespace Telegram.Domain.Message
{
    class PanelAdmin : PanelAdminBar
    {
        private readonly DomainUserItem _userItem;
        private readonly ISender _sender;
        private readonly IUserStorage userStorage;

        public PanelAdmin(DomainUserItem userItem, ISender sender , IUserStorage userStorage)
        {
            _userItem = userItem;
            _sender = sender;
            this.userStorage = userStorage;
        }

        public DomainUserItem PanelAdminMessage(InMemoryStorage storage)
        {
            switch(_userItem.DomainStatus)
            {
                case DomainStatusEnum.Admin_Greeteng_Notification:
                    _sender.SendMessage(_userItem.ChatId, "Admin.", MainWindow);
                    _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                    break;
                case DomainStatusEnum.Admin_Greeteng:
                    if (_userItem.Message != null)
                    {
                        if (_userItem.Message.Equals("Отправить сообщение \U00002709"))
                        {
                            _sender.SendMessage(_userItem.ChatId, "Отправить сообщениe :", MessageSend);
                            _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend;
                        }
                        else if (_userItem.Message.Equals("Статус пользователя \U00002755"))
                        {
                            _sender.SendMessage(_userItem.ChatId, "Имя пользователя :", null);
                            _userItem.DomainStatus = DomainStatusEnum.Admin_UpdateStatus_InputName;
                        }
                        else
                        {
                            _sender.SendMessage(_userItem.ChatId, "Я не знаю такой команды." , MainWindow);
                        }
                    }
                    else
                    {
                        _sender.SendMessage(_userItem.ChatId, "Я не знаю такой команды.", MainWindow);
                    }
                    break;
                case DomainStatusEnum.Admin_UpdateStatus_InputName:
                    _userItem.Admin_InputName = _userItem.Message;
                    int r = 0;
                    foreach (var item in storage.GetUsers())
                    {
                        if (item.FirstName == _userItem.Message)
                        {
                            r++;
                        }
                    }
                    if (r >= 1)
                    {
                        _userItem.DomainStatus = DomainStatusEnum.Admin_UpdateStatus_RepairStatus;
                        _sender.SendMessage(_userItem.ChatId, $"{_userItem.Admin_InputName} найден. \U00002705", null);
                        _sender.SendMessage(_userItem.ChatId, "Cтатус : ", RepairStatusBar);
                    }
                    else if (r.Equals(0))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Пользователя с данным именем не найдено \U0000274C", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                    }
                    break;
                case DomainStatusEnum.Admin_UpdateStatus_RepairStatus:
                    if (_userItem.Message.Equals("User") || _userItem.Message.Equals("@WeatherConditions_bot User"))
                    {
                        foreach (var item in storage.GetUsers())
                        {
                            if (item.FirstName.Equals(_userItem.Admin_InputName))
                            {
                                if (item.ChatId == _userItem.ChatId)
                                {
                                    _userItem.Status = StatusEnum.User;
                                   _userItem.DomainStatus = DomainStatusEnum.Null;
                                    userStorage.UpdateStatus(new Telegram.Storage.Core.UserItem() { ChatId = _userItem.ChatId, Status = StatusEnum.User, Id = _userItem.Id, FirstName = _userItem.FirstName, DateOfRegistration = _userItem.DateOfRegistration });
                                    _sender.SendMessage(_userItem.ChatId, $"{_userItem.FirstName} : User" , null);
                                }
                                else
                                {
                                    item.Status = StatusEnum.User;
                                    item.DomainStatus = DomainStatusEnum.Null;
                                    userStorage.UpdateStatus(new Telegram.Storage.Core.UserItem() { ChatId = item.ChatId, Status = StatusEnum.User, Id = item.Id, FirstName = item.FirstName, DateOfRegistration = item.DateOfRegistration });
                                    _sender.SendMessage(_userItem.ChatId, $"{item.FirstName} : User", MainWindow);
                                    _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                                }
                            }
                        }
                    }
                    else if (_userItem.Message.Equals("Mut") || _userItem.Message.Equals("@WeatherConditions_bot Mut"))
                    {
                        foreach (var item in storage.GetUsers())
                        {
                            if (item.FirstName.Equals(_userItem.Admin_InputName))
                            {
                                if (item.ChatId == _userItem.ChatId)
                                {
                                    _sender.SendMessage(_userItem.ChatId, "Вы не можете себе изменить статус на Мут \U0000274C", null);
                                }
                                else
                                {
                                    item.Status = StatusEnum.Ban;
                                    item.DomainStatus = DomainStatusEnum.Null;
                                    userStorage.UpdateStatus(new Telegram.Storage.Core.UserItem() { ChatId = item.ChatId, Status = StatusEnum.Ban, Id = item.Id, FirstName = item.FirstName, DateOfRegistration = item.DateOfRegistration });
                                    _sender.SendMessage(item.ChatId, $"{item.FirstName} : Мут", PanelAdmin.MainWindow);
                                    _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                                }
                            }
                        }
                    }
                    else if (_userItem.Message.Equals("Отмена") || _userItem.Message.Equals("@WeatherConditions_bot Back"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                        _sender.SendMessage(_userItem.ChatId, "Отмена. \U0001F519", MainWindow);
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend:
                    if (_userItem.Message.Equals("@WeatherConditions_bot All") || _userItem.Message.Equals("All"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Cообщение :", null);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_All_InputMessage;
                    }
                    else if (_userItem.Message.Equals("@WeatherConditions_bot User"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Имя пользователя :", null);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_User_InputNick;
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_User_InputNick:
                    _userItem.Admin_InputName = _userItem.Message;
                    int i = 0;
                    foreach (var item in storage.GetUsers())
                    {
                        if (item.FirstName == _userItem.Message)
                        {
                            i++;       
                        }
                    }
                    if (i.Equals(1))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_User_InputMessage;
                        _sender.SendMessage(_userItem.ChatId, "Cообщение :", null);
                    }
                    else if (i.Equals(0))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Пользователя с данным именем не найдено \U0000274C", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_User_InputMessage:
                    _userItem.Admin_Photo = _userItem.Photo;
                    _userItem.Admin_Caption = _userItem.Caption;
                    _userItem.Admin_MessageSend = _userItem.Message;
                    _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_User_MainPage;
                    if (_userItem.Message != null)
                        _sender.SendMessage(_userItem.ChatId, $"Ваше сообщение :\n\n {_userItem.Message}", MessageSendAll);
                    else if (_userItem.Photo != null)
                    {
                        _sender.SendMessage(_userItem.ChatId, $"Ваше сообщение :\n\n", null);
                        _sender.SendPhoto(_userItem.ChatId, _userItem.Photo, _userItem.Caption, MessageSendAll);
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_User_MainPage:
                    if (_userItem.Message.Equals("Отправить"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Отправка...", null);
                        foreach (var item in storage.GetUsers())
                        {
                            if (item.FirstName.Equals(_userItem.Admin_InputName))
                            {
                                if (_userItem.Admin_MessageSend != null)
                                    _sender.SendMessage(item.ChatId, _userItem.Admin_MessageSend, null);
                                else if (_userItem.Admin_Photo != null)
                                {
                                    _sender.SendPhoto(item.ChatId, _userItem.Admin_Photo, _userItem.Admin_Caption, null);
                                }
                            }
                        }
                        _sender.SendMessage(_userItem.ChatId, "Отправлено. \U00002705", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                    }
                    else if (_userItem.Message.Equals("Отправить себе"))
                    {
                        if (_userItem.Admin_MessageSend != null)
                            _sender.SendMessage(_userItem.ChatId, _userItem.Admin_MessageSend, null);
                        else if (_userItem.Admin_Photo != null)
                        {
                            _sender.SendPhoto(_userItem.ChatId, _userItem.Admin_Photo, _userItem.Admin_Caption, null);
                        }
                    }
                    else if (_userItem.Message.Equals("Отмена"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                        _sender.SendMessage(_userItem.ChatId, "Отмена. \U0001F519", MainWindow);
                    }
                    else if (_userItem.Message.Equals("Изменить сообщение"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Вводите сообщение.", null);
                        _userItem.Admin_MessageSend = null;
                        _userItem.Admin_Photo = null;
                        _userItem.Admin_Caption = null;
                        _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_User_InputMessage;
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_All_InputMessage:
                    _userItem.Admin_Photo = _userItem.Photo;
                    _userItem.Admin_Caption = _userItem.Caption;
                    _userItem.Admin_MessageSend = _userItem.Message;
                    _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_All;
                    if (_userItem.Message != null)
                        _sender.SendMessage(_userItem.ChatId, $"Ваше сообщение :\n\n {_userItem.Message}", MessageSendAll);
                    else if (_userItem.Photo != null)
                    {
                        _sender.SendMessage(_userItem.ChatId, $"Ваше сообщение :\n\n", null);
                        _sender.SendPhoto(_userItem.ChatId, _userItem.Photo, _userItem.Caption, MessageSendAll);
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_All:
                    if (_userItem.Message.Equals("Изменить сообщение"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Вводите сообщение.", null);
                        _userItem.Admin_MessageSend = null;
                        _userItem.Admin_Photo = null;
                        _userItem.Admin_Caption = null;
                        _userItem.DomainStatus = DomainStatusEnum.Admin_MessageSend_All_InputMessage;
                    }
                    else if (_userItem.Message.Equals("Отправить себе"))
                    {
                        if (_userItem.Admin_MessageSend != null)
                            _sender.SendMessage(_userItem.ChatId, _userItem.Admin_MessageSend, null);
                        else if (_userItem.Admin_Photo != null)
                        {
                            _sender.SendPhoto(_userItem.ChatId, _userItem.Admin_Photo, _userItem.Admin_Caption, null);
                        }
                    }
                    else if (_userItem.Message.Equals("Отправить"))
                    {
                        _sender.SendMessage(_userItem.ChatId, "Отправка...", null);
                        foreach (var item in storage.GetUsers())
                        {
                            if (item.Status.Equals(StatusEnum.User))
                            {
                                if (_userItem.Admin_MessageSend != null)
                                    _sender.SendMessage(item.ChatId, _userItem.Admin_MessageSend, null);
                                else if (_userItem.Admin_Photo != null)
                                {
                                    _sender.SendPhoto(item.ChatId, _userItem.Admin_Photo, _userItem.Admin_Caption, null);
                                }
                            }
                        }
                        _sender.SendMessage(_userItem.ChatId, "Отправлено. \U00002705", MainWindow);
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                    }
                    else if (_userItem.Message.Equals("Отмена"))
                    {
                        _userItem.DomainStatus = DomainStatusEnum.Admin_Greeteng;
                        _sender.SendMessage(_userItem.ChatId, "Отмена. \U0001F519", MainWindow);
                    }
                    break;
                case DomainStatusEnum.Admin_MessageSend_User:
                    break;
            }

            return _userItem;
        }
    }
}

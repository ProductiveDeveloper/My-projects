using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Domain.UserItem.Core
{
    public enum DomainStatusEnum
    {
        //User
        Null,
        User_Greeting,
        User_MainWindow,
        User_Weather_InputCity,
        User_Weather_IputCountDays,
        User_Reminder_InputDays,
        User_Reminder_InputCity,
        //Admin
        Admin_Greeteng_Notification,
        Admin_Greeteng,
        Admin_MessageSend,
        Admin_MessageSend_User,
        Admin_MessageSend_All,
        Admin_MessageSend_All_InputMessage,
        Admin_MessageSend_User_InputNick,
        Admin_MessageSend_User_GetUser,
        Admin_MessageSend_User_InputMessage,
        Admin_MessageSend_User_MainPage,
        Admin_MessageSend_User_SendMessage,
        Admin_UpdateStatus_InputName,
        Admin_UpdateStatus_InputNumberPhone,
        Admin_UpdateStatus_RepairStatus, 
    }
}

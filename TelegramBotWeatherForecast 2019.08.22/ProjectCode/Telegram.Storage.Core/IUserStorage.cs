using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Storage.Core
{
    public interface IUserStorage
    {
        void AddUser(UserItem user);

        UserItem GetUser(Guid Id);

        UserItem GetUser(long ChatId);

        List<UserItem> GetUsers();

        int Count { get; }

        void UpdateStatus(UserItem user);

        void RemoveUser(Guid Id);

        void Clear();
    }
}

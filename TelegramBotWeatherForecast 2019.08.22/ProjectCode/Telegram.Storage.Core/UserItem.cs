using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Storage.Core
{
    public class UserItem : IUserItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long ChatId { get; set; }

        public string FirstName { get; set; }

        public StatusEnum Status { get; set; }

        public string Message { get; set; }

        public DateTimeOffset DateOfRegistration { get; set; }// = DateTimeOffset.Now;        
    }
}

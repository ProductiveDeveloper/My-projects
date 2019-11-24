using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Storage.Core
{
    public interface IUserItem
    {
        Guid Id { get; set; }

        long ChatId { get; set; }

        string FirstName { get; set; }

        StatusEnum Status { get; set; }

        string Message { get; set; }

        DateTimeOffset DateOfRegistration { get; set; }
    }
}

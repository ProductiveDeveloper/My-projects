using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Storage.Core;

namespace Telegram.Bot.Storage.File
{
    class Parser
    {
        internal static List<UserItem> WordParser (List<string> lines)
        {           
            List<UserItem> result = new List<UserItem>();

            UserItem userItem = new UserItem();
            int counter = 0;
            foreach (var line in lines)
            {
                if (line == "")
                    continue;
                switch(counter)
                {
                    case 0:
                        counter++;
                        userItem.Id = Guid.Parse(line);
                        break;
                    case 1:
                        counter++;
                        userItem.FirstName = line;
                        break;
                    //в зависимости от количества статусов обновлять метод
                    case 2:
                        if (Convert.ToString(StatusEnum.Admin).Equals(line))
                            userItem.Status = StatusEnum.Admin;
                        else if (Convert.ToString(StatusEnum.Ban).Equals(line))
                            userItem.Status = StatusEnum.Ban;
                        else if (Convert.ToString(StatusEnum.User).Equals(line))
                            userItem.Status = StatusEnum.User;
                        else
                            userItem.Status = StatusEnum.Null;
                        counter++;
                        break;
                    case 3:
                        userItem.DateOfRegistration = DateTimeOffset.Parse(line);
                        counter++;
                        break;
                    case 4:
                        userItem.ChatId = Convert.ToInt64(line);
                        result.Add(userItem);
                        userItem = new UserItem();
                        counter = 0;
                        break;                
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Telegram.Storage.Core;

namespace Telegram.Bot.Storage.File
{
    public class FileStorage : IUserStorage
    {
        internal readonly DirectoryInfo DirectoryPath;

        internal StreamWriter Storage;

        private string path;
        private string FileStoragePath;

        public FileStorage(string path)
        {
            if (!Directory.Exists(path + @"\FileStorage"))
            {
                DirectoryPath = new DirectoryInfo(path + @"\FileStorage");
                DirectoryPath.Create();
                Storage = new StreamWriter(path + @"\FileStorage" + @"\Storage.txt");
                Storage.Close();
            }
            this.path = path + @"\FileStorage" + @"\Storage.txt";
            FileStoragePath = @"\FileStorage";
        }

        public int Count
        {
            get
            {
                return GetUsers().Count;
            }
        }

        public void AddUser(UserItem user)
        {
            //string dateString = $"{user.DateOfRegistration.Day}/{user.DateOfRegistration.Month}/{user.DateOfRegistration.Year} {user.DateOfRegistration.Hour}:{user.DateOfRegistration.Minute} PM";
            //user.DateOfRegistration = DateTimeOffset.Parse(dateString);
            user.DateOfRegistration = DateTimeOffset.Now;
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.Write($"{user.Id}\n" +
                    $"{user.FirstName}\n" +
                    $"{user.Status}\n" +
                    $"{user.DateOfRegistration}\n" +
                    $"{user.ChatId}\n");
            }
        }

        public void Clear()
        {
            Storage = new StreamWriter(path);
            Storage.Close();
        }

        public UserItem GetUser(Guid Id)
        {
            List<UserItem> userItems = GetUsers();

            foreach (var item in userItems)
            {
                if (item.Id == Id)
                    return item;
            }
            return null;
        }

        public UserItem GetUser(long ChatId)
        {
            List<UserItem> userItems = GetUsers();

            foreach (var item in userItems)
            {
                if (item.ChatId == ChatId)
                    return item;
            }
            return null;
        }

        public List<UserItem> GetUsers()
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return Parser.WordParser(lines);
        }

        public void RemoveUser(Guid Id)
        {
            List<UserItem> userItems = GetUsers();

            Clear();

            foreach (var item in userItems)
            {
                if (item.Id != Id)
                {
                    AddUser(item);
                }
            }
        }

        public void UpdateStatus(UserItem user)
        {
            List<UserItem> userItems = GetUsers();

            Clear();

            foreach (var item in userItems)
            {
                if (item.Id != user.Id)
                {
                    AddUser(item);
                }
                else
                {
                    UserItem userItem = item;
                    userItem.Status = user.Status;
                    AddUser(userItem);
                }
            }
        }
    }
}

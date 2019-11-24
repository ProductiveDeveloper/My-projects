using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Telegram.Bot.Storage.Sql;
using Telegram.Storage.Core;

namespace Telegram.Bot.Storage.Sql.Tests
{
    [TestClass]
    public class SqlStorageTests
    {
        const string connectionStringTemplate =
    "Data Source={0};Initial Catalog={1};Integrated Security=true;";
        static string connectionString = string.Format(
            connectionStringTemplate,
            @"HP\SQLEXPRESS",
            "TelegramBotTESTS");

        SqlStorage SqlStorage = new SqlStorage(connectionString);

        [TestMethod]
        public void Add_User_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            Guid guid = Guid.NewGuid();
            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = dateTimeOffset,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };

            Assert.AreEqual(SqlStorage.Count, 0);

            SqlStorage.AddUser(user);

            Assert.AreEqual(SqlStorage.Count, 1);
            SqlStorage.Clear();
        }

        [TestMethod]
        public void Get_User_Guid_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            var date = DateTimeOffset.Now;
            Guid guid = Guid.NewGuid();
            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            SqlStorage.AddUser(user);

            Assert.AreEqual(1, SqlStorage.Count);

            UserItem item = SqlStorage.GetUser(guid);

            Assert.AreEqual(1, SqlStorage.Count);

            Assert.AreEqual(item.ChatId, user.ChatId);
            Assert.AreEqual(item.DateOfRegistration, user.DateOfRegistration);
            Assert.AreEqual(item.FirstName, user.FirstName);
            Assert.AreEqual(item.Id, user.Id);
            Assert.AreEqual(item.Message, user.Message);
            Assert.AreEqual(item.Status, user.Status);

            SqlStorage.Clear();
        }

        [TestMethod]
        public void Get_Users_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };

            SqlStorage.AddUser(user);

            List<UserItem> userItems = SqlStorage.GetUsers();

            Assert.AreEqual(userItems.Count, 1);
            Assert.AreEqual(userItems[0].ChatId , user.ChatId);
            Assert.AreEqual(userItems[0].DateOfRegistration, user.DateOfRegistration);
            Assert.AreEqual(userItems[0].FirstName, user.FirstName);
            Assert.AreEqual(userItems[0].Id, user.Id);
            Assert.AreEqual(userItems[0].Message, user.Message);
            Assert.AreEqual(userItems[0].Status, user.Status);

            SqlStorage.Clear();
        }

        [TestMethod]
        public void Count_Users_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            SqlStorage.AddUser(user);
            Assert.AreEqual(SqlStorage.Count, 1);
            SqlStorage.Clear();
            Assert.AreEqual(SqlStorage.Count, 0);
        }

        [TestMethod]
        public void Update_Status_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            var date = DateTimeOffset.Now;
            Guid guid = Guid.NewGuid();
            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            SqlStorage.AddUser(user);

            Assert.AreEqual(SqlStorage.Count, 1);

            UserItem user2 = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Admin
            };

            SqlStorage.UpdateStatus(user2);

            UserItem item = SqlStorage.GetUser(guid);

            Assert.AreEqual(SqlStorage.Count, 1);
            
            Assert.AreEqual(item.ChatId, user.ChatId);
            Assert.AreEqual(item.DateOfRegistration, user.DateOfRegistration);
            Assert.AreEqual(item.FirstName, user.FirstName);
            Assert.AreEqual(item.Id, user.Id);
            Assert.AreEqual(item.Message, user.Message);
            Assert.AreEqual(item.Status, StatusEnum.Admin);
            
            SqlStorage.Clear();
        }

        [TestMethod]
        public void Remove_User_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            Guid guid = Guid.NewGuid();
            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = dateTimeOffset,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            SqlStorage.AddUser(user);

            Assert.AreEqual(SqlStorage.Count, 1);

            SqlStorage.RemoveUser(guid);

            Assert.AreEqual(SqlStorage.Count, 0);

            SqlStorage.Clear();
        }

        [TestMethod]
        public void Clear_Users_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            UserItem user = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };

            UserItem user2 = new UserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };

            SqlStorage.AddUser(user);
            SqlStorage.AddUser(user2);

            Assert.AreEqual(2, SqlStorage.Count);

            SqlStorage.Clear();

            Assert.AreEqual(0 , SqlStorage.Count);
        }

        [TestMethod]
        public void Get_User_ChatId_Storage_Sql_Tests_Method()
        {
            SqlStorage.Clear();

            var date = DateTimeOffset.Now;
            long ChatId = 12345678;
            UserItem user = new UserItem()
            {
                ChatId = ChatId,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            SqlStorage.AddUser(user);

            Assert.AreEqual(1, SqlStorage.Count);

            UserItem item = SqlStorage.GetUser(ChatId);

            Assert.AreEqual(1, SqlStorage.Count);

            Assert.AreEqual(item.ChatId, user.ChatId);
            Assert.AreEqual(item.DateOfRegistration, user.DateOfRegistration);
            Assert.AreEqual(item.FirstName, user.FirstName);
            Assert.AreEqual(item.Id, user.Id);
            Assert.AreEqual(item.Message, user.Message);
            Assert.AreEqual(item.Status, user.Status);

            SqlStorage.Clear();
        }
    }
}

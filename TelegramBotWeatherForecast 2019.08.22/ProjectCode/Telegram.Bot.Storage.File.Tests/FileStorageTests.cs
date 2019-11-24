using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Bot.Storage.File;
using System;
using Telegram.Storage.Core;
using System.Collections.Generic;

namespace Telegram.Bot.Storage.File.Tests
{
    [TestClass]
    public class FileStorageTests
    {
        public static string path = @"C:\Users\Ракета\source\repos\BuildingLibrariesForTelegrams\Telegram.Bot.Storage.File.Tests";
        FileStorage fileStorage = new FileStorage(path);

        [TestMethod]
        public void Add_User_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            Assert.AreEqual(fileStorage.Count, 1);

            fileStorage.Clear();
        }

        [TestMethod]
        public void Get_User_Guid_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            var date2 = DateTimeOffset.Now;
            var guid2 = Guid.NewGuid();
            UserItem userItem2 = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date2,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem2);

            Assert.AreEqual(2, fileStorage.Count);

            UserItem GetUserItem = fileStorage.GetUser(guid);
            UserItem GetUserItem2 = fileStorage.GetUser(guid2);

            Assert.AreEqual(GetUserItem.Id, userItem.Id);
            Assert.AreEqual(GetUserItem2.Id, userItem2.Id);

            fileStorage.Clear();
        }

        [TestMethod]
        public void Get_Users_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            var date2 = DateTimeOffset.Now;
            var guid2 = Guid.NewGuid();
            UserItem userItem2 = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date2,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem2);

            Assert.AreEqual(2, fileStorage.Count);

            List<UserItem> userItems = fileStorage.GetUsers();

            Assert.AreEqual(2, userItems.Count);

            Assert.AreEqual(userItems[0].Id, userItem.Id);
            Assert.AreEqual(userItems[1].Id, userItem2.Id);

            fileStorage.Clear();
        }

        [TestMethod]
        public void Count_Users_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            Assert.AreEqual(1, fileStorage.Count);

            var date2 = DateTimeOffset.Now;
            var guid2 = Guid.NewGuid();
            UserItem userItem2 = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date2,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem2);

            Assert.AreEqual(2 , fileStorage.Count);

            fileStorage.Clear();

            Assert.AreEqual(0, fileStorage.Count);
        }

        [TestMethod]
        public void Update_Status_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Admin
            };
            fileStorage.AddUser(userItem);

            UserItem userItem2 = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Ban
            };

            Assert.AreEqual(1, fileStorage.Count);

            fileStorage.UpdateStatus(userItem2);

            Assert.AreEqual(fileStorage.GetUser(guid).Status, StatusEnum.Ban);

            fileStorage.UpdateStatus(userItem);

            Assert.AreEqual(fileStorage.GetUser(guid).Status, StatusEnum.Admin);

            Assert.AreEqual(fileStorage.Count, 1);

            fileStorage.Clear();
        }

        [TestMethod]
        public void Remove_User_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            var date2 = DateTimeOffset.Now;
            var guid2 = Guid.NewGuid();
            UserItem userItem2 = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date2,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem2);

            Assert.AreEqual(2, fileStorage.Count);

            fileStorage.RemoveUser(guid);

            Assert.AreEqual(1 , fileStorage.Count);

            fileStorage.RemoveUser(guid2);

            Assert.AreEqual(0, fileStorage.Count);

            fileStorage.Clear();
        }

        [TestMethod]
        public void Clear_Users_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            var guid = Guid.NewGuid();
            UserItem userItem = new UserItem()
            {
                ChatId = 123,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            Assert.AreEqual(1, fileStorage.Count);

            fileStorage.Clear();

            Assert.AreEqual(0, fileStorage.Count);
        }

        [TestMethod]
        public void Get_User_ChatId_Storage_File_Tests_Method()
        {
            fileStorage.Clear();

            var date = DateTimeOffset.Now;
            long ChatId = 12345678;
            UserItem userItem = new UserItem()
            {
                ChatId = ChatId,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem);

            var date2 = DateTimeOffset.Now;
            long ChatId2 = 876433234;
            UserItem userItem2 = new UserItem()
            {
                ChatId = ChatId2,
                DateOfRegistration = date2,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            fileStorage.AddUser(userItem2);

            Assert.AreEqual(2, fileStorage.Count);

            UserItem GetUserItem = fileStorage.GetUser(ChatId);
            UserItem GetUserItem2 = fileStorage.GetUser(ChatId2);

            Assert.AreEqual(GetUserItem.Id, userItem.Id);
            Assert.AreEqual(GetUserItem2.Id, userItem2.Id);

            fileStorage.Clear();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Telegram.Bot.Storage.InMemory;
using Telegram.Domain.UserItem.Core;
using Telegram.Storage.Core;

namespace Telegram.Bot.Storage.InMemory.Tests
{
    [TestClass]
    public class InMemoryStorageTests
    {
        [TestMethod]
        public void Add_User_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DateTimeOffset date = DateTimeOffset.Now;
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };

            inMemoryStorage.AddUser(user);

            List<IDomainUserItem> userItems = inMemoryStorage.GetUsers();

            Assert.AreEqual(1, userItems.Count);

            foreach (var item in userItems)
            {
                Assert.AreEqual(item, user);
            }
            Assert.AreEqual(1, inMemoryStorage.Count);
        }

        [TestMethod]
        public void Get_User_Id_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DateTimeOffset date = DateTimeOffset.Now;
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user);
            Assert.AreEqual(inMemoryStorage.Count, 1);
            IUserItem userItem = inMemoryStorage.GetUser(guid);
            Assert.AreEqual(inMemoryStorage.Count, 1);
            Assert.AreEqual(userItem, user);
        }

        [TestMethod]
        public void Get_Users_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DateTimeOffset date = DateTimeOffset.Now;
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user);

            Assert.AreEqual(inMemoryStorage.Count, 1);
            List<IDomainUserItem> userItems = inMemoryStorage.GetUsers();
            Assert.AreEqual(userItems.Count, 1);
            Assert.AreEqual(userItems[0], user);
        }

        [TestMethod]
        public void Count_Users_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user);
            List<IDomainUserItem> userItems = inMemoryStorage.GetUsers();

            Assert.AreEqual(1, userItems.Count);

            DomainUserItem user_2 = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user_2);
            userItems = inMemoryStorage.GetUsers();

            Assert.AreEqual(2, userItems.Count);
        }

        [TestMethod]
        public void Update_Status_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user);
            Assert.AreEqual(inMemoryStorage.Count, 1);
            inMemoryStorage.UpdateStatus(user);
            Assert.AreEqual(StatusEnum.Null, inMemoryStorage.GetUser(guid).Status);
            Assert.AreEqual(inMemoryStorage.Count, 1);
        }

        [TestMethod]
        public void Remove_User_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            Guid guid2 = Guid.NewGuid();
            DomainUserItem user2 = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };

            inMemoryStorage.AddUser(user);
            inMemoryStorage.AddUser(user2);

            Assert.AreEqual(inMemoryStorage.Count, 2);

            inMemoryStorage.RemoveUser(guid);

            Assert.AreEqual(inMemoryStorage.Count, 1);
            Assert.AreNotEqual(inMemoryStorage.GetUser(guid2).Id, guid);
        }

        [TestMethod]
        public void Clear_Users_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            Guid guid = Guid.NewGuid();
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = guid,
                Message = "Null",
                Status = StatusEnum.Null
            };
            Guid guid2 = Guid.NewGuid();
            DomainUserItem user2 = new DomainUserItem()
            {
                ChatId = 12345678,
                DateOfRegistration = DateTimeOffset.Now,
                FirstName = "Null",
                Id = guid2,
                Message = "Null",
                Status = StatusEnum.Null
            };

            inMemoryStorage.AddUser(user);
            inMemoryStorage.AddUser(user2);

            Assert.AreEqual(2 , inMemoryStorage.Count);

            inMemoryStorage.Clear();

            Assert.AreEqual(0, inMemoryStorage.Count);
        }

        [TestMethod]
        public void Get_User_ChatId_Storage_InMemory_Tests_Method()
        {
            InMemoryStorage inMemoryStorage = new InMemoryStorage();

            long ChatId = 123456678;
            DateTimeOffset date = DateTimeOffset.Now;
            DomainUserItem user = new DomainUserItem()
            {
                ChatId = ChatId,
                DateOfRegistration = date,
                FirstName = "Null",
                Id = Guid.NewGuid(),
                Message = "Null",
                Status = StatusEnum.Null
            };
            inMemoryStorage.AddUser(user);
            Assert.AreEqual(inMemoryStorage.Count, 1);
            IUserItem userItem = inMemoryStorage.GetUser(ChatId);
            Assert.AreEqual(inMemoryStorage.Count, 1);
            Assert.AreEqual(userItem, user);
        }
    }
}

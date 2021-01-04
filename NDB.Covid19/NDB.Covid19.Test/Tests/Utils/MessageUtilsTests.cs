using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonServiceLocator;
using FluentAssertions;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using Xunit;
using Xunit.Sdk;
using NDB.Covid19.Configuration;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.PersistedData.SQLite;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class MessageUtilsTests: IDisposable
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class BeforeAfterAttr : BeforeAfterTestAttribute
        {
            public override void Before(MethodInfo methodUnderTest)
            {
                base.Before(methodUnderTest);
                MessageUtils.RemoveAll();
                CreateTestMessages();
            }

            public override void After(MethodInfo methodUnderTest)
            {
                base.After(methodUnderTest);
                MessageUtils.RemoveAll();
            }

            public void CreateTestMessages()
            {
                MessageSQLiteModel messageSqLiteModel = new MessageSQLiteModel()
                {
                    IsRead = false,
                    MessageLink = "https://www.netcompany.com",
                    TimeStamp = SystemTime.Now().Subtract(TimeSpan.FromDays(20)),
                    Title = "Du har opholdt dig på tæt afstand af en COVID - 19 positiv"
                };

                MessageSQLiteModel messageSqLiteModel2 = new MessageSQLiteModel()
                {
                    IsRead = false,
                    MessageLink = "https://www.netcompany.com",
                    TimeStamp = SystemTime.Now().Subtract(TimeSpan.FromDays(15)),
                    Title = "Du har opholdt dig på tæt afstand af en COVID - 19 positiv"
                };

                MessageSQLiteModel messageSqLiteModel3 = new MessageSQLiteModel()
                {
                    IsRead = false,
                    MessageLink = "https://www.netcompany.com",
                    TimeStamp = SystemTime.Now(),
                    Title = "Du har opholdt dig på tæt afstand af en COVID - 19 positiv"
                };
                ServiceLocator.Current.GetInstance<IMessagesManager>().SaveNewMessage(messageSqLiteModel);
                ServiceLocator.Current.GetInstance<IMessagesManager>().SaveNewMessage(messageSqLiteModel2);
                ServiceLocator.Current.GetInstance<IMessagesManager>().SaveNewMessage(messageSqLiteModel3);
            }

        }

        public MessageUtilsTests()
        {
            DependencyInjectionConfig.Init();
            var secureStorageService = ServiceLocator.Current.GetInstance<SecureStorageService>();
            secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
            ApiStubHelper.StartServer();
        }

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        [BeforeAfterAttr]
        public async void SaveShouldNotThrowErrors()
        {
            try
            {
                int id = await MessageUtils.SaveMessages(new MessageSQLiteModel());
                id.Should().BeGreaterOrEqualTo(1);
            }
            catch
            {
                Assert.True(false);
                return;
            }
            Assert.True(true);
        }

        [Fact]
        [BeforeAfterAttr]
        public async void GetMessagesShouldReturnThreeRecords()
        {
            (await MessageUtils.GetMessages()).Should().HaveCount(3);
        }

        [Fact]
        [BeforeAfterAttr]
        public async void RemoveAllShouldHaveZeroRecords()
        {
            MessageUtils.RemoveAll();
            (await MessageUtils.GetMessages()).Should().HaveCount(0);
        }

        [Fact]
        [BeforeAfterAttr]
        public async void RemoveShouldHaveTwoRecords()
        {
            var models = await MessageUtils.GetMessages();
            await MessageUtils.RemoveMessages(models.GetRange(0,1));

            var messagesAfterRemoval = await MessageUtils.GetMessages();
            messagesAfterRemoval.Should().HaveCount(2);

            messagesAfterRemoval.Any(model => model.ID == models[0].ID).Should().BeFalse();
            messagesAfterRemoval.Any(model => model.ID == models[1].ID).Should().BeTrue();
            messagesAfterRemoval.Any(model => model.ID == models[2].ID).Should().BeTrue();
        }

        [Fact]
        [BeforeAfterAttr]
        public async void RemoveAllOlderThanShouldHaveOneRecord()
        {
            await MessageUtils.RemoveAllOlderThan(Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES);

            var messagesAfterRemoval = await MessageUtils.GetMessages();
            messagesAfterRemoval.Should().HaveCount(1);
        }

        [Fact]
        [BeforeAfterAttr]
        public async void MarkAsReadShouldMarkRecords()
        {
            var models = MessageUtils.ToMessageItemViewModelList(await MessageUtils.GetMessages());
            MessageUtils.MarkAsRead(models[1], true);

            var messagesAfterMark = await MessageUtils.GetMessages();
            messagesAfterMark.Should().HaveCount(3);

            messagesAfterMark[0].IsRead.Should().BeFalse();
            messagesAfterMark[1].IsRead.Should().BeTrue();
            messagesAfterMark[2].IsRead.Should().BeFalse();
        }

        [Fact]
        [BeforeAfterAttr]
        public async void GetAllUnreadShouldReturnTwoRecords()
        {
            var models = MessageUtils.ToMessageItemViewModelList(await MessageUtils.GetMessages());
            MessageUtils.MarkAsRead(models[1], true);

            var messagesAfterMark = await MessageUtils.GetAllUnreadMessages();
            messagesAfterMark.Should().HaveCount(2);

            messagesAfterMark.All(message => message.IsRead == false).Should().BeTrue();
        }
        
        [Fact]
        [BeforeAfterAttr]
        public async void GetMarkAllAsReadShouldMarkAllMessagesAsRead()
        {
            MessageUtils.MarkAllAsRead();

            List<MessageSQLiteModel> messagesAfterMark = await MessageUtils.GetAllUnreadMessages();
            messagesAfterMark.Should().HaveCount(0);

            messagesAfterMark.All(message => message.IsRead).Should().BeTrue();
        }
    }
}

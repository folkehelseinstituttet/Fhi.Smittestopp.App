using System;
using System.Threading;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class ResendNotificationTests : IDisposable
    {
        private static readonly LocalNotificationManagerMock LocalNotificationsManager =
            (LocalNotificationManagerMock) NotificationsHelper.LocalNotificationsManager;

        private static SecureStorageService _secureStorageService =>
            ServiceLocator.Current.GetInstance<SecureStorageService>();

        private static PermissionsMock Permissions =>
            (PermissionsMock) ServiceLocator.Current.GetInstance<IPermissionsHelper>();

        public ResendNotificationTests()
        {
            DependencyInjectionConfig.Init();
            _secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        [Theory]
        // Today
        [InlineData(0, -1, false)] // Before lower bound
        [InlineData(0, 0, false)] // Equal lower bound
        [InlineData(0, 1, false)] // After lower bound
        [InlineData(0, -1, false, true)] // Before upper bound
        [InlineData(0, 0, false, true)] // Equal upper bound
        [InlineData(0, 1, false, true)] // After upper bound
        // Next day
        [InlineData(1, -1, false)] // Before lower bound
        [InlineData(1, 0, false)] // Equal lower bound
        [InlineData(1, 1, false)] // After lower bound
        [InlineData(1, -1, false, true)] // Before upper bound
        [InlineData(1, 0, false, true)] // Equal upper bound
        [InlineData(1, 1, false, true)] // After upper bound
        // After 48 hours
        [InlineData(2, -1, false)] // Before lower bound
        [InlineData(2, 0, true)] // Equal lower bound
        [InlineData(2, 1, true)] // After lower bound
        [InlineData(2, -1, true, true)] // Before upper bound
        [InlineData(2, 0, true, true)] // Equal upper bound
        [InlineData(2, 1, false, true)] // After upper bound
        // After 48hours but odd day
        [InlineData(3, -1, false)] // Before lower bound
        [InlineData(3, 0, false)] // Equal lower bound
        [InlineData(3, 1, false)] // After lower bound
        [InlineData(3, -1, false, true)] // Before upper bound
        [InlineData(3, 0, false, true)] // Equal upper bound
        [InlineData(3, 1, false, true)] // After upper bound
        // After 48h but even day
        [InlineData(4, -1, false)] // Before lower bound
        [InlineData(4, 0, true)] // Equal lower bound
        [InlineData(4, 1, true)] // After lower bound
        [InlineData(4, -1, true, true)] // Before upper bound
        [InlineData(4, 0, true, true)] // Equal upper bound
        [InlineData(4, 1, false, true)] // After upper bound
        // Older than 14 days
        [InlineData(14, -1, false)] // Before lower bound
        [InlineData(14, 0, false)] // Equal lower bound
        [InlineData(14, 1, false)] // After lower bound
        [InlineData(14, -1, false, true)] // Before upper bound
        [InlineData(14, 0, false, true)] // Equal upper bound
        [InlineData(14, 1, false, true)] // After upper bound
        public async void ShouldUpdateLastMessageDate(
            int daysOfTimeShift,
            int minutesOfTimeShift,
            bool shouldBeCalled = false,
            bool isUpperBound = false)
        {
            ResetData();

            SystemTime.SetDateTime(
                DateTime.Now.Date.AddHours(Conf.HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND_BEGIN - 1).ToUniversalTime());
            await ServiceLocator.Current.GetInstance<IMessagesManager>().SaveNewMessage(new MessageSQLiteModel()
            {
                TimeStamp = SystemTime.Now(),
                ID = 1,
                MessageLink = "",
                Title = ""
            });
            LocalNotificationsManager.GenerateLocalNotification(new NotificationViewModel(), 0);
            LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NewMessageReceived] = false;

            DateTime preFetchDateTime = DateTime.MaxValue;

            int initialDay =
                daysOfTimeShift > 1 ? daysOfTimeShift - 2 : 0; // To fasten up the execution time
            // Simulates pulling on each day
            for (int i = initialDay; i <= daysOfTimeShift; i++)
            {
                if (i == daysOfTimeShift)
                {
                    LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NewMessageReceived] = false;
                }

                preFetchDateTime =
                    MessageUtils.GetDateTimeFromSecureStorageForKey(
                        SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY, "");
                SystemTime.SetDateTime(
                    DateTime.Now.Date.AddDays(i)
                        .AddHours(isUpperBound
                            ? Conf.HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND_END
                            : Conf.HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND_BEGIN).AddMinutes(minutesOfTimeShift)
                        .ToUniversalTime());

                try
                {
                    await new FetchExposureKeysHelper().FetchExposureKeyBatchFilesFromServerAsync(null,
                        CancellationToken.None);
                }
                catch
                {
                    // ignore as ZipDownloader is not mocked in this test
                }
            }

            Assert.Equal(shouldBeCalled,
                preFetchDateTime <
                MessageUtils.GetDateTimeFromSecureStorageForKey(SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY, ""));
            Assert.Equal(shouldBeCalled, LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NewMessageReceived]);

            SystemTime.ResetDateTime();
        }

        private void ResetData()
        {
            SystemTime.ResetDateTime();
            MessageUtils.RemoveAll();
            Permissions.BluetoothEnabled = true;
            Permissions.LocationEnabled = true;
            LocalNotificationsManager.ResetHasBeenCalledMap();
            foreach (string key in SecureStorageKeys.GetAllKeysForCleaningDevice())
            {
                _secureStorageService.Delete(key);
            }
        }

        public void Dispose()
        {
            ResetData();
        }
    }
}
using System;
using System.Threading;
using CommonServiceLocator;
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

        private static SecureStorageService _secureStorageService => ServiceLocator.Current.GetInstance<SecureStorageService>();
        private static PermissionsMock Permissions => (PermissionsMock) ServiceLocator.Current.GetInstance<IPermissionsHelper>();

        public ResendNotificationTests()
        {
            DependencyInjectionConfig.Init();
            _secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        [Theory]
        // Today
        [InlineData(0, -1, false)] // Before 9PM
        [InlineData(0, 1, false)] // After 9PM
        [InlineData(0, 0, false)] // Equal 9PM
        // Next day
        [InlineData(1, -1, false)] // Before 9PM
        [InlineData(1, 1, false)] // After 9PM
        [InlineData(1, 0, false)] // Equal 9PM
        // After 48 hours
        [InlineData(2, -1, false)] // Before 9PM
        [InlineData(2, 1, true)] // After 9PM
        [InlineData(2, 0, true)] // Equal 9PM
        [InlineData(3, -1, false)] // Before 9PM
        [InlineData(3, 1, true)] // After 9PM
        [InlineData(3, 0, true)] // Equal 9PM
        // Older than 14 days
        [InlineData(14, -1, false)] // Before 9PM
        [InlineData(14, 1, false)] // After 9PM
        [InlineData(14, 0, false)] // Equal 9PM
        public async void ShouldUpdateLastMessageDate(int daysOfTimeShift, int minutesOfTimeShift, bool shouldBeCalled)
        {
            ResetData();

            SystemTime.SetDateTime(DateTime.Now.Date.AddHours(12).ToUniversalTime());
            await ServiceLocator.Current.GetInstance<IMessagesManager>().SaveNewMessage(new MessageSQLiteModel()
            {
                TimeStamp = SystemTime.Now(),
                ID = 1,
                MessageLink = "",
                Title = ""
            });
            LocalNotificationsManager.GenerateLocalNotification(new NotificationViewModel(), 0);
            LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NewMessageReceived] = false;

            DateTime preFetchDateTime = MessageUtils.GetDateTimeFromSecureStorageForKey(SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY, "");
            SystemTime.SetDateTime(DateTime.Now.Date.AddDays(daysOfTimeShift).AddHours(21).AddMinutes(minutesOfTimeShift).ToUniversalTime());

            try
            {
                await new FetchExposureKeysHelper().FetchExposureKeyBatchFilesFromServerAsync(null, CancellationToken.None);
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }

            Assert.Equal(shouldBeCalled, preFetchDateTime < MessageUtils.GetDateTimeFromSecureStorageForKey(SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY, ""));
            Assert.Equal(shouldBeCalled, LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NewMessageReceived]);

            SystemTime.ResetDateTime();
        }

        private void ResetData()
        {
            SystemTime.ResetDateTime();
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

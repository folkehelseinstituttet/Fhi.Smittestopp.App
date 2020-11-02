using NDB.Covid19.ExposureNotification.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Test.Mocks
{
    class LocalNotificationManagerMock: ILocalNotificationsManager
    {
        public bool HasBeenCalled { get; set; }
        public void GenerateLocalNotification(NotificationViewModel notificationViewModel, int triggerInSeconds)
        {
            HasBeenCalled = true;
            MessageUtils.SaveDateTimeToSecureStorageForKey(
                SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY,
                SystemTime.Now(),
                "Unit test GenerateLocalNotification");
        }

        public void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel)
        {
            GenerateLocalNotification(viewModel, 0);
        }
    }
}

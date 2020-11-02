using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Interfaces
{
    public interface ILocalNotificationsManager
    {
        void GenerateLocalNotification(NotificationViewModel notificationViewModel, int triggerInSeconds);
        void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel);
    }
}

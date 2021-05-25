using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Interfaces
{
    public interface ILocalNotificationsManager
    {
        void GenerateLocalNotification(NotificationViewModel notificationViewModel, long triggerInSeconds);
        void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel);
        void GenerateLocalPermissionsNotification(NotificationViewModel viewModel);
        void GenerateDelayedNotification(NotificationViewModel viewModel, long ticks);
    }
}
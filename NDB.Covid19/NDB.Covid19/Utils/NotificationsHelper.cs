using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Utils
{
    public class NotificationsHelper
    {
        public static readonly ILocalNotificationsManager LocalNotificationsManager =
            ServiceLocator.Current.GetInstance<ILocalNotificationsManager>();

        public static void CreateNotification(NotificationsEnum notificationType, int triggerInSeconds)
        {
            LocalNotificationsManager.GenerateLocalNotification(notificationType.Data(), triggerInSeconds);
        }

        public static void CreateNotificationOnlyIfInBackground(NotificationsEnum notificationType)
        {
            LocalNotificationsManager.GenerateLocalNotificationOnlyIfInBackground(notificationType.Data());
        }
    }
}

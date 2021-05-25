using System.Threading.Tasks;
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using NDB.Covid19.ExposureNotifications.Helpers;

namespace NDB.Covid19.Droid.Services
{
    [BroadcastReceiver]
    [IntentFilter(new[]
    {
        "no.fhi.smittestopp_exposure_notification.background_notification"
    })]
    public class BackgroundNotificationBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationType type = (NotificationType) intent.GetIntExtra("type", 0);
            NotificationsEnum data = (NotificationsEnum) intent.GetIntExtra("data", 0);
            switch (type)
            {
                case NotificationType.Local:
                    GenerateLocalNotificationBroadcasted(context, data.Data(), 0);
                    break;
                case NotificationType.Permissions:
                    GenerateLocalPermissionsNotificationBroadcasted(context, data.Data());
                    break;
                case NotificationType.InBackground:
                    GenerateLocalNotificationOnlyIfInBackgroundBroadcasted(context, data.Data());
                    break;
                case NotificationType.ForegroundWithUpdates:
                    long ticks = intent.GetLongExtra("ticks", 0);
                    GenerateForegroundServiceNotificationWithUpdatesBroadcasted(
                        context, data.Data(), ticks);
                    break;
                default:
                    return;
            }
        }

        private void GenerateLocalPermissionsNotificationBroadcasted(Context context, NotificationViewModel viewModel)
        {
            NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(context);
            notificationManagerCompat.Notify(
                    (int) viewModel.Type,
                new LocalNotificationsManager(context)
                    .CreateNotification(viewModel));
        }

        private void GenerateLocalNotificationOnlyIfInBackgroundBroadcasted(Context context,
            NotificationViewModel viewModel)
        {
            ActivityManager.RunningAppProcessInfo myProcess = new ActivityManager.RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(myProcess);
            bool isInBackground = myProcess.Importance != Importance.Foreground;

            if (isInBackground)
            {
                new LocalNotificationsManager(context).GenerateLocalNotification(viewModel, 0);
                LocalPreferencesHelper.LastDateTimeTermsNotificationWasShown = SystemTime.Now();
            }
        }

        private void GenerateLocalNotificationBroadcasted(Context context, NotificationViewModel notificationViewModel,
            int triggerInSeconds)
        {
            Task.Run(async () =>
            {
                NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(context);
                await Task.Delay(triggerInSeconds * 1000);
                notificationManagerCompat.Notify(
                    (int) notificationViewModel.Type,
                    new LocalNotificationsManager(context)
                        .CreateNotification(notificationViewModel));
            });
        }
        private void GenerateForegroundServiceNotificationWithUpdatesBroadcasted(
            Context context, NotificationViewModel notificationViewModel, long ticksLeft)
        {
            Task.Run(() =>
            {
                NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(context);
                notificationManagerCompat.Notify(
                    (int)notificationViewModel.Type,
                    new LocalNotificationsManager(context)
                        .CreateNotificationWithExtraLongData(notificationViewModel, ticksLeft));
            });
        }
    }
}
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Java.Lang;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Views.Messages;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using XamarinShortcutBadger;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

namespace NDB.Covid19.Droid.Utils
{
    public class LocalNotificationsManager : ILocalNotificationsManager
    {
        public const int NotificationId = 616;
        private readonly string _channelId = "Local_Notifications";
        private NotificationChannel _channel;
        public LocalNotificationsManager()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }
            CreateChannel();
        }

        private void CreateChannel()
        {
            string channelName = Current.Activity.Resources.GetString(Resource.String.channel_name);
            string channelDescription = Current.Activity.Resources.GetString(Resource.String.channel_description);
            NotificationImportance importance = NotificationImportance.High;

            if (_channel == null)
            {
                _channel = new NotificationChannel(_channelId, channelName, importance)
                {
                    Description = channelDescription
                };

                _channel.SetShowBadge(true);

                NotificationManager notificationManager =
                    (NotificationManager) Current.Activity.GetSystemService(Context.NotificationService);
                notificationManager?.CreateNotificationChannel(_channel);
            }
        }

        public void GenerateLocalNotification(NotificationViewModel notificationViewModel, int triggerInSeconds)
        {
            Current.Activity.RunOnUiThread(async () =>
            {
                NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(Current.Activity);
                await Task.Delay(triggerInSeconds * 1000);
                notificationManagerCompat.Notify(NotificationId, await CreateNotification(notificationViewModel));
            });
        }

        public async Task<Notification> CreateNotification(NotificationViewModel notificationViewModel)
        {
            PendingIntent resultPendingIntent = InitResultIntentBasingOnViewModel(notificationViewModel);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Current.Activity, _channelId)
                .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                .SetContentTitle(notificationViewModel.Title) // Set the title
                .SetContentText(notificationViewModel.Body) // the message to display.
                .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                .SetVibrate(null)
                .SetSound(null)
                .SetNumber(1)
                .SetCategory(NotificationCompat.CategoryMessage)
                .SetOnlyAlertOnce(true);

            // This is the icon to display
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                builder.SetColor(Resource.Color.colorPrimary);
            }

            builder.SetSmallIcon(Resource.Drawable.ic_smittestop);

            Notification notification = builder.Build();

            bool isLowerVersion = Build.VERSION.SdkInt < BuildVersionCodes.O;
            bool isBadgeCounterSupported = ShortcutBadger.IsBadgeCounterSupported(Current.AppContext);
            bool isMessage = notificationViewModel.Type == NotificationsEnum.NewMessageReceived;
            bool areNotificationsEnabled = NotificationManagerCompat.From(Current.AppContext).AreNotificationsEnabled();

            // Use Plugin for badges on older platforms that support them
            if (isLowerVersion &&
                isBadgeCounterSupported &&
                isMessage &&
                areNotificationsEnabled)
            {
                ShortcutBadger.ApplyNotification(Current.AppContext, notification, 1);
            }

            return notification;
        }

        public void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel)
        {
            ActivityManager.RunningAppProcessInfo myProcess = new ActivityManager.RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(myProcess);
            bool isInBackground = myProcess.Importance != Importance.Foreground;

            if (isInBackground)
            {
                new LocalNotificationsManager().GenerateLocalNotification(viewModel, 0);
                LocalPreferencesHelper.TermsNotificationWasShown = true;
            }
        }

        private PendingIntent InitResultIntentBasingOnViewModel(NotificationViewModel notificationViewModel)
        {
            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent;

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(Current.Activity);

            if (notificationViewModel.Type == NotificationsEnum.NewMessageReceived.Data().Type)
            {
                resultIntent = new Intent(Current.Activity, typeof(MessagesActivity));
                stackBuilder.AddParentStack(Class.FromType(typeof(MessagesActivity)));
            }
            else
            {
                resultIntent = new Intent(Current.Activity, typeof(InitializerActivity));
                stackBuilder.AddParentStack(Class.FromType(typeof(InitializerActivity)));
            }

            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            return stackBuilder.GetPendingIntent(0, (int) PendingIntentFlags.UpdateCurrent);
        }
    }
}
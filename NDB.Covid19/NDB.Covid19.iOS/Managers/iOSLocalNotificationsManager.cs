using System;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.ExposureNotifications.Helpers;
using UIKit;
using UserNotifications;

namespace NDB.Covid19.iOS.Managers
{
    public class iOSLocalNotificationsManager : UNUserNotificationCenterDelegate, ILocalNotificationsManager
    {
        public bool NotificationHasBeenTapped { get; set; }
        public EventHandler<string> OnNotificationTappedHandler { get; set; }
        public static string NewMessageIdentifier { get; } = "newMessageNotification";
        public static string NewNotificationIdentifier { get; } = "newNotification";

        const int _notificationDelay = 1;

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            if (notification.Request.Identifier == NewMessageIdentifier)
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 1;
            }

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        // If we hit this, it means that the app has been styarted from a notification
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center,
            UNNotificationResponse response, Action completionHandler)
        {
            NotificationHasBeenTapped = true;
            OnNotificationTappedHandler?.Invoke(this, response.Notification.Request.Identifier);
        }

        public void GenerateLocalNotification(NotificationViewModel notificationViewModel, long triggerInSeconds)
        {
            CreateLocalNotification(notificationViewModel, triggerInSeconds);
        }

        public void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel)
        {
            if (AppDelegate.ShouldOperateIn12_5Mode)
            {
                if (!AppDelegate.DidEnterBackgroundState &&
                    UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                {
                    return;
                }
            }
            else
            {
                if (!SceneDelegate.DidEnterBackgroundState &&
                    UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                {
                    return;
                }
            }
            GenerateLocalNotification(viewModel, 0);
            LocalPreferencesHelper.LastDateTimeTermsNotificationWasShown = SystemTime.Now();
        }

        private void CreateLocalNotification(NotificationViewModel notificationViewModel, double timeIntervalTrigger)
        {
            InvokeOnMainThread(() =>
            {
                string requestID =
                    notificationViewModel.Type == NotificationsEnum.NewMessageReceived
                        ? NewMessageIdentifier
                        : NewNotificationIdentifier;
                if (notificationViewModel.Type == NotificationsEnum.TimedReminderFinished)
                {
                    requestID = notificationViewModel.Type.ToString();
                }
                // For already delivered Notifications, the existing Notification will get updated and promoted to the top
                // of the list on the Home and Lock screens and in the Notification Center if it has already been read by the user.

                UNUserNotificationCenter.Current.GetNotificationSettings(settings =>
                {
                    bool alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);

                    if (alertsAllowed)
                    {
                        UNMutableNotificationContent content = new UNMutableNotificationContent
                        {
                            Title = notificationViewModel.Title,
                            Body = notificationViewModel.Body,
                            Badge = 1
                        };

                        UNTimeIntervalNotificationTrigger trigger =
                            UNTimeIntervalNotificationTrigger.CreateTrigger(
                                timeIntervalTrigger == 0 ? _notificationDelay : timeIntervalTrigger, false);
                        UNNotificationRequest request =
                            UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                        UNUserNotificationCenter.Current.AddNotificationRequest(request, err =>
                        {
                            if (err != null)
                            {
                                NSErrorException e = new NSErrorException(err);
                                LogUtils.LogException(LogSeverity.ERROR, e,
                                    $"{nameof(iOSLocalNotificationsManager)}.{nameof(CreateLocalNotification)} failed");
                            }
                        });
                    }
                });
            });
        }

        public void GenerateLocalPermissionsNotification(NotificationViewModel viewModel)
        {
            GenerateLocalNotification(viewModel, 0);
        }
        public void GenerateDelayedNotification(NotificationViewModel viewModel, long ticks)
        {
            GenerateLocalNotification(viewModel, ticks);
        }
    }
}
using System;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Utils
{
    public class NotificationsHelper
    {
        public static readonly ILocalNotificationsManager LocalNotificationsManager =
            ServiceLocator.Current.GetInstance<ILocalNotificationsManager>();

        public static readonly IPermissionsHelper PermissionsHelper =
            ServiceLocator.Current.GetInstance<IPermissionsHelper>();

        public static void CreateNotification(NotificationsEnum notificationType, int triggerInSeconds)
        {
            LocalNotificationsManager.GenerateLocalNotification(notificationType.Data(), triggerInSeconds);
        }

        public static void CreateNotificationOnlyIfInBackground(NotificationsEnum notificationType)
        {
            LocalNotificationsManager.GenerateLocalNotificationOnlyIfInBackground(notificationType.Data());
        }

        public static void CreatePermissionsNotification()
        {
            if (!PermissionsHelper.AreAllPermissionsGranted())
            {
                DateTime now = SystemTime.Now();
                if (LocalPreferencesHelper.LastPermissionsNotificationDateTimeUtc.Date < now.Date)
                {
                    bool hasBluetooth = PermissionsHelper.IsBluetoothEnabled();
                    bool hasLocation = PermissionsHelper.IsLocationEnabled();

                    NotificationViewModel viewModel;
                    if (!hasBluetooth && !hasLocation)
                    {
                        viewModel = NotificationsEnum.BluetoothAndLocationOff.Data();
                    } else if (!hasBluetooth)
                    {
                        viewModel = NotificationsEnum.BluetoothOff.Data();
                    }
                    else if (!hasLocation)
                    {
                        viewModel = NotificationsEnum.LocationOff.Data();
                    }
                    else
                    {
                        return;
                    }
                    LocalNotificationsManager.GenerateLocalNotification(viewModel, 0);
                    LocalPreferencesHelper.LastPermissionsNotificationDateTimeUtc = now.Date;
                }
            }
        }
    }
}

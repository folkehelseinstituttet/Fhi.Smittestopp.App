﻿using System.Collections.Generic;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Test.Mocks
{
    class LocalNotificationManagerMock: ILocalNotificationsManager
    {
        public Dictionary<NotificationsEnum, bool> HasBeenCalled { get; set; } = new Dictionary<NotificationsEnum, bool>();

        public void GenerateLocalNotification(NotificationViewModel notificationViewModel, int triggerInSeconds)
        {
            HasBeenCalled[notificationViewModel.Type] = true;
            if (notificationViewModel.Type == NotificationsEnum.NewMessageReceived)
            {
                MessageUtils.SaveDateTimeToSecureStorageForKey(
                    SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY,
                    SystemTime.Now(),
                    "Unit test GenerateLocalNotification");
            }
        }

        public void GenerateLocalNotificationOnlyIfInBackground(NotificationViewModel viewModel)
        {
            GenerateLocalNotification(viewModel, 0);
        }

        public void GenerateLocalPermissionsNotification(NotificationViewModel viewModel)
        {
            HasBeenCalled[viewModel.Type] = true;
        }

        public void ResetHasBeenCalledMap()
        {
            HasBeenCalled.Clear();
        }
    }
}

using System;
using System.Threading;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class PermissionsTests : IDisposable
    {
        private static readonly PermissionsMock PermissionsHelper = (PermissionsMock) ServiceLocator.Current.GetInstance<IPermissionsHelper>();
        private static readonly LocalNotificationManagerMock LocalNotificationsManager =
            (LocalNotificationManagerMock) NotificationsHelper.LocalNotificationsManager;

        public PermissionsTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Theory]
        [InlineData(true, true, NotificationsEnum.NoNotification)]
        [InlineData(true, false, NotificationsEnum.LocationOff)]
        [InlineData(false, true, NotificationsEnum.BluetoothOff)]
        [InlineData(false, false, NotificationsEnum.BluetoothAndLocationOff)]
        public void PermissionsNotificationIsGenerated(bool hasBluetooth, bool hasLocation, NotificationsEnum type)
        {
            ResetData();
            PermissionsHelper.BluetoothEnabled = hasBluetooth;
            PermissionsHelper.LocationEnabled = hasLocation;
            
            LocalNotificationsManager.HasBeenCalled[type] = false;
            LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NoNotification] = false;
            try
            {
                new FetchExposureKeysHelper()
                    .FetchExposureKeyBatchFilesFromServerAsync(
                        null,
                        CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.True(
                !LocalNotificationsManager.HasBeenCalled[NotificationsEnum.NoNotification] ||
                LocalNotificationsManager.HasBeenCalled[type]);
        }

        private void ResetData()
        {
            SystemTime.ResetDateTime();
            PermissionsHelper.BluetoothEnabled = true;
            PermissionsHelper.LocationEnabled = true;
            LocalNotificationsManager.ResetHasBeenCalledMap();
        }

        public void Dispose()
        {
            ResetData();
        }
    }
}

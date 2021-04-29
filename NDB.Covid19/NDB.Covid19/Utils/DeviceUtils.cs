using System;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils.DeveloperTools;
using NDB.Covid19.WebServices.Utils;
using Xamarin.Essentials;

namespace NDB.Covid19.Utils
{
    public static class DeviceUtils
    {
        public static void CleanDataFromDevice()
        {
            // Clear the stored data in the singleton
            ServiceLocator.Current.GetInstance<IDeveloperToolsService>().ClearAllFields();
            // Needed to reset authorization header
            HttpClientManager.MakeNewInstance();
            ServiceLocator.Current.GetInstance<IPreferences>().Clear();
            MessageUtils.RemoveAll();
            OnboardingStatusHelper.Status = OnboardingStatus.NoConsentsGiven;

            foreach (string key in SecureStorageKeys.GetAllKeysForCleaningDevice())
            {
                ServiceLocator.Current.GetInstance<SecureStorageService>().Delete(key);
            }
        }

        public static async void StopScanServices()
        {
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.StopAsync();
            }
            catch (Exception e)
            {
                // To make it not crash on devices with normal Play Services before the app is whitelisted
                if (!e.HandleExposureNotificationException(nameof(DeviceUtils), nameof(StopScanServices)))
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }

        public static string DeviceModel
        {
            get
            {
                IDeviceInfo deviceInfo = ServiceLocator.Current.GetInstance<IDeviceInfo>();
                return ServiceLocator.Current.GetInstance<IDeviceInfo>().Platform == DevicePlatform.Android
                    ? deviceInfo.Model
                    : IOSHardwareMapper.GetModel(deviceInfo.Model);
            }
        }

        public static string DeviceType => ServiceLocator.Current.GetInstance<IDeviceInfo>().Platform == DevicePlatform.Android
            ? "Android-Google" //Only Android devices that support Google play services are supported for now.
            : "IOS";
    }
}
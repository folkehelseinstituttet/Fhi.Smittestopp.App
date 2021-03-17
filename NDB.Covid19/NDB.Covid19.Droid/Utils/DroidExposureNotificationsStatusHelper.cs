using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Nearby;
using Android.Gms.Nearby.ExposureNotification;
namespace NDB.Covid19.Droid.Utils
{
    public static class DroidExposureNotificationsStatusHelper
    {
        private static IExposureNotificationClient Instance =>
            NearbyClass.GetExposureNotificationClient(Application.Context);
        public static async Task<bool> IsNearbyExposureNotificationBluetoothAndLocationDisabled()
        {
            try
            {
                return (await Instance.NativeGetStatus().CastTask())
                    .ToArray()
                    .ToList()
                    .Any(s =>
                        s == ExposureNotificationStatus.BluetoothDisabled ||
                        s == ExposureNotificationStatus.LocationDisabled ||
                        s == ExposureNotificationStatus.BluetoothSupportUnknown);
            }
            catch (Exception)
            {
                return true;
            }
        }
        
        public static async Task<bool> HasNearbyExposureNotificationStatus(ExposureNotificationStatus status)
        {
            try
            {
                return (await Instance.NativeGetStatus().CastTask())
                    .ToArray()
                    .ToList()
                    .Any(s =>
                        s == status);
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
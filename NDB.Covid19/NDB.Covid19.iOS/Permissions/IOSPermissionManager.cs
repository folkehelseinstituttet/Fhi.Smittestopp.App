using System.Threading.Tasks;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.iOS.Permissions
{
    public class IOSPermissionManager { 
        
        public async Task<bool> PermissionUnknown()
        {
            return await ExposureNotification.GetStatusAsync() == Status.Unknown;
        }

        public async Task<bool> IsBluetoothOff()
        {
            return await ExposureNotification.GetStatusAsync() == Status.BluetoothOff;
        }

        public async Task<bool> IsENActive()
        {
            return await Xamarin.ExposureNotifications.ExposureNotification.GetStatusAsync() == Status.Active;
        }
        
        /// <summary>
        /// Returns true if Status.Active || status == Status.Disabled
        /// Meaning that everything is ready for either starting or stopping the scanner.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PoweredOn()
        {
            Status status = await ExposureNotification.GetStatusAsync();
            return status == Status.Active || status == Status.Disabled; 
        }
    }
}

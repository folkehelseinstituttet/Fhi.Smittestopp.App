using NDB.Covid19.Interfaces;

namespace NDB.Covid19.iOS.Permissions
{
    class IOSPermissionsHelper : IPermissionsHelper
    {
        public bool IsBluetoothEnabled()
        {
            return !(new IOSPermissionManager().PoweredOff().GetAwaiter().GetResult());
        }

        public bool IsLocationEnabled()
        {
            // not required on iOS
            return true;
        }

        public bool AreAllPermissionsGranted()
        {
            return IsLocationEnabled() && IsBluetoothEnabled();
        }
    }
}
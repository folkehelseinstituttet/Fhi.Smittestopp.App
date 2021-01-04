using NDB.Covid19.Interfaces;

namespace NDB.Covid19.iOS.Permissions
{
    class IOSPermissionsHelper : IPermissionsHelper
    {
        public bool IsBluetoothEnabled()
        {
            IOSPermissionManager iosPermissionManager = new IOSPermissionManager();
            // On iOS there is no clear indication if bluetooth is ON (the opposite of
            // BLUETOOTH_OFF state is not BLUETOOTH_ON) so we assume
            // that every state other than Active indicates no permissions.
            return iosPermissionManager.IsENActive().GetAwaiter().GetResult();
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
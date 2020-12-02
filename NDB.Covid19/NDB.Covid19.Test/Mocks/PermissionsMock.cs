using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Test.Mocks
{
    class PermissionsMock : IPermissionsHelper
    {
        public bool BluetoothEnabled { private get; set; }
        public bool LocationEnabled { private get; set; }
        public bool AllPermissionsGranted => BluetoothEnabled && LocationEnabled;

        public bool IsBluetoothEnabled() => BluetoothEnabled;

        public bool IsLocationEnabled() => LocationEnabled;

        public bool AreAllPermissionsGranted() => AllPermissionsGranted;
    }
}

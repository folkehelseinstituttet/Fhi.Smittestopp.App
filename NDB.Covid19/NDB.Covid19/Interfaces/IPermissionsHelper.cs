namespace NDB.Covid19.Interfaces
{
    public interface IPermissionsHelper
    {
        bool IsBluetoothEnabled();
        bool IsLocationEnabled();
        bool AreAllPermissionsGranted();
    }
}

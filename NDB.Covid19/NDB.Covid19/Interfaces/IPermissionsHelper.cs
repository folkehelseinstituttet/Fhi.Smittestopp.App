using System.Threading.Tasks;

namespace NDB.Covid19.Interfaces
{
    public interface IPermissionsHelper
    {
        Task<bool> IsBluetoothEnabled();
        Task<bool> IsLocationEnabled();
        Task<bool> AreAllPermissionsGranted();
    }
}

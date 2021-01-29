using System;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Provider;
using I18NPortable;
using NDB.Covid19.Droid.Utils.MessagingCenter;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.DroidRequestCodes;
using static Plugin.CurrentActivity.CrossCurrentActivity;

namespace NDB.Covid19.Droid.Utils
{
    public class PermissionUtils : IPermissionsHelper
    {
        private TaskCompletionSource<bool> _bluetoothTCS = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _locationTCS = new TaskCompletionSource<bool>();

        public async Task<bool> HasPermissions()
        {
            bool hasBluetoothSupport = await CheckBluetoothSupport();
            bool hasLocationSupport = await CheckLocationSupport();
            
            PermissionsMessagingCenter.PermissionsChanged = false;
            
            return hasBluetoothSupport && hasLocationSupport;
        }

        private async Task<bool> CheckBluetoothSupport()
        {
            _bluetoothTCS = new TaskCompletionSource<bool>();
            
            if (IsBluetoothEnabled())
            {
                _bluetoothTCS.TrySetResult(true);
            }
            else
            {
                await HasBluetoothSupportAsync();
            }

            return await _bluetoothTCS.Task;
        }
        
        private async Task<bool> CheckLocationSupport()
        {
            _locationTCS = new TaskCompletionSource<bool>();
            
            if (IsLocationEnabled())
            {
                _locationTCS.TrySetResult(true);
            }
            else
            {
                await HasLocationPermissionsAsync();
            }

            return await _locationTCS.Task;
        }
        
        public async Task<bool> CheckPermissionsIfChangedWhileIdle()
        {
            if (PermissionsMessagingCenter.PermissionsChanged)
            {
                return await HasPermissions();
            }

            return true;
        }

        public void SubscribePermissionsMessagingCenter(object subscriber, Action<object> action)
        {
            PermissionsMessagingCenter.SubscribeForPermissionsChanged(subscriber, action);
        }

        public void UnsubscribePermissionsMessagingCenter(object subscriber)
        {
            PermissionsMessagingCenter.Unsubscribe(subscriber);
        }

        public bool HasPermissionsWithoutDialogs() => IsBluetoothEnabled() && IsLocationEnabled();

        private async Task HasBluetoothSupportAsync()
        {
            if (await HasBluetoothAdapter() && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                return;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel()
                {
                    Title = "PERMISSION_BLUETOOTH_NEEDED_TITLE".Translate(),
                    Body = "PERMISSION_ENABLE_LOCATION_AND_BLUETOOTH".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok),
                    CancelbtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Cancel)
                },
                GoToBluetoothSettings,
                CancelTask);
        }

        private async Task<bool> HasBluetoothAdapter()
        {
            if (BluetoothAdapter.DefaultAdapter != null)
            {
                return true;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel()
                {
                    Title = "NO_BLUETOOTH_TITLE".Translate(),
                    Body = "NO_BLUETOOTH_MSG".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok)
                });

            return false;
        }

        private async Task HasLocationPermissionsAsync()
        {
            if (IsLocationEnabled())
            {
                return;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel
                {
                    Title = "PERMISSION_LOCATION_NEEDED_TITLE".Translate(),
                    Body = "PERMISSION_ENABLE_LOCATION_AND_BLUETOOTH".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok)
                },
                GoToLocationSettings);
        }

        public bool IsBluetoothEnabled()
        {
            return BluetoothAdapter.DefaultAdapter?.IsEnabled == true;
        }

        public bool IsLocationEnabled()
        {
            if ((int) Build.VERSION.SdkInt >= 30)
            {
                // Location is not required on Android 11 and above
                return true;
            }
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                LocationManager lm = (LocationManager) Current.AppContext.GetSystemService(Context.LocationService);
                return lm.IsLocationEnabled;
            }

            int mode = Settings.Secure.GetInt(Current.AppContext.ContentResolver, Settings.Secure.LocationMode,
                (int) SecurityLocationMode.Off);
            return (mode != (int) SecurityLocationMode.Off);
        }

        public bool AreAllPermissionsGranted()
        {
            return HasPermissionsWithoutDialogs();
        }

        private void CancelTask()
        {
            _bluetoothTCS.TrySetResult(false);
        }

        private void GoToBluetoothSettings()
        {
            try
            {
                Current.Activity.StartActivityForResult(new Intent().SetAction(Settings.ActionBluetoothSettings),
                    BluetoothRequestCode);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e,
                    $"{nameof(PermissionUtils)}.{nameof(GoToBluetoothSettings)}: Failed to go to bluetooth settings. Trying other intent.");
                try
                {
                    // This is needed for some Samsung devices as the previous solution
                    // requires BLUETOOTH_ADMIN permissions and we do not want to use them.
                    Intent intent = new Intent(Intent.ActionMain, null);
                    intent.AddCategory(Intent.CategoryLauncher);
                    ComponentName cn = new ComponentName(
                        "com.android.settings",
                        "com.android.settings.bluetooth.BluetoothSettings");
                    intent.SetComponent(cn);
                    intent.SetFlags(ActivityFlags.NewTask);
                    Current.AppContext.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    LogUtils.LogException(LogSeverity.WARNING, ex,
                        $"{nameof(PermissionUtils)}.{nameof(GoToBluetoothSettings)}: Failed to go to bluetooth settings. Skipping.");
                }
            }
        }

        private void GoToLocationSettings()
        {
            try
            {
                Current.Activity.StartActivityForResult(new Intent().SetAction(Settings.ActionLocationSourceSettings),
                    LocationRequestCode);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e, "GoToLocationSettings");
            }
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case BluetoothRequestCode when resultCode != Result.FirstUser:
                    _bluetoothTCS.TrySetResult(IsBluetoothEnabled());
                    break;
                case LocationRequestCode when resultCode != Result.FirstUser:
                    _locationTCS.TrySetResult(IsLocationEnabled());
                    break;
            }
        }
    }
}
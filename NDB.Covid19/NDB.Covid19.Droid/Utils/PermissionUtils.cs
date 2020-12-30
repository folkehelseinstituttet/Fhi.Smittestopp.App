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
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public async Task<bool> HasPermissions()
        {
            _tcs = new TaskCompletionSource<bool>();
            bool hasLocationPermissions = await HasLocationPermissionsAsync();
            bool hasBluetoothSupport = await HasBluetoothSupportAsync();
            if (hasLocationPermissions && hasBluetoothSupport)
            {
                _tcs.TrySetResult(true);
            }

            bool finalResult = await _tcs.Task;
            PermissionsMessagingCenter.PermissionsChanged = false;
            return finalResult;
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

        public void UnsubscribePErmissionsMessagingCenter(object subscriber)
        {
            PermissionsMessagingCenter.Unsubscribe(subscriber);
        }

        public bool HasPermissionsWithoutDialogs() => IsBluetoothEnabled() && IsLocationEnabled();

        public async Task<bool> HasBluetoothSupportAsync()
        {
            if ((await HasBluetoothAdapter()) && BluetoothAdapter.DefaultAdapter.IsEnabled)
            {
                return true;
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
            return false;
        }

        public async Task<bool> HasBluetoothAdapter()
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

        public async Task<bool> HasLocationPermissionsAsync()
        {
            if (IsLocationEnabled())
            {
                return true;
            }

            await DialogUtils.DisplayDialogAsync(
                Current.Activity,
                new DialogViewModel()
                {
                    Title = "PERMISSION_LOCATION_NEEDED_TITLE".Translate(),
                    Body = "PERMISSION_ENABLE_LOCATION_AND_BLUETOOTH".Translate(),
                    OkBtnTxt = Current.Activity.Resources.GetString(Android.Resource.String.Ok)
                },
                GoToLocationSettings);

            return false;
        }

        public bool IsBluetoothEnabled()
        {
            return BluetoothAdapter.DefaultAdapter?.IsEnabled == true;
        }

        public bool IsLocationEnabled()
        {
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
            _tcs.TrySetResult(false);
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
                LogUtils.LogException(Enums.LogSeverity.WARNING, e, "GoToLocationSettings");
            }
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == BluetoothRequestCode || requestCode == LocationRequestCode) &&
                resultCode != Result.FirstUser)
            {
                _tcs.TrySetResult(HasPermissionsWithoutDialogs());
            }
        }
    }
}
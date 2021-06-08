using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Locations;
using CommonServiceLocator;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Utils.MessagingCenter;

namespace NDB.Covid19.Droid.Services
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] {"android.bluetooth.adapter.action.STATE_CHANGED", "android.location.PROVIDERS_CHANGED"})]
    class PermissionsBroadcastReceiver : BroadcastReceiver
    {
        private readonly PermissionUtils _permissionsUtils = ServiceLocator.Current.GetInstance<PermissionUtils>();

        public override void OnReceive(Context context, Intent intent)
        {
            if (BluetoothAdapter.DefaultAdapter == null) return;

            var intentAction = intent.Action;
            if (intentAction.Equals(LocationManager.ProvidersChangedAction)
                || (intentAction.Equals(BluetoothAdapter.ActionStateChanged)
                    && intent.GetIntExtra(BluetoothAdapter.ExtraState, BluetoothAdapter.Error) == (int) State.Off))
            {
                NotifyAboutPermissionsChange();
            }
        }

        private async void NotifyAboutPermissionsChange()
        {
            bool isEnabled;

            try
            {
                isEnabled = await Xamarin.ExposureNotifications.ExposureNotification.IsEnabledAsync();
            }
            catch (Exception e)
            {
                // To make it not crash on devices with normal Play Services before the app is whitelisted
                if (e.HandleExposureNotificationException(nameof(PermissionsBroadcastReceiver),
                    nameof(NotifyAboutPermissionsChange)))
                {
                    isEnabled = false;
                }
                else
                {
#if DEBUG
                    throw;
#endif
                }

                isEnabled = false;
            }

            if (!_permissionsUtils.HasPermissionsWithoutDialogs() && isEnabled)
            {
                PermissionsMessagingCenter.NotifyPermissionsChanged(this);
            }
        }
    }
}
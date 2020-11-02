using System;
using Android.App;
using Android.Content;

namespace NDB.Covid19.Droid.Services
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] {"android.intent.action.AIRPLANE_MODE"})]
    class FlightModeHandlerBroadcastReceiver : BroadcastReceiver
    {
        public static bool IsFlightModeOn { get; set; }
        public static Action OnFlightModeChange { get; set; } = null;

        public override async void OnReceive(Context context, Intent intent)
        {
            IsFlightModeOn = intent.GetBooleanExtra("state", false);
            // Wait until BT will be turned on or off
            await BluetoothStateBroadcastReceiver.GetBluetoothState(OnFlightModeChange);
            OnFlightModeChange?.Invoke();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Locations;
using Plugin.CurrentActivity;

namespace NDB.Covid19.Droid.Services
{
    public enum BluetoothState
    {
        NO_ADAPTER,
        ON,
        OFF
    }

    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] {"android.bluetooth.adapter.action.STATE_CHANGED"})]
    class BluetoothStateBroadcastReceiver : BroadcastReceiver
    {
        public Action OnBluetoothStateChange { get; set; } = null;
        private TaskCompletionSource<bool> _isBluetoothOnAsync;

        public override void OnReceive(Context context, Intent intent)
        {
            var intentAction = intent.Action;
            if (intentAction != null && (intentAction.Equals(LocationManager.ProvidersChangedAction)
                                         || intentAction.Equals(BluetoothAdapter.ActionStateChanged)))
            {
                int btState = intent.GetIntExtra(BluetoothAdapter.ExtraState, BluetoothAdapter.Error);

                if (btState == (int) State.On)
                {
                    _isBluetoothOnAsync?.TrySetResult(true);
                    OnBluetoothStateChange?.Invoke();
                }
                else if (btState == (int) State.Off)
                {
                    _isBluetoothOnAsync?.TrySetResult(false);
                    OnBluetoothStateChange?.Invoke();
                }
            }
        }

        private Task<bool> GetBluetoothStateAsync()
        {
            _isBluetoothOnAsync = new TaskCompletionSource<bool>();

            return _isBluetoothOnAsync.Task;
        }

        public static async Task<BluetoothState> GetBluetoothState(Action callbackAction)
        {
            BluetoothStateBroadcastReceiver bluetoothStateBroadcastReceiver = new BluetoothStateBroadcastReceiver();
            bluetoothStateBroadcastReceiver.OnBluetoothStateChange += callbackAction;

            CrossCurrentActivity.Current.AppContext.RegisterReceiver(bluetoothStateBroadcastReceiver,
                new IntentFilter("android.bluetooth.adapter.action.STATE_CHANGED"));

            const int timeoutMs = 2000;
            CancellationTokenSource ct = new CancellationTokenSource(timeoutMs);
            ct.Token.Register(() =>
            {
                bluetoothStateBroadcastReceiver._isBluetoothOnAsync?.TrySetResult(
                    BluetoothAdapter.DefaultAdapter != null && BluetoothAdapter.DefaultAdapter.IsEnabled);
                bluetoothStateBroadcastReceiver.OnBluetoothStateChange?.Invoke();
            }, false);

            BluetoothState bluetoothStateAsync;

            if (BluetoothAdapter.DefaultAdapter == null)
            {
                bluetoothStateAsync = BluetoothState.NO_ADAPTER;
            }

            bluetoothStateAsync = await bluetoothStateBroadcastReceiver.GetBluetoothStateAsync()
                ? BluetoothState.ON
                : BluetoothState.OFF;

            CrossCurrentActivity.Current.AppContext.UnregisterReceiver(bluetoothStateBroadcastReceiver);

            return bluetoothStateAsync;
        }
    }
}
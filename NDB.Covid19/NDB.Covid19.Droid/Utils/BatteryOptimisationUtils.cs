using Android.App;
using Android.Content;
using Android.OS;

namespace NDB.Covid19.Droid.Utils
{
    public class BatteryOptimisationUtils
    {

        private static readonly string PACKAGE_NAME = "no.fhi.smittestopp_exposure_notification";

        private static readonly int MY_REQUEST_IGNORE_BATTERY_OPTIMIZATION_ID = 1002;

        public static bool IsAppLaunchedToShowDialog { get; set; }

        public static bool IsAppLaunchedToPullKeys { get; set; }

        public static bool CheckIsEnableBatteryOptimizations()
        {
            PowerManager pm = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
            bool result = pm.IsIgnoringBatteryOptimizations(PACKAGE_NAME);
            return result;
        }
        public static void StopBatteryOptimizationSetting(Activity current)
        {
            Intent intent = new Intent();
            intent.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
            intent.SetData(Android.Net.Uri.Parse("package:" + PACKAGE_NAME));
            current.StartActivityForResult(intent, MY_REQUEST_IGNORE_BATTERY_OPTIMIZATION_ID);
          

        }
        public static void StartBatterySetting(Context context)
        {
            Intent intent = new Intent();
            intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
            context.StartActivity(intent);
        }

    }
}

using Android.Content.PM;
using Android.Gms.Common;
using Android.Support.V4.Content.PM;
using NDB.Covid19.Utils;
using Plugin.CurrentActivity;

namespace NDB.Covid19.Droid.Utils
{
    public abstract class PlayServicesVersionUtils
    {
        public static bool PlayServicesVersionNumberIsLargeEnough(PackageManager packageManager)
        {
            long minimumPlayServicesVersionNumber = 201300000; //GPS version 20.13.0
            long playServicesVersionNumber = PackageInfoCompat.
                GetLongVersionCode(CrossCurrentActivity.Current.AppContext.PackageManager.GetPackageInfo(GoogleApiAvailability.GooglePlayServicesPackage, 0));
            bool isLargeEnough = playServicesVersionNumber >= minimumPlayServicesVersionNumber;
            if (!isLargeEnough)
            {
                LogUtils.LogMessage(Enums.LogSeverity.INFO, "PlayServicesVersionUtils: User is prevented from using the app because of too low GPS version");
            }
            return isLargeEnough;
        }
    }
}

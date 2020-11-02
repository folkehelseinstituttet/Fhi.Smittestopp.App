using Android.Gms.Common;
using AndroidX.Core.Content.PM;
using Java.Lang;
using NDB.Covid19.Interfaces;
using Plugin.CurrentActivity;

namespace NDB.Covid19.Droid.Services
{
    class DroidApiDataHelperHandler : IApiDataHelper
    {
       
        public bool IsGoogleServiceEnabled()
        {
            return GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(CrossCurrentActivity.Current
                .AppContext) == ConnectionResult.Success;
        }

        public string GetBackGroudServiceVersion()
        {
            string version = "";
            try
            {
                version = PackageInfoCompat.GetLongVersionCode(CrossCurrentActivity.Current.AppContext.PackageManager.GetPackageInfo(GoogleApiAvailability.GooglePlayServicesPackage, 0)).ToString();
            }
            catch (Exception e)
            {
                //Do not log this. It will create a deadlock.
                System.Diagnostics.Debug.Print("Couldn't get versioncode");
                System.Diagnostics.Debug.Print(e.ToString());
            }
            return version;
        }
        public string GetBackGroundServicVersionLogString()
        {
            return $" (GPS: {GetBackGroudServiceVersion()})";
        }
    }
}
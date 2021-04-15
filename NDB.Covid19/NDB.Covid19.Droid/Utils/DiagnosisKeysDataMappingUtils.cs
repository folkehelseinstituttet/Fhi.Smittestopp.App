using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common.Apis;
using Android.Gms.Nearby;
using Android.Gms.Nearby.ExposureNotification;
using Android.Runtime;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices.ExposureNotification;
using EN = Xamarin.ExposureNotifications;

namespace NDB.Covid19.Droid.Utils
{
    public static class DiagnosisKeysDataMappingUtils
    {
        private static string _logPrefix = $"Android {nameof(DiagnosisKeysDataMappingUtils)}: ";

        private static IExposureNotificationClient Instance => NearbyClass.GetExposureNotificationClient(Application.Context);

        public static async Task SetDiagnosisKeysDataMappingAsync()
        {
            try
            {
                long APIVersion = await Instance.GetVersionAsync();
                Debug.WriteLine("Exposure Notification version is " + APIVersion);
            }
            catch (ApiException e)
            {
                // This is EN API v1, do nothing in this case.
                if(e.StatusCode == CommonStatusCodes.ApiNotConnected)
                {
                    LogUtils.LogException(LogSeverity.WARNING, e, _logPrefix + "SetDiagnosisKeysDataMappingAsync is not available in EN API v1, aborting.");
                    return;
                }
            }

            // Quota for SetDiagnosisKeysDataMappingAsync is reached, do nothing
            if ((SystemTime.Now() - LocalPreferencesHelper.GetLastDiagnosisKeysDataMappingDateTime()).TotalDays <= 7)
            {
                LogUtils.LogMessage(LogSeverity.WARNING, _logPrefix + "SetDiagnosisKeysDataMappingAsync called too early, aborting.");
                return;
            }

            EN.DailySummaryConfiguration config = await new ExposureNotificationWebService().GetDailySummaryConfiguration();
            DiagnosisKeysDataMapping.DiagnosisKeysDataMappingBuilder builder = new DiagnosisKeysDataMapping.DiagnosisKeysDataMappingBuilder();
            JavaDictionary<Java.Lang.Integer, Java.Lang.Integer> newMap = new JavaDictionary<Java.Lang.Integer, Java.Lang.Integer>();
            foreach (var pair in config.DaysSinceOnsetInfectiousness)
            {
                newMap[new Java.Lang.Integer(pair.Key)] = new Java.Lang.Integer((int)pair.Value);
            }
            builder.SetDaysSinceOnsetToInfectiousness(newMap);
            builder.SetInfectiousnessWhenDaysSinceOnsetMissing((int)config.DefaultInfectiousness);
            builder.SetReportTypeWhenMissing((int)config.DefaultReportType);
            try
            {
                await Instance.SetDiagnosisKeysDataMappingAsync(builder.Build());
            }
            catch (Exception e)
            {
                // Do nothing if setting DataMapping fails
                LogUtils.LogException(LogSeverity.ERROR, e, _logPrefix + "Failed to call SetDiagnosisKeysDataMappingAsync.");
                return;
            }
            LocalPreferencesHelper.UpdateLastDiagnosisKeysDataMappingDateTime();
            LogUtils.LogMessage(LogSeverity.INFO, _logPrefix + "Successfully updated the mapping with SetDiagnosisKeysDataMappingAsync");
        }
    }
}

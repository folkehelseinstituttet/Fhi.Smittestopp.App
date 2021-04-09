using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common.Apis;
using Android.Gms.Nearby;
using Android.Gms.Nearby.ExposureNotification;
using Android.Runtime;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.Utils
{
    public static class DiagnosisKeysDataMappingUtils
    {
        static string _logPrefix = $"Android {nameof(DiagnosisKeysDataMappingUtils)}: ";

        private static IExposureNotificationClient Instance => NearbyClass.GetExposureNotificationClient(Application.Context);

        public static async Task SetDiagnosisKeysDataMappingAsync()
        {
            try
            {
                long APIVersion = await Instance.GetVersionAsync();
                Debug.WriteLine(APIVersion);
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
                LogUtils.LogException(LogSeverity.WARNING, null, _logPrefix + "SetDiagnosisKeysDataMappingAsync called too early, aborting.");
                return;
            }

            var config = await new ExposureNotificationHandler().GetDailySummaryConfigurationAsync();
            var builder = new DiagnosisKeysDataMapping.DiagnosisKeysDataMappingBuilder();
            var newMap = new JavaDictionary<Java.Lang.Integer, Java.Lang.Integer>();
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

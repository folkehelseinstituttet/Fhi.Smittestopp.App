using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.PersistedData;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureDetectedHelper
    {
        static string _logPrefix = $"{nameof(ExposureDetectedHelper)}";
        private static SecureStorageService _secureStorageService => ServiceLocator.Current.GetInstance<SecureStorageService>();

        public static async Task EvaluateRiskInSummaryAndCreateMessage(ExposureDetectionSummary summary, object messageSender)
        {
            if (summary.MatchedKeyCount == 0)
            {
                Debug.WriteLine($"{_logPrefix}: MatchedKeyCount was 0, so no message was shown");
                return;
            }

            if (summary.HighestRiskScore < 1)
            {
                Debug.WriteLine($"{_logPrefix}: Highest risk score is less than 1");
                return;
            }

            if (!IsAttenuationDurationOverThreshold(summary))
            {
                Debug.WriteLine($"{_logPrefix}: No exposure incidents were above the exposure time threshold");
                return;
            }

            await MessageUtils.CreateMessage(messageSender);
        }

        public static bool IsAttenuationDurationOverThreshold(ExposureDetectionSummary summary)
        {
            double weightedMinutesOfExposuresFromSummary = 0;

            double exposureDurationMinutesThreshold = LocalPreferencesHelper.ExposureTimeThreshold;
            double lowAttenuationBucketMultiplier = LocalPreferencesHelper.LowAttenuationDurationMultiplier;
            double middleAttenuationBucketMultiplier = LocalPreferencesHelper.MiddleAttenuationDurationMultiplier;
            double highAttenuationBucketMultiplier = LocalPreferencesHelper.HighAttenuationDurationMultiplier;

            weightedMinutesOfExposuresFromSummary =
                (summary.AttenuationDurations[0].Minutes * lowAttenuationBucketMultiplier) +
                (summary.AttenuationDurations[1].Minutes * middleAttenuationBucketMultiplier) +
                (summary.AttenuationDurations[2].Minutes * highAttenuationBucketMultiplier);

            return weightedMinutesOfExposuresFromSummary >= exposureDurationMinutesThreshold;
        }

        public static void SaveLastSummary(ExposureDetectionSummary summary)
        {
            try
            {
                string summaryJson = ExposureDetectionSummaryJsonHelper.ExposureDectionSummaryToJson(summary);
                _secureStorageService.SaveValue(SecureStorageKeys.LAST_SUMMARY_KEY, summaryJson);
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e, $"{_logPrefix}.{nameof(SaveLastSummary)}");
            }
        }
    }
}

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

        /// <summary>
        /// [EN API v1] method to evaluate the summary of exposures and make a decision for Exposure Notification generation.
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="messageSender"></param>
        /// <returns></returns>
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

        /// <summary>
        /// [EN API v1] method to weight the durations in different attenuation buckets in the summary of exposures,
        /// and evaluate the weighted duration against a certain threshold
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
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

        /// <summary>
        /// [EN API v2] method to evaluate the risk of daily summary of exposures towards
        /// a maximum score threshold.
        /// </summary>
        /// <param name="dailySummary"></param>
        /// <returns></returns>
        public static bool RiskInDailySummaryAboveThreshold(DailySummary dailySummary)
        {
            if (dailySummary.Summary.MaximumScore >= LocalPreferencesHelper.MaximumScoreThreshold)
            {
                Debug.WriteLine($"{_logPrefix}: Maximum score for DailySummary {dailySummary.Timestamp.Date} is " +
                    $"{dailySummary.Summary.MaximumScore} and is higher than MaximumScoreThreshold {LocalPreferencesHelper.MaximumScoreThreshold}");
                return true;
            }

            Debug.WriteLine($"{_logPrefix}: Maximum score for DailySummary ${dailySummary.Timestamp.Date} is " +
                    $"{dailySummary.Summary.MaximumScore} and is lower than MaximumScoreThreshold {LocalPreferencesHelper.MaximumScoreThreshold}");
            return false;
        }

        /// <summary>
        /// [EN API v2] method to identify that the Exposure Notification for a given date has already been
        /// shown to the user.
        /// </summary>
        /// <param name="dateOfExposure"></param>
        /// <returns></returns>
        public static bool HasNotShownExposureNotificationForDate(DateTime dateOfExposure)
        {
            //TODO: Add logic for limiting amount of Exposure Notifications to 1 for a given date
            // 1. Use secure storage to save the given DateTime
            // 2. Delete the date time after 14 days (maybe in another method)
            return true;
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

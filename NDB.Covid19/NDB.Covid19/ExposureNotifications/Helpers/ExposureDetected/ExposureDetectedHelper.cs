using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.PersistedData;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using Newtonsoft.Json;
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
        /// a scoresum threshold.
        /// </summary>
        /// <param name="dailySummary"></param>
        /// <returns></returns>
        public static bool RiskInDailySummaryAboveThreshold(DailySummary dailySummary)
        {
            if (dailySummary.Summary.ScoreSum >= LocalPreferencesHelper.ScoreSumThreshold)
            {
                Debug.WriteLine($"{_logPrefix}: Score sum for DailySummary {dailySummary.Timestamp.Date} is " +
                    $"{dailySummary.Summary.ScoreSum} and is higher than ScoreSumThreshold {LocalPreferencesHelper.ScoreSumThreshold}");
                return true;
            }

            Debug.WriteLine($"{_logPrefix}: Score sum for DailySummary {dailySummary.Timestamp.Date} is " +
                    $"{dailySummary.Summary.ScoreSum} and is lower than ScoreSumThreshold {LocalPreferencesHelper.ScoreSumThreshold}");
            return false;
        }

        /// <summary>
        /// [EN API v2] method to identify that the Exposure Notification for a given date has already been
        /// shown to the user.
        /// </summary>
        /// <param name="dateOfExposure"></param>
        /// <param name="previouslySavedDatesOfExposures"></param>
        /// <returns>True if the date has not yet been saved</returns>
        public static bool HasNotShownExposureNotificationForDate(DateTime dateOfExposure, List<DateTime> previouslySavedDatesOfExposures)
        {
            return !previouslySavedDatesOfExposures.Any(item => item.Date == dateOfExposure.Date);
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

        /// <summary>
        /// [EN API v2] method to delete previously saved dates of exposures from DailySummaries that triggered
        /// message creation in the app based and that are older than 14 days to the current date.
        /// </summary>
        /// <returns>New list where only date for the last 14 days are present</returns>
        public static List<DateTime> DeleteDatesOfExposureOlderThan14DaysAndReturnNewList()
        {
            try
            {
                if (_secureStorageService.KeyExists(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY))
                {
                    string datesOfExposuresSavedEarlier = _secureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
                    List<DateTime> datesOfPreviousExposures = JsonConvert.DeserializeObject<List<DateTime>>(datesOfExposuresSavedEarlier);
                    List<DateTime> datesOfPreviousExposuresNoOlderThan14Days =
                        datesOfPreviousExposures
                        .Where(
                            dt => ((SystemTime.Now() - dt.Date).TotalDays < 14))
                        .ToList();

                    string datesAsString = JsonConvert.SerializeObject(datesOfPreviousExposuresNoOlderThan14Days);
                    LogUtils.LogMessage(Enums.LogSeverity.INFO,
                        $"{_logPrefix}.DeleteDatesOfExposureOlderThan14Days: Updated dates of previous exposures, " +
                        $"deleted {datesOfPreviousExposures.Count - datesOfPreviousExposuresNoOlderThan14Days.Count} dates older than 14 days");
                    _secureStorageService.SaveValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY, datesAsString);

                    return datesOfPreviousExposuresNoOlderThan14Days;
                }
                else
                {
                    LogUtils.LogMessage(Enums.LogSeverity.INFO, $"{_logPrefix}.DeleteDatesOfExposureOlderThan14Days: No exposures discovered yet using EN API v2");
                    return new List<DateTime>();
                }
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e, $"{_logPrefix}.DeleteDatesOfExposureOlderThan14Days: " +
                    $"Unexpected error has occured when saving Exposure Dates to Secure Storage. " +
                    $"Resetting the saved value to avoid error in the future.");
                _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
                return new List<DateTime>();
            }
        }

        /// <summary>
        /// [EN API v2] method to update previously saved dates of exposures from DailySummaries with new dates,
        /// for which message(s) has been created during the last exposure check.
        /// </summary>
        /// <param name="datesOfExposuresOverThreshold"></param>
        /// <returns></returns>
        public static async Task UpdateDatesOfExposures(List<DateTime> datesOfExposuresOverThreshold)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_secureStorageService.KeyExists(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY))
                    {
                        string datesOfExposuresSavedEarlier = _secureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
                        List<DateTime> datesOfPreviousExposures = JsonConvert.DeserializeObject<List<DateTime>>(datesOfExposuresSavedEarlier);
                        List<DateTime> datesOfPreviousExposuresAndNewlyDiscovered = datesOfPreviousExposures.Concat(datesOfExposuresOverThreshold).ToList();
                        string datesAsString = JsonConvert.SerializeObject(datesOfPreviousExposuresAndNewlyDiscovered);
                        LogUtils.LogMessage(Enums.LogSeverity.INFO, $"{_logPrefix}.UpdateDatesOfExposure: Dates: {datesAsString}");
                        _secureStorageService.SaveValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY, datesAsString);
                    }
                    else
                    {
                        LogUtils.LogMessage(Enums.LogSeverity.INFO, $"{_logPrefix}.UpdateDatesOfExposure: No exposures discovered yet using EN API v2");
                        string datesAsString = JsonConvert.SerializeObject(datesOfExposuresOverThreshold);
                        LogUtils.LogMessage(Enums.LogSeverity.INFO, $"{_logPrefix}.UpdateDatesOfExposure: Dates: {datesAsString}");
                        _secureStorageService.SaveValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY, datesAsString);
                    }
                } catch (Exception e)
                {
                    LogUtils.LogException(Enums.LogSeverity.ERROR, e, $"{_logPrefix}.UpdateDatesOfExposure: " +
                        $"Unexpected error has occured when saving Exposure Dates to Secure Storage. " +
                        $"Resetting the saved value to avoid error in the future.");
                    _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
                }
            });
        }
    }
}

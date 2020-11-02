using System;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public class JsonCompatibleExposureDetectionSummary
    {
        public int DaysSinceLastExposure { get; set; }
        public ulong MatchedKeyCount { get; set; }
        public int HighestRiskScore { get; set; }
        public TimeSpan[] AttenuationDurations { get; set; }
        public int SummationRiskScore { get; set; }

        public JsonCompatibleExposureDetectionSummary()
        {
        }

        public JsonCompatibleExposureDetectionSummary(ExposureDetectionSummary exposureDetectionSummary)
        {
            DaysSinceLastExposure = exposureDetectionSummary.DaysSinceLastExposure;
            MatchedKeyCount = exposureDetectionSummary.MatchedKeyCount;
            HighestRiskScore = exposureDetectionSummary.HighestRiskScore;
            AttenuationDurations = exposureDetectionSummary.AttenuationDurations;
            SummationRiskScore = exposureDetectionSummary.SummationRiskScore;
        }
    }
}

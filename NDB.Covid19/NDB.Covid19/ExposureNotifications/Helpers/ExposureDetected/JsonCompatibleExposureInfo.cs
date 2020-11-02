using System;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public class JsonCompatibleExposureInfo
    {
        public DateTime Timestamp { get; set; }
        public TimeSpan Duration { get; set; }
        public int AttenuationValue { get; set; }
        public int TotalRiskScore { get; set; }
        public RiskLevel TransmissionRiskLevel { get; set; }

        public JsonCompatibleExposureInfo()
        {
        }

        public JsonCompatibleExposureInfo(ExposureInfo exposureInfo)
        {
            Timestamp = exposureInfo.Timestamp;
            Duration = exposureInfo.Duration;
            AttenuationValue = exposureInfo.AttenuationValue;
            TotalRiskScore = exposureInfo.TotalRiskScore;
            TransmissionRiskLevel = exposureInfo.TransmissionRiskLevel;
        }
    }
}

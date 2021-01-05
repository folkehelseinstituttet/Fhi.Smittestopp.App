using System;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Models
{
    public class ExposureKeyModel : TemporaryExposureKey
    {
        public ExposureKeyModel(TemporaryExposureKey tek) : base(tek.Key, tek.RollingStart, tek.RollingDuration, tek.TransmissionRiskLevel)
        {
        }

        public ExposureKeyModel(
            byte[] keyData,
            DateTimeOffset rollingStart,
            TimeSpan rollingDuration,
            RiskLevel transmissionRisk) : base(keyData, rollingStart, rollingDuration, transmissionRisk)
        {
        }

        public ExposureKeyModel(
            byte[] keyData,
            DateTimeOffset rollingStart,
            TimeSpan rollingDuration,
            RiskLevel transmissionRisk, int daysSinceOnsetOfSymptoms) : base(keyData, rollingStart, rollingDuration, transmissionRisk)
        {
            DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;
        }

        public int DaysSinceOnsetOfSymptoms { get; set; }
    }
}

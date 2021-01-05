using System;
using NDB.Covid19.PersistedData;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Models
{
    public class ExposureKeyModel : TemporaryExposureKey
    {
        public ExposureKeyModel(TemporaryExposureKey tek) : base(tek.Key, tek.RollingStart, tek.RollingDuration, tek.TransmissionRiskLevel)
        {
            IsSharingAllowed = LocalPreferencesHelper.AreCountryConsentsGiven;
        }

        public ExposureKeyModel(
            byte[] keyData,
            DateTimeOffset rollingStart,
            TimeSpan rollingDuration,
            RiskLevel transmissionRisk) : base(keyData, rollingStart, rollingDuration, transmissionRisk)
        {
            IsSharingAllowed = LocalPreferencesHelper.AreCountryConsentsGiven;
        }

        public ExposureKeyModel(
            byte[] keyData,
            DateTimeOffset rollingStart,
            TimeSpan rollingDuration,
            RiskLevel transmissionRisk, int daysSinceOnsetOfSymptoms) : base(keyData, rollingStart, rollingDuration, transmissionRisk)
        {
            DaysSinceOnsetOfSymptoms = daysSinceOnsetOfSymptoms;
            IsSharingAllowed = LocalPreferencesHelper.AreCountryConsentsGiven;
        }

        public int DaysSinceOnsetOfSymptoms { get; set; }
        public bool IsSharingAllowed { get; set; } = false;
    }
}

using Newtonsoft.Json;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureDetectionSummaryJsonHelper
    {
        public static string ExposureDectionSummaryToJson(ExposureDetectionSummary exposureDetectionSummary)
        {
            JsonCompatibleExposureDetectionSummary jsonCompatibleExposureDetectionSummary
                = new JsonCompatibleExposureDetectionSummary(exposureDetectionSummary);
            return JsonConvert.SerializeObject(jsonCompatibleExposureDetectionSummary);
        }

        public static ExposureDetectionSummary ExposureDetectionSummaryFromJsonCompatibleString(string jsonCompatibleExposureDetectionSummaryJson)
        {
            JsonCompatibleExposureDetectionSummary jsonCompatibleExposureDetectionSummary
                = JsonConvert.DeserializeObject<JsonCompatibleExposureDetectionSummary>(jsonCompatibleExposureDetectionSummaryJson);
            return new ExposureDetectionSummary(
                daysSinceLastExposure: jsonCompatibleExposureDetectionSummary.DaysSinceLastExposure,
                matchedKeyCount: jsonCompatibleExposureDetectionSummary.MatchedKeyCount,
                highestRiskScore: jsonCompatibleExposureDetectionSummary.HighestRiskScore,
                attenuationDurations: jsonCompatibleExposureDetectionSummary.AttenuationDurations,
                summationRiskScore: jsonCompatibleExposureDetectionSummary.SummationRiskScore);
        }
    }
}

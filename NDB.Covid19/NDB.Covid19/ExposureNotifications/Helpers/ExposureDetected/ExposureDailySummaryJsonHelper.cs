using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureDailySummaryJsonHelper
    {
        public static string ExposureDailySummaryToJson(IEnumerable<DailySummary>  dailySummaries)
        {
            IEnumerable <JsonCompatibleExposureDailySummary> jsonCompatibleExposureDailySummaries
                = dailySummaries.Select(dailySummary => new JsonCompatibleExposureDailySummary(dailySummary));
            return JsonConvert.SerializeObject(jsonCompatibleExposureDailySummaries);
        }

        public static IEnumerable<DailySummary> ExposureDailySummaryFromJsonCompatibleString(string jsonCompatibleExposureDailySummaryJson)
        {
            IEnumerable<JsonCompatibleExposureDailySummary> jsonCompatibleExposureDailySummaries
                = JsonConvert.DeserializeObject<IEnumerable<JsonCompatibleExposureDailySummary>>(jsonCompatibleExposureDailySummaryJson);
            return jsonCompatibleExposureDailySummaries.Select(jsonCompatibleExposureDailySummary => new DailySummary(
                timestamp: jsonCompatibleExposureDailySummary.Timestamp,
                summary: jsonCompatibleExposureDailySummary.Summary,                
                reports: jsonCompatibleExposureDailySummary.Reports));
        }
    }
}

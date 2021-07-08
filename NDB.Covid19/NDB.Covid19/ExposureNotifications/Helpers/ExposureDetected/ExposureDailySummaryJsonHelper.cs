using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureDailySummaryJsonHelper
    {
        public static string ExposureDailySummariesToJson(IEnumerable<DailySummary> dailySummaries)
        {
            IEnumerable<JsonCompatibleExposureDailySummary> jsonCompatibleExposureDailySummaries
                = dailySummaries.Select(dailySummary => new JsonCompatibleExposureDailySummary(dailySummary));
            return JsonConvert.SerializeObject(jsonCompatibleExposureDailySummaries);
        }
    }
}

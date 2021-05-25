using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Models
{
    public class DailySummaryConfigurationDTO
    {
        public DailySummaryConfiguration DailySummaryConfiguration { get; set; }
        public double? ScoreSumThreshold { get; set; }

        public DailySummaryConfigurationDTO()
        {
        }
    }
}

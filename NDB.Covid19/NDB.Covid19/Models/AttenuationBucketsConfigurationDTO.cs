namespace NDB.Covid19.Models
{
    public class AttenuationBucketsConfigurationDTO
    {
        public Xamarin.ExposureNotifications.Configuration Configuration { get; set; }
        public AttenuationBucketsParametersDTO AttenuationBucketsParams { get; set; }

        public AttenuationBucketsConfigurationDTO()
        {
        }
    }
}

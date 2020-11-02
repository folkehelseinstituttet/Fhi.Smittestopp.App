namespace NDB.Covid19.Models
{
    public class AttenuationBucketsParametersDTO
    {
        public double ExposureTimeThreshold { get; set; }
        public double LowAttenuationBucketMultiplier { get; set; }
        public double MiddleAttenuationBucketMultiplier { get; set; }
        public double HighAttenuationBucketMultiplier { get; set; }

        public AttenuationBucketsParametersDTO()
        {
        }
    }
}

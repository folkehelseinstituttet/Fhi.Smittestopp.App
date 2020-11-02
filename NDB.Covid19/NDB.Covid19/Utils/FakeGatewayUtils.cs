using System.Collections.Generic;
using System.Threading.Tasks;
using MoreLinq;
using NDB.Covid19.Models;
using NDB.Covid19.WebServices;

namespace NDB.Covid19.Utils
{
    public static class FakeGatewayUtils
    {
        public static IEnumerable<ExposureKeyModel> LastPulledExposureKeys = new List<ExposureKeyModel>();
        public static bool IsFakeGatewayTest { get; set; } = false;

        private static SelfDiagnosisSubmissionDTO CreateFakeDataForRegions(List<string> regions)
        {
            LastPulledExposureKeys.ForEach(key =>
            {
                key.DaysSinceOnsetOfSymptoms = 2;
            });
            
            return new SelfDiagnosisSubmissionDTO(LastPulledExposureKeys)
            {
                Regions = regions
            };
        }

        public static async Task<ApiResponse> PostKeysToFakeGateway(string region)
        {
            try
            {
                IsFakeGatewayTest = true;
                await Xamarin.ExposureNotifications.ExposureNotification.SubmitSelfDiagnosisAsync();

                return await new FakeGatewayWebService().UploadKeys(CreateFakeDataForRegions(new List<string>
                    {region}));
            }
            finally
            {
                IsFakeGatewayTest = false;
            }
        }
    }
}

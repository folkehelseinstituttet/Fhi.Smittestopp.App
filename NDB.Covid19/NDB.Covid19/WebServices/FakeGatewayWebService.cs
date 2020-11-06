using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;

namespace NDB.Covid19.WebServices
{
    class FakeGatewayWebService : BaseWebService
    {
        public async Task<ApiResponse> UploadKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDto)
        {
            ApiResponse response =
                await new BaseWebService().Post(selfDiagnosisSubmissionDto, Conf.URL_GATEWAY_STUB_UPLOAD);
            HandleErrorsSilently(response);

            return response;
        }
    }
}

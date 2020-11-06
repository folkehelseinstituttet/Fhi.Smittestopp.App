using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    // This class is used to put redacted key info in the additionalInfo of the error log
    // it is not supposed to be used as a general error handler
    public class PostExposureKeysErrorHandler : BaseErrorHandler, IErrorHandler
    {
        private readonly string RedactedKeys;

        public PostExposureKeysErrorHandler(string RedactedKeys)
        {
            this.RedactedKeys = RedactedKeys;
        }

        public bool IsResponsible(ApiResponse apiResponse)
        {
            return apiResponse.IsSuccessfull == false;
        }

        public void HandleError(ApiResponse apiResponse)
        {
            LogUtils.LogApiError(Enums.LogSeverity.ERROR, apiResponse, true, RedactedKeys);
        }
    }
}

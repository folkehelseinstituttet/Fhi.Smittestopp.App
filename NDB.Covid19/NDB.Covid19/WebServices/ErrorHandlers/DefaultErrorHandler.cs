using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class DefaultErrorHandler : BaseErrorHandler, IErrorHandler
    {
        public bool IsSilent = false;

        public DefaultErrorHandler()
        {
        }

        public DefaultErrorHandler(bool IsSilent)
        {
            this.IsSilent = IsSilent;
        }

        public bool IsResponsible(ApiResponse apiResponse)
        {
            return apiResponse.IsSuccessfull == false;
        }

        public void HandleError(ApiResponse apiResponse)
        {
            LogUtils.LogApiError(Enums.LogSeverity.ERROR, apiResponse, IsSilent);
            if (!IsSilent)
            {
                ShowErrorToUser();
            }
        }
    }
}
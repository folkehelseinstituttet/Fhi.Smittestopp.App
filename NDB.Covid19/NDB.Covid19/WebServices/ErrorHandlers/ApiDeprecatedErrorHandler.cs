using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class ApiDeprecatedErrorHandler : BaseErrorHandler, IErrorHandler
    {
        public bool IsResponsible(ApiResponse apiResponse)
        {
            return (int)apiResponse.StatusCode == 410;
        }

        public void HandleError(ApiResponse apiResponse)
        {
            LogUtils.LogApiError(Enums.LogSeverity.WARNING, apiResponse, false);
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }
    }
}
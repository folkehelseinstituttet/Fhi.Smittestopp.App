using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class TimeoutErrorHandler : BaseErrorHandler, IErrorHandler
    {
        public override string ErrorMessageTitle => "CONNECTION_ERROR_TITLE".Translate();
        public override string ErrorMessage => "BAD_CONNECTION_ERROR_MESSAGE".Translate();
        public bool IsSilent = false;

        public TimeoutErrorHandler(bool IsSilent)
        {
            this.IsSilent = IsSilent;
        }

        public TimeoutErrorHandler()
        {
        }

        public bool IsResponsible(ApiResponse apiResponse)
        {
            return apiResponse.Exception != null
                && apiResponse.Exception is TaskCanceledException; //As long as we never cancel tasks manually, then we can expect that this always means time out.
        }

        public void HandleError(ApiResponse apiResponse)
        {
            string message = $"{apiResponse.ErrorLogMessage}. Timed out after {Conf.DEFAULT_TIMEOUT_SERVICECALLS_SECONDS} seconds because of bad connection.";
            LogUtils.LogApiError(Enums.LogSeverity.WARNING, apiResponse, IsSilent, "", message) ;
            if (!IsSilent)
            {
                ShowErrorToUser();
            }
        }
    }
}
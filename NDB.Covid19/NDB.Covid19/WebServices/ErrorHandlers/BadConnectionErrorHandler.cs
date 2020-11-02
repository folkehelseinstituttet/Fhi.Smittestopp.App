using System.IO;
using System.Net;
using I18NPortable;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class BadConnectionErrorHandler : BaseErrorHandler, IErrorHandler
    {
        public override string ErrorMessageTitle => "CONNECTION_ERROR_TITLE".Translate();
        public override string ErrorMessage => "BAD_CONNECTION_ERROR_MESSAGE".Translate();
        public bool IsSilent = false;

        public BadConnectionErrorHandler(bool IsSilent)
        {
            this.IsSilent = IsSilent;
        }

        public BadConnectionErrorHandler()
        {
        }

        public bool IsResponsible(ApiResponse apiResponse)
        {
            bool isIOSException = apiResponse.Exception?.InnerException is IOException
                && (apiResponse.Exception.InnerException.Message.Contains("the transport connection")
                || apiResponse.Exception.InnerException.Message.Contains("The server returned an invalid or unrecognized response"));

            //This will contain a more detailed error description in the Inner exception
            bool isWebException = apiResponse.Exception is WebException;

            return isIOSException || isWebException;
        }

        public void HandleError(ApiResponse apiResponse)
        {
            LogUtils.LogApiError(Enums.LogSeverity.WARNING, apiResponse, IsSilent, "", "Failed contact to server: Bad connection") ;
            if (!IsSilent)
            {
                ShowErrorToUser();
            }
        }
    }
}
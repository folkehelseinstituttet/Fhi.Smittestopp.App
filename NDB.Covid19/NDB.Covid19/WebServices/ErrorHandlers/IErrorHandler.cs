using NDB.Covid19.Models;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public interface IErrorHandler
    {
        bool IsResponsible(ApiResponse apiResponse);
        void HandleError(ApiResponse apiResponse);
    }
}

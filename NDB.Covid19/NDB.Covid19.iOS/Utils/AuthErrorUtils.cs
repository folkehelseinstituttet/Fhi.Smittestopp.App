using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Views.ErrorStatus;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using System;
using UIKit;
using Xamarin.Auth;

namespace NDB.Covid19.iOS.Utils
{
    public static class AuthErrorUtils
    {
        public static void GoToNotInfectedError(UIViewController parent, LogSeverity severity, Exception e, string printMessage)
        {
            UIViewController newVC = ErrorPageViewController.Create(ErrorViewModel.REGISTER_ERROR_NOMATCH_HEADER, errorMessageTxt: ErrorViewModel.REGISTER_ERROR_NOMATCH_DESCRIPTION);
            LogUtils.LogException(severity, e, printMessage);
            GoToVC(parent, newVC);
        }

        public static void GoToManyTriesError(UIViewController parent, LogSeverity severity, Exception e, string printMessage)
        {
            UIViewController newVC = ErrorPageViewController.Create(ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_HEADER, errorMessageTxt: ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_DESCRIPTION);
            LogUtils.LogException(severity, e, printMessage);
            GoToVC(parent, newVC);
        }

        public static void GoToTechnicalError(UIViewController parent, LogSeverity severity, Exception e, string errorMessage)
        {
            UIViewController newVC = ErrorPageViewController.Create(ErrorViewModel.REGISTER_ERROR_HEADER, errorMessageTxt: ErrorViewModel.REGISTER_ERROR_DESCRIPTION);
            LogUtils.LogException(severity, e, errorMessage);
            GoToVC(parent, newVC);
        }

        public static void GoToTechnicalErrorFHINumbers(UIViewController parent, LogSeverity severity, Exception e, string errorMessage)
        {
            UIViewController newVC = ErrorPageViewController.Create(ErrorViewModel.REGISTER_ERROR_FETCH_FHI_DATA_HEADER, errorMessageTxt: ErrorViewModel.REGISTER_ERROR_FETCH_FHI_DATA_DESCRIPTION);
            LogUtils.LogException(severity, e, errorMessage);
            GoToVC(parent, newVC);
        }

        public static void GoToErrorPageForAuthErrorType(UIViewController parent, AuthErrorType authErrorType)
        {
            AuthException authException = new AuthException(authErrorType.ToString());
            switch (authErrorType)
            {
                case AuthErrorType.MaxTriesExceeded:
                    GoToManyTriesError(parent, LogSeverity.WARNING, authException, "Max number of tries was exceeded");
                    break;
                case AuthErrorType.NotInfected:
                    GoToNotInfectedError(parent, LogSeverity.WARNING, authException, "User is not infected");
                    break;
                case AuthErrorType.Unknown:
                    GoToTechnicalError(parent, LogSeverity.WARNING, authException, "User sees Technical error page after ID Porten login: Unknown auth error or user press backbtn");
                    break;
            }
        }

        static void GoToVC(UIViewController parent, UIViewController newVC)
        {
            if (parent.NavigationController != null)
            {
                parent.NavigationController.PushViewController(newVC, true);
            }
            else
            {
                parent.PresentViewController(newVC, true, null);
            }
        }
    }

}

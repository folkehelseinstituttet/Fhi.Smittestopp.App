using System;
using Android.App;
using Android.Content;
using Android.OS;
using NDB.Covid19.Droid.Views.AuthenticationFlow.ErrorActivities;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using static NDB.Covid19.ViewModels.ErrorViewModel;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid.Utils
{
    public static class AuthErrorUtils
    {
        public static void GoToNotInfectedError(Activity parent, LogSeverity severity, Exception e, string errorMessage)
        {
            GoToErrorPage(parent, REGISTER_ERROR_NOMATCH_HEADER, REGISTER_ERROR_NOMATCH_DESCRIPTION, REGISTER_ERROR_DISMISS, REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);
            LogUtils.LogException(severity, e, errorMessage);
        }

        public static void GoToManyTriesError(Activity parent, LogSeverity severity, Exception e, string errorMessage)
        {
            GoToErrorPage(parent, REGISTER_ERROR_TOOMANYTRIES_HEADER, REGISTER_ERROR_TOOMANYTRIES_DESCRIPTION, REGISTER_ERROR_DISMISS, REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);
            LogUtils.LogException(severity, e, errorMessage);
        }

        public static void GoToTechnicalError(Activity parent, LogSeverity severity, Exception e, string errorMessage)
        {
            GoToErrorPage(parent, REGISTER_ERROR_HEADER, REGISTER_ERROR_DESCRIPTION, REGISTER_ERROR_DISMISS, REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);
            LogUtils.LogException(severity, e, errorMessage);
        }

        public static void GoToTechnicalErrorFHINumbers(Activity parent, LogSeverity severity, Exception e, string errorMessage)
        {
            GoToErrorPage(parent, REGISTER_ERROR_FETCH_FHI_DATA_HEADER, REGISTER_ERROR_FETCH_FHI_DATA_DESCRIPTION, REGISTER_ERROR_DISMISS, REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);
            LogUtils.LogException(severity, e, errorMessage);
        }

        public static void GoToUnderagedError(Activity parent, LogSeverity severity, Exception e, string errorMessage)
        {
            GoToErrorPage(parent, REGISTER_ERROR_NOT16YEARSOLD_HEADER, REGISTER_ERROR_NOT16YEARSOLD_DESCRIPTION, REGISTER_ERROR_DISMISS, REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);
            LogUtils.LogException(severity, e, errorMessage);
        }

        public static void GoToErrorPage(Activity parent, string title, string description, string button, string continuebutton, string subtitle = null)
        {
            UpdateCorrelationId(null);
            Intent intent = new Intent(parent, typeof(GeneralErrorActivity));
            Bundle bundle = new Bundle();
            bundle.PutString("title", title);
            bundle.PutString("description", description);
            bundle.PutString("button", button);
            bundle.PutString("continuebutton", continuebutton);
            if (subtitle != null)
            {
                bundle.PutString("subtitle", subtitle);
            }
            if (title == REGISTER_ERROR_NOMATCH_HEADER)
            {
                bundle.PutBoolean("canContinueReportingInfected", true);
            }
            intent.PutExtras(bundle);
            parent.StartActivity(intent);
        }
    }
}
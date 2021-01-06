using System;
using I18NPortable;
using NDB.Covid19.OAuth2;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19.ViewModels
{
    public class CountriesConsentViewModel
    {
        public static DialogViewModel CloseDialogViewModel => new DialogViewModel
        {
            Title = ErrorViewModel.REGISTER_LEAVE_HEADER,
            Body = ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
            OkBtnTxt = ErrorViewModel.REGISTER_LEAVE_CONFIRM,
            CancelbtnTxt = ErrorViewModel.REGISTER_LEAVE_CANCEL
        };
        
        public static DialogViewModel GiveConsentViewModel => new DialogViewModel
        {
            Title = COUNTRIES_CONSENT_DIALOG_HEADER,
            Body = COUNTRIES_CONSENT_DIALOG_MESSAGE,
            OkBtnTxt = COUNTRIES_CONSENT_DIALOG_OK,
            CancelbtnTxt = COUNTRIES_CONSENT_DIALOG_CANCEL
        };

        public static DialogViewModel AbortDuringEUConsentViewModel => new DialogViewModel
        {
            Title = COUNTRIES_CONSENT_DIALOG_HEADER,
            Body = EU_CONSENT_ABORT_DIALOG_CONTENT,
            OkBtnTxt = EU_CONSENT_ABORT_DIALOG_BUTTON_TEXT
        };
        
        public static string BUTTON_TEXT => "COUNTRIES_CONSENT_BUTTON_TEXT".Translate();
        public static string HEADER_TEXT => "COUNTRIES_CONSENT_HEADER_TEXT".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_DESCRIPTION =>
            "COUNTRIES_CONSENT_BE_AWARE_TEXT_DESCRIPTION".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_HEADER => "COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_HEADER".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_TEXT => "COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_TEXT".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_HEADER => "COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_HEADER".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_TEXT => "COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_TEXT".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_BE_AWARE_TEXT => "COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_BE_AWARE_TEXT".Translate();
        public static string COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_EXPLANATION_TEXT => "COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_EXPLANATION_TEXT".Translate();
        public static string CLOSE_BUTTON_ACCESSIBILITY_LABEL => "CLOSE_BUTTON_ACCESSIBILITY_LABEL".Translate();
        public static string COUNTRIES_CONSENT_DIALOG_HEADER => "COUNTRIES_CONSENT_DIALOG_HEADER".Translate();
        public static string COUNTRIES_CONSENT_DIALOG_MESSAGE => "COUNTRIES_CONSENT_DIALOG_MESSAGE".Translate();
        public static string COUNTRIES_CONSENT_DIALOG_OK => "COUNTRIES_CONSENT_DIALOG_OK".Translate();
        public static string COUNTRIES_CONSENT_DIALOG_CANCEL => "COUNTRIES_CONSENT_DIALOG_CANCEL".Translate();
        public static string EU_CONSENT_NEXT_WITH_CONSENT_BUTTON_TEXT => "EU_CONSENT_NEXT_WITH_CONSENT_BUTTON_TEXT".Translate();
        public static string EU_CONSENT_NEXT_WITHOUT_CONSENT_BUTTON_TEXT => "EU_CONSENT_NEXT_WITHOUT_CONSENT_BUTTON_TEXT".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_HEADER => "EU_CONSENT_ABORT_DIALOG_HEADER".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_CONTENT => "EU_CONSENT_ABORT_DIALOG_CONTENT".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_BUTTON_TEXT => "EU_CONSENT_ABORT_DIALOG_BUTTON_TEXT".Translate();


        public static void InvokeNextButtonClick(Action onSuccess, Action onFail, bool consentGiven)
        {
            if (AuthenticationState.PersonalData != null)
            {
                LocalPreferencesHelper.AreCountryConsentsGiven = consentGiven;
                onSuccess?.Invoke();
            }
            else
            {
                onFail?.Invoke();
            }
        }
    }
}
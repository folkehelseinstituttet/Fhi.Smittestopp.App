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

        public static DialogViewModel AbortDuringEUConsentViewModel => new DialogViewModel
        {
            Title = EU_CONSENT_ABORT_DIALOG_HEADER,
            Body = EU_CONSENT_ABORT_DIALOG_CONTENT,
            OkBtnTxt = EU_CONSENT_ABORT_DIALOG_BUTTON_ONE_TEXT,
            CancelbtnTxt = EU_CONSENT_ABORT_DIALOG_BUTTON_TWO_TEXT
        };
        
        public static string HEADER_TEXT => "CONSENT3_HEADER_TEXT".Translate();
        public static string CONSENT3_BODYTEXT_1 => "CONSENT3_BODYTEXT_1".Translate();
        public static string CONSENT3_SHAREDATA_HEADER => "CONSENT3_SHAREDATA_HEADER".Translate();
        public static string CONSENT3_BODYTEXT_2 => "CONSENT3_BODYTEXT_2".Translate();
        public static string CONSENT3_EU_CONSENT_BUTTON_BODYTEXT => "CONSENT3_EU_CONSENT_BUTTON_BODYTEXT".Translate();
        public static string CONSENT3_ONLY_NORWAY_CONSENT_BUTTON_BODYTEXT => "CONSENT3_ONLY_NORWAY_CONSENT_BUTTON_BODYTEXT".Translate();
        public static string CONSENT3_CONSENTTOSHARE => "CONSENT3_CONSENTTOSHARE".Translate();
        public static string CLOSE_BUTTON_ACCESSIBILITY_LABEL => "COUNTRIES_CONSENT_CLOSE_BUTTON_ACCESSIBILITY_LABEL".Translate();
        public static string EU_CONSENT_NEXT_EU_CONSENT_BUTTON_TEXT => "EU_CONSENT_NEXT_EU_CONSENT_BUTTON_TEXT".Translate();
        public static string EU_CONSENT_NEXT_ONLY_NORWAY_CONSENT_BUTTON_TEXT => "EU_CONSENT_NEXT_ONLY_NORWAY_CONSENT_BUTTON_TEXT".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_HEADER => "EU_CONSENT_ABORT_DIALOG_HEADER".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_CONTENT => "EU_CONSENT_ABORT_DIALOG_CONTENT".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_BUTTON_ONE_TEXT => "EU_CONSENT_ABORT_DIALOG_BUTTON_ONE_TEXT".Translate();
        public static string EU_CONSENT_ABORT_DIALOG_BUTTON_TWO_TEXT => "EU_CONSENT_ABORT_DIALOG_BUTTON_TWO_TEXT".Translate();


        public static void InvokeNextButtonClick(Action onSuccess, Action onFail, bool consentGiven)
        {
            if (AuthenticationState.PersonalData != null && AuthenticationState.PersonalData.Validate())
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
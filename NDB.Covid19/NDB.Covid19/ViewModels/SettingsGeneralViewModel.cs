using System;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using CommonServiceLocator;
using NDB.Covid19.Utils;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.ViewModels
{
    public class SettingsGeneralViewModel
    {
        public static string SETTINGS_GENERAL_TITLE => "SETTINGS_GENERAL_TITLE".Translate();
        public static string SETTINGS_GENERAL_EXPLANATION_ONE => "SETTINGS_GENERAL_EXPLANATION_ONE".Translate();
        public static string SETTINGS_GENERAL_EXPLANATION_TWO => "SETTINGS_GENERAL_EXPLANATION_TWO".Translate();
        public static string SETTINGS_GENERAL_MOBILE_DATA_HEADER => "SETTINGS_GENERAL_MOBILE_DATA_HEADER".Translate();
        public static string SETTINGS_GENERAL_MOBILE_DATA_DESC => "SETTINGS_GENERAL_MOBILE_DATA_DESC".Translate();
        public static string SETTINGS_GENERAL_BACKGROUND_ACTIVITY_EXPLANATION => "SETTINGS_GENERAL_BACKGROUND_ACTIVITY_EXPLANATION".Translate();
        public static string SETTINGS_GENERAL_BACKGROUND_ACTIVITY_HEADER => "SETTINGS_GENERAL_BACKGROUND_ACTIVITY_HEADER".Translate();
        public static string SETTINGS_GENERAL_BACKGROUND_ACTIVITY_DESC => "SETTINGS_GENERAL_BACKGROUND_ACTIVITY_DESC".Translate();
        public static string SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER => "SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER".Translate();
        public static string SETTINGS_GENERAL_RESTART_REQUIRED_TEXT => "SETTINGS_GENERAL_RESTART_REQUIRED_TEXT".Translate();
        public static string SETTINGS_GENERAL_MORE_INFO_LINK => "SETTINGS_GENERAL_MORE_INFO_LINK".Translate();
        public static string SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT => "SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT".Translate();
        public static string SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT => "SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT".Translate();
        public static string SETTINGS_GENERAL_EN => "SETTINGS_GENERAL_EN".Translate();
        public static string SETTINGS_GENERAL_NN => "SETTINGS_GENERAL_NN".Translate();
        public static string SETTINGS_GENERAL_NB => "SETTINGS_GENERAL_NB".Translate();
        public static string SETTINGS_GENERAL_LT => "SETTINGS_GENERAL_LT".Translate();
        public static string SETTINGS_GENERAL_PL => "SETTINGS_GENERAL_PL".Translate();
        public static string SETTINGS_GENERAL_SO => "SETTINGS_GENERAL_SO".Translate();
        public static string SETTINGS_GENERAL_TI => "SETTINGS_GENERAL_TI".Translate();
        public static string SETTINGS_GENERAL_AR => "SETTINGS_GENERAL_AR".Translate();
        public static string SETTINGS_GENERAL_UR => "SETTINGS_GENERAL_UR".Translate();


        public static SettingsLanguageSelection Selection { get; private set; }

        public static DialogViewModel AreYouSureDialogViewModel = new DialogViewModel
        {
            Body = "SETTINGS_GENERAL_DIALOG_BODY".Translate(),
            CancelbtnTxt = "SETTINGS_GENERAL_DIALOG_CANCEL".Translate(),
            OkBtnTxt = "SETTINGS_GENERAL_DIALOG_OK".Translate(),
            Title = "SETTINGS_GENERAL_DIALOG_TITLE".Translate()
        };

        public static DialogViewModel GetChangeLanguageViewModel => new DialogViewModel
        {
            Title = "SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER".Translate(),
            Body = "SETTINGS_GENERAL_RESTART_REQUIRED_TEXT".Translate(),
            OkBtnTxt = "SETTINGS_GENERAL_DIALOG_OK".Translate()
        };

        /// <summary>
        /// Opens the link in an in-app browser.
        /// </summary>
        public static void OpenSmitteStopLink()
        {
            try
            {
                ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(SETTINGS_GENERAL_MORE_INFO_LINK);
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e, "Failed to open link on general settings page");
            }
        }


        public bool GetStoredCheckedState() => LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();

        public void OnCheckedChange(bool isChecked) => LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(isChecked);

        public void SetSelection(SettingsLanguageSelection selection)
        {
            Selection = selection;
        }
    }
}

using System;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;

namespace NDB.Covid19.ViewModels
{
    public class SettingsPage5ViewModel
    {
        public static string SETTINGS_PAGE_5_HEADER => "SETTINGS_PAGE_5_HEADER_TEXT".Translate();
        public static string SETTINGS_PAGE_5_CONTENT => "SETTINGS_PAGE_5_CONTENT_TEXT".Translate();
        public static string SETTINGS_PAGE_5_LINK => "SETTINGS_PAGE_5_LINK".Translate();
        public static string SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_TEXT => "SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_TEXT".Translate();
        public static string SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_URL => "SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_URL".Translate();

        public SettingsPage5ViewModel() { }

        /// <returns>Versioning of the app to display on the "about" page</returns>
        public static string GetVersionInfo()
        {
            IAppInfo appInfo = ServiceLocator.Current.GetInstance<IAppInfo>();
            return $"V{appInfo.VersionString} B{appInfo.BuildString} A{Conf.APIVersion} {GetPartialUrlFromConf()} ";
        }

        /// <summary>
        /// Opens the accessibility statement in an in-app browser.
        /// </summary>
        public static void OpenAccessibilityStatementLink()
        {
            try
            {
                ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_URL);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "Failed to open accessibility statement");
            }
        }


        /// <returns>The environment the app runs against to be displayed in the "About" page</returns>
        private static string GetPartialUrlFromConf()
        {
            try
            {
                string endpoint = Conf.BASE_URL;
                bool isHttps = endpoint.StartsWith("https://");
                int httpsLength = (isHttps ? "https://" : "http://").Length;
                int length = endpoint.IndexOf(isHttps? "." : ":9095") - httpsLength;
                return endpoint.Substring(httpsLength, length);
            }
            catch
            {
                return "u";
            }
        }
    }
}

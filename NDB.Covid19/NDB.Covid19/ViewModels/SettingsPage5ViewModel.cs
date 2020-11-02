using I18NPortable;
using NDB.Covid19.Config;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.ViewModels
{
    public class SettingsPage5ViewModel
    {
        public static string SETTINGS_PAGE_5_HEADER => "SETTINGS_PAGE_5_HEADER_TEXT".Translate();
        public static string SETTINGS_PAGE_5_CONTENT => "SETTINGS_PAGE_5_CONTENT_TEXT".Translate();
        public static string SETTINGS_PAGE_5_LINK => "SETTINGS_PAGE_5_LINK".Translate();

        public SettingsPage5ViewModel() { }

        /// <returns>Versioning of the app to display on the "about" page</returns>
        public static string GetVersionInfo()
        {
            IAppInfo appInfo = CommonServiceLocator.ServiceLocator.Current.GetInstance<IAppInfo>();
            return $"V{appInfo.VersionString} B{appInfo.BuildString} A{Conf.APIVersion} {GetPartialUrlFromConf()} ";
        }

        /// <returns>The environment the app runs against to be displayed in the "About" page</returns>
        public static string GetPartialUrlFromConf()
        {
            try
            {
                string endpoint = Conf.URL_PREFIX;
                int index1 = "Https://".Length;
                int length = endpoint.IndexOf(".smittestop") - index1;
                return endpoint.Substring(index1, length);
            }
            catch
            {
                return "u";
            }
        }
    }
}

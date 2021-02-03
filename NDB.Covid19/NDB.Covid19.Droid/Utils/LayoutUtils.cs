using Android.Views;
using NDB.Covid19.PersistedData;
using static NDB.Covid19.Droid.Resource;

namespace NDB.Covid19.Droid.Utils
{
    public class LayoutUtils
    {
        public static LayoutDirection GetLayoutDirection()
        {
            string selectedLanguage = LocalPreferencesHelper.GetAppLanguage();
            return selectedLanguage switch
            {
                "ar" => LayoutDirection.Rtl,
                "ur" => LayoutDirection.Rtl,
                _ => LayoutDirection.Ltr,
            };
        }

        public static int GetBackArrow()
        {
            string selectedLanguage = LocalPreferencesHelper.GetAppLanguage();
            return selectedLanguage switch
            {
                "ar" => Drawable.ic_circle_arrow_right,
                "ur" => Drawable.ic_circle_arrow_right,
                _ => Drawable.ic_circle_arrow_left,
            };
        }

        public static int GetForwardArrow()
        {
            string selectedLanguage = LocalPreferencesHelper.GetAppLanguage();
            return selectedLanguage switch
            {
                "ar" => Drawable.ic_circle_arrow_left,
                "ur" => Drawable.ic_circle_arrow_left,
                _ => Drawable.ic_circle_arrow_right,
            };
        }
    }
}

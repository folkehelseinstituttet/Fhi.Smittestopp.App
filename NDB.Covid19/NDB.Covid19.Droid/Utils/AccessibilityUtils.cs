using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace NDB.Covid19.Droid.Utils
{
    public static class AccessibilityUtils
    {
        // Method for limiting the font scale in Accessibility utils
        public static void AdjustFontScale(Activity context) {
            Android.Content.Res.Configuration configuration = context.Resources.Configuration;
            if (configuration.FontScale > 1.25) {
                configuration.FontScale = 1.25f;
                DisplayMetrics metrics = context.Resources.DisplayMetrics;
                IWindowManager wm = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                wm.DefaultDisplay.GetMetrics(metrics);
                metrics.ScaledDensity = configuration.FontScale * metrics.Density;
                context.CreateConfigurationContext(configuration);
            }
        }

        public static HeadingAccessibilityDelegate GetHeadingAccessibilityDelegate()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.P)
            {
                return null;
            }
            return new HeadingAccessibilityDelegate();
        }
    }
}
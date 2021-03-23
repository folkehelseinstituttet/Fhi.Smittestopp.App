using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.SettingsPage4ViewModel;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class SettingsHelpActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = HEADER;
            SetContentView(Resource.Layout.settings_help);

            Init();
        }

        void Init()
        {
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();

            ImageButton backButton = FindViewById<ImageButton>(Resource.Id.arrow_back_help);
            backButton.ContentDescription = ViewModels.SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            TextView textField = FindViewById<TextView>(Resource.Id.settings_help_text);
            TextView titleField = FindViewById<TextView>(Resource.Id.settings_help_title);

            titleField.Text = HEADER;
            titleField.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            textField.TextAlignment = TextAlignment.ViewStart;
            textField.TextFormatted =
                HtmlCompat.FromHtml($"{CONTENT_TEXT_BEFORE_SUPPORT_LINK} <a href=\"{SUPPORT_LINK}\">{SUPPORT_LINK_SHOWN_TEXT}</a><br><br>"
            /* Note:
            This functionality is not planned for release 1.0. Kept for future use.
                                    //$"{EMAIL_TEXT} <a href=\"mailto:{EMAIL}\">{EMAIL}</a> {PHONE_NUM_Text} <a href=\"tel:{PHONE_NUM}\">{PHONE_NUM}</a>.<br><br>" +
                                    //$"{PHONE_OPEN_TEXT}<br><br>" +
                                    //$"{PHONE_OPEN_MON_THU}<br>" +
                                    //$"{PHONE_OPEN_FRE}<br><br>" +
                                    //$"{PHONE_OPEN_SAT_SUN_HOLY}"
            */
                                    , HtmlCompat.FromHtmlModeLegacy);
            textField.ContentDescriptionFormatted =
                HtmlCompat.FromHtml($"{CONTENT_TEXT_BEFORE_SUPPORT_LINK} <a href=\"https://{SUPPORT_LINK}\">{SUPPORT_LINK_SHOWN_TEXT}</a><br><br>"
            /* Note:
            This functionality is not planned for release 1.0. Kept for future use.
                                    //$"{EMAIL_TEXT} <a href=\"mailto:{EMAIL}\">{EMAIL}</a> {PHONE_NUM_Text} <a href=\"tel:{PHONE_NUM}\">{PHONE_NUM_ACCESSIBILITY}</a>.<br><br>" +
                                    //$"{PHONE_OPEN_TEXT}<br><br>" +
                                    //$"{PHONE_OPEN_MON_THU_ACCESSIBILITY}<br>" +
                                    //$"{PHONE_OPEN_FRE_ACCESSIBILITY}<br><br>" +
                                    //$"{PHONE_OPEN_SAT_SUN_HOLY}"
            */
                                    , HtmlCompat.FromHtmlModeLegacy);
            textField.MovementMethod = LinkMovementMethod.Instance;
            backButton.Click += new SingleClick((sender, args) => Finish()).Run;

            backButton.SetBackgroundResource(LayoutUtils.GetBackArrow());
        }
    }
}
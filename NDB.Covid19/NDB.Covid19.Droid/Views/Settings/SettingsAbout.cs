using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullSensor, LaunchMode = LaunchMode.SingleTop, WindowSoftInputMode = SoftInput.AdjustResize)]
    class SettingsAbout : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = SettingsPage5ViewModel.SETTINGS_PAGE_5_HEADER;
            SetContentView(Resource.Layout.settings_about);
            Init();
        }

        void Init()
        {
            ImageButton backButton = FindViewById<ImageButton>(Resource.Id.arrow_back_about);
            backButton.ContentDescription = ViewModels.SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView titleField = FindViewById<TextView>(Resource.Id.settings_about_title);
            TextView textField = FindViewById<TextView>(Resource.Id.settings_about_text);
            TextView hiddenLink = FindViewById<TextView>(Resource.Id.settings_about_link);
            FindViewById<TextView>(Resource.Id.settings_about_version_info_textview).Text = ViewModels.SettingsPage5ViewModel.GetVersionInfo();

            titleField.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_HEADER;
            titleField.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            textField.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_CONTENT +
                             $" {SettingsPage5ViewModel.SETTINGS_PAGE_5_LINK}";

            backButton.Click += new SingleClick((sender, args) => Finish()).Run;

            hiddenLink.Text = SettingsPage5ViewModel.SETTINGS_PAGE_5_LINK;

            LinkUtil.LinkifyTextView(hiddenLink);
        }
    }
}
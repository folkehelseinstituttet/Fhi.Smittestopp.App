using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using NDB.Covid19.Droid.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class SettingsActivity : AppCompatActivity
    {
        private static readonly ViewModels.SettingsViewModel _settingsViewModel = new ViewModels.SettingsViewModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.settings_page);
            Init();
        }

        void Init()
        {
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
            ConstraintLayout settingsIntroLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_intro_frame);
            ConstraintLayout howItWorksLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_saddan_frame);
            ConstraintLayout gdprLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_behandling_frame);
            ConstraintLayout helpLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_hjaelp_frame);
            ConstraintLayout aboutLayout = FindViewById<ConstraintLayout>(Resource.Id.om_frame);
            ConstraintLayout deploymentLayout = FindViewById<ConstraintLayout>(Resource.Id.test_frame);
            ConstraintLayout generalLayout = FindViewById<ConstraintLayout>(Resource.Id.general_settings);

            TextView settingsIntroButton = settingsIntroLayout.FindViewById<TextView>(Resource.Id.settings_link_text);
            TextView howItWorksButton = howItWorksLayout.FindViewById<TextView>(Resource.Id.settings_link_text);
            TextView gdprButton = gdprLayout.FindViewById<TextView>(Resource.Id.settings_link_text);
            TextView helpButton = helpLayout.FindViewById<TextView>(Resource.Id.settings_link_text);
            TextView aboutButton = aboutLayout.FindViewById<TextView>(Resource.Id.settings_link_text);
            TextView generalButton = generalLayout.FindViewById<TextView>(Resource.Id.settings_general_link_text);
            TextView deploymentButton = deploymentLayout.FindViewById<TextView>(Resource.Id.settings_link_text);

            ImageView _fhiLogo;
            _fhiLogo = FindViewById<ImageView>(Resource.Id.settings_icon_imageview);
            _fhiLogo.ContentDescription = ViewModels.InfectionStatusViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;

            settingsIntroButton.Text = _settingsViewModel.SettingItemList[0].Text;
            settingsIntroButton.TextAlignment = TextAlignment.ViewStart;
            howItWorksButton.Text = _settingsViewModel.SettingItemList[1].Text;
            howItWorksButton.TextAlignment = TextAlignment.ViewStart;
            gdprButton.Text = _settingsViewModel.SettingItemList[2].Text;
            gdprButton.TextAlignment = TextAlignment.ViewStart;
            helpButton.Text = _settingsViewModel.SettingItemList[3].Text;
            helpButton.TextAlignment = TextAlignment.ViewStart;
            aboutButton.Text = _settingsViewModel.SettingItemList[4].Text;
            aboutButton.TextAlignment = TextAlignment.ViewStart;
            generalButton.Text = _settingsViewModel.SettingItemList[5].Text;
            generalButton.TextAlignment = TextAlignment.ViewStart;

            if (_settingsViewModel.ShowDebugItem)
            {
                deploymentButton.Text = _settingsViewModel.SettingItemList[6].Text;
                deploymentLayout.Visibility = ViewStates.Visible;
            }

            ViewGroup closeButton = FindViewById<ViewGroup>(Resource.Id.ic_close_white);
            closeButton.ContentDescription = ViewModels.SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            closeButton.AccessibilityTraversalAfter = Resource.Id.settings_general_link_text;
            closeButton.Click += new SingleClick((sender, e) => Finish()).Run;
            settingsIntroButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToOnBoarding(this, false)).Run;
            howItWorksButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToSettingsHowItWorksPage(this)).Run;
            helpButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToSettingsHelpPage(this)).Run;
            aboutButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToSettingsAboutPage(this)).Run;
            gdprButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToConsentsWithdrawPage(this)).Run;
            deploymentButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToDebugPage(this)).Run;
            generalButton.Click += new SingleClick((sender, args) => NavigationHelper.GoToGenetalSettingsPage(this)).Run;
        }
    }
}
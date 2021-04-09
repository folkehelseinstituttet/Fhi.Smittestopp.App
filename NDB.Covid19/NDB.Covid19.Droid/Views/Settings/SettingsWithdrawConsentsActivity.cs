using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class SettingsWithdrawConsentsActivity : AppCompatActivity
    {
        Button _resetConsentsButton;
        ProgressBar _progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE;
            SetContentView(Resource.Layout.settings_consents);
            Init();
        }

        void Init()
        {
            ImageButton backButton = FindViewById<ImageButton>(Resource.Id.arrow_back);
            backButton.ContentDescription = ViewModels.SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            _resetConsentsButton = FindViewById<Button>(Resource.Id.buttonResetConsents);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.consentActivityIndicator);

            TextView header = FindViewById<TextView>(Resource.Id.welcome_page_five_title);

            header.Text = ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE;
            header.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            _resetConsentsButton.Text = ConsentViewModel.WITHDRAW_CONSENT_BUTTON_TEXT;
            backButton.Click += new SingleClick((sender, args) => Finish()).Run;
            _resetConsentsButton.Click += new SingleClick(ResetButtonToggled).Run;

            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
            backButton.SetBackgroundResource(LayoutUtils.GetBackArrow());
        }

        private async void ResetButtonToggled(object sender, EventArgs e)
        {
            ShowSpinner(true);

            await DialogUtils.DisplayDialogAsync(this,
                new DialogViewModel()
                {
                    Title = ConsentViewModel.CONSENT_REMOVE_TITLE,
                    Body = ConsentViewModel.CONSENT_REMOVE_MESSAGE,
                    OkBtnTxt = ConsentViewModel.CONSENT_OK_BUTTON_TEXT,
                    CancelbtnTxt = ConsentViewModel.CONSENT_NO_BUTTON_TEXT
                },
                PerformWithdrawAsync,
                (() => ShowSpinner(false)));
        }

        private void PerformWithdrawAsync()
        {
            DeviceUtils.StopScanServices();
            DeviceUtils.CleanDataFromDevice();
            NavigationHelper.RestartApp(this);
            ShowSpinner(false);
        }
        private void ShowSpinner(bool show)
        {
            _resetConsentsButton.Enabled = !show;
            _resetConsentsButton.Visibility = show ? ViewStates.Invisible : ViewStates.Visible;
            _progressBar.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}
using Android.App;
using Android.OS;
using Android.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.PersistedData;
using System;
using Android.Content.PM;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.Views
{
    [Activity(MainLauncher = true, Theme = "@style/AppTheme.Launcher", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class InitializerActivity : Activity
    {
        Button _launcherButton;
        Button _launcherButtonNynorsk;
        TextView _continueInEnTextView;
        RelativeLayout _continueInEnRelativeLayoutButton;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (!IsTaskRoot)
            {
                Finish();
            }

            base.OnCreate(savedInstanceState);


            if (ConsentsHelper.IsNotFullyOnboarded)
            {
                NavigationHelper.GoToWelcomeWhatsNewPage(this);
                Finish();
                return;
            }

            SetContentView(Resource.Layout.layout_with_launcher_button_ag_api);
            _launcherButton = FindViewById<Button>(Resource.Id.launcher_button);
            _launcherButtonNynorsk = FindViewById<Button>(Resource.Id.launcher_button_nynorsk);
            _continueInEnRelativeLayoutButton = FindViewById<RelativeLayout>(Resource.Id.continue_in_en_layout);
            _continueInEnTextView = FindViewById<TextView>(Resource.Id.continue_in_en_text);

            _launcherButton.Text = InitializerViewModel.LAUNCHER_PAGE_START_BTN;
            _launcherButtonNynorsk.Text = InitializerViewModel.LAUNCHER_PAGE_START_BTN_NYNORSK;
            _continueInEnTextView.Text = InitializerViewModel.LAUNCHER_PAGE_CONTINUE_IN_ENG;
            
            _launcherButton.Click += new SingleClick(LauncherButton_Click).Run;
            _launcherButtonNynorsk.Click += new SingleClick(LauncherButtonNynorsk_Click).Run;
            _continueInEnRelativeLayoutButton.Click += new SingleClick(CountinueInEnButton_Click).Run;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                NavigationHelper.GoToStartPageIfIsOnboarded(this);
            }
            else
            {
                ShowOutdatedGPSDialog();
            }
        }

        private void LauncherButton_Click(object sender, EventArgs e)
        {
            LocalPreferencesHelper.SetAppLanguage(Conf.DEFAULT_LANGUAGE);
            LocalesService.Initialize();
            Continue();
        }
        
        private void LauncherButtonNynorsk_Click(object sender, EventArgs e)
        {
            LocalPreferencesHelper.SetAppLanguage("nn");
            LocalesService.Initialize();
            Continue();
        }

        private void ShowOutdatedGPSDialog()
        {
            DialogUtils.DisplayDialogAsync(
                this,
                "BASE_ERROR_TITLE".Translate(),
                "LAUNCHER_PAGE_GPS_VERSION_DIALOG_MESSAGE_ANDROID".Translate(),
                "ERROR_OK_BTN".Translate()
            );
        }

        private void Continue()
        {
            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                NavigationHelper.GoToOnBoarding(this, true);
            }
            else
            {
                ShowOutdatedGPSDialog();
            }
        }

        private void CountinueInEnButton_Click(object sender, EventArgs e)
        {
            LocalPreferencesHelper.SetAppLanguage("en");
            LocalesService.Initialize();
            Continue();
        }
    }
}
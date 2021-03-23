using Android.App;
using Android.OS;
using Android.Widget;
using NDB.Covid19.Droid.Utils;
using System;
using Android.Content.PM;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;
using I18NPortable;
using NDB.Covid19.Utils;
using Android.Views;
using AndroidX.Core.Content;
using Android.Graphics;

namespace NDB.Covid19.Droid.Views
{
    [Activity(MainLauncher = true, Theme = "@style/AppTheme.Launcher", ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class InitializerActivity : Activity
    {
        ImageView _fhiLogo;
        ImageView _appLogo;
        Button _launcherButton;
        
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

            _fhiLogo = FindViewById<ImageView>(Resource.Id.launcer_icon_imageview);
            _appLogo = FindViewById<ImageView>(Resource.Id.app_logo);

            _fhiLogo.ContentDescription = InitializerViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            _appLogo.ContentDescription = InitializerViewModel.SMITTESPORING_APP_LOGO_ACCESSIBILITY;

            _launcherButton = FindViewById<Button>(Resource.Id.launcher_button);

            _launcherButton.Text = InitializerViewModel.LAUNCHER_PAGE_START_BTN;
            
            _launcherButton.Click += new SingleClick(LauncherButton_Click).Run;

            Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorPrimary)));
            Window.DecorView.SystemUiVisibility &= (StatusBarVisibility) ~SystemUiFlags.LightStatusBar;
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
                NavigationHelper.GoToLanguageSelection(this);
            }
            else
            {
                ShowOutdatedGPSDialog();
            }
        }
    }
}
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Nio.FileNio;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.Droid.Utils;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    class LoadingPageActivity : AppCompatActivity
    {
        bool _isRunning;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE;
            SetContentView(Resource.Layout.loading_page);
            FindViewById<ProgressBar>(Resource.Id.progress_bar).Visibility = ViewStates.Visible;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!_isRunning)
            {
                StartPushActivity();
                _isRunning = true;
            }
        }

        async void StartPushActivity()
        {
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.SubmitSelfDiagnosisAsync();
                LogUtils.LogMessage(LogSeverity.INFO, "Successfully pushed keys to server");
                OnActivityFinished();
            }
            catch(Exception e)
            {
                OnError(e);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Xamarin.ExposureNotifications.ExposureNotification.OnActivityResult(requestCode, resultCode, data);
        }

        void OnActivityFinished()
        {
            RunOnUiThread(() =>
            {
                StartActivity(new Intent(this, typeof(RegisteredActivity)));
            });
            
        }

        void OnError(Exception e)
        {
            if (e is AccessDeniedException)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user refused to share keys", null);
                GoToInfectionStatusPage();
            }
            else
            {
                RunOnUiThread(() => AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, e, "Pushing keys failed"));
            }
        }

        private void GoToInfectionStatusPage() => NavigationHelper.GoToResultPageAndClearTop(this);

        public override void OnBackPressed()
        {
            // Disabled back button
        }
    }
}
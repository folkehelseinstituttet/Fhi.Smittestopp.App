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
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class LoadingPageActivity : AppCompatActivity
    {
        bool _isRunning;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE;
            SetContentView(Resource.Layout.loading_page);
            FindViewById<ProgressBar>(Resource.Id.progress_bar).Visibility = ViewStates.Visible;
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
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
                LogUtils.LogMessage(LogSeverity.INFO, "The user agreed to share keys", null, GetCorrelationId());
                OnActivityFinished();
            }
            catch(Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(LoadingPageActivity), nameof(StartPushActivity));
                OnError(e);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                Xamarin.ExposureNotifications.ExposureNotification.OnActivityResult(requestCode, resultCode, data);
            }
            catch (Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(LoadingPageActivity), nameof(OnActivityResult));
            }
            
        }

        void OnActivityFinished()
        {
            RunOnUiThread(() =>
            {
                StartActivity(new Intent(this, typeof(RegisteredActivity)));
            });
            
        }

        void OnError(Exception e, bool isOnFail = false)
        {
            if (e is AccessDeniedException)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user refused to share keys", null, GetCorrelationId());
                RunOnUiThread(() => { GoToInfectionStatusPage(); });
            }
            else
            {
                if (!isOnFail)
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "Something went wrong during key sharing",
                        e.Message,
                        GetCorrelationId());
                }
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
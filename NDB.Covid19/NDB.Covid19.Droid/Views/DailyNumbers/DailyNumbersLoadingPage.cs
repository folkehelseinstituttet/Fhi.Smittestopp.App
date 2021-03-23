using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.DailyNumbers
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class DailyNumbersLoadingActivity : AppCompatActivity
    {
        bool _isRunning;
        private DailyNumbersViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE;
            SetContentView(Resource.Layout.loading_page);
            _viewModel = new DailyNumbersViewModel();
            FindViewById<ProgressBar>(Resource.Id.progress_bar).Visibility = ViewStates.Visible;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!_isRunning)
            {
                LoadDataAndStartDiseaseRateActivity();
                _isRunning = true;
            }
        }

        async void LoadDataAndStartDiseaseRateActivity()
        {
            try
            {
                var isSuccess = await DailyNumbersViewModel.UpdateFHIDataAsync();
                if (!isSuccess && LocalPreferencesHelper.HasNeverSuccessfullyFetchedFHIData)
                {
                    OnError(new NullReferenceException("No FHI data"));
                    return;
                }
                LogUtils.LogMessage(LogSeverity.INFO, "Data for the disease rate of the day is loaded");
                OnActivityFinished();
            }
            catch (Exception e)
            {
                if (!IsFinishing)
                {
                    OnError(e);
                }
            }
        }

        void OnActivityFinished()
        {
            RunOnUiThread(() =>
            {
                StartActivity(new Intent(this, typeof(DailyNumbersActivity)));
                Finish();
            });
        }

        void OnError(Exception e)
        {
            if (LocalPreferencesHelper.HasNeverSuccessfullyFetchedFHIData)
            {
                RunOnUiThread(() => AuthErrorUtils.GoToTechnicalErrorFHINumbers(this, LogSeverity.ERROR, e, "Could not load data for disease rate of the day, showing technical error page"));
            }
            else
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "Could not load data for disease rate of the day, showing old data");
                RunOnUiThread(() => StartActivity(new Intent(this, typeof(DailyNumbersActivity))));
            }
        }
    }
}
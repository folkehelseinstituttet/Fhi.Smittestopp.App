
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using NDB.Covid19.Droid.Services;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.QuestionnaireViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
         ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class QuestionnaireTestOptionActivity : AppCompatActivity
    {
        private Button _closeButton;
        private QuestionnaireViewModel _questionnaireViewModel;
        private Button _msisTestButton;
        private Button _selfTestButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = REGISTER_QUESTIONAIRE_CHOOSE_OPTION;
            SetContentView(Resource.Layout.questionnaire_test_option);
            Init();
            if (!RemovedFromRecentDetectorService.IsRunning)
            {
                StartService(new Intent(this, typeof(RemovedFromRecentDetectorService)));
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire", null, GetCorrelationId());
        }

        private void Init()
        {
            // Generate and set correlation id for current authentication flow
            UpdateCorrelationId(LogUtils.GenerateCorrelationId());

            _questionnaireViewModel = new QuestionnaireViewModel();

            TextView questionnaireTitle = FindViewById<TextView>(Resource.Id.questionnaire_title);
            questionnaireTitle.Text = REGISTER_QUESTIONAIRE_CHOOSE_OPTION;
            questionnaireTitle.ContentDescription = REGISTER_QUESTIONAIRE_CHOOSE_OPTION;
            questionnaireTitle.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            ViewGroup close = FindViewById<ViewGroup>(Resource.Id.close_cross_btn);
            close.Click += new SingleClick((o, ev) => {
                NavigationHelper.GoToResultPageAndClearTop(this);
            }).Run;

            _msisTestButton = FindViewById<Button>(Resource.Id.questionnaire_button_msis_test);
            _msisTestButton.ContentDescription = REGISTER_QUESTIONAIRE_MSIS_TEST_BUTTON_TEXT;
            _msisTestButton.Text = REGISTER_QUESTIONAIRE_MSIS_TEST_BUTTON_TEXT;
            _msisTestButton.Click += new SingleClick((o, ev) =>
            {
                GoToInformationAndConsentPage();
                HasTestedPositiveWithSelfTest = false;
            }).Run;

            _selfTestButton = FindViewById<Button>(Resource.Id.questionnaire_button_self_test);
            _selfTestButton.ContentDescription = REGISTER_QUESTIONAIRE_SELF_TEST_BUTTON_TEXT;
            _selfTestButton.Text = REGISTER_QUESTIONAIRE_SELF_TEST_BUTTON_TEXT;
            _selfTestButton.Click += new SingleClick((o, ev) =>
            {
                GoToInformationAndConsentPage();
                HasTestedPositiveWithSelfTest = true;
            }).Run;

            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
        }

        void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnFail)}: " +
                "AuthenticationState.personaldata was garbage collected (Android)");
        }

        void OnValidationFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnFail)}: " +
                "AuthenticationState.personaldata is not valid (Android)");
        }

        private void GoToInfectionStatusPage() => NavigationHelper.GoToResultPageAndClearTop(this);

        private void GoToInformationAndConsentPage() => StartActivity(new Intent(this, typeof(InformationAndConsentActivity)));
    }
}

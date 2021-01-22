using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.CountriesConsentViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class CountriesConsentActivity: AppCompatActivity
    {
        private ViewGroup _closeButton;
        private TextView _header;
        private TextView _bodytext1;
        private TextView _bodytext2;
        private TextView _consentEUExplanation;
        private TextView _consentNorwayExplanation;
        private Button _consentButtonEU;
        private Button _consentButtonOnlyNorway;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = HEADER_TEXT;
            SetContentView(Resource.Layout.countries_consent);
            InitLayout();
        }

        private void InitLayout()
        {
            //Buttons
            _closeButton = FindViewById<ViewGroup>(Resource.Id.close_cross_btn);
            _consentButtonEU = FindViewById<Button>(Resource.Id.consentButton_EU);
            _consentButtonOnlyNorway = FindViewById<Button>(Resource.Id.consentButton_onlyNorway);

            //TextViews
            _header = FindViewById<TextView>(Resource.Id.header_textView);
            _bodytext1 = FindViewById<TextView>(Resource.Id.bodytext1);
            _bodytext2 = FindViewById<TextView>(Resource.Id.bodytext2);
            _consentEUExplanation = FindViewById<TextView>(Resource.Id.consent_EU_explanation);
            _consentNorwayExplanation = FindViewById<TextView>(Resource.Id.consent_onlyNorway_explanation);

            //Text initialization
            _consentButtonEU.Text = EU_CONSENT_NEXT_EU_CONSENT_BUTTON_TEXT;
            _consentButtonOnlyNorway.Text = EU_CONSENT_NEXT_ONLY_NORWAY_CONSENT_BUTTON_TEXT;
            _header.Text = HEADER_TEXT;
            _bodytext1.Text = CONSENT3_BODYTEXT_1;
            _bodytext2.Text = CONSENT3_BODYTEXT_2;
            _consentEUExplanation.Text = CONSENT3_EU_CONSENT_BUTTON_BODYTEXT;
            _consentNorwayExplanation.Text = CONSENT3_ONLY_NORWAY_CONSENT_BUTTON_BODYTEXT;

            ////Accessibility
            _closeButton.ContentDescription = CLOSE_BUTTON_ACCESSIBILITY_LABEL;

            //Button click events
            _closeButton.Click += new StressUtils.SingleClick((sender, e) => ShowAbortDialog(), 500).Run;
            _consentButtonEU.Click += new StressUtils.SingleClick((o, args) =>
            {
                InvokeNextButtonClick(GoToCountriesQuestionnairePage, OnFail, true);
            }, 500).Run;
            _consentButtonOnlyNorway.Click += new StressUtils.SingleClick((o, args) =>
            {
                InvokeNextButtonClick(GoToLoadingPage, OnFail, false);
            }, 500).Run;
        }

        private void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(CountriesConsentActivity)}.{nameof(OnFail)}: " +
                "AuthenticationState.PersonalData was garbage collected (Android)");
        }
        
        private void GoToLoadingPage() =>
            StartActivity(new Intent(this, typeof(LoadingPageActivity)));

        private void GoToCountriesQuestionnairePage()
        {
            RunOnUiThread(() =>
            {
                Intent intent = new Intent(this, typeof(QuestionnaireCountriesSelectionActivity));
                StartActivity(intent);
            });
        }
        
        private async void ShowAbortDialog()
        {
            await DialogUtils.DisplayDialogAsync(
                this,
                AbortDuringEUConsentViewModel,
                GoToLoadingPage,
                null
            );
        }
    }
}
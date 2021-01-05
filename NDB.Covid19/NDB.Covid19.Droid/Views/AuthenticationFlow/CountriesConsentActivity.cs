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
using static NDB.Covid19.ViewModels.CountriesConsentViewModel;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class CountriesConsentActivity: AppCompatActivity
    {
        private ViewGroup _closeButton;
        private TextView _header;
        private TextView _consentDescriptionText;
        private TextView _lookupHeader;
        private TextView _lookupText;
        private TextView _notificationHeader;
        private TextView _notificationText;
        private TextView _beAwareText;
        private TextView _consentExplanationText;
        private Button _consentButton;
        
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
            _consentButton = FindViewById<Button>(Resource.Id.consent_button);

            //TextViews
            _header = FindViewById<TextView>(Resource.Id.header_textView);
            _consentDescriptionText = FindViewById<TextView>(Resource.Id.contentDescription_textView);
            _lookupHeader = FindViewById<TextView>(Resource.Id.lookup_header_textview);
            _lookupText = FindViewById<TextView>(Resource.Id.lookup_text_textview);
            _notificationHeader = FindViewById<TextView>(Resource.Id.notification_header_textview);
            _notificationText = FindViewById<TextView>(Resource.Id.notification_text_textview);
            _beAwareText = FindViewById<TextView>(Resource.Id.beAware_text_textview);
            _consentExplanationText = FindViewById<TextView>(Resource.Id.explanation_text_textview);

            //Text initialization
            _consentButton.Text = BUTTON_TEXT;
            _header.Text = HEADER_TEXT;
            _consentDescriptionText.TextFormatted = HtmlCompat.FromHtml($"{COUNTRIES_CONSENT_BE_AWARE_TEXT_DESCRIPTION}", HtmlCompat.FromHtmlModeLegacy);
            _lookupHeader.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_HEADER;
            _lookupText.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_TEXT;
            _notificationHeader.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_HEADER;
            _notificationText.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_TEXT;
            _beAwareText.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_BE_AWARE_TEXT;
            _consentExplanationText.Text = COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_EXPLANATION_TEXT;

            ////Accessibility
            _closeButton.ContentDescription = CLOSE_BUTTON_ACCESSIBILITY_LABEL;

            //Button click events
            _closeButton.Click += new StressUtils.SingleClick((sender, e) => ShowAreYouSureToDenyConsentDialog(), 500).Run;
            _consentButton.Click += new StressUtils.SingleClick((o, args) =>
            {
                InvokeNextButtonClick(GoToCountriesQuestionnairePage, OnFail, true);
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
        
        private async void ShowAreYouSureToDenyConsentDialog()
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(
                this,
                DENY_COUNTRIES_CONSENT_DIALOG_HEADER,
                DENY_COUNTRIES_CONSENT_DIALOG_MESSAGE,
                DENY_COUNTRIES_CONSENT_DIALOG_OK,
                DENY_COUNTRIES_CONSENT_DIALOG_CANCEL);
            if (isOkPressed)
            {
                InvokeNextButtonClick(GoToLoadingPage, OnFail, false);
            }
        }        
    }
}
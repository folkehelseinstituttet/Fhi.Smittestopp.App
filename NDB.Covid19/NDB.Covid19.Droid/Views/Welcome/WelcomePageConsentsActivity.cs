using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using static NDB.Covid19.Droid.Utils.StressUtils;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;
using AndroidX.Core.Text;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.Views.Welcome
{

    [Activity(Label = "", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class WelcomePageConsentsActivity: AppCompatActivity
    {
        public event EventHandler<Boolean> ButtonPressed;

        private SwitchCompat _switchCustom;
        private LinearLayout _consentWarning;
        private TextView _consentWarningTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.welcome_page_five);

            LinearLayout headerLayout = FindViewById<LinearLayout>(Resource.Id.consent_header);
            TextView header = headerLayout.FindViewById<TextView>(Resource.Id.welcome_page_five_title);
            header.Text = ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE;

            Button previousButton = FindViewById<Button>(Resource.Id.welcome_page_five_prev_button);
            previousButton.Click += new SingleClick(PreviousButtonPressed, 500).Run;
            previousButton.Text = WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT;

            Button nextButton = FindViewById<Button>(Resource.Id.welcome_page_five_button_next);
            nextButton.Click += new SingleClick(NextButtonPressed, 500).Run;
            nextButton.Text = WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT;

            _switchCustom = FindViewById<SwitchCompat>(Resource.Id.welcome_page_five_switch);
            _switchCustom.CheckedChange += OnCheckedChange;
            _switchCustom.ContentDescription = ConsentViewModel.SWITCH_ACCESSIBILITY_CONSENT_SWITCH_DESCRIPTOR;

            _consentWarning = FindViewById<LinearLayout>(Resource.Id.welcome_page_five_consent_warning);
            SetConsentWarningShown(false);
            _consentWarningTextView = FindViewById<TextView>(Resource.Id.welcome_page_five_consent_warning_text);
            _consentWarningTextView.Text = ConsentViewModel.CONSENT_REQUIRED;

            TextView consentTextView = FindViewById<TextView>(Resource.Id.welcome_page_five_switch_text);
            consentTextView.Text = ConsentViewModel.GIVE_CONSENT_TEXT;
            consentTextView.LabelFor = _switchCustom.Id;

            RelativeLayout RelativeLayout1 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_frivillig_brug);
            RelativeLayout RelativeLayout2 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_sadan_fungerer_appen);
            RelativeLayout RelativeLayout3 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvad_registreres);
            RelativeLayout RelativeLayout4 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvordan_accepterer);
            RelativeLayout RelativeLayout5 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_ret);
            RelativeLayout RelativeLayout6 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_kontaktregistringer);
            RelativeLayout RelativeLayout7 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_mere);
            RelativeLayout RelativeLayout8 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_behandlingen);
            RelativeLayout RelativeLayout9 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_aendringer);

            TextView consentOneTitle = RelativeLayout1.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentTwoTitle = RelativeLayout2.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentThreeTitle = RelativeLayout3.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentFourTitle = RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentFiveTitle = RelativeLayout5.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentSixTitle = RelativeLayout6.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentSevenTitle = RelativeLayout7.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentEightTitle = RelativeLayout8.FindViewById<TextView>(Resource.Id.consent_page_title);
            TextView consentNineTitle = RelativeLayout9.FindViewById<TextView>(Resource.Id.consent_page_title);

            consentOneTitle.Text = ConsentViewModel.CONSENT_ONE_TITLE;
            consentTwoTitle.Text = ConsentViewModel.CONSENT_TWO_TITLE;
            consentThreeTitle.Text = ConsentViewModel.CONSENT_THREE_TITLE;
            consentFourTitle.Text = ConsentViewModel.CONSENT_FOUR_TITLE;
            consentFiveTitle.Text = ConsentViewModel.CONSENT_FIVE_TITLE;
            consentSixTitle.Text = ConsentViewModel.CONSENT_SIX_TITLE;
            consentSevenTitle.Text = ConsentViewModel.CONSENT_SEVEN_TITLE;
            consentEightTitle.Text = ConsentViewModel.CONSENT_EIGHT_TITLE;
            consentNineTitle.Text = ConsentViewModel.CONSENT_NINE_TITLE;

            consentOneTitle.ContentDescription = ConsentViewModel.CONSENT_ONE_TITLE.ToLower();
            consentTwoTitle.ContentDescription = ConsentViewModel.CONSENT_TWO_TITLE.ToLower();
            consentThreeTitle.ContentDescription = ConsentViewModel.CONSENT_THREE_TITLE.ToLower();
            consentFourTitle.ContentDescription = ConsentViewModel.CONSENT_FOUR_TITLE.ToLower();
            consentFiveTitle.ContentDescription = ConsentViewModel.CONSENT_FIVE_TITLE.ToLower();
            consentSixTitle.ContentDescription = ConsentViewModel.CONSENT_SIX_TITLE.ToLower();
            consentSevenTitle.ContentDescription = ConsentViewModel.CONSENT_SEVEN_TITLE.ToLower();
            consentEightTitle.ContentDescription = ConsentViewModel.CONSENT_EIGHT_TITLE.ToLower();
            consentNineTitle.ContentDescription = ConsentViewModel.CONSENT_NINE_TITLE.ToLower();

            RelativeLayout1.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_ONE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout2.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_TWO_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout3.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_THREE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FOUR_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout5.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FIVE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout6.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_SIX_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout7.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_SEVEN_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout8.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_EIGHT_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            RelativeLayout9.FindViewById<TextView>(Resource.Id.consent_page_text).TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_NINE_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);

            Button policyLinkBtn = FindViewById<Button>(Resource.Id.consent_paragraph_policy_btn);
            policyLinkBtn.Text = ConsentViewModel.CONSENT_SEVEN_BUTTON_TEXT;
            policyLinkBtn.Click += PolicyLinkBtn_Click;
        }

        private void PolicyLinkBtn_Click(object sender, EventArgs e)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdatePadding();
        }

        private void UpdatePadding()
        {
            ConstraintLayout checkboxLayout = FindViewById<ConstraintLayout>(Resource.Id.checkbox_layout);
            LinearLayout consentInfoLayout = FindViewById<LinearLayout>(Resource.Id.consent_info_layout);
            // Post method moves setting padding to the end of action queue for consent_info_layout and sets padding when
            // height of checkbox_layout will be known. In this way we have dynamic padding.
            consentInfoLayout.Post(() => consentInfoLayout.SetPadding(
                consentInfoLayout.PaddingLeft,
                consentInfoLayout.PaddingTop,
                consentInfoLayout.PaddingRight,
                checkboxLayout.Height));
        }

        public bool IsChecked() => _switchCustom != null && _switchCustom.Checked;

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            EventHandler<Boolean> handler = ButtonPressed;
            handler?.Invoke(this, e.IsChecked);

            _switchCustom.AnnounceForAccessibility(_switchCustom.Checked
                    ? ConsentViewModel.SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_GIVEN
                    : ConsentViewModel.SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_NOT_GIVEN);

            if (_switchCustom.Checked)
            {
                SetConsentWarningShown(false);
            }
        }

        private void SetConsentWarningShown(bool shown)
        {
            _consentWarning.Visibility = shown ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;

            if (shown)
            {
                _consentWarningTextView.SendAccessibilityEvent(Android.Views.Accessibility.EventTypes.ViewAccessibilityFocused);
            }
            UpdatePadding();
        }
        
        private void PreviousButtonPressed(object sender, EventArgs eventArgs)
        {
            if (!Conf.IsReleaseOne &&
                OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
            {
                OnBackPressed();
                return;
            }
            Finish();
        }

        private void NextButtonPressed(object sender, EventArgs eventArgs)
        {
            if (IsChecked())
            {
                OnboardingStatusHelper.Status = Conf.IsReleaseOne ?
                    OnboardingStatus.OnlyMainOnboardingCompleted :
                    OnboardingStatus.CountriesOnboardingCompleted;
                NavigationHelper.GoToResultPageAndClearTop(this);
            }
            else
            {
                SetConsentWarningShown(true);
            }
        }

        public override void OnBackPressed()
        {
            if (!Conf.IsReleaseOne &&
                OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
            {
                NavigationHelper.GoToWelcomeWhatsNewPage(this);
                return;
            }
            base.OnBackPressed();
        }
    }
}

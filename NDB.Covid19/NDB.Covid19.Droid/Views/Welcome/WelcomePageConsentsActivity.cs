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
using NDB.Covid19.Utils;
using Android.Views;
using Android.Text.Util;

namespace NDB.Covid19.Droid.Views.Welcome
{

    [Activity(Label = "", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class WelcomePageConsentsActivity: AppCompatActivity
    {
        public event EventHandler<Boolean> ButtonPressed;

        private SwitchCompat _switchCustom;
        private LinearLayout _consentWarning;
        private TextView _consentWarningTextView;
        TextView _aboutHeader;
        TextView _aboutText1;
        TextView _aboutText2;
        TextView _howItWorksHeader;
        TextView _howItWorksText1;
        TextView _howItWorksText2;
        TextView _howItWorksText3;
        TextView _samtykkeText1;
        TextView _samtykkeText2;
        TextView _processingofpersonaldataText1;
        TextView _processingofpersonaldataText2;
        TextView _processingofpersonaldataText3;
        TextView _policyLinkText;
        TextView _samtykkebottomHeader;
        TextView _samtykkebottomText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.welcome_page_five);
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();

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

            //ABOUT
            _aboutHeader = FindViewById<TextView>(Resource.Id.consent1_about_header);
            _aboutHeader.Text = ConsentViewModel.CONSENT_ONE_TITLE;
            _aboutText1 = FindViewById<TextView>(Resource.Id.consent1_about_text_section1);
            _aboutText1.Text = ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_ONE;
            _aboutText2 = FindViewById<TextView>(Resource.Id.consent1_about_text_section2);
            _aboutText2.Text = ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_TWO;

            //HOW IT WORKS
            _howItWorksHeader = FindViewById<TextView>(Resource.Id.consent1_howitworks_header);
            _howItWorksHeader.Text = ConsentViewModel.CONSENT_TWO_TITLE;
            _howItWorksText1 = FindViewById<TextView>(Resource.Id.consent1_howitworks_text_section1);
            _howItWorksText1.Text = ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_ONE;
            _howItWorksText2 = FindViewById<TextView>(Resource.Id.consent1_howitworks_text_section2);
            _howItWorksText2.Text = ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_TWO;
            _howItWorksText3 = FindViewById<TextView>(Resource.Id.consent1_howitworks_text_section3);
            _howItWorksText3.Text = ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_THREE;

            //SAMTYKKE
            _samtykkeText1 = FindViewById<TextView>(Resource.Id.consent1_samtykke_text_section1);
            _samtykkeText1.Text = ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_ONE;
            _samtykkeText2 = FindViewById<TextView>(Resource.Id.consent1_samtykke_text_section2);
            _samtykkeText2.Text = ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_TWO;

            //BEHANDLING AF PERSONOPLYSNINGWER
            _processingofpersonaldataText1 = FindViewById<TextView>(Resource.Id.consent1_processingofpersonaldata_text_section1);
            _processingofpersonaldataText1.Text = ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_ONE;
            _processingofpersonaldataText2 = FindViewById<TextView>(Resource.Id.consent1_processingofpersonaldata_text_section2);
            _processingofpersonaldataText2.Text = ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_TWO;
            Linkify.AddLinks(_processingofpersonaldataText2, MatchOptions.EmailAddresses);
            _processingofpersonaldataText3 = FindViewById<TextView>(Resource.Id.consent1_processingofpersonaldata_text_section3);
            _processingofpersonaldataText3.Text = ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_THREE;

            //PERSONVERNERKLÆRINGEN LINK
            _policyLinkText = FindViewById<TextView>(Resource.Id.consent_paragraph_policy_link);
            _policyLinkText.TextFormatted = HtmlCompat.FromHtml($"<a href=\"{ConsentViewModel.CONSENT_SEVEN_LINK_URL}\">{ConsentViewModel.CONSENT_SEVEN_LINK_TEXT}</a>", HtmlCompat.FromHtmlModeLegacy);
            _policyLinkText.MovementMethod = new Android.Text.Method.LinkMovementMethod();


            //SAMTYKKE, BOTTOM
            _samtykkebottomHeader = FindViewById<TextView>(Resource.Id.consent1_samtykkebottom_header);
            _samtykkebottomHeader.Text = ConsentViewModel.CONSENT_SIX_TITLE;
            _samtykkebottomText = FindViewById<TextView>(Resource.Id.consent1_samtykkebottom_text);
            _samtykkebottomText.Text = ConsentViewModel.CONSENT_SIX_PARAGRAPH;


            RelativeLayout RelativeLayout4 = FindViewById<RelativeLayout>(Resource.Id.consent_paragraph_hvordan_accepterer);
            TextView contactInformation = RelativeLayout4.FindViewById<TextView>(Resource.Id.consent_page_text);
            contactInformation.TextAlignment = TextAlignment.ViewStart;
            contactInformation.TextFormatted = HtmlCompat.FromHtml(ConsentViewModel.CONSENT_FOUR_PARAGRAPH, HtmlCompat.FromHtmlModeLegacy);
            Linkify.AddLinks(contactInformation, MatchOptions.EmailAddresses | MatchOptions.PhoneNumbers);


            // CONTENT DESCRIPTIONS OF HEADER
            _aboutHeader.ContentDescription = ConsentViewModel.CONSENT_ONE_TITLE.ToLower();
            _howItWorksHeader.ContentDescription = ConsentViewModel.CONSENT_TWO_TITLE.ToLower();
            _samtykkebottomHeader.ContentDescription = ConsentViewModel.CONSENT_SIX_TITLE.ToLower();

            HeadingAccessibilityDelegate headingAccessibilityDelegate = AccessibilityUtils.GetHeadingAccessibilityDelegate();
            header.SetAccessibilityDelegate(headingAccessibilityDelegate);
            _aboutHeader.SetAccessibilityDelegate(headingAccessibilityDelegate);
            _howItWorksHeader.SetAccessibilityDelegate(headingAccessibilityDelegate);
            _samtykkebottomHeader.SetAccessibilityDelegate(headingAccessibilityDelegate);
        }

        private void PolicyLinkBtn_Click(object sender, EventArgs e)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }

        protected override void OnResume()
        {
            base.OnResume();
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
        }
        
        private void PreviousButtonPressed(object sender, EventArgs eventArgs)
        {
            if (ConsentsHelper.IsNotFullyOnboarded)
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
                OnboardingStatusHelper.Status = ConsentsHelper.GetStatusDependingOnRelease();
                NavigationHelper.GoToResultPageAndClearTop(this);
            }
            else
            {
                SetConsentWarningShown(true);
            }
        }

        public override void OnBackPressed()
        {
            if (ConsentsHelper.IsNotFullyOnboarded)
            {
                NavigationHelper.GoToWelcomeWhatsNewPage(this);
                return;
            }
            base.OnBackPressed();
        }
    }
}

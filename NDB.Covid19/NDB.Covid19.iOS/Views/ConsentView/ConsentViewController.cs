using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.ConsentView
{
    public partial class ConsentViewController : BaseViewController
    {
        ConsentViewModel _consentViewModel;
        public ConsentViewController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _consentViewModel = new ConsentViewModel();
            SetTexts();
            InitPrivacyPolicyButton();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetAccessibility();
            AcceptSwitchBtn.On = _consentViewModel.ConsentIsGiven;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        void SetTexts()
        {
            InitTitle(TitleLabel, ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE);
            InitSubTitle(Header1Lbl, ConsentViewModel.CONSENT_ONE_TITLE);
            InitBodyText(Paragraph1Lbl, ConsentViewModel.CONSENT_ONE_PARAGRAPH);
            InitSubTitle(Header2Lbl, ConsentViewModel.CONSENT_TWO_TITLE);
            InitBodyText(Paragraph2Lbl, ConsentViewModel.CONSENT_TWO_PARAGRAPH);
            InitSubTitle(Header3Lbl, ConsentViewModel.CONSENT_THREE_TITLE);
            InitBodyText(Paragraph3Lbl, ConsentViewModel.CONSENT_THREE_PARAGRAPH);
            InitSubTitle(Header4Lbl, ConsentViewModel.CONSENT_FOUR_TITLE);
            InitBodyText(Paragraph4Lbl, ConsentViewModel.CONSENT_FOUR_PARAGRAPH);
            InitSubTitle(Header5Lbl, ConsentViewModel.CONSENT_FIVE_TITLE);
            InitBodyText(Paragraph5Lbl, ConsentViewModel.CONSENT_FIVE_PARAGRAPH);
            InitSubTitle(Header6Lbl, ConsentViewModel.CONSENT_SIX_TITLE);
            InitBodyText(Paragraph6Lbl, ConsentViewModel.CONSENT_SIX_PARAGRAPH);
            InitSubTitle(Header7Lbl, ConsentViewModel.CONSENT_SEVEN_TITLE);
            InitBodyText(Paragraph7Lbl, ConsentViewModel.CONSENT_SEVEN_PARAGRAPH);
            InitSubTitle(Header8Lbl, ConsentViewModel.CONSENT_EIGHT_TITLE);
            InitBodyText(Paragraph8Lbl, ConsentViewModel.CONSENT_EIGHT_PARAGRAPH);
            InitSubTitle(Header9Lbl, ConsentViewModel.CONSENT_NINE_TITLE);
            InitBodyText(Paragraph9Lbl, ConsentViewModel.CONSENT_NINE_PARAGRAPH);
            InitButtonStyling(NextBtn, WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT);
            InitButtonSecondaryStyling(BackBtn, WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT);

            WarningLbl.Font = StyleUtil.Font(FontType.FontRegular, 12, 16);
            WarningLbl.Text = ConsentViewModel.CONSENT_REQUIRED;

            AcceptSwitchBtn.AccessibilityLabel = ConsentViewModel.SWITCH_ACCESSIBILITY_CONSENT_SWITCH_DESCRIPTOR;
            AcceptTextLbl.IsAccessibilityElement = false;
            AcceptTextLbl.Text = ConsentViewModel.GIVE_CONSENT_TEXT;

            ActivityIndicator.AccessibilityElementsHidden = true;
        }

        void SetAccessibility() {
            InvokeOnMainThread(() =>
            {
                SetAccessibilityText(TitleLabel, ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE);
                SetAccessibilityText(Header1Lbl, ConsentViewModel.CONSENT_ONE_TITLE);
                SetAccessibilityText(Paragraph1Lbl, ConsentViewModel.CONSENT_ONE_PARAGRAPH);
                SetAccessibilityText(Header2Lbl, ConsentViewModel.CONSENT_TWO_TITLE);
                SetAccessibilityText(Paragraph2Lbl, ConsentViewModel.CONSENT_TWO_PARAGRAPH);
                SetAccessibilityText(Header3Lbl, ConsentViewModel.CONSENT_THREE_TITLE);
                SetAccessibilityText(Paragraph3Lbl, ConsentViewModel.CONSENT_THREE_PARAGRAPH);
                SetAccessibilityText(Header4Lbl, ConsentViewModel.CONSENT_FOUR_TITLE);
                SetAccessibilityText(Paragraph4Lbl, ConsentViewModel.CONSENT_FOUR_PARAGRAPH);
                SetAccessibilityText(Header5Lbl, ConsentViewModel.CONSENT_FIVE_TITLE);
                SetAccessibilityText(Paragraph5Lbl, ConsentViewModel.CONSENT_FIVE_PARAGRAPH);
                SetAccessibilityText(Header6Lbl, ConsentViewModel.CONSENT_SIX_TITLE);
                SetAccessibilityText(Paragraph6Lbl, ConsentViewModel.CONSENT_SIX_PARAGRAPH);
                SetAccessibilityText(Header7Lbl, ConsentViewModel.CONSENT_SEVEN_TITLE);
                SetAccessibilityText(Paragraph7Lbl, ConsentViewModel.CONSENT_SEVEN_PARAGRAPH);
                SetAccessibilityText(Header8Lbl, ConsentViewModel.CONSENT_EIGHT_TITLE);
                SetAccessibilityText(Paragraph8Lbl, ConsentViewModel.CONSENT_EIGHT_PARAGRAPH);
                SetAccessibilityText(Header9Lbl, ConsentViewModel.CONSENT_NINE_TITLE);
                SetAccessibilityText(Paragraph9Lbl, ConsentViewModel.CONSENT_NINE_PARAGRAPH);
            });
            
        }

        protected void SetAccessibilityText(UILabel label, string text)
        {
            label.IsAccessibilityElement = true;
            label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
        }

        protected void InitTitle(UILabel label, string text)
        {
            InitLabelWithSpacing(label, FontType.FontBold, text, 1.14, 24, 26);
        }

        protected void InitBodyText(UILabel label, string text)
        {
            InitLabelWithHTMLFormat(label, text);
        }

        protected void InitBoxText(UILabel label, string text)
        {
            InitLabel(label, FontType.FontRegular, text, 16, 20);
        }

        protected void InitSubTitle(UILabel label, string text)
        {
            InitLabel(label, FontType.FontBold, text, 16, 22);
        }

        partial void BackBtn_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);
        }

        partial void NextBtn_TouchUpInside(UIButton sender)
        {
            SetWarningViewVisibility();
            if (_consentViewModel.ConsentIsGiven)
            {
                OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted;
                GoToResultPage();
            }
            else
            {
                UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, WarningLbl);
            }
        }

        void GoToResultPage()
        {
            InvokeOnMainThread(() =>
            {
                try
                {
                    LocalPreferencesHelper.IsOnboardingCompleted = true;
                    NavigationHelper.GoToResultPage(this, true);
                }
                catch (Exception e)
                {
                    LogUtils.LogMessage(LogSeverity.WARNING, $"Onboarding failed: Failed to retrieve Device ID from server {e.Message}");
                }
            });
        }

        public void SetWarningViewVisibility() { WarningView.Alpha = _consentViewModel.ConsentIsGiven ? 0 : 100; }

        partial void AcceptSwitched(UISwitch sender)
        {

            _consentViewModel.ConsentIsGiven = sender.On;
            if (sender.On)
            {
                SetWarningViewVisibility();
            }

            UIAccessibility.PostNotification(
                UIAccessibilityPostNotification.ScreenChanged,
                new NSString(sender.On ? ConsentViewModel.SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_GIVEN : ConsentViewModel.SWITCH_ACCESSIBILITY_ANNOUNCEMENT_CONSENT_NOT_GIVEN));
            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, sender);
        }

        void InitPrivacyPolicyButton()
        {
            PrivacyPolicy.Frame = new CGRect(0, 0, 0, 50); // The frame should not be needed here, but it is since the cornerRadius in StyleUtil is set only once, not dynamically updated on redraw.
            PrivacyPolicy.TranslatesAutoresizingMaskIntoConstraints = false;
            PrivacyPolicy.HeightAnchor.ConstraintEqualTo(50).Active = true;
            InitButtonStyling(PrivacyPolicy, ConsentViewModel.CONSENT_SEVEN_BUTTON_TEXT);
        }

        partial void PrivacyPolicy_TouchUpInside(UIButton sender)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }
    }
}
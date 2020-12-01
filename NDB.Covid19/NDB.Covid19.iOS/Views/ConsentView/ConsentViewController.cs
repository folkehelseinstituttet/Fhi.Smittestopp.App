using System;
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
            InitSubTitle(About_header, ConsentViewModel.CONSENT_ONE_TITLE);
            InitBodyText(About_section1, ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_ONE);
            InitBodyText(About_section2, ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_TWO);
            InitSubTitle(HowItWorks_header, ConsentViewModel.CONSENT_TWO_TITLE);
            InitBodyText(HowItWorks_section1, ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_ONE);
            InitBodyText(HowItWorks_section2, ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_TWO);
            InitBodyText(Samtykke_section1, ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_ONE);
            InitBodyText(Samtykke_section2, ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_TWO);
            InitBodyText(ContactInformation_section, ConsentViewModel.CONSENT_FOUR_PARAGRAPH);
            InitBodyText(MoreInformation_section, ConsentViewModel.CONSENT_FIVE_PARAGRAPH);
            InitSubTitle(SamtykkeBottom_header, ConsentViewModel.CONSENT_SIX_TITLE);
            InitBodyText(SamtykkeBottom_section, ConsentViewModel.CONSENT_SIX_PARAGRAPH, FontType.FontItalic);
            InitButtonStyling(NextBtn, WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT);
            InitButtonSecondaryStyling(BackBtn, WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT);

            WarningLbl.Font = StyleUtil.Font(FontType.FontBold, 22, 24);
            WarningLbl.Text = ConsentViewModel.CONSENT_REQUIRED;

            AcceptSwitchBtn.AccessibilityLabel = ConsentViewModel.SWITCH_ACCESSIBILITY_CONSENT_SWITCH_DESCRIPTOR;
            AcceptTextLbl.IsAccessibilityElement = false;
            AcceptTextLbl.Font = StyleUtil.Font(FontType.FontMedium, 18, 20);
            AcceptTextLbl.Text = ConsentViewModel.GIVE_CONSENT_TEXT;

            ActivityIndicator.AccessibilityElementsHidden = true;
        }

        void SetAccessibility() {
            InvokeOnMainThread(() =>
            {
                SetAccessibilityText(TitleLabel, ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE);
                SetAccessibilityText(About_header, ConsentViewModel.CONSENT_ONE_TITLE);
                SetAccessibilityText(About_section1, ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_ONE);
                SetAccessibilityText(About_section2, ConsentViewModel.CONSENT_ONE_PARAGRAPH_SECTION_TWO);
                SetAccessibilityText(HowItWorks_header, ConsentViewModel.CONSENT_TWO_TITLE);
                SetAccessibilityText(HowItWorks_section1, ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_ONE);
                SetAccessibilityText(HowItWorks_section2, ConsentViewModel.CONSENT_TWO_PARAGRAPH_SECTION_TWO);
                SetAccessibilityText(Samtykke_section1, ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_ONE);
                SetAccessibilityText(Samtykke_section2, ConsentViewModel.CONSENT_THREE_PARAGRAPH_SECTION_TWO);
                SetAccessibilityText(ContactInformation_section, ConsentViewModel.CONSENT_FOUR_PARAGRAPH);
                SetAccessibilityText(MoreInformation_section, ConsentViewModel.CONSENT_FIVE_PARAGRAPH);
                SetAccessibilityText(SamtykkeBottom_header, ConsentViewModel.CONSENT_SIX_TITLE);
                SetAccessibilityText(SamtykkeBottom_section, ConsentViewModel.CONSENT_SIX_PARAGRAPH);
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

        protected void InitBodyText(UILabel label, string text, FontType fontType = FontType.FontRegular)
        {
            InitLabelWithHTMLFormat(label, text, fontType);
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
                OnboardingStatusHelper.Status = ConsentsHelper.GetStatusDependingOnRelease();
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
            InitButtonSecondaryStyling(PrivacyPolicy, ConsentViewModel.CONSENT_SEVEN_BUTTON_TEXT);
        }

        partial void PrivacyPolicy_TouchUpInside(UIButton sender)
        {
            ConsentViewModel.OpenPrivacyPolicyLink();
        }
    }
}
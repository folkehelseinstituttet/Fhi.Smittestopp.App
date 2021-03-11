using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using UIKit;
using static NDB.Covid19.ViewModels.CountriesConsentViewModel;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class CountriesConsentViewController : BaseViewController
    {
        public CountriesConsentViewController(IntPtr handle) : base(handle)
        {
        }

        public static CountriesConsentViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("CountriesConsent", null);
            CountriesConsentViewController vc = storyboard.InstantiateInitialViewController() as CountriesConsentViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            CloseBtn.AccessibilityLabel = CLOSE_BUTTON_ACCESSIBILITY_LABEL;
            HeaderLabel.SetAttributedText(HEADER_TEXT);
            BodyText1.SetAttributedText(CONSENT3_BODYTEXT_1);
            StyleUtil.InitLabel(ShareHeader, StyleUtil.FontType.FontBold, CONSENT3_SHAREDATA_HEADER, 16, 22);
            BodyText2.SetAttributedText(CONSENT3_BODYTEXT_2);
            ConsentEU_Explanation.SetAttributedText(CONSENT3_EU_CONSENT_BUTTON_BODYTEXT);
            Consent_onlyNorway_Explanation.SetAttributedText(CONSENT3_ONLY_NORWAY_CONSENT_BUTTON_BODYTEXT);
            StyleUtil.InitLabel(ConsentText, StyleUtil.FontType.FontItalic, CONSENT3_CONSENTTOSHARE, 16, 22);

            StyleUtil.InitButtonStyling(NextButtonWithEUConsent, EU_CONSENT_NEXT_EU_CONSENT_BUTTON_TEXT);
            StyleUtil.InitButtonStyling(NextButtonOnlyNorwayConsent, EU_CONSENT_NEXT_ONLY_NORWAY_CONSENT_BUTTON_TEXT);

            HeaderLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            ShareHeader.AccessibilityTraits = UIAccessibilityTrait.Header;

            SetupStyling();

            if (UIAccessibility.IsVoiceOverRunning)
            {
                UIAccessibility.PostNotification(UIAccessibilityPostNotification.LayoutChanged, HeaderLabel);
                removeAccessibilityElementAndEnableAfterDelay(CloseBtn);
            }
        }

        public void SetupStyling()
        {
            HeaderLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            BodyText1.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            ShareHeader.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            BodyText2.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            ConsentEU_Explanation.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Consent_onlyNorway_Explanation.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            ConsentText.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        void GoToLoadingPage()
        {
            NavigationController?.PushViewController(LoadingPageViewController.Create(), true);
        }

        void GoToQuestionnaireCountriesPage()
        {
            NavigationController?.PushViewController(QuestionnaireCountriesViewController.Create(), true);
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            DialogHelper.ShowDialog(this, AbortDuringEUConsentViewModel, (action) => GoToLoadingPage(), UIAlertActionStyle.Default, null);
        }

        void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(CountriesConsentViewController)}.{nameof(OnFail)}: " +
                "AuthenticationState.PersonalData was garbage collected (iOS)");
        }

        partial void OnNextWithEUConsent(DefaultBorderButton sender)
        {
            InvokeNextButtonClick(GoToQuestionnaireCountriesPage, OnFail, true);
        }

        partial void OnNextOnlyNorwayConsent(DefaultBorderButton sender)
        {
            InvokeNextButtonClick(GoToLoadingPage, OnFail, false);
        }
    }
}
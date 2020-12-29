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
            DescriptionLabel.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_DESCRIPTION);
            LookUp_Header.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_HEADER, StyleUtil.FontType.FontBold);
            LookUp_Text.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_LOOKUP_TEXT);
            Notification_Header.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_HEADER, StyleUtil.FontType.FontBold);
            Notification_Text.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_NOTIFICATION_TEXT);
            Consent_BeAware_Text.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_BE_AWARE_TEXT);
            Consent_Explanation_Text.SetAttributedText(COUNTRIES_CONSENT_BE_AWARE_TEXT_CONSENT_EXPLANATION_TEXT, StyleUtil.FontType.FontItalic);

            StyleUtil.InitButtonStyling(NextButton, BUTTON_TEXT);

            SetupStyling();

        }

        public void SetupStyling()
        {
            HeaderLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            DescriptionLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            LookUp_Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            LookUp_Text.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Notification_Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Notification_Text.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Consent_BeAware_Text.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Consent_Explanation_Text.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
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
            DialogHelper.ShowDialog(this, CloseDialogViewModel, CloseConfirmed, UIAlertActionStyle.Destructive);
        }

        void CloseConfirmed(UIAlertAction obj)
        {
            NavigationController?.DismissViewController(true, null);
        }

        void OnFail()
        {
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(CountriesConsentViewController)}.{nameof(OnFail)}: " +
                "AuthenticationState.PersonalData was garbage collected (iOS)");
        }

        partial void OnNext(DefaultBorderButton sender)
        {
            DialogHelper.ShowDialog(
                this,
                GiveConsentViewModel,
                action => InvokeNextButtonClick(GoToQuestionnaireCountriesPage, OnFail, true),
                UIAlertActionStyle.Default,
                action => InvokeNextButtonClick(GoToLoadingPage, OnFail, false));
        }
    }
}
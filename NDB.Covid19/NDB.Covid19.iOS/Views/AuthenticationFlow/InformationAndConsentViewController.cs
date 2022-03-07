using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DeviceCheck;
using Foundation;
using NDB.Covid19.Models.UserDefinedExceptions;
using NDB.Covid19.OAuth2;
using NDB.Covid19.ViewModels;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;
using NDB.Covid19.Enums;
using System.Collections.Generic;
using static NDB.Covid19.ProtoModels.TemporaryExposureKey.Types;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class InformationAndConsentViewController : BaseViewController, IUIAccessibilityContainer
    {
        public InformationAndConsentViewController(IntPtr handle) : base(handle)
        {
        }

        public static InformationAndConsentViewController GetInformationAndConsentViewController()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("InformationAndConsent", null);
            InformationAndConsentViewController vc = storyboard.InstantiateInitialViewController() as InformationAndConsentViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        InformationAndConsentViewModel _viewModel;
        UIViewController _authViewController;
        private ReportType _reportInfectedType;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _reportInfectedType = IsReportingSelfTest ? ReportType.SelfReport : ReportType.ConfirmedTest;
            _viewModel = new InformationAndConsentViewModel(OnAuthSuccess, OnAuthError, _reportInfectedType);
            _viewModel.Init();

            SetTexts();
            SetupStyling();
            SetupAccessibility();
            AddObservers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Information and Consent page", null);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            LogInWithIDPortenBtn.HideSpinner();
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Information and Consent page", null);
            });

            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user left Information and Consent page", null);
            });
        }

        private void SetTexts()
        {
            if (IsReportingSelfTest)
            {
                HeaderLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_HEADER);
                DescriptionLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_DESCRIPTION);
                LookUp_Header.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_LOOKUP_HEADER, StyleUtil.FontType.FontBold);
                LookUp_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_LOOKUP_TEXT);
                Consent_Explanation_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_CONSENT_EXPLANATION_TEXT, StyleUtil.FontType.FontItalic);
                Notification_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_SELF_TEST_NOTIFICATION_TEXT);
            }
            else
            {
                HeaderLabel.SetAttributedText(InformationAndConsentViewModel.INFORMATION_CONSENT_HEADER_TEXT);
                DescriptionLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_DESCRIPTION);
                LookUp_Header.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_LOOKUP_HEADER, StyleUtil.FontType.FontBold);
                LookUp_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_LOOKUP_TEXT);
                Consent_Explanation_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_CONSENT_EXPLANATION_TEXT, StyleUtil.FontType.FontItalic);
                Notification_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_NOTIFICATION_TEXT);
            }
            Consent_BeAware_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_CONSENT_BEAWARE_TEXT);
            Notification_Header.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_NOTIFICATION_HEADER, StyleUtil.FontType.FontBold);
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

            StyleUtil.InitButtonStyling(LogInWithIDPortenBtn, InformationAndConsentViewModel.INFORMATION_CONSENT_ID_PORTEN_BUTTON_TEXT);
        }

        private void SetupAccessibility()
        {
            HeaderLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            LookUp_Header.AccessibilityTraits = UIAccessibilityTrait.Header;
            Notification_Header.AccessibilityTraits = UIAccessibilityTrait.Header;

            CloseBtn.AccessibilityLabel = InformationAndConsentViewModel.CLOSE_BUTTON_ACCESSIBILITY_LABEL;

            if (UIAccessibility.IsVoiceOverRunning)
            {
                this.SetAccessibilityElements(NSArray.FromNSObjects(ScrollView, CloseBtn));
                PostAccessibilityNotificationAndReenableElement(CloseBtn, HeaderLabel);
            }
        }

        void OnAuthError(object sender, AuthErrorType authErrorType)
        {
            _viewModel.Cleanup();

            Debug.Print("OnAuthError");
            LogUtils.LogMessage(LogSeverity.INFO, "Authentication failed.");
            LogInWithIDPortenBtn.HideSpinner();
            Utils.AuthErrorUtils.GoToErrorPageForAuthErrorType(this, authErrorType);
            _authViewController.DismissViewController(true, null);
        }

        void OnAuthSuccess(object sender, AuthSuccessType e)
        {
            Debug.Print("OnAuthSuccess");
            LogUtils.LogMessage(LogSeverity.INFO, $"Successfully authenticated and verified user. Navigating to {nameof(QuestionnaireViewController)} for flow: {e}");
            LogInWithIDPortenBtn.HideSpinner();
            IsReportingSelfTest = e == AuthSuccessType.SelfDiagnosis;
            GoToQuestionnairePage();
            _authViewController.DismissViewController(true, null);
        }

        partial void OnLoginBtnTapped(CustomSubclasses.DefaultBorderButton sender)
        {
            InvokeOnMainThread(() =>
            {
                LogInWithIDPortenBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);
            });

            LogUtils.LogMessage(Enums.LogSeverity.INFO, "Startet login with ID porten");
            _authViewController = AuthenticationState.Authenticator.GetUI();
            PresentViewController(_authViewController, true, null);
        }

        //After calling this method you cannot return to this page.
        //The entire Navigationcontroller has to be dismissed, and this controller must be reloaded and call ViewDidLoad()
        void GoToQuestionnairePage()
        {
            _viewModel.Cleanup();
            NavigationController?.PushViewController(QuestionnaireViewController.Create(), true);
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            _viewModel.Cleanup();
            NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_CONSENT_MODAL_IS_CLOSED);
        }
    }
}
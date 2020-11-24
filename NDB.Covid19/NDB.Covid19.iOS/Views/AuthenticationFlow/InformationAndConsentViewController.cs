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

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class InformationAndConsentViewController : BaseViewController
    {
        public InformationAndConsentViewController (IntPtr handle) : base (handle)
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
        UIActivityIndicatorView _spinner;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new InformationAndConsentViewModel(OnAuthSuccess, OnAuthError);
            _viewModel.Init();
            CloseBtn.AccessibilityLabel = InformationAndConsentViewModel.CLOSE_BUTTON_ACCESSIBILITY_LABEL;
            HeaderLabel.SetAttributedText(InformationAndConsentViewModel.INFORMATION_CONSENT_HEADER_TEXT);
            DescriptionLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_DESCRIPTION);
            LookUp_Header.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_LOOKUP_HEADER, StyleUtil.FontType.FontBold);
            LookUp_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_LOOKUP_TEXT);
            Notification_Header.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_NOTIFICATION_HEADER, StyleUtil.FontType.FontBold);
            Notification_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_NOTIFICATION_TEXT);
            Consent_BeAware_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_CONSENT_BEAWARE_TEXT);
            Consent_Explanation_Text.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_CONSENT_EXPLANATION_TEXT, StyleUtil.FontType.FontItalic);

            StyleUtil.InitButtonStyling(LogInWithIDPortenBtn, InformationAndConsentViewModel.INFORMATION_CONSENT_ID_PORTEN_BUTTON_TEXT);

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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            LogInWithIDPortenBtn.HideSpinner();
        }

        void OnAuthError(object sender, AuthErrorType authErrorType)
        {
            _viewModel.Cleanup();

            Debug.Print("OnAuthError");
            LogInWithIDPortenBtn.HideSpinner();
            Utils.AuthErrorUtils.GoToErrorPageForAuthErrorType(this, authErrorType);
            _authViewController.DismissViewController(true, null);
        }

        void OnAuthSuccess(object sender, EventArgs e)
        {
            Debug.Print("OnAuthSuccess");
            LogInWithIDPortenBtn.HideSpinner();
            GoToQuestionnairePage();
            _authViewController.DismissViewController(true, null);
        }

        async partial void OnLoginBtnTapped(CustomSubclasses.DefaultBorderButton sender)
        {
            InvokeOnMainThread(() =>
            {
                LogInWithIDPortenBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);
            });

            LogUtils.LogMessage(Enums.LogSeverity.INFO, "Startet login with nemid");
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
        }
    }
}
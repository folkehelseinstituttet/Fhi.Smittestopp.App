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
            DescriptionLabel.SetAttributedText(InformationAndConsentViewModel.INFORMATION_CONSENT_CONTENT_TEXT);
            TitleLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_TITLE, StyleUtil.FontType.FontBold);
            BodyOneLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_BODY_ONE);
            BodyTwoLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_BODY_TWO);
            DescriptionOneLabel.SetAttributedText(InformationAndConsentViewModel.INFOCONSENT_DESCRIPTION_ONE);
            StyleUtil.InitButtonStyling(LoginNemIDBtn, InformationAndConsentViewModel.INFORMATION_CONSENT_NEMID_BUTTON_TEXT);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            LoginNemIDBtn.HideSpinner();
        }

        void OnAuthError(object sender, AuthErrorType authErrorType)
        {
            _viewModel.Cleanup();

            Debug.Print("OnAuthError");
            LoginNemIDBtn.HideSpinner();
            Utils.AuthErrorUtils.GoToErrorPageForAuthErrorType(this, authErrorType);
            _authViewController.DismissViewController(true, null);
        }

        void OnAuthSuccess(object sender, EventArgs e)
        {
            Debug.Print("OnAuthSuccess");
            LoginNemIDBtn.HideSpinner();
            GoToQuestionnairePage();
            _authViewController.DismissViewController(true, null);
        }

        async partial void OnLoginBtnTapped(DefaultBorderButton sender)
        {
            InvokeOnMainThread(() =>
            {
                LoginNemIDBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);
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

        public Task<string> GetDeviceVerificationToken()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            DCDevice.CurrentDevice.GenerateToken((NSData token, NSError error) =>
            {
                if (error == null)
                {
                    Debug.WriteLine("Successfully obtained the device token from DeviceCheck");
                    tcs.SetResult(token.GetBase64EncodedString(NSDataBase64EncodingOptions.None));
                }
                else
                {
                    Debug.WriteLine("Failed to obtaine the device token from DeviceCheck");
                    tcs.SetException(new DeviceVerificationException("Failed to obtain the device token from DeviceCheck API"));
                }
            });
            return tcs.Task;
        }
    }
}
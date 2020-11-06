using System;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using Xamarin.Essentials;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class UploadCompletedViewController : BaseViewController
    {
        UIButton _learnMoreViewBtn;
        QuestionnaireViewModel _viewModel;

        public UploadCompletedViewController(IntPtr handle) : base(handle)
        {
        }
        public static UploadCompletedViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("UploadCompleted", null);
            UploadCompletedViewController vc = (UploadCompletedViewController)storyboard.InstantiateInitialViewController();
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new QuestionnaireViewModel();

            SetStyling();
            SetupLearnMoreButton();
            SetAccessibilityAttributes();
            SetLogoBasedOnAppLanguage();
            LogUtils.LogMessage(Enums.LogSeverity.INFO, "User has succesfully shared their keys");
        }

        private void SetLogoBasedOnAppLanguage()
        {
            string appLanguage = LocalesService.GetLanguage();
            HealthAuthoritiesLogo.Image = appLanguage != null && appLanguage.ToLower() == "en"
                ? UIImage.FromBundle("HealthAuthoritiesLogo_En")
                : UIImage.FromBundle("HealthAuthoritiesLogo");
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _learnMoreViewBtn.TouchUpInside += OnLearnMoreBtnTapped;

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _learnMoreViewBtn.TouchUpInside -= OnLearnMoreBtnTapped;
        }


        private void SetStyling()
        {
            TitleLabel.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_HEADER);
            TitleLabel.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_RECEIPT_HEADER;
            ContentLabelOne.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_TEXT);
            ContentLabelTwo.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION);
            BoxTitleLabel.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER, StyleUtil.FontType.FontBold);
            StyleUtil.InitLabelWithSpacing(BoxContentLabelTwo, StyleUtil.FontType.FontRegular, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE, 1.28, 12, 16);
            StyleUtil.InitButtonStyling(ToStartPageBtn, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_DISMISS);
        }

        void SetupLearnMoreButton()
        {
            BoxView.Subviews[0].Layer.CornerRadius = 12;
            _learnMoreViewBtn = new UIButton();
            _learnMoreViewBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            _learnMoreViewBtn.AccessibilityTraits = UIAccessibilityTrait.Link;
            StyleUtil.EmbedViewInsideButton(LearnMoreView, _learnMoreViewBtn);
        }        

        void SetAccessibilityAttributes()
        {
            CloseButton.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
            _learnMoreViewBtn.AccessibilityLabel = _viewModel.ReceipetPageReadMoreButtonAccessibility;
        }

        void OnLearnMoreBtnTapped(object sender, EventArgs e)
        {
            CommonServiceLocator.ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_LINK, BrowserLaunchMode.SystemPreferred);
        }

        partial void CloseButton_TouchUpInside(UIButton sender)
        {
            ClosePage();
        }

        partial void GoToStartPageButton_TouchUpInside(UIButton sender)
        {
            ClosePage();
        }

        void ClosePage()
        {
            NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
        }
    }
}

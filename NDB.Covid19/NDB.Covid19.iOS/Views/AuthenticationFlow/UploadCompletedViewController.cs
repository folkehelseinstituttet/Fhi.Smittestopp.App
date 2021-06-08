using System;
using Foundation;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using Xamarin.Essentials;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using NDB.Covid19.Enums;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class UploadCompletedViewController : BaseViewController, IUIAccessibilityContainer
    {
        UIButton _learnMoreViewBtn;
        QuestionnaireViewModel _viewModel;

        public UploadCompletedViewController(IntPtr handle) : base(handle)
        {
        }

        public static UploadCompletedViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("UploadCompleted", null);
            UploadCompletedViewController vc =
                storyboard.InstantiateInitialViewController() as UploadCompletedViewController;
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
            LogUtils.LogMessage(Enums.LogSeverity.INFO, "User has succesfully shared their keys", null, GetCorrelationId());
            UpdateCorrelationId(null);
            AddObservers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _learnMoreViewBtn.TouchUpInside += OnLearnMoreBtnTapped;
            LogUtils.LogMessage(Enums.LogSeverity.INFO, "The user is seeing Registered", null, GetCorrelationId());
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
            _learnMoreViewBtn.TouchUpInside -= OnLearnMoreBtnTapped;
        }

        void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Registered", null, GetCorrelationId());
            });

            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user left Registered", null, GetCorrelationId());
            });
        }


        private void SetStyling()
        {
            CloseButton.TintColor = ColorHelper.PRIMARY_COLOR;
            TitleLabel.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_HEADER);
            ContentLabelOne.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_TEXT);
            ContentLabelTwo.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION);
            BoxTitleLabel.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER,
                StyleUtil.FontType.FontBold);
            StyleUtil.InitLabelWithSpacing(BoxContentLabelTwo, StyleUtil.FontType.FontRegular,
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE, 1.28, 12, 16);
            StyleUtil.InitButtonStyling(ToStartPageBtn, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_DISMISS);
        }

        void SetupLearnMoreButton()
        {
            if (AppDelegate.ShouldOperateIn12_5Mode)
            {
                BoxView.Layer.CornerRadius = 12;
                BoxView.Layer.BackgroundColor = ColorHelper.INFO_BUTTON_BACKGROUND.CGColor;
                BoxView.Layer.BorderWidth = 1;
                BoxView.Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            }
            else
            {
                UIView stackView = BoxView.Subviews[0];
                stackView.Layer.CornerRadius = 12;
                stackView.Layer.MasksToBounds = true;
                stackView.Layer.BackgroundColor = ColorHelper.INFO_BUTTON_BACKGROUND.CGColor;
                stackView.Layer.BorderWidth = 1;
                stackView.Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            }

            _learnMoreViewBtn = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                AccessibilityTraits = UIAccessibilityTrait.Link
            };
            StyleUtil.EmbedViewInsideButton(BoxView, _learnMoreViewBtn);
        }

        void SetAccessibilityAttributes()
        {
            CloseButton.AccessibilityLabel =
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
            _learnMoreViewBtn.AccessibilityLabel = _viewModel.ReceipetPageReadMoreButtonAccessibility;

            TitleLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            BoxTitleLabel.AccessibilityTraits = UIAccessibilityTrait.Header;

            if (UIAccessibility.IsVoiceOverRunning)
            {
                this.SetAccessibilityElements(NSArray.FromNSObjects(ScrollView, ToStartPageBtn, CloseButton));
                PostAccessibilityNotificationAndReenableElement(CloseButton, TitleLabel);
            }
        }

        void OnLearnMoreBtnTapped(object sender, EventArgs e)
        {
            CommonServiceLocator.ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(
                QuestionnaireViewModel.REGISTER_QUESTIONAIRE_RECEIPT_LINK, BrowserLaunchMode.SystemPreferred);
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
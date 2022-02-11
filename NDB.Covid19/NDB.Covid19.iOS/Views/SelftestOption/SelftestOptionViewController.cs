using System;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.SelftestOption
{
    public partial class SelftestOptionViewController : BaseViewController, IUIAccessibilityContainer
    {
        public string RegisterTestTitle = "RegistertestTitleText";
        public SelftestOptionViewController(IntPtr handle) : base(handle)
        {
        }

        public static SelftestOptionViewController GetSelftestOptionViewController()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("SelftestOption", null);
            SelftestOptionViewController vc = storyboard.InstantiateInitialViewController() as SelftestOptionViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        SelftestRegistrationViewModel _viewmodel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewmodel = new SelftestRegistrationViewModel();
            SetTexts();
            SetupStyling();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void SetTexts()
        {
            RegisterTestTitle = SelftestRegistrationViewModel.HEADER_LABEL_SELFTEST_REGISTRATION_TEXT;
        }

        private void SetupStyling()
        {
            Header.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 32, 36);

            Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header.AccessibilityTraits = UIAccessibilityTrait.Header;
            Header.Text = RegisterTestTitle;
            Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header.AccessibilityTraits = UIAccessibilityTrait.Header;

            CloseBtn.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
            CloseBtn.TintColor = ColorHelper.PRIMARY_COLOR;

            StyleUtil.InitButtonStyling(ContinueWithMSISBtn, SelftestRegistrationViewModel.POSITIVE_TEST_MSISBUTTON_TEXT);
            StyleUtil.InitButtonStyling(ContinueWithSelfTestBtn, SelftestRegistrationViewModel.POSTIVE_TEST_SELFTESTBUTTON_TEXT);
            ContinueWithMSISBtn.TitleLabel.TextAlignment = UITextAlignment.Center;
            ContinueWithSelfTestBtn.TitleLabel.TextAlignment = UITextAlignment.Center;   
            ContinueWithMSISBtn.TitleLabel.Font = Font(FontType.FontMedium, 16f, 16f);
            ContinueWithSelfTestBtn.TitleLabel.Font = Font(FontType.FontMedium, 16f, 16f);
            ContinueWithSelfTestBtn.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            ContinueWithMSISBtn.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            nfloat verticalSpacing = 30;
            nfloat horizontalSpacing = 24;
            ContinueWithMSISBtn.ContentEdgeInsets = new UIEdgeInsets(top: verticalSpacing, left: horizontalSpacing, bottom: verticalSpacing, right: horizontalSpacing);
            ContinueWithSelfTestBtn.ContentEdgeInsets = new UIEdgeInsets(top: verticalSpacing, left: horizontalSpacing, bottom: verticalSpacing, right: horizontalSpacing);
        }

        private void AdjustTextHeight(UILabel Header)
        {
            //Enable auto-layout to be resized programatically
            Header.TranslatesAutoresizingMaskIntoConstraints = true;
            //Extend size of the label
            Header.SizeToFit();
        }

        partial void CloseBtn_TouchUpInside(UIKit.UIButton sender)
        {
            LeaveController();
        }

        partial void ContinueWithMSISBtn_TouchUpInside(UIKit.UIButton sender)
        {
            NavigationController?.PushViewController(InformationAndConsentViewController.GetInformationAndConsentViewController(), true);
            IsReportingSelfTest = false;
        }

        partial void ContinueWithSelftestBtn_TouchUpInside(UIKit.UIButton sender)
        {
            NavigationController?.PushViewController(InformationAndConsentViewController.GetInformationAndConsentViewController(), true);
            IsReportingSelfTest = true;
        }

    }
}   


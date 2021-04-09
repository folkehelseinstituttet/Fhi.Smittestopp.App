using System;
using CoreGraphics;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage3
{
    public partial class SettingsPage3ViewController : BaseViewController
    {
        public SettingsPage3ViewController(IntPtr handle) : base(handle)
        {
        }

        ConsentViewModel _vm;
        UIButton _privacyPolicyButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _vm = new ConsentViewModel();
            InitPrivacyPolicyButton();
            InitLabels();
            SetStyling();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            PostAccessibilityNotificationAndReenableElement(BackButton, PageTitle);
        }

        void InitLabels()
        {
            ConsentHelper.SetConsentLabels(LabelStackView, _vm.GetConsentSectionsTexts(), _privacyPolicyButton);
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;
            TitleStackView.BottomAnchor.ConstraintEqualTo(ScrollView.TopAnchor, 0).Active = true;
        }

        void SetStyling()
        {
            DeleteBtnWidthConstraint.Active = false;
            StyleUtil.InitButtonStyling(DeleteConsentBtn, ConsentViewModel.WITHDRAW_CONSENT_BUTTON_TEXT);
            PageTitle.SetAttributedText(ConsentViewModel.WELCOME_PAGE_CONSENT_TITLE);
            BottomStackView.Layer.MasksToBounds = false;
            BottomStackView.Layer.ShadowColor = UIColor.Gray.CGColor;
            BottomStackView.Layer.ShadowOffset = new CGSize(0f, 10f);
            BottomStackView.Layer.ShadowOpacity = 1;
            BottomStackView.Layer.ShadowRadius = 9;
        }

        void InitPrivacyPolicyButton()
        {
            _privacyPolicyButton = new UIButton(UIButtonType.System);
            _privacyPolicyButton.Frame = new CGRect(0, 0, 0, 0);
            _privacyPolicyButton.TouchUpInside += (sender, e) => {
                ConsentViewModel.OpenPrivacyPolicyLink();
            };

            _privacyPolicyButton.AccessibilityTraits = UIAccessibilityTrait.Link;
            StyleUtil.InitLinkButtonStyling(_privacyPolicyButton, ConsentViewModel.CONSENT_SEVEN_LINK_TEXT);
        }

        partial void DeleteConsentBtn_TouchUpInside(DefaultBorderButton sender)
        {
            CreateDeleteWarning();
        }

        void CreateDeleteWarning()
        {
            UIAlertController controller = UIAlertController.Create(
                ConsentViewModel.CONSENT_REMOVE_TITLE,
                ConsentViewModel.CONSENT_REMOVE_MESSAGE,
                UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create(ConsentViewModel.CONSENT_OK_BUTTON_TEXT, UIAlertActionStyle.Destructive, DeleteWarningOKBtnTapped));
            controller.AddAction(UIAlertAction.Create(ConsentViewModel.CONSENT_NO_BUTTON_TEXT, UIAlertActionStyle.Cancel, null));
            PresentViewController(controller, true, null);
        }

        void DeleteWarningOKBtnTapped(UIAlertAction obj)
        {
            DeleteBtnWidthConstraint.Constant = DeleteConsentBtn.Frame.Width;
            DeleteBtnWidthConstraint.Active = true;
            DeleteConsentBtn.SetTitle(string.Empty, UIControlState.Normal);

            UIActivityIndicatorView spinner = ShowSpinner();

            DeviceUtils.StopScanServices();
            DeviceUtils.CleanDataFromDevice();
            spinner.StopAnimating();
            spinner.RemoveFromSuperview();
            DeleteConsentBtn.SetTitle(ConsentViewModel.WITHDRAW_CONSENT_BUTTON_TEXT, UIControlState.Normal);
            DeleteBtnWidthConstraint.Active = false;

            // Show a dialog without any buttons, that way forcing the user to quit the app.
            UIAlertController controller = UIAlertController.Create(
                ConsentViewModel.WITHDRAW_CONSENT_SUCCESS_TITLE,
                ConsentViewModel.WITHDRAW_CONSENT_SUCCESS_TEXT,
                UIAlertControllerStyle.Alert);
            PresentViewController(controller, true, null);
        }

        UIActivityIndicatorView ShowSpinner()
        {
            UIActivityIndicatorView spinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            spinner.HidesWhenStopped = true;
            spinner.TranslatesAutoresizingMaskIntoConstraints = false;

            View.Add(spinner);

            // Pin the spinner to the center of the DeleteConsentBtn
            NSLayoutConstraint.ActivateConstraints(new[] {
                spinner.CenterXAnchor.ConstraintEqualTo(DeleteConsentBtn.CenterXAnchor),
                spinner.CenterYAnchor.ConstraintEqualTo(DeleteConsentBtn.CenterYAnchor)
            });

            spinner.StartAnimating();

            return spinner;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}
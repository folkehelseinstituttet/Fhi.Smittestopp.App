using System;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Initializer
{
    public partial class InizializerViewController : BaseViewController
    {

        UITapGestureRecognizer _gestureRecognizer;
        bool AvailableOnDevice;

        public InizializerViewController(IntPtr handle) : base(handle)
        {
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            StyleStartButton();

            fhiLogo.AccessibilityLabel = InitializerViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            appLogo.AccessibilityLabel = InitializerViewModel.SMITTESPORING_APP_LOGO_ACCESSIBILITY;

            HeaderView.SizeToFit();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // The app is supported from iOS 12.5 incl. and until iOS 13.0 excl.
            // and from 13.6 incl. and higher.
            string currentiOSVersion = UIDevice.CurrentDevice.SystemVersion;
            AvailableOnDevice = currentiOSVersion.CompareTo("13.6") >= 0 ||
                (currentiOSVersion.CompareTo("12.5") >= 0 && currentiOSVersion.CompareTo("13.0") < 0);

            if (AvailableOnDevice)
            {
                if (ConsentsHelper.IsNotFullyOnboarded)
                {
                    NavigationHelper.GoToWelcomeWhatsNewPage(this);
                    return;
                }
                NavigationHelper.GoToResultPageIfOnboarded(this);
            }
            else
            {
                ShowOutdatedOSDialog();
            }
        }

        private void StyleStartButton()
        {
            StartButton.SemanticContentAttribute = UISemanticContentAttribute.ForceRightToLeft;
            StartButton.ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 20);
            StartButton.TitleEdgeInsets = new UIEdgeInsets(0, -6, 0, 6);
            StartButton.ImageEdgeInsets = new UIEdgeInsets(0, 6, 0, -6);
            StyleUtil.InitButtonStyling(StartButton, InitializerViewModel.LAUNCHER_PAGE_START_BTN);

            if (OnboardingStatusHelper.Status == OnboardingStatus.CountriesOnboardingCompleted)
            {
                StartButton.Hidden = true;
            }
        }

        partial void StartButton_TouchUpInside(UIButton sender)
        {
            Continue();
        }

        private void ShowOutdatedOSDialog()
        {
            DialogViewModel dialogViewModel = new DialogViewModel
            {
                Title = "BASE_ERROR_TITLE".Translate(),
                Body = "LAUNCHER_PAGE_OS_VERSION_DIALOG_MESSAGE_IOS".Translate(),
                OkBtnTxt = "ERROR_OK_BTN".Translate()
            };
            DialogHelper.ShowDialog(this, dialogViewModel, (action) => { });
        }

        void Continue()
        {
            if (AvailableOnDevice)
            {
                NavigationHelper.GoToLanguageSelectionPage(this);
            }
            else
            {
                ShowOutdatedOSDialog();
            }
        }
    }
}
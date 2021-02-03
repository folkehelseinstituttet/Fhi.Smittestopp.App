using System;
using Foundation;
using I18NPortable;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Initializer
{
    public partial class InizializerViewController : BaseViewController
    {
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
            StyleUtil.InitButtonStyling(StartButton, InitializerViewModel.LAUNCHER_PAGE_START_BTN);
            fhiLogo.AccessibilityLabel = InitializerViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            appLogo.AccessibilityLabel = InitializerViewModel.SMITTESPORING_APP_LOGO_ACCESSIBILITY;
            HeaderView.SizeToFit();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 5))
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
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 5))
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
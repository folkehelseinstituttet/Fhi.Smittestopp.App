using System;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.PersistedData;
using UIKit;

namespace NDB.Covid19.iOS
{
    public partial class InizializerViewController : BaseViewController
    {

        UITapGestureRecognizer _gestureRecognizer;

        public InizializerViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            StyleUtil.InitButtonStyling(StartButton, InitializerViewModel.LAUNCHER_PAGE_START_BTN);
            StyleUtil.InitLabel(ContinueInEnLbl, StyleUtil.FontType.FontRegular, InitializerViewModel.LAUNCHER_PAGE_CONTINUE_IN_ENG, 12, 20);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 5))
            {
                if (OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted)
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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SetupButton();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ContinueInEnStackView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        partial void StartButton_TouchUpInside(UIButton sender)
        {
            LocalPreferencesHelper.SetAppLanguage("da");
            LocalesService.Initialize();
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
                NavigationHelper.GoToOnboardingPage(this);
            }
            else
            {
                ShowOutdatedOSDialog();
            }
        }

        void SetupButton()
        {
            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OnContinueInEnViewBtnTapped(_gestureRecognizer));
            ContinueInEnStackView.AddGestureRecognizer(_gestureRecognizer);
        }

        void OnContinueInEnViewBtnTapped(UITapGestureRecognizer recognizer)
        {
            LocalPreferencesHelper.SetAppLanguage("en");
            LocalesService.Initialize();
            Continue();
        }
    }
}
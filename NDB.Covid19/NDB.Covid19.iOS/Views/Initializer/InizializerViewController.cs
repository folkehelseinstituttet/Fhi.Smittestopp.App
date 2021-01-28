using System;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
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
            StyleUtil.InitButtonStyling(StartButtonNB, InitializerViewModel.LAUNCHER_PAGE_START_BTN_NB);
            StyleUtil.InitButtonStyling(StartButtonNN, InitializerViewModel.LAUNCHER_PAGE_START_BTN_NN);
            StyleUtil.InitStartPageButtonStyling(StartButtonNB);
            StyleUtil.InitStartPageButtonStyling(StartButtonNN);
            StyleUtil.InitLabel(ContinueInEnLbl, StyleUtil.FontType.FontSemiBold, InitializerViewModel.LAUNCHER_PAGE_CONTINUE_IN_ENG, 16, 24);
            ContinueInEnLbl.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
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

        partial void StartButtonNB_TouchUpInside(UIButton sender)
        {
            LocalPreferencesHelper.SetAppLanguage("nb");
            LocalesService.Initialize();
            Continue();
        }

        partial void StartButtonNN_TouchUpInside(UIButton sender)
        {
            LocalPreferencesHelper.SetAppLanguage("nn");
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
            if (AvailableOnDevice)
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
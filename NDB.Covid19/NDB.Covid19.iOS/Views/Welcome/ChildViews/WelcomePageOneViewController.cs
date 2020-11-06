using NDB.Covid19.ViewModels;
using System;
using NDB.Covid19.PersistedData;
using UIKit;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageOneViewController : PageViewController
    {
        public WelcomePageOneViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();


            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, TitleLabel);
            BackArrow.Hidden = !LocalPreferencesHelper.IsOnboardingCompleted;
            BackArrow.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
        }

        partial void BackArrowBtn_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);
        }

        void SetTexts()
        {
            InitTitle(TitleLabel, WelcomeViewModel.WELCOME_PAGE_ONE_TITLE);
            InitBodyText(BodyText1, WelcomeViewModel.WELCOME_PAGE_ONE_BODY_ONE);
            InitBodyText(BodyText2, WelcomeViewModel.WELCOME_PAGE_ONE_BODY_TWO);
        }
    }
}
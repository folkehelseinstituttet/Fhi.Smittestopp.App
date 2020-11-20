using System;
using UIKit;
using NDB.Covid19.ViewModels;
using NDB.Covid19.PersistedData;
using NDB.Covid19.iOS.Utils;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageTwoViewController : PageViewController
    {
        public WelcomePageTwoViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();

            PageTitle.AccessibilityLabel = WelcomeViewModel.WELCOME_PAGE_TWO_ACCESSIBILITY_TITLE;
            BodyText1.AccessibilityLabel = WelcomeViewModel.WELCOME_PAGE_TWO_ACCESSIBILITY_BODY_ONE;

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, PageTitle);
            BackArrow.Hidden = !LocalPreferencesHelper.IsOnboardingCompleted;
            BackArrow.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            StyleUtil.FlashScrollIndicatorsInSubScrollViews(View.Subviews);
        }

        partial void BackArrowBtn_TouchUpInside(UIButton sender)
        {
            NavigationController.PopViewController(true);
        }

        void SetTexts()
        {
            InitTitle(PageTitle, WelcomeViewModel.WELCOME_PAGE_TWO_TITLE);
            InitBodyText(BodyText1, WelcomeViewModel.WELCOME_PAGE_TWO_BODY_ONE);
            InitBodyText(BodyText2, WelcomeViewModel.WELCOME_PAGE_TWO_BODY_TWO);
        }

        void SetAccessibility()
        {
            PageTitle.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(WelcomeViewModel.WELCOME_PAGE_TWO_TITLE);
            BodyText1.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(WelcomeViewModel.WELCOME_PAGE_TWO_BODY_ONE);
        }
    }
}
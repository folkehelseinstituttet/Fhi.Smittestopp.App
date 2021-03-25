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
            SetAccessibility();

            BackArrow.Hidden = !LocalPreferencesHelper.IsOnboardingCompleted;
            BackArrow.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            PostAccessibilityNotificationAndReenableElement(BackArrow, PageTitle);
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
            PageTitle.AccessibilityLabel = WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_TWO;
            PageTitle.AccessibilityValue = WelcomeViewModel.WELCOME_PAGE_TWO_TITLE;
            PageTitle.AccessibilityTraits = UIAccessibilityTrait.Header;
            BodyText1.AccessibilityLabel = WelcomeViewModel.WELCOME_PAGE_TWO_ACCESSIBILITY_BODY_ONE;
        }
    }
}
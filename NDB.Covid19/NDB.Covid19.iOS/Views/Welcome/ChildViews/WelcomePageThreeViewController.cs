using NDB.Covid19.ViewModels;
using System;
using NDB.Covid19.PersistedData;
using UIKit;
using NDB.Covid19.iOS.Utils;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageThreeViewController : PageViewController
    {
        public WelcomePageThreeViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTexts();

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
            InitTitle(PageTitle, WelcomeViewModel.WELCOME_PAGE_FOUR_TITLE);
            PageTitle.AccessibilityTraits = UIAccessibilityTrait.Header;
            PageTitle.AccessibilityLabel = WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_THREE;
            PageTitle.AccessibilityValue = WelcomeViewModel.WELCOME_PAGE_FOUR_TITLE;
            InitBodyText(Label1, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_ONE);
            InitBodyText(Label2, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_TWO);
            InitBodyText(Label3, WelcomeViewModel.WELCOME_PAGE_FOUR_BODY_THREE);
        }
    }
}
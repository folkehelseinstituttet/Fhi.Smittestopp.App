using NDB.Covid19.ViewModels;
using System;
using NDB.Covid19.PersistedData;
using UIKit;
using NDB.Covid19.iOS.Utils;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public partial class WelcomePageFourViewController : PageViewController
    {
        public WelcomePageFourViewController (IntPtr handle) : base (handle)
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
            PostAccessibilityNotificationAndReenableElement(BackArrow, TitleLabel);
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
            InitTitle(TitleLabel, WelcomeViewModel.WELCOME_PAGE_THREE_TITLE);
            TitleLabel.AccessibilityLabel = WelcomeViewModel.ANNOUNCEMENT_PAGE_CHANGED_TO_FOUR;
            TitleLabel.AccessibilityValue = WelcomeViewModel.WELCOME_PAGE_THREE_TITLE;
            TitleLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            InitBodyText(BodyText1, WelcomeViewModel.WELCOME_PAGE_THREE_BODY_ONE);
            InitBodyText(BodyText2, WelcomeViewModel.WELCOME_PAGE_THREE_BODY_TWO);
            InitBodyText(BoxText, WelcomeViewModel.WELCOME_PAGE_THREE_INFOBOX_BODY);
        }
    }
}
using System;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StressUtils;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.Welcome
{
    public partial class WelcomePageWhatIsNewViewController : BaseViewController
    {
        SingleClick singleClick = null;

        public WelcomePageWhatIsNewViewController(IntPtr handle) : base(handle)
        {
            singleClick = new SingleClick(NextButton_TouchUpInside, 500);
        }

        ~WelcomePageWhatIsNewViewController()
        {
            singleClick = null;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupTexts();
            SetupStyling();

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, TitleLabel);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NextButton.AddTarget(singleClick.Run, UIControlEvent.TouchUpInside);
            StyleUtil.FlashScrollIndicatorsInSubScrollViews(View.Subviews);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            NextButton.RemoveTarget(singleClick.Run, UIControlEvent.TouchUpInside);
        }

        private void NextButton_TouchUpInside(object sender, EventArgs e)
        {
            OnboardingStatusHelper.Status = ConsentsHelper.GetStatusDependingOnRelease();
            GoToInfectionStatusPage();
        }

        void GoToInfectionStatusPage()
        {
            InvokeOnMainThread(() =>
            {
                UIViewController vc = NavigationHelper.ViewControllerByStoryboardName("InfectionStatus");
                NavigationController.PushViewController(vc, true);
            });
        }

        public void SetupStyling()
        {
            InitButtonStyling(NextButton, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_BUTTON);
        }

        private void SetupTexts()
        {
            InitLabelWithSpacing(TitleLabel, FontType.FontBold, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_TITLE, 1.14, 24, 26);
            InitLabelWithSpacing(Label1, FontType.FontRegular, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_BULLET_ONE, 1.28, 16, 18);
            InitLabelWithSpacing(Label2, FontType.FontRegular, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_BULLET_TWO, 1.28, 16, 18);
            InitLabelWithSpacing(LabelBottom, FontType.FontRegular, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_FOOTER, 1.28, 16, 18);

            TitleLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            SetAccessibilityText(TitleLabel, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_TITLE);
            SetAccessibilityText(Label1, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_BULLET_ONE);
            SetAccessibilityText(Label2, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_BULLET_TWO);
            SetAccessibilityText(LabelBottom, WelcomePageWhatIsNewViewModel.WELCOME_PAGE_WHATS_NEW_FOOTER);
        }

        private void SetAccessibilityText(UILabel label, string text)
        {
            label.IsAccessibilityElement = true;
            label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
        }
    }
}


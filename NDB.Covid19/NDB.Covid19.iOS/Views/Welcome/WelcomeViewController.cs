using System;
using System.Collections.Generic;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StressUtils;

namespace NDB.Covid19.iOS.Views.Welcome
{
    public partial class WelcomeViewController : BaseViewController
    {
        public bool IsOnBoarding;
        WelcomePageViewController _pageController;
        int _currentPageIndex = 0;
        SingleClick _singleClick = null;

        public WelcomeViewController(IntPtr handle) : base(handle)
        {
            _singleClick = new SingleClick(NextBtn_TouchUpInside, 500);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupStyling();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NextBtn.AddTarget(_singleClick.Run, UIControlEvent.TouchUpInside);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            NextBtn.RemoveTarget(_singleClick.Run, UIControlEvent.TouchUpInside);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "PageViewSegue")
            {
                WelcomePageViewController pageViewController = segue.DestinationViewController as WelcomePageViewController;
                if (!IsOnBoarding)
                {
                    pageViewController.PageTitles = new List<string> { "WelcomePageOne", "WelcomePageTwo", "WelcomePageThree", "WelcomePageFour" };
                }
                _pageController = pageViewController;
                PageControl.Pages = _pageController.PageTitles.Count;
                PageControl.CurrentPage = 0;
                PageControl.UpdateCurrentPageDisplay();
            }
        }

        void GoToConsentPage()
        {
            InvokeOnMainThread(() =>
            {
                UIViewController vc = NavigationHelper.ViewControllerByStoryboardName("Consent");
                NavigationController.PushViewController(vc, true);
            });
        }

        public void SetupStyling()
        {
            StyleUtil.InitButtonStyling(NextBtn, WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT);
            StyleUtil.InitButtonSecondaryStyling(PreviousBtn, WelcomeViewModel.PREVIOUS_PAGE_BUTTON_TEXT);
            SetPreviousButtonHidden(true);

            PageControl.UserInteractionEnabled = false;
            PageControl.PageIndicatorTintColor = UIColor.White;
            PageControl.CurrentPageIndicatorTintColor = "#32345C".ToUIColor();
            PageControl.IsAccessibilityElement = false;

            NextBtn.BackgroundColor = "#32345C".ToUIColor();
            NextBtn.SetTitleColor("#E1EAED".ToUIColor(), UIControlState.Normal);

            PreviousBtn.SetTitleColor("#32345C".ToUIColor(), UIControlState.Normal);
            PreviousBtn.Layer.BorderColor = "#32345C".ToUIColor().CGColor;

        }

        void SetPreviousButtonHidden(bool hide)
        {
            if (hide)
            {
                NSLayoutConstraint nextBtnLeadingConstraint = null;
                foreach (NSLayoutConstraint constraint in ButtonsView.Constraints)
                {
                    if (constraint.SecondItem == NextBtn && constraint.SecondAttribute == NSLayoutAttribute.Leading)
                    {
                        nextBtnLeadingConstraint = constraint;
                        break;
                    }
                }
                if (nextBtnLeadingConstraint == null)
                {
                    ButtonsView.LeadingAnchor.ConstraintEqualTo(NextBtn.LeadingAnchor, 0).Active = true;
                }
            }
            else
            {
                foreach (NSLayoutConstraint constraint in ButtonsView.Constraints)
                {
                    if (constraint.SecondItem == NextBtn && constraint.SecondAttribute == NSLayoutAttribute.Leading)
                    {
                        ButtonsView.RemoveConstraint(constraint);
                        break;
                    }
                }
            }
        }

        private void NextBtn_TouchUpInside(object sender, EventArgs e)
        {
            if (_currentPageIndex + 1 == _pageController.PageTitles.Count)
            {
                if (IsOnBoarding)
                {
                    GoToConsentPage();
                }
                else
                {
                    NavigationController.PopViewController(true);
                }
            }
            else
            {
                NextBtn.Enabled = false;
                _currentPageIndex = _pageController.GoToNextPage();
                UpdateLayout();
            }
        }

        partial void PreviousBtn_TouchUpInside(UIButton sender)
        {
            _currentPageIndex = _pageController.GoToPreviousPage();
            UpdateLayout();
        }

        void UpdateLayout()
        {
            SetPreviousButtonHidden(_currentPageIndex == 0);
            PageControl.CurrentPage = _currentPageIndex;
            PageControl.UpdateCurrentPageDisplay();

            StyleUtil.InitButtonStyling(NextBtn, _currentPageIndex == 4
                ? WelcomeViewModel.WELCOME_PAGE_BACKGROUND_LIMITATIONS_NEXT_BUTTON
                : WelcomeViewModel.NEXT_PAGE_BUTTON_TEXT);
        }

        public void SetNextBtnVisibility(bool hide)
        {
            NextBtn.Alpha = hide ? 0 : 100;
        }

        public void EnableNextBtn(bool enabled)
        {
            NextBtn.Enabled = enabled;
        }

        ~WelcomeViewController()
        {  
            _singleClick = null;
        }
    }
}
using System;
using CoreFoundation;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    public abstract class PageViewController : UIViewController
    {
        public int PageIndex { get; set; }
        public WelcomeViewController WelcomeViewController => ParentViewController.ParentViewController is WelcomeViewController ?
                ParentViewController.ParentViewController as WelcomeViewController : null;

        public PageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Clear;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            WelcomeViewController?.EnableNextBtn(true);
        }

        protected void InitTitle(UILabel label, string text)
        {
            InitLabelWithSpacing(label, FontType.FontBold, text, 1.14, 24, 26);
            label.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(text);
        }

        protected void InitBodyText(UILabel label, string text)
        {
            InitLabelWithSpacing(label, FontType.FontRegular, text, 1.28, 16, 20);
            label.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(text);
        }

        protected void InitBoxText(UILabel label, string text)
        {
            InitLabel(label, FontType.FontRegular, text, 16, 20);
            label.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(text);
        }

        protected void InitSubTitle(UILabel label, string text)
        {
            InitLabel(label, FontType.FontBold, text, 16, 22);
            label.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(text);
        }

        /// <summary>
        /// Some view controllers that inherit from PageViewController have UI elements, e.g., back button,
        /// on the top in View stack that leads to it being read first by VO eventhough a UIAccessibility.PostNotification
        /// is being called to indicate another element that should be in focus. This can be addressed by removing the UIView
        /// that is read first to focus on the element that should be targeted by UIAccessibility.PostNotification call, and
        /// put it back into accessibility elements after a short delay (1 sec).
        /// </summary>
        /// <param name="uiViewToDisable">UIView to be removed from VoiceOver reading hierarchy</param>
        protected void removeAccessibilityElementAndEnableAfterDelay(UIView uiViewToDisable)
        {
            uiViewToDisable.IsAccessibilityElement = false;
            DispatchTime when = new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(1));
            DispatchQueue.MainQueue.DispatchAfter(when, () =>
            {
                uiViewToDisable.IsAccessibilityElement = true;
            });
        }
    }
}

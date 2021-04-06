using System;
using System.Linq;
using CoreFoundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views
{
    public class BaseViewController: UIViewController
    {
        protected internal BaseViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetBackgroundColor();
        }

        void SetBackgroundColor()
        {
            View.BackgroundColor = "#E1EAED".ToUIColor();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE, ShowForceUpdatePage);
        }

        public override void ViewWillDisappear(bool animated)
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
            base.ViewWillDisappear(animated);
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.DarkContent;
        }

        void ShowForceUpdatePage(object _)
        {
            InvokeOnMainThread(() =>
            {
                UIViewController forceUpdateVC = NavigationHelper.ViewControllerByStoryboardName("ForceUpdate");
                forceUpdateVC.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                PresentViewController(forceUpdateVC, true, null);
            });
        }

        /// <summary>
        /// Some view controllers that inherit from BaseViewController have UI elements, e.g., close button,
        /// on the top in View stack that leads to it being read first by VO eventhough a UIAccessibility.PostNotification
        /// is being called to indicate another element that should be in focus. This can be addressed by removing the UIView
        /// that is read first to focus on the element that should be targeted by UIAccessibility.PostNotification call, and
        /// put it back into accessibility elements after a short delay (1 sec).
        /// </summary>
        /// <param name="uiViewToDisable">UIView to be removed from VoiceOver reading hierarchy</param>
        protected void PostAccessibilityNotificationAndReenableElement(UIView uiViewToDisable, UIView uiViewPostNotification)
        {
            if (UIAccessibility.IsVoiceOverRunning)
            {
                UIAccessibility.PostNotification(UIAccessibilityPostNotification.LayoutChanged, uiViewPostNotification);
                uiViewToDisable.IsAccessibilityElement = false;
                DispatchTime when = new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(1));
                DispatchQueue.MainQueue.DispatchAfter(when, () =>
                {
                    uiViewToDisable.IsAccessibilityElement = true;
                });
            }
        }

        /// <summary>
        /// If the ViewController is embedded in a NavigationController it will be popped. Otherwise it will be dismissed.
        /// </summary>
        /// <param name="animation">If set to <c>true</c> animation.</param>
        /// <param name="completionHandler">Completion handler.</param>
        public virtual void LeaveController(bool animation = true, Action completionHandler = null)
        {
            if (NavigationController != null)
            {
                if (NavigationController.ViewControllers.Count() > 1)
                {
                    NavigationController.PopViewController(animation);
                    completionHandler?.Invoke();
                }
                else
                {
                    NavigationController.DismissViewController(animation, completionHandler);
                }
            }
            else
            {
                DismissViewController(animation, completionHandler);
            }
        }
    }
}

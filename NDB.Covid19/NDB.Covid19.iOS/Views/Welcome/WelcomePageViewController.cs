using NDB.Covid19.iOS.Utils;
using System;
using System.Collections.Generic;
using NDB.Covid19.iOS.Views.Welcome.ChildViews;
using UIKit;

namespace NDB.Covid19.iOS.Views.Welcome
{
    public partial class WelcomePageViewController : UIPageViewController
    {
        public List<string> PageTitles = new List<string> {"WelcomePageOne", "WelcomePageTwo", "WelcomePageThree", "WelcomePageFour"};
        PageViewController _currentPage;

        public WelcomePageViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _currentPage = this.ViewControllerAtIndex(0) as PageViewController;
            PageViewController[] viewControllers = new PageViewController[] { _currentPage };

            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            
        }

        public void SetCurrentPage(int index)
        {
            _currentPage = this.ViewControllerAtIndex(index) as PageViewController;
        }
        public int GoToNextPage()
        {
            _currentPage = NextViewController();
            PageViewController[] viewControllers = new PageViewController[] { _currentPage };
            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
            return _currentPage.PageIndex;
        }

        public int GoToPreviousPage()
        {
            _currentPage = PreviousViewController();
            PageViewController[] viewControllers = new PageViewController[] { _currentPage };
            SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Reverse, true, null);
            return _currentPage.PageIndex;
        }

        public PageViewController ViewControllerAtIndex(int index)
        {
            PageViewController vc = NavigationHelper.ViewControllerByStoryboardName(PageTitles[index]) as PageViewController;
            vc.PageIndex = index;
            return vc;
        }

        public PageViewController NextViewController()
        {
            int index = _currentPage.PageIndex;
            index++;
            return index == PageTitles.Count ? null : ViewControllerAtIndex(index);
        }

        public PageViewController PreviousViewController()
        {
            int index = _currentPage.PageIndex;
            if (index == 0)
            {
                return null;
            }
            else
            {
                index--;
                return ViewControllerAtIndex(index);
            }
        }
    }
}
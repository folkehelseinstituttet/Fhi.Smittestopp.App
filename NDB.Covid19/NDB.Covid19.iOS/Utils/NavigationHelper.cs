using System.Diagnostics;
using Foundation;
using NDB.Covid19.iOS.Views.InfectionStatus;
using NDB.Covid19.iOS.Views.Initializer;
using NDB.Covid19.iOS.Views.Welcome;
using NDB.Covid19.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class NavigationHelper
    {
        public static void GoToAppSettings()
        {
            Debug.Print("Go to Apps settings");
            NSUrl url = new NSUrl(UIApplication.OpenSettingsUrlString);
            UIApplication.SharedApplication.OpenUrl(url);
        }

        public static void GoToSettings()
        {
            Debug.Print("Go to settings");
            NSUrl url = new NSUrl("App-Prefs:root=General");
            UIApplication.SharedApplication.OpenUrl(url);
        }

        public static UIViewController ViewControllerByStoryboardName(string storyboardName)
        {
            UIStoryboard storyboard = UIStoryboard.FromName(storyboardName, null);
            UIViewController vc = storyboard.InstantiateInitialViewController();
            return vc;
        }

        public static void GoToHelpTextViewController(UIViewController fromController, string title, string body, string okBtnText)
        {
            Views.HelpText.HelpTextViewController vc = ViewControllerByStoryboardName("HelpText") as Views.HelpText.HelpTextViewController;
            vc.TitleText = title;
            vc.Content = body;
            vc.OkBtnText = okBtnText;
            fromController.PresentViewController(vc, true, null);
        }

        /// <summary>
        /// Finds the current visible ViewController.
        /// If UIAlertController OR UIActivityViewController are on top they will be dismissed first.
        /// </summary>
        public static UIViewController TopController()
        {
            UIViewController root = UIApplication.SharedApplication.KeyWindow.RootViewController;
            return CurrentTopViewController(root);
        }

        static UIViewController CurrentTopViewController(UIViewController current)
        {
            if (current.PresentedViewController != null && !IsIgnoredViewControllers(current.PresentedViewController))
            {
                return CurrentTopViewController(current.PresentedViewController);
            }
            if (current is UINavigationController)
            {
                UIViewController visible = (current as UINavigationController).VisibleViewController;
                if (IsIgnoredViewControllers(visible))
                {
                    return (current as UINavigationController).TopViewController;
                }
                return visible;
            }
            return current;
        }

        static bool IsIgnoredViewControllers(UIViewController vc)
        {
            Debug.Print(vc?.GetType().Name);
            if (!vc.IsBeingDismissed)
            {
                if (vc is UIAlertController || vc is UIActivityViewController)
                {
                    vc.DismissViewController(true, null);
                }
            }
            return (vc is UIAlertController || vc is UIActivityViewController);
        }

        public static void GoToDeveloperToolsPage(UIViewController parent, UINavigationController controller)
        {
            UIViewController enDeveloperTools = NavigationHelper.ViewControllerByStoryboardName("ENDeveloperTools");
            controller?.PushViewController(enDeveloperTools, true);
        }

        public static void GoToOnboardingPage(UIViewController parent)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("Welcome", null);
            WelcomeViewController vc = storyboard.InstantiateInitialViewController() as WelcomeViewController;
            vc.IsOnBoarding = true;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            parent.PresentViewController(navigationController, true, null);
        }

        public static void GoToLanguageSelectionPage(UIViewController parent)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("LanguageSelection", null);
            LanguageSelectionViewController vc = storyboard.InstantiateInitialViewController() as LanguageSelectionViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            parent.PresentViewController(navigationController, true, null);
        }

        public static void GoToWelcomeWhatsNewPage(UIViewController parent)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("WelcomePageWhatIsNew", null);
            WelcomePageWhatIsNewViewController vc = storyboard.InstantiateInitialViewController() as WelcomePageWhatIsNewViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            parent.PresentViewController(navigationController, true, null);
        }


        public static void GoToResultPage(UIViewController parent, bool fromOnboarding)
        {
            UIViewController vc = InfectionStatusViewController.Create(fromOnboarding);

            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            parent.PresentViewController(navigationController, true, null);
        }

        public static void GoToResultPageIfOnboarded(UIViewController parent)
        {
            ConsentsHelper.DoActionWhenOnboarded(() => GoToResultPage(parent, false));
        }

        //NavigationController needs to have all viewcontrollers inside dismissed in order to go to result page
        public static void GoToResultPageFromAuthFlow(UINavigationController navigationController)
        {
            navigationController?.DismissViewController(true, null);
        }
    }
}

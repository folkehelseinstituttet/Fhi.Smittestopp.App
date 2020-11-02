using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices.ErrorHandlers;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public class IOSDialogService : IDialogService
    {
        public void ShowMessageDialog(string title, string message, string okBtn, PlatformDialogServiceArguments platformArguments)
        {
            UIViewController presentViewController = null;
            UIAlertController dialogViewController = null;
            if (platformArguments.Context != null && platformArguments.Context is UIViewController viewController)
            {
                presentViewController = viewController;
            }
            else
            {

                AttempToLookForPresentViewController(out presentViewController, out dialogViewController);
            }

            DialogViewModel viewModel = new DialogViewModel()
            {
                Title = title,
                Body = message,
                OkBtnTxt = okBtn
            };

            if (dialogViewController != null)
            {
                dialogViewController.DismissViewController(false, () =>
                {
                    DialogHelper.ShowDialog(presentViewController, viewModel, null);
                });
            }
            else
            {
                DialogHelper.ShowDialog(presentViewController, viewModel, null);
            }
        }

        void AttempToLookForPresentViewController(
            out UIViewController presentViewController,
            out UIAlertController dialogViewController)
        {
            dialogViewController = null;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            presentViewController = window.RootViewController;
            UIViewController previousPresentViewController = window.RootViewController;
            while (presentViewController.PresentedViewController != null)
            {
                previousPresentViewController = presentViewController;
                presentViewController = presentViewController.PresentedViewController;
            }

            if (presentViewController is UIAlertController alertViewController)
            {
                dialogViewController = alertViewController;
                presentViewController = previousPresentViewController;
            }
        }
    }
}
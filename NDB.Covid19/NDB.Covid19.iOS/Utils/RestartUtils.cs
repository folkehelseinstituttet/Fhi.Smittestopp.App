using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class RestartUtils
    {
        public static void RestartApp()
        {
            UIView.Transition(
                withView: UIApplication.SharedApplication.KeyWindow,
                duration: 1,
                options: UIViewAnimationOptions.TransitionNone,
                animation:
                () =>
                {
                    UIStoryboard storyboard = UIStoryboard.FromName("Initializer", null);
                    var vc = storyboard.InstantiateInitialViewController() as Views.Initializer.InizializerViewController;
                    vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

                    UIApplication.SharedApplication.KeyWindow.RootViewController = vc;
                },
                completion: () => { });
        }
    }
}
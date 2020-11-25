using NDB.Covid19.Interfaces;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public class IOSResetViews: IResetViews
    {
        public void ResetViews()
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
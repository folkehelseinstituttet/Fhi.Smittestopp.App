// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace NDB.Covid19.iOS.Views.Welcome
{
    [Register ("WelcomePageWhatIsNewViewController")]
    partial class WelcomePageWhatIsNewViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Label1 != null) {
                Label1.Dispose ();
                Label1 = null;
            }

            if (Label2 != null) {
                Label2.Dispose ();
                Label2 = null;
            }

            if (Label3 != null) {
                Label3.Dispose ();
                Label3 = null;
            }

            if (LabelBottom != null) {
                LabelBottom.Dispose ();
                LabelBottom = null;
            }

            if (NextButton != null) {
                NextButton.Dispose ();
                NextButton = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
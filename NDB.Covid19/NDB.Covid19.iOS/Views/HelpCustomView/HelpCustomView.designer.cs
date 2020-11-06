// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NDB.Covid19.iOS.Views.HelpCustomView
{
    [Register ("HelpCustomView")]
    partial class HelpCustomView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TextContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TextLabel { get; set; }

        [Action ("OnCloseBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundView != null) {
                BackgroundView.Dispose ();
                BackgroundView = null;
            }

            if (CloseBtn != null) {
                CloseBtn.Dispose ();
                CloseBtn = null;
            }

            if (TextContainerView != null) {
                TextContainerView.Dispose ();
                TextContainerView = null;
            }

            if (TextLabel != null) {
                TextLabel.Dispose ();
                TextLabel = null;
            }
        }
    }
}
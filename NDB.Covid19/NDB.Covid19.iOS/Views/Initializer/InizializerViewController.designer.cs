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

namespace NDB.Covid19.iOS.Views.Initializer
{
    [Register ("InizializerViewController")]
    partial class InizializerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContinueInEnLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView ContinueInEnStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView HeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartButtonNB { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartButtonNN { get; set; }

        [Action ("StartButtonNB_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void StartButtonNB_TouchUpInside (UIKit.UIButton sender);

        [Action ("StartButtonNN_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void StartButtonNN_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContinueInEnLbl != null) {
                ContinueInEnLbl.Dispose ();
                ContinueInEnLbl = null;
            }

            if (ContinueInEnStackView != null) {
                ContinueInEnStackView.Dispose ();
                ContinueInEnStackView = null;
            }

            if (HeaderView != null) {
                HeaderView.Dispose ();
                HeaderView = null;
            }

            if (StartButtonNB != null) {
                StartButtonNB.Dispose ();
                StartButtonNB = null;
            }

            if (StartButtonNN != null) {
                StartButtonNN.Dispose ();
                StartButtonNN = null;
            }
        }
    }
}
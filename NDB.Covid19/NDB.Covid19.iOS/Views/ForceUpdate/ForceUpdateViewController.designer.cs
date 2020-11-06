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

namespace NDB.Covid19.iOS.Views.ForceUpdate
{
    [Register ("ForceUpdateViewController")]
    partial class ForceUpdateViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AppStoreLinkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TextLabel { get; set; }

        [Action ("AppStoreLinkButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AppStoreLinkButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AppStoreLinkButton != null) {
                AppStoreLinkButton.Dispose ();
                AppStoreLinkButton = null;
            }

            if (TextLabel != null) {
                TextLabel.Dispose ();
                TextLabel = null;
            }
        }
    }
}
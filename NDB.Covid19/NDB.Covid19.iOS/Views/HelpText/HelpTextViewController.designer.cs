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

namespace NDB.Covid19.iOS.Views.HelpText
{
    [Register ("HelpTextViewController")]
    partial class HelpTextViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView BodyText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Action ("OkBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OkBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BodyText != null) {
                BodyText.Dispose ();
                BodyText = null;
            }

            if (OkBtn != null) {
                OkBtn.Dispose ();
                OkBtn = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
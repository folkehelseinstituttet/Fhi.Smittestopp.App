// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.ErrorStatus
{
    [Register ("ErrorPageViewController")]
    partial class ErrorPageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsContentTextView ErrorMessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ErrorTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkButton { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("DismissErrorBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DismissErrorBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (ErrorMessageLabel != null) {
                ErrorMessageLabel.Dispose ();
                ErrorMessageLabel = null;
            }

            if (ErrorTitleLabel != null) {
                ErrorTitleLabel.Dispose ();
                ErrorTitleLabel = null;
            }

            if (OkButton != null) {
                OkButton.Dispose ();
                OkButton = null;
            }
        }
    }
}
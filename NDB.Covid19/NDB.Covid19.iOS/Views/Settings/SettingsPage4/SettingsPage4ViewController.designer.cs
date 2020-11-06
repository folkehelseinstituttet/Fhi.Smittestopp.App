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

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage4
{
    [Register ("SettingsPage4ViewController")]
    partial class SettingsPage4ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.SettingsContentTextView ContentText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CustomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (ContentText != null) {
                ContentText.Dispose ();
                ContentText = null;
            }

            if (CustomView != null) {
                CustomView.Dispose ();
                CustomView = null;
            }

            if (HeaderLabel != null) {
                HeaderLabel.Dispose ();
                HeaderLabel = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }
        }
    }
}
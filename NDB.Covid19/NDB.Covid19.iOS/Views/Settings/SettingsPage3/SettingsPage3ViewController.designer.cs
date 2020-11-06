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

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage3
{
    [Register ("SettingsPage3ViewController")]
    partial class SettingsPage3ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint DeleteBtnWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.DefaultBorderButton DeleteConsentBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView LabelStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.SetttingsPageTitleLabel PageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView TitleStackView { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("DeleteConsentBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DeleteConsentBtn_TouchUpInside (Views.CustomSubclasses.DefaultBorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (DeleteBtnWidthConstraint != null) {
                DeleteBtnWidthConstraint.Dispose ();
                DeleteBtnWidthConstraint = null;
            }

            if (DeleteConsentBtn != null) {
                DeleteConsentBtn.Dispose ();
                DeleteConsentBtn = null;
            }

            if (LabelStackView != null) {
                LabelStackView.Dispose ();
                LabelStackView = null;
            }

            if (PageTitle != null) {
                PageTitle.Dispose ();
                PageTitle = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (TitleStackView != null) {
                TitleStackView.Dispose ();
                TitleStackView = null;
            }
        }
    }
}
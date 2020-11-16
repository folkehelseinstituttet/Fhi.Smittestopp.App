// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage3
{
    [Register ("SettingsPage3ViewController")]
    partial class SettingsPage3ViewController
    {
        [Outlet]
        UIKit.UIView BottomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView BottomStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint DeleteBtnWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton DeleteConsentBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView LabelStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel PageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView TitleStackView { get; set; }


        [Action ("BackButton_TouchUpInside:")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);


        [Action ("DeleteConsentBtn_TouchUpInside:")]
        partial void DeleteConsentBtn_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (BottomStackView != null) {
                BottomStackView.Dispose ();
                BottomStackView = null;
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
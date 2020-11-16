// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    [Register ("InformationAndConsentViewController")]
    partial class InformationAndConsentViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel BodyOneLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel BodyTwoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel DescriptionOneLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton LogInWithIDPortenBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel TitleLabel { get; set; }

        [Action ("OnCloseBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);

        [Action ("OnLoginBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnLoginBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BodyOneLabel != null) {
                BodyOneLabel.Dispose ();
                BodyOneLabel = null;
            }

            if (BodyTwoLabel != null) {
                BodyTwoLabel.Dispose ();
                BodyTwoLabel = null;
            }

            if (CloseBtn != null) {
                CloseBtn.Dispose ();
                CloseBtn = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (DescriptionOneLabel != null) {
                DescriptionOneLabel.Dispose ();
                DescriptionOneLabel = null;
            }

            if (HeaderLabel != null) {
                HeaderLabel.Dispose ();
                HeaderLabel = null;
            }

            if (LogInWithIDPortenBtn != null) {
                LogInWithIDPortenBtn.Dispose ();
                LogInWithIDPortenBtn = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
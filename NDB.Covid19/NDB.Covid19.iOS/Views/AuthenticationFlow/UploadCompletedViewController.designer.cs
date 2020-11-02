// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.UploadCompleted
{
    [Register ("UploadCompletedViewController")]
    partial class UploadCompletedViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BoxContentLabelTwo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.SettingsPageContentLabel BoxTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BoxView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.SettingsPageContentLabel ContentLabelOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.SettingsPageContentLabel ContentLabelTwo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView HealthAuthoritiesLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LearnMoreView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        NDB.Covid19.iOS.SetttingsPageTitleLabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ToStartPageBtn { get; set; }

        [Action ("CloseButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("GoToStartPageButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void GoToStartPageButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BoxContentLabelTwo != null) {
                BoxContentLabelTwo.Dispose ();
                BoxContentLabelTwo = null;
            }

            if (BoxTitleLabel != null) {
                BoxTitleLabel.Dispose ();
                BoxTitleLabel = null;
            }

            if (BoxView != null) {
                BoxView.Dispose ();
                BoxView = null;
            }

            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (ContentLabelOne != null) {
                ContentLabelOne.Dispose ();
                ContentLabelOne = null;
            }

            if (ContentLabelTwo != null) {
                ContentLabelTwo.Dispose ();
                ContentLabelTwo = null;
            }

            if (HealthAuthoritiesLogo != null) {
                HealthAuthoritiesLogo.Dispose ();
                HealthAuthoritiesLogo = null;
            }

            if (LearnMoreView != null) {
                LearnMoreView.Dispose ();
                LearnMoreView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (ToStartPageBtn != null) {
                ToStartPageBtn.Dispose ();
                ToStartPageBtn = null;
            }
        }
    }
}
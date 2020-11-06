// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPageGeneral
{
    [Register ("SettingsPageGeneralSettingsViewController")]
    partial class SettingsPageGeneralSettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChooseLanguageHeaderLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContentLabelOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Header { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HeaderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.RadioButton RadioButton1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RadioButton1Lbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Views.CustomSubclasses.RadioButton RadioButton2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RadioButton2Lbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RestartAppLabl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SmittestopLinkButtionLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView SmittestopLinkButtonStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchButton { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("RadioButton1_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RadioButton1_TouchUpInside (Views.CustomSubclasses.RadioButton sender);

        [Action ("RadioButton2_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RadioButton2_TouchUpInside (Views.CustomSubclasses.RadioButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (ChooseLanguageHeaderLbl != null) {
                ChooseLanguageHeaderLbl.Dispose ();
                ChooseLanguageHeaderLbl = null;
            }

            if (ContentLabel != null) {
                ContentLabel.Dispose ();
                ContentLabel = null;
            }

            if (ContentLabelOne != null) {
                ContentLabelOne.Dispose ();
                ContentLabelOne = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (Header != null) {
                Header.Dispose ();
                Header = null;
            }

            if (HeaderLabel != null) {
                HeaderLabel.Dispose ();
                HeaderLabel = null;
            }

            if (RadioButton1 != null) {
                RadioButton1.Dispose ();
                RadioButton1 = null;
            }

            if (RadioButton1Lbl != null) {
                RadioButton1Lbl.Dispose ();
                RadioButton1Lbl = null;
            }

            if (RadioButton2 != null) {
                RadioButton2.Dispose ();
                RadioButton2 = null;
            }

            if (RadioButton2Lbl != null) {
                RadioButton2Lbl.Dispose ();
                RadioButton2Lbl = null;
            }

            if (RestartAppLabl != null) {
                RestartAppLabl.Dispose ();
                RestartAppLabl = null;
            }

            if (SmittestopLinkButtionLbl != null) {
                SmittestopLinkButtionLbl.Dispose ();
                SmittestopLinkButtionLbl = null;
            }

            if (SmittestopLinkButtonStackView != null) {
                SmittestopLinkButtonStackView.Dispose ();
                SmittestopLinkButtonStackView = null;
            }

            if (switchButton != null) {
                switchButton.Dispose ();
                switchButton = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.ConsentView
{
    [Register ("ConsentViewController")]
    partial class ConsentViewController
    {
        [Outlet]
        UIKit.UIStackView AcceptSwitch { get; set; }


        [Outlet]
        UIKit.UISwitch AcceptSwitchBtn { get; set; }


        [Outlet]
        UIKit.UILabel AcceptTextLbl { get; set; }


        [Outlet]
        UIKit.UIActivityIndicatorView ActivityIndicator { get; set; }


        [Outlet]
        UIKit.UIButton BackBtn { get; set; }


        [Outlet]
        UIKit.UILabel Header1Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header2Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header3Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header4Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header5Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header6Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header7Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header8Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Header9Lbl { get; set; }


        [Outlet]
        UIKit.UIButton NextBtn { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph1Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph2Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph3Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph4Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph5Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph6Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph7Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph8Lbl { get; set; }


        [Outlet]
        UIKit.UILabel Paragraph9Lbl { get; set; }


        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel WarningLbl { get; set; }


        [Outlet]
        UIKit.UIView WarningView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel About_header { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel About_section1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel About_section2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContactInformation_section { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HowItWorks_header { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HowItWorks_section1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HowItWorks_section2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MoreInformation_section { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PrivacyPolicy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Samtykke_section1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Samtykke_section2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SamtykkeBottom_header { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SamtykkeBottom_section { get; set; }


        [Action ("AcceptSwitched:")]
        partial void AcceptSwitched (UIKit.UISwitch sender);


        [Action ("BackBtn_TouchUpInside:")]
        partial void BackBtn_TouchUpInside (UIKit.UIButton sender);


        [Action ("NextBtn_TouchUpInside:")]
        partial void NextBtn_TouchUpInside (UIKit.UIButton sender);


        [Action ("UIButtonfuZKK3g0_TouchUpInside:")]
        partial void UIButtonfuZKK3g0_TouchUpInside (UIKit.UIButton sender);

        [Action ("PrivacyPolicy_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PrivacyPolicy_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (About_header != null) {
                About_header.Dispose ();
                About_header = null;
            }

            if (About_section1 != null) {
                About_section1.Dispose ();
                About_section1 = null;
            }

            if (About_section2 != null) {
                About_section2.Dispose ();
                About_section2 = null;
            }

            if (ContactInformation_section != null) {
                ContactInformation_section.Dispose ();
                ContactInformation_section = null;
            }

            if (HowItWorks_header != null) {
                HowItWorks_header.Dispose ();
                HowItWorks_header = null;
            }

            if (HowItWorks_section1 != null) {
                HowItWorks_section1.Dispose ();
                HowItWorks_section1 = null;
            }

            if (HowItWorks_section2 != null) {
                HowItWorks_section2.Dispose ();
                HowItWorks_section2 = null;
            }

            if (MoreInformation_section != null) {
                MoreInformation_section.Dispose ();
                MoreInformation_section = null;
            }

            if (PrivacyPolicy != null) {
                PrivacyPolicy.Dispose ();
                PrivacyPolicy = null;
            }

            if (Samtykke_section1 != null) {
                Samtykke_section1.Dispose ();
                Samtykke_section1 = null;
            }

            if (Samtykke_section2 != null) {
                Samtykke_section2.Dispose ();
                Samtykke_section2 = null;
            }

            if (SamtykkeBottom_header != null) {
                SamtykkeBottom_header.Dispose ();
                SamtykkeBottom_header = null;
            }

            if (SamtykkeBottom_section != null) {
                SamtykkeBottom_section.Dispose ();
                SamtykkeBottom_section = null;
            }
        }
    }
}
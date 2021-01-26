// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

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
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton RadioButton1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel RadioButton1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton RadioButton2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel RadioButton2Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton RadioButton3 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel RadioButton3Lbl { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton RadioButton4 { get; set; }

		[Outlet]
		UIKit.UILabel RadioButton4Lbl { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton RadioButton5 { get; set; }

		[Outlet]
		UIKit.UILabel RadioButton5Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel RestartAppLabl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel SmittestopLinkButtonLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView SmittestopLinkButtonStackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UISwitch switchButton { get; set; }

		[Action ("BackButton_TouchUpInside:")]
		partial void BackButton_TouchUpInside (UIKit.UIButton sender);

		[Action ("RadioButton1_TouchUpInside:")]
		partial void RadioButton1_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("RadioButton2_TouchUpInside:")]
		partial void RadioButton2_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("RadioButton3_TouchUpInside:")]
		partial void RadioButton3_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("RadioButton4_TouchUpInside:")]
		partial void RadioButton4_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("RadioButton5_TouchUpInside:")]
		partial void RadioButton5_TouchUpInside (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);
		
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

			if (RadioButton3 != null) {
				RadioButton3.Dispose ();
				RadioButton3 = null;
			}

			if (RadioButton3Lbl != null) {
				RadioButton3Lbl.Dispose ();
				RadioButton3Lbl = null;
			}

			if (RadioButton4 != null) {
				RadioButton4.Dispose ();
				RadioButton4 = null;
			}

			if (RadioButton4Lbl != null) {
				RadioButton4Lbl.Dispose ();
				RadioButton4Lbl = null;
			}

			if (RadioButton5 != null) {
				RadioButton5.Dispose ();
				RadioButton5 = null;
			}

			if (RadioButton5Lbl != null) {
				RadioButton5Lbl.Dispose ();
				RadioButton5Lbl = null;
			}

			if (RestartAppLabl != null) {
				RestartAppLabl.Dispose ();
				RestartAppLabl = null;
			}

			if (SmittestopLinkButtonLbl != null) {
				SmittestopLinkButtonLbl.Dispose ();
				SmittestopLinkButtonLbl = null;
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

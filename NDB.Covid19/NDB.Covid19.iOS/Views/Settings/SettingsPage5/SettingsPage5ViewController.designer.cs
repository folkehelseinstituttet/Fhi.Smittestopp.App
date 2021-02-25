// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage5
{
	[Register ("SettingsPage5ViewController")]
	partial class SettingsPage5ViewController
	{
		[Outlet]
		UIKit.UIButton AccessibilityStatementButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton BackButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel BuildVersionLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextView ContentText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

		[Action ("AccessibilityStatementButton_TouchUpInside:")]
		partial void AccessibilityStatementButton_TouchUpInside (UIKit.UIButton sender);

		[Action ("BackButton_TouchUpInside:")]
		partial void BackButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (BuildVersionLbl != null) {
				BuildVersionLbl.Dispose ();
				BuildVersionLbl = null;
			}

			if (ContentText != null) {
				ContentText.Dispose ();
				ContentText = null;
			}

			if (AccessibilityStatementButton != null) {
				AccessibilityStatementButton.Dispose ();
				AccessibilityStatementButton = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}
		}
	}
}

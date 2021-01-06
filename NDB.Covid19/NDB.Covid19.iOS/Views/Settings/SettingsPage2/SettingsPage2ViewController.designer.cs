// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage2
{
	[Register ("SettingsPage2ViewController")]
	partial class SettingsPage2ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton BackButton { get; set; }

		[Outlet]
		UIKit.UILabel Header1Label { get; set; }

		[Outlet]
		UIKit.UILabel Header2Label { get; set; }

		[Outlet]
		UIKit.UILabel Header3Label { get; set; }

		[Outlet]
		UIKit.UILabel Header4Label { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

		[Outlet]
		UIKit.UILabel IntroLabel { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph1Label1 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph1Label2 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph2Label1 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph2Label2 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph2Label3 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph2Label4 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph3Label1 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph4Label1 { get; set; }

		[Outlet]
		UIKit.UILabel Paragraph4Label2 { get; set; }

		[Outlet]
		UIKit.UITextView Paragraph4Label3 { get; set; }

		[Action ("BackButton_TouchUpInside:")]
		partial void BackButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (Header1Label != null) {
				Header1Label.Dispose ();
				Header1Label = null;
			}

			if (Header2Label != null) {
				Header2Label.Dispose ();
				Header2Label = null;
			}

			if (Header3Label != null) {
				Header3Label.Dispose ();
				Header3Label = null;
			}

			if (Header4Label != null) {
				Header4Label.Dispose ();
				Header4Label = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (IntroLabel != null) {
				IntroLabel.Dispose ();
				IntroLabel = null;
			}

			if (Paragraph1Label1 != null) {
				Paragraph1Label1.Dispose ();
				Paragraph1Label1 = null;
			}

			if (Paragraph1Label2 != null) {
				Paragraph1Label2.Dispose ();
				Paragraph1Label2 = null;
			}

			if (Paragraph2Label1 != null) {
				Paragraph2Label1.Dispose ();
				Paragraph2Label1 = null;
			}

			if (Paragraph2Label2 != null) {
				Paragraph2Label2.Dispose ();
				Paragraph2Label2 = null;
			}

			if (Paragraph2Label3 != null) {
				Paragraph2Label3.Dispose ();
				Paragraph2Label3 = null;
			}

			if (Paragraph2Label4 != null) {
				Paragraph2Label4.Dispose ();
				Paragraph2Label4 = null;
			}

			if (Paragraph3Label1 != null) {
				Paragraph3Label1.Dispose ();
				Paragraph3Label1 = null;
			}

			if (Paragraph4Label1 != null) {
				Paragraph4Label1.Dispose ();
				Paragraph4Label1 = null;
			}

			if (Paragraph4Label2 != null) {
				Paragraph4Label2.Dispose ();
				Paragraph4Label2 = null;
			}

			if (Paragraph4Label3 != null) {
				Paragraph4Label3.Dispose ();
				Paragraph4Label3 = null;
			}
		}
	}
}

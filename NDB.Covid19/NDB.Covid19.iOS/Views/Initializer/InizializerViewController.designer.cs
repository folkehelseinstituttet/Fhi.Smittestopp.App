// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Initializer
{
	[Register ("InizializerViewController")]
	partial class InizializerViewController
	{
		[Outlet]
		UIKit.UIImageView appLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ContinueInEnLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView ContinueInEnStackView { get; set; }

		[Outlet]
		UIKit.UIImageView fhiLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView HeaderView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton StartButtonNB { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton StartButtonNN { get; set; }

		[Action ("StartButtonNB_TouchUpInside:")]
		partial void StartButtonNB_TouchUpInside (UIKit.UIButton sender);

		[Action ("StartButtonNN_TouchUpInside:")]
		partial void StartButtonNN_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ContinueInEnLbl != null) {
				ContinueInEnLbl.Dispose ();
				ContinueInEnLbl = null;
			}

			if (ContinueInEnStackView != null) {
				ContinueInEnStackView.Dispose ();
				ContinueInEnStackView = null;
			}

			if (HeaderView != null) {
				HeaderView.Dispose ();
				HeaderView = null;
			}

			if (StartButtonNB != null) {
				StartButtonNB.Dispose ();
				StartButtonNB = null;
			}

			if (StartButtonNN != null) {
				StartButtonNN.Dispose ();
				StartButtonNN = null;
			}

			if (fhiLogo != null) {
				fhiLogo.Dispose ();
				fhiLogo = null;
			}

			if (appLogo != null) {
				appLogo.Dispose ();
				appLogo = null;
			}
		}
	}
}

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
		UIKit.UIImageView fhiLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView HeaderView { get; set; }

		[Outlet]
		UIKit.UIButton StartButton { get; set; }

		[Action ("StartButton_TouchUpInside:")]
		partial void StartButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (appLogo != null) {
				appLogo.Dispose ();
				appLogo = null;
			}

			if (fhiLogo != null) {
				fhiLogo.Dispose ();
				fhiLogo = null;
			}

			if (HeaderView != null) {
				HeaderView.Dispose ();
				HeaderView = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}
		}
	}
}

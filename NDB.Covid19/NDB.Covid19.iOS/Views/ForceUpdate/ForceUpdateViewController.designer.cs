// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.ForceUpdate
{
	[Register ("ForceUpdateViewController")]
	partial class ForceUpdateViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton AppStoreLinkButton { get; set; }

		[Outlet]
		UIKit.UIImageView FhiLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TextLabel { get; set; }

		[Action ("AppStoreLinkButton_TouchUpInside:")]
		partial void AppStoreLinkButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AppStoreLinkButton != null) {
				AppStoreLinkButton.Dispose ();
				AppStoreLinkButton = null;
			}

			if (TextLabel != null) {
				TextLabel.Dispose ();
				TextLabel = null;
			}

			if (FhiLogo != null) {
				FhiLogo.Dispose ();
				FhiLogo = null;
			}
		}
	}
}

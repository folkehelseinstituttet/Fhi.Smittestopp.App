// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
	[Register ("WelcomePageTwoViewController")]
	partial class WelcomePageTwoViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton BackArrow { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel BodyText1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel BodyText2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel PageTitle { get; set; }

		[Action ("BackArrowBtn_TouchUpInside:")]
		partial void BackArrowBtn_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackArrow != null) {
				BackArrow.Dispose ();
				BackArrow = null;
			}

			if (BodyText1 != null) {
				BodyText1.Dispose ();
				BodyText1 = null;
			}

			if (BodyText2 != null) {
				BodyText2.Dispose ();
				BodyText2 = null;
			}

			if (PageTitle != null) {
				PageTitle.Dispose ();
				PageTitle = null;
			}
		}
	}
}

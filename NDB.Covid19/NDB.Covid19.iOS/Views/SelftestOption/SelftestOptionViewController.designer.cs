// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.SelftestOption
{
	[Register ("SelftestOptionViewController")]
	partial class SelftestOptionViewController
	{
		[Outlet]
		UIKit.UIButton CloseBtn { get; set; }

		[Outlet]
		UIKit.UIButton ContinueWithMSISBtn { get; set; }

		[Outlet]
		UIKit.UIButton ContinueWithSelfTestBtn { get; set; }

		[Outlet]
		UIKit.UILabel Header { get; set; }

		[Action ("CloseBtn_TouchUpInside:")]
		partial void CloseBtn_TouchUpInside (UIKit.UIButton sender);

		[Action ("ContinueWithMSISBtn_TouchUpInside:")]
		partial void ContinueWithMSISBtn_TouchUpInside (UIKit.UIButton sender);

		[Action ("ContinueWithSelftestBtn_TouchUpInside:")]
		partial void ContinueWithSelftestBtn_TouchUpInside (UIKit.UIButton sender);

		
		void ReleaseDesignerOutlets ()
		{
			if (CloseBtn != null) {
				CloseBtn.Dispose ();
				CloseBtn = null;
			}

			if (ContinueWithMSISBtn != null) {
				ContinueWithMSISBtn.Dispose ();
				ContinueWithMSISBtn = null;
			}

			if (ContinueWithSelfTestBtn != null) {
				ContinueWithSelfTestBtn.Dispose ();
				ContinueWithSelfTestBtn = null;
			}

			if (Header != null) {
				Header.Dispose ();
				Header = null;
			}
		}
	}
}

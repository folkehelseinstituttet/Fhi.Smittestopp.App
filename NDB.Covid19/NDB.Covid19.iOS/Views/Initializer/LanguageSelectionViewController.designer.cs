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
	[Register ("LanguageSelectionViewController")]
	partial class LanguageSelectionViewController
	{
		[Outlet]
		UIKit.UIButton ArabicButton { get; set; }

		[Outlet]
		UIKit.UIButton BokmalButton { get; set; }

		[Outlet]
		UIKit.UIButton EnglishButton { get; set; }

		[Outlet]
		UIKit.UIButton LithuanianButton { get; set; }

		[Outlet]
		UIKit.UIButton NynorskButton { get; set; }

		[Outlet]
		UIKit.UIButton PolishButton { get; set; }

		[Outlet]
		UIKit.UIButton SomaliButton { get; set; }

		[Outlet]
		UIKit.UIButton TigrinyaButton { get; set; }

		[Outlet]
		UIKit.UIButton UrduButton { get; set; }

		[Action ("ArabicButton_TouchUpInside:")]
		partial void ArabicButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("BokmalButton_TouchUpInside:")]
		partial void BokmalButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("EnglishButton_TouchUpInside:")]
		partial void EnglishButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("LithuanianButton_TouchUpInside:")]
		partial void LithuanianButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("NynorskButton_TouchUpInside:")]
		partial void NynorskButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("PolishButton_TouchUpInside:")]
		partial void PolishButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("SomaliButton_TouchUpInside:")]
		partial void SomaliButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("TigrinyaButton_TouchUpInside:")]
		partial void TigrinyaButton_TouchUpInside (Foundation.NSObject sender);

		[Action ("UrduButton_TouchUpInside:")]
		partial void UrduButton_TouchUpInside (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ArabicButton != null) {
				ArabicButton.Dispose ();
				ArabicButton = null;
			}

			if (BokmalButton != null) {
				BokmalButton.Dispose ();
				BokmalButton = null;
			}

			if (EnglishButton != null) {
				EnglishButton.Dispose ();
				EnglishButton = null;
			}

			if (NynorskButton != null) {
				NynorskButton.Dispose ();
				NynorskButton = null;
			}

			if (LithuanianButton != null) {
				LithuanianButton.Dispose ();
				LithuanianButton = null;
			}

			if (PolishButton != null) {
				PolishButton.Dispose ();
				PolishButton = null;
			}

			if (SomaliButton != null) {
				SomaliButton.Dispose ();
				SomaliButton = null;
			}

			if (TigrinyaButton != null) {
				TigrinyaButton.Dispose ();
				TigrinyaButton = null;
			}

			if (UrduButton != null) {
				UrduButton.Dispose ();
				UrduButton = null;
			}
		}
	}
}

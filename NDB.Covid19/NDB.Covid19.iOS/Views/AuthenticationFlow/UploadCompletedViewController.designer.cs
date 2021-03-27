// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
	[Register ("UploadCompletedViewController")]
	partial class UploadCompletedViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel BoxContentLabelTwo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel BoxTitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView BoxView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel ContentLabelOne { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel ContentLabelTwo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView HealthAuthoritiesLogo { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel TitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton ToStartPageBtn { get; set; }

		[Action ("CloseButton_TouchUpInside:")]
		partial void CloseButton_TouchUpInside (UIKit.UIButton sender);

		[Action ("GoToStartPageButton_TouchUpInside:")]
		partial void GoToStartPageButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (BoxContentLabelTwo != null) {
				BoxContentLabelTwo.Dispose ();
				BoxContentLabelTwo = null;
			}

			if (BoxTitleLabel != null) {
				BoxTitleLabel.Dispose ();
				BoxTitleLabel = null;
			}

			if (BoxView != null) {
				BoxView.Dispose ();
				BoxView = null;
			}

			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (ContentLabelOne != null) {
				ContentLabelOne.Dispose ();
				ContentLabelOne = null;
			}

			if (ContentLabelTwo != null) {
				ContentLabelTwo.Dispose ();
				ContentLabelTwo = null;
			}

			if (HealthAuthoritiesLogo != null) {
				HealthAuthoritiesLogo.Dispose ();
				HealthAuthoritiesLogo = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (ToStartPageBtn != null) {
				ToStartPageBtn.Dispose ();
				ToStartPageBtn = null;
			}
		}
	}
}

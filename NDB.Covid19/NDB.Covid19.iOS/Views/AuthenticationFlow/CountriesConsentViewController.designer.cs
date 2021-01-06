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
	[Register ("CountriesConsentViewController")]
	partial class CountriesConsentViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton CloseBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel Consent_BeAware_Text { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel Consent_Explanation_Text { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel DescriptionLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel LookUp_Header { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel LookUp_Text { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextButtonWithEUConsent { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextButtonWithoutEUConsent { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel Notification_Header { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel Notification_Text { get; set; }

		[Action ("OnCloseBtnTapped:")]
		partial void OnCloseBtnTapped (UIKit.UIButton sender);

		[Action ("OnNextWithEUConsent:")]
		partial void OnNextWithEUConsent (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

		[Action ("OnNextWithoutEUConsent:")]
		partial void OnNextWithoutEUConsent (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CloseBtn != null) {
				CloseBtn.Dispose ();
				CloseBtn = null;
			}

			if (Consent_BeAware_Text != null) {
				Consent_BeAware_Text.Dispose ();
				Consent_BeAware_Text = null;
			}

			if (Consent_Explanation_Text != null) {
				Consent_Explanation_Text.Dispose ();
				Consent_Explanation_Text = null;
			}

			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (LookUp_Header != null) {
				LookUp_Header.Dispose ();
				LookUp_Header = null;
			}

			if (LookUp_Text != null) {
				LookUp_Text.Dispose ();
				LookUp_Text = null;
			}

			if (NextButtonWithEUConsent != null) {
				NextButtonWithEUConsent.Dispose ();
				NextButtonWithEUConsent = null;
			}

			if (NextButtonWithoutEUConsent != null) {
				NextButtonWithoutEUConsent.Dispose ();
				NextButtonWithoutEUConsent = null;
			}

			if (Notification_Header != null) {
				Notification_Header.Dispose ();
				Notification_Header = null;
			}

			if (Notification_Text != null) {
				Notification_Text.Dispose ();
				Notification_Text = null;
			}
		}
	}
}

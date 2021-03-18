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
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel BodyText1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel BodyText2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton CloseBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel Consent_onlyNorway_Explanation { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel ConsentEU_Explanation { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel ConsentText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel HeaderLabel { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextButtonOnlyNorwayConsent { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextButtonWithEUConsent { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel ShareHeader { get; set; }

		[Action ("OnCloseBtnTapped:")]
		partial void OnCloseBtnTapped (UIKit.UIButton sender);

		[Action ("OnNextOnlyNorwayConsent:")]
		partial void OnNextOnlyNorwayConsent (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

		[Action ("OnNextWithEUConsent:")]
		partial void OnNextWithEUConsent (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (BodyText1 != null) {
				BodyText1.Dispose ();
				BodyText1 = null;
			}

			if (BodyText2 != null) {
				BodyText2.Dispose ();
				BodyText2 = null;
			}

			if (CloseBtn != null) {
				CloseBtn.Dispose ();
				CloseBtn = null;
			}

			if (Consent_onlyNorway_Explanation != null) {
				Consent_onlyNorway_Explanation.Dispose ();
				Consent_onlyNorway_Explanation = null;
			}

			if (ConsentEU_Explanation != null) {
				ConsentEU_Explanation.Dispose ();
				ConsentEU_Explanation = null;
			}

			if (ConsentText != null) {
				ConsentText.Dispose ();
				ConsentText = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (NextButtonOnlyNorwayConsent != null) {
				NextButtonOnlyNorwayConsent.Dispose ();
				NextButtonOnlyNorwayConsent = null;
			}

			if (NextButtonWithEUConsent != null) {
				NextButtonWithEUConsent.Dispose ();
				NextButtonWithEUConsent = null;
			}

			if (ShareHeader != null) {
				ShareHeader.Dispose ();
				ShareHeader = null;
			}
		}
	}
}

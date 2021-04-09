// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
	[Register ("QuestionnaireCountriesViewController")]
	partial class QuestionnaireCountriesViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView ButtonView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView CountryTableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ListExplainLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextBtn { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel SubtitleLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.NSLayoutConstraint TableViewHeightConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TitleLbl { get; set; }

		[Action ("NextBtnTapped:")]
		partial void NextBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

		[Action ("OnCloseBtnTapped:")]
		partial void OnCloseBtnTapped (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ButtonView != null) {
				ButtonView.Dispose ();
				ButtonView = null;
			}

			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (CountryTableView != null) {
				CountryTableView.Dispose ();
				CountryTableView = null;
			}

			if (ListExplainLbl != null) {
				ListExplainLbl.Dispose ();
				ListExplainLbl = null;
			}

			if (NextBtn != null) {
				NextBtn.Dispose ();
				NextBtn = null;
			}

			if (SubtitleLbl != null) {
				SubtitleLbl.Dispose ();
				SubtitleLbl = null;
			}

			if (TableViewHeightConstraint != null) {
				TableViewHeightConstraint.Dispose ();
				TableViewHeightConstraint = null;
			}

			if (TitleLbl != null) {
				TitleLbl.Dispose ();
				TitleLbl = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NDB.Covid19.iOS
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
        NDB.Covid19.iOS.DefaultBorderButton NextBtn { get; set; }

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
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NextBtnTapped (NDB.Covid19.iOS.DefaultBorderButton sender);

        [Action ("OnCloseBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
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
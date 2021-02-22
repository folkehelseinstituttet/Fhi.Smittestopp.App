// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.InfectionStatus
{
    [Register ("InfectionStatusViewController")]
    partial class InfectionStatusViewController
    {
        [Outlet]
        UIKit.UIImageView appLogo { get; set; }


        [Outlet]
        UIKit.UILabel MenuLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ActivityExplainerLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AreYouInfectetLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AreYouInfectetView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView fhiLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LogInAndRegisterLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MenuIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MessageIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MessageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NewRegistrationLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OnOffBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView OnOffBtnContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ScrollDownBackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView StatusContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StatusText { get; set; }


        [Action ("OnMenubtnTapped:")]
        partial void OnMenubtnTapped (UIKit.UIButton sender);


        [Action ("OnOffBtnTapped:")]
        partial void OnOffBtnTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ActivityExplainerLbl != null) {
                ActivityExplainerLbl.Dispose ();
                ActivityExplainerLbl = null;
            }

            if (AreYouInfectetLbl != null) {
                AreYouInfectetLbl.Dispose ();
                AreYouInfectetLbl = null;
            }

            if (AreYouInfectetView != null) {
                AreYouInfectetView.Dispose ();
                AreYouInfectetView = null;
            }

            if (fhiLogo != null) {
                fhiLogo.Dispose ();
                fhiLogo = null;
            }

            if (LogInAndRegisterLbl != null) {
                LogInAndRegisterLbl.Dispose ();
                LogInAndRegisterLbl = null;
            }

            if (MenuIcon != null) {
                MenuIcon.Dispose ();
                MenuIcon = null;
            }

            if (MessageIcon != null) {
                MessageIcon.Dispose ();
                MessageIcon = null;
            }

            if (MessageLbl != null) {
                MessageLbl.Dispose ();
                MessageLbl = null;
            }

            if (MessageView != null) {
                MessageView.Dispose ();
                MessageView = null;
            }

            if (NewRegistrationLbl != null) {
                NewRegistrationLbl.Dispose ();
                NewRegistrationLbl = null;
            }

            if (OnOffBtn != null) {
                OnOffBtn.Dispose ();
                OnOffBtn = null;
            }

            if (OnOffBtnContainer != null) {
                OnOffBtnContainer.Dispose ();
                OnOffBtnContainer = null;
            }

            if (ScrollDownBackgroundView != null) {
                ScrollDownBackgroundView.Dispose ();
                ScrollDownBackgroundView = null;
            }

            if (StatusContainer != null) {
                StatusContainer.Dispose ();
                StatusContainer = null;
            }

            if (StatusText != null) {
                StatusText.Dispose ();
                StatusText = null;
            }
        }
    }
}
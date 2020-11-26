// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Welcome
{
    [Register ("WelcomeViewController")]
    partial class WelcomeViewController
    {
        [Outlet]
        UIKit.UILabel HelpTextLabel { get; set; }


        [Outlet]
        UIKit.UIButton LegislationBtn { get; set; }


        [Outlet]
        UIKit.UIButton OkBtn { get; set; }


        [Outlet]
        UIKit.UIImageView WelcomeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView ActivityIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView ButtonsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NextBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl PageControl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PreviousBtn { get; set; }


        [Action ("GetStartedButton_TouchUpInside:")]
        partial void GetStartedButton_TouchUpInside (UIKit.UIButton sender);


        [Action ("PreviousBtn_TouchUpInside:")]
        partial void PreviousBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ActivityIndicator != null) {
                ActivityIndicator.Dispose ();
                ActivityIndicator = null;
            }

            if (ButtonsView != null) {
                ButtonsView.Dispose ();
                ButtonsView = null;
            }

            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (NextBtn != null) {
                NextBtn.Dispose ();
                NextBtn = null;
            }

            if (PageControl != null) {
                PageControl.Dispose ();
                PageControl = null;
            }

            if (PreviousBtn != null) {
                PreviousBtn.Dispose ();
                PreviousBtn = null;
            }
        }
    }
}
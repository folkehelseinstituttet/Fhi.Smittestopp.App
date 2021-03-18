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

namespace NDB.Covid19.iOS.Views.Welcome.ChildViews
{
    [Register ("WelcomePageThreeViewController")]
    partial class WelcomePageThreeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackArrow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PageTitle { get; set; }

        [Action ("BackArrowBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackArrowBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BackArrow != null) {
                BackArrow.Dispose ();
                BackArrow = null;
            }

            if (Label1 != null) {
                Label1.Dispose ();
                Label1 = null;
            }

            if (Label2 != null) {
                Label2.Dispose ();
                Label2 = null;
            }

            if (Label3 != null) {
                Label3.Dispose ();
                Label3 = null;
            }

            if (PageTitle != null) {
                PageTitle.Dispose ();
                PageTitle = null;
            }
        }
    }
}
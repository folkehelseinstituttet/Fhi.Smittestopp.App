// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.MessagePage
{
    [Register ("MessagePageCell")]
    partial class MessagePageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView IndicatorView { get; set; }

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
        UIKit.UILabel UnreadLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IndicatorView != null) {
                IndicatorView.Dispose ();
                IndicatorView = null;
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

            if (UnreadLabel != null) {
                UnreadLabel.Dispose ();
                UnreadLabel = null;
            }
        }
    }
}
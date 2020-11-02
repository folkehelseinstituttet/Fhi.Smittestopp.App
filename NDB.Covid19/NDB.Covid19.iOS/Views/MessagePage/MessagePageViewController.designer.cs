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

namespace NDB.Covid19.iOS.Views.MessagePage
{
    [Register ("MessagePageViewController")]
    partial class MessagePageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AuthorityImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelLastUpdate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView MessageTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoItemsLabel1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoItemsLabel2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView NoItemsView { get; set; }

        [Action ("BackButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AuthorityImageView != null) {
                AuthorityImageView.Dispose ();
                AuthorityImageView = null;
            }

            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (LabelLastUpdate != null) {
                LabelLastUpdate.Dispose ();
                LabelLastUpdate = null;
            }

            if (MessageTable != null) {
                MessageTable.Dispose ();
                MessageTable = null;
            }

            if (NoItemsLabel1 != null) {
                NoItemsLabel1.Dispose ();
                NoItemsLabel1 = null;
            }

            if (NoItemsLabel2 != null) {
                NoItemsLabel2.Dispose ();
                NoItemsLabel2 = null;
            }

            if (NoItemsView != null) {
                NoItemsView.Dispose ();
                NoItemsView = null;
            }
        }
    }
}
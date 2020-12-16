// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.Settings
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView fhiLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView SettingsItemsTable { get; set; }

        [Action ("OnCloseBtnTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CloseBtn != null) {
                CloseBtn.Dispose ();
                CloseBtn = null;
            }

            if (fhiLogo != null) {
                fhiLogo.Dispose ();
                fhiLogo = null;
            }

            if (SettingsItemsTable != null) {
                SettingsItemsTable.Dispose ();
                SettingsItemsTable = null;
            }
        }
    }
}
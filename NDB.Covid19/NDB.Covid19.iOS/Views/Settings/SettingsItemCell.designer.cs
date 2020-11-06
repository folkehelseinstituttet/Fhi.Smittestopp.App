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

namespace NDB.Covid19.iOS.Views.Settings
{
    [Register ("SettingsItemCell")]
    partial class SettingsItemCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TextLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TextLbl != null) {
                TextLbl.Dispose ();
                TextLbl = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
    [Register ("CountryTableCell")]
    partial class CountryTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CheckboxImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountryNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CheckboxImage != null) {
                CheckboxImage.Dispose ();
                CheckboxImage = null;
            }

            if (CountryNameLabel != null) {
                CountryNameLabel.Dispose ();
                CountryNameLabel = null;
            }
        }
    }
}
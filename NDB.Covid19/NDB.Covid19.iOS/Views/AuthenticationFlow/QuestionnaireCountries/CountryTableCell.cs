using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
    public partial class CountryTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("CountryTableCell");
        public static readonly UINib Nib;
        public static float ROW_HEIGHT = 60;

        static CountryTableCell()
        {
            Nib = UINib.FromName("CountryTableCell", NSBundle.MainBundle);
        }

        public CountryTableCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(CountryDetailsViewModel country)
        {
            StyleUtil.InitLabelWithSpacing(CountryNameLabel, StyleUtil.FontType.FontBold, country.Name, 1.14, 20, 28);
            CheckboxImage.Image = (country.Checked ? UIImage.FromBundle("checkboxFilled") : UIImage.FromBundle("checkboxUnfilled"));

            if (country.Checked)
            {
                AccessibilityTraits |= UIAccessibilityTrait.Selected;
            }
            else
            {
                AccessibilityTraits &= ~UIAccessibilityTrait.Selected;
            }
        }
    }
}
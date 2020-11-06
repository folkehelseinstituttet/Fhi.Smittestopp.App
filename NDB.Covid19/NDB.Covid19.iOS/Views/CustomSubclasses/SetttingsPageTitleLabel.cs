using System;
using NDB.Covid19.iOS.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class SetttingsPageTitleLabel : UILabel
    {
        public SetttingsPageTitleLabel (IntPtr handle) : base (handle)
        {
            Lines = 100;
            TextColor = UIColor.White;
            StyleUtil.InitLabelWithSpacing(this, StyleUtil.FontType.FontBold, Text ?? "", 1.14, 24, 26);
        }

        /// <summary>
        /// When using this method the label is styled using StyleUtil.InitLabelWithSpacing()
        /// </summary>
        /// <param name="text"></param>
        public void SetAttributedText(string text)
        {
            Text = text;
            StyleUtil.InitLabelWithSpacing(this, StyleUtil.FontType.FontBold, text, 1.14, 24, 26);
        }
    }
}
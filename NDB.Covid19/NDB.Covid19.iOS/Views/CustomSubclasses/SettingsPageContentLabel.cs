using System;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class SettingsPageContentLabel : UILabel
    {
        public SettingsPageContentLabel(IntPtr handle) : base(handle)
        {
            Lines = 100;
            TextColor = UIColor.White;
            TranslatesAutoresizingMaskIntoConstraints = false;
            TextColor = UIColor.White;
            StyleUtil.InitLabelWithSpacing(this, FontType.FontRegular, Text ?? "", 1.28, 16, 22);
        }

        /// <summary>
        /// When using this method the label is styled using StyleUtil.InitLabelWithSpacing()
        /// </summary>
        /// <param name="text"></param>
        public void SetAttributedText(string text, FontType fontType = FontType.FontRegular)
        {
            Text = text;
            StyleUtil.InitLabelWithSpacing(this, fontType, text, 1.28, 16, 22);
        }
    }
}
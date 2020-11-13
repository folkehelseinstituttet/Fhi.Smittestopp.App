using NDB.Covid19.iOS.Utils;
using System;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.ENDeveloperTools
{
    public partial class ENDeveloperToolsOutputData : UILabel
    {
        public ENDeveloperToolsOutputData (IntPtr handle) : base (handle)
        {
            Lines = 100;
            TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.InitLabelWithSpacing(this, FontType.FontRegular, Text ?? "", 1.28, 16, 22);
        }
    }
}
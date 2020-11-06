using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings
{
    public partial class SettingsItemCell : UITableViewCell
    {
        public SettingsItemCell (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            TextLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 24, 34);
        }

        public void SetData(string text)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle { HyphenationFactor = 1.0f };
            UIStringAttributes attributes = new UIStringAttributes {ParagraphStyle = paragraphStyle};
            TextLbl.AttributedText = new NSMutableAttributedString(text, attributes);
        }
    }
}
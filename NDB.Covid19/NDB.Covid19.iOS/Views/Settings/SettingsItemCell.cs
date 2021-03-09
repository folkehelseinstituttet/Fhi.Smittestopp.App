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
            TextLbl.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        public void SetData(string text)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle { HyphenationFactor = 1.0f };
            UIStringAttributes attributes = new UIStringAttributes
            {
                ParagraphStyle = paragraphStyle,
                UnderlineColor = ColorHelper.BURGERMENU_UNDERLINE_COLOR,
                UnderlineStyle = NSUnderlineStyle.Single
            };
            TextLbl.AttributedText = new NSMutableAttributedString(text, attributes);
            TextLbl.TextAlignment = LayoutUtils.GetTextAlignment();
        }
    }
}
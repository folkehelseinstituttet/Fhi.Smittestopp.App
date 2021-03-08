using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class ColorHelper
    {
        public static readonly UIColor PRIMARY_COLOR = UIColor.Clear.FromHex(0x32345C);
        public static readonly UIColor TEXT_COLOR_ON_PRIMARY = UIColor.Clear.FromHex(0xffffff);
        public static readonly UIColor TEXT_COLOR_ON_BACKGROUND = UIColor.Clear.FromHex(0x32345C);
        public static readonly UIColor STATUS_ACTIVE = UIColor.FromRGB(217, 240, 212);
        public static readonly UIColor STATUS_INACTIVE = UIColor.FromRGB(253, 144, 133);
        public static readonly UIColor BURGERMENU_UNDERLINE_COLOR = UIColor.Clear.FromHex(0xF37668);
        public static readonly UIColor INFO_BUTTON_BACKGROUND = UIColor.Clear.FromHex(0xF3F9FB);
        public static readonly UIColor MESSAGE_BORDER_COLOR = UIColor.FromRGB(91, 93, 125);
        public static readonly UIColor DEFAULT_BACKGROUND_COLOR = UIColor.FromRGB(225, 234, 237);
        public static readonly UIColor LINK_COLOR = UIColor.FromRGB(8, 108, 175);


        public static UIColor FromHex(this UIColor color,int hexValue)
        {
            return UIColor.FromRGB(
                (((float)((hexValue & 0xFF0000) >> 16))/255.0f),
                (((float)((hexValue & 0xFF00) >> 8))/255.0f),
                (((float)(hexValue & 0xFF))/255.0f)
            );
        }

    }
}
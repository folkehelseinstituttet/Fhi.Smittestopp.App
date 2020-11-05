using System.Runtime.CompilerServices;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class ColorHelper
    {
        public static readonly UIColor PRIMARY_COLOR = UIColor.Clear.FromHex(0x32345C);
        public static readonly UIColor TEXT_COLOR_ON_PRIMARY = UIColor.Clear.FromHex(0xffffff);
        public static readonly UIColor TEXT_COLOR_ON_BACKGROUND = UIColor.Clear.FromHex(0x32345C);

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
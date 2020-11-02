using System.Text.RegularExpressions;
using Foundation;

namespace NDB.Covid19.iOS.Utils
{
    public class AccessibilityUtils
    {
        public static NSAttributedString RemovePoorlySpokenSymbols(string textLabelDisplayed)
        {
            return new NSAttributedString(textLabelDisplayed.Replace("|", ""));
        }

        public static string RemovePoorlySpokenSymbolsString(string textLabelDisplayed)
        {
            return Regex.Replace(textLabelDisplayed.Replace("|", ""), "<.*?>", string.Empty);
        }
    }
}

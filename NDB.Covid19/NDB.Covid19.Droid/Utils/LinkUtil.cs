using Android.Widget;
using Android.Text.Method;

namespace NDB.Covid19.Droid.Utils
{
    public class LinkUtil
    {
        /// <summary>
		/// This method take a textview,finds and adds inline links and make them clickable
		/// </summary>
		/// <param name="textView"></param>
        public static void LinkifyTextView(TextView textView)
        {
            //Pattern for recognizing a URL, based off RFC 3986 - https://stackoverflow.com/questions/5713558/detect-and-extract-url-from-a-string/5713866
            Java.Util.Regex.Pattern urlPattern =
                Java.Util.Regex.Pattern.Compile(
                    "(?:^|[\\W])((ht|f)tp(s?):\\/\\/|www\\.)"
                    + "(([\\w\\-]+\\.){1,}?([\\w\\-.~]+\\/?)*"
                    + "[\\p{Alnum}.,%_=?&#\\-+()\\[\\]\\*$~@!:/{};']*)");

            Android.Text.Util.Linkify.AddLinks(textView, urlPattern, "https://");

            textView.MovementMethod = LinkMovementMethod.Instance;
        }
    }
}

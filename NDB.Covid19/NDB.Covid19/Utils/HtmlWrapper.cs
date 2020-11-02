namespace NDB.Covid19.Utils
{
    public class HtmlWrapper
    {
        public static string HtmlForLabelWithText(string body, float fontSize, bool bold, bool isAndroid, string textColor = null)
        {
            string style = StyleForLabelWithFontSize(fontSize, textColor);
            return WrapHtmlWithBodyAndStyle(body, style, bold, isAndroid);
        }

        public static string StyleForLabelWithFontSize(float fontSize, string textColor = null)
        {
            //Default color, if no color is assigned
            string TextColor = "#ffffff";

            return @"
                span.wrap
                {
                    display:inline-block;
                    width:100%;
                    white-space: nowrap;
                    overflow:hidden !important;
                    text-overflow: ellipsis;
                }
                body {
                    margin: 0; 
                    padding: 0;
                    font-family: Raleway;
                    font-size: " + fontSize + @"px;
                    text-align: justify;
                    color: " + (textColor == null ? TextColor : textColor) + @";
                    text-align: left;
                    -webkit-font-smoothing: none;
                    -webkit-text-size-adjust: none;
                }
                .ios-bold {
                    font-family: Raleway-Bold;
                }
                .draft {
                    background-color: #B50050;
                    color: white;
                    font-size: 11px;
                    padding: 1px 4px 1px 4px;
                }
                ";
        }

        public static string WrapHtmlWithBodyAndStyle(string body, string style, bool bold, bool isAndroid)
        {
            string headers = isAndroid ? "" : @"<meta name=""viewport"" content=""initial-scale=1.0"" />";

            string bodyHtml = body;
            if (bold)
            {
                bodyHtml = isAndroid ? "<strong>" + body + "</strong>" : "<span class='ios-bold'>" + body + "</span>";
            }
            else if (isAndroid)
            {
                bodyHtml = bodyHtml.Replace("<em>", "<strong>");
                bodyHtml = bodyHtml.Replace("</em>", "</strong>");
            }
            else
            {
                bodyHtml = bodyHtml.Replace("<em>", "<span class='ios-bold'>");
                bodyHtml = bodyHtml.Replace("</em>", "</span>");
            }

            bodyHtml = $"<span class=\"wrap\">{bodyHtml}</span>";

            return @"
                <html>
                <head>
                " + headers + @"
                <style type=""text/css"">
                " + style + @"
                </style>
                </head>
                <body>
                " + bodyHtml + @"
                </body>
                </html>            
                ";
        }
    }
}
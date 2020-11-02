using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Security.Application;
using static Microsoft.Security.Application.Encoder;

namespace NDB.Covid19.Utils
{
    public static class Anonymizer
    {
        public static string ReplaceCpr(string input)
        {
            string tmp = input;
            Regex kebabCase = new Regex(@"[0-3][0-9][0-1][1-9]\d{2}-\d{4}?[^0-9]*?");
            Regex continuousCase = new Regex(@"[0-3][0-9][0-1][1-9]\d{2}\d{4}?[^0-9]*?");
            Regex spaceCase = new Regex(@"[0-3][0-9][0-1][1-9]\d{2} \d{4}?[^0-9]*?");

            tmp = kebabCase.Replace(tmp, "xxxxxx-xxxx");
            tmp = continuousCase.Replace(tmp, "xxxxxxxxxx");
            return spaceCase.Replace(tmp, "xxxxxx xxxx");
        }

        public static string ReplacePhoneNumber(string input)
        {
            string tmp = input;

            // 1st line: +45 or 0045
            // 2nd line: 11-11-11-11 or 11 11 11 11
            // 3rd line: 1111-1111 or 1111 1111
            // 4th line: 11111111
            Regex directionalRegex = new Regex(@"((\+[0-9]{2})|([0]{2}[0-9]{2}))",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Regex fourTwoDigitsRegex = new Regex(@"([0-9]{2}(\ |\-)[0-9]{2}(\ |\-)[0-9]{2}(\ |\-)[0-9]{2})",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Regex twoFourDigitsRegex = new Regex(@"([0-9]{4}(\ |\-)[0-9]{4})",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            Regex oneEightDigitsRegex =
                new Regex(@"([0-9]{8})", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

            tmp = directionalRegex.Replace(tmp, "+xx");
            tmp = fourTwoDigitsRegex.Replace(tmp, "xx-xx-xx-xx");
            tmp = twoFourDigitsRegex.Replace(tmp, "xxxx-xxxx");
            return oneEightDigitsRegex.Replace(tmp, "xxxxxxxx");
        }

        public static string ReplaceEmailAddress(string input)
        {
            string tmp = input;
            string pattern = @"(?<=[\w]{0})[\w-\._\+%\\]*(?=[\w]{0}@)|(?<=@[\w]{0})[\w-_\+%]*(?=\.)";
            return Regex.Replace(tmp, pattern, m => new string('*', m.Length));
        }

        public static string ReplaceMacAddress(string input)
        {
            string tmp = input;
            Regex macAddressRegex = new Regex(
                @"(?:[0-9a-fA-F]{2}[:.-]){5}[0-9a-fA-F]{2}|
                        (?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|
                        (?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$|
                        (?:[0-9a-fA-F]{3}[:.-]){5}[0-9a-fA-F]{3}|
                        (?:[0-9a-fA-F]{3}-){4}[0-9a-fA-F]{3}|
                        (?:[0-9a-fA-F]{3}){5}[0-9a-fA-F]{3}$|
                        (?:[0-9a-fA-F]{3}[:.-]){3}[0-9a-fA-F]{3}|
                        (?:[0-9a-fA-F]{3}-){3}[0-9a-fA-F]{3}|
                        (?:[0-9a-fA-F]{3}){3}[0-9a-fA-F]{3}$",
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            return macAddressRegex.Replace(tmp, "xx:xx:xx:xx");
        }

        public static string ReplaceIMEI(string input)
        {
            string tmp = input;
            Regex imeiRegex = new Regex(@"([0-9]{15})", RegexOptions.Multiline);
            return imeiRegex.Replace(tmp,
                match => Luhn(match.ToString()) ? "xxxxxxxxxxxxxxx" : match.ToString());
        }

        public static string RedactText(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string tmp = input;
            tmp = ReplaceIMEI(tmp);
            tmp = ReplaceCpr(tmp);
            tmp = ReplacePhoneNumber(tmp);
            tmp = ReplaceEmailAddress(tmp);
            tmp = ReplaceMacAddress(tmp);
            tmp = Sanitizer.GetSafeHtmlFragment(tmp);
            tmp = HtmlEncode(tmp);
            return tmp;
        }

        private static bool Luhn(string digits)
        {
            return digits.All(char.IsDigit) && digits.Reverse()
                .Select(c => c - 48)
                .Select((thisNum, i) => i % 2 == 0
                    ? thisNum
                    : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                ).Sum() % 10 == 0;
        }
    }
}
using System;
using System.Globalization;
using System.Linq;
using NDB.Covid19.Config;
using Xamarin.Essentials;

namespace NDB.Covid19.Utils
{
    public static class DateUtils
    {
        public static string GetDateFromDateTime(this DateTime? date, string dateFormat)
        {
            if (date != null)
            {
                DateTime dateTime = (DateTime)date;
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
                bool currentCultureIsSupported = Conf.SUPPORTED_LANGUAGES.Contains(cultureInfo.TwoLetterISOLanguageName);
                string dateString;
                DateTime calenderDateTime = new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond,
                    currentCultureIsSupported
                        ? CultureInfo.CurrentCulture.Calendar
                        : new GregorianCalendar());
                dateString = calenderDateTime.ToString(dateFormat, currentCultureIsSupported ? cultureInfo : defaultCultureInfo);
                return dateString.Replace("-", "/");
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ReplaceAndInsertNewlineiOS(string text, string sizeCategory)
        {
            bool insertNewline;

            switch (sizeCategory)
            {
                case "UICTContentSizeCategoryXS":
                    insertNewline = false;
                    break;
                case "UICTContentSizeCategoryS":
                    insertNewline = false;
                    break;
                case "UICTContentSizeCategoryM":
                    insertNewline = false;
                    break;
                case "UICTContentSizeCategoryL":
                    insertNewline = false;
                    break;
                default:
                    insertNewline = true;
                    break;
            }

            //A witdh of 750 is the width of an iPhone 6, 7, 8 or SE (2nd gen)
            //String replace is necessary as the standard format returned, with a danish phone, is the following format dd-mm-yyyy and not the desired dd/mm/yyyy
            if (DeviceDisplay.MainDisplayInfo.Width <= 750 && insertNewline)
            {
                text = ReplaceLastOccurrence(text.Replace("-", "/"), "/", "/" + Environment.NewLine);
            }
            else
            {
                text = text.Replace("-", "/");
            }

            return text;
        }

        /// <summary>
        /// Method to replace the last occurrence of a string.
        /// </summary>
        /// <param name="Source">Source is the string on which you want to do the operation.</param>
        /// <param name="Find">Find is the string that you want to replace. </param>
        /// <param name="Replace">Replace is the string that you want to replace it with.</param>
        /// <returns></returns>
        static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public static string ToGreGorianUtcString(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }
    }
}

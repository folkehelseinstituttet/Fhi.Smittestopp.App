using System;
using System.Globalization;
using System.Linq;
using NDB.Covid19.Configuration;
using NDB.Covid19.PersistedData;
using Xamarin.Essentials;

namespace NDB.Covid19.Utils
{
    public static class DateUtils
    {
        public static string GetDateFromDateTime(this DateTime? date, string dateFormat, bool changeCulture = false, string oldCulture = "nn", string newCulture = "nn")
        {
            if (date != null)
            {
                DateTime dateTime = (DateTime)date;
                string appLanguage = LocalesService.GetLanguage();
                CultureInfo selectedCulture = CultureInfo.GetCultureInfo(appLanguage);
                // Due to a bug in C# string representation in nb culture, nn must be used
                CultureInfo defaultCulture = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
                bool shouldUseDefaultCulture = appLanguage == "ar" || appLanguage == "ur" || appLanguage == "ti" || appLanguage == "nb";
                string dateString;
                if (changeCulture && appLanguage == oldCulture)
                {
                    selectedCulture = CultureInfo.GetCultureInfo(newCulture);
                }
                DateTime calenderDateTime = new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond,
                    new GregorianCalendar());
                dateString = calenderDateTime.ToString(dateFormat, shouldUseDefaultCulture ? defaultCulture : selectedCulture);
                return dateString.Replace("-", ".");
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

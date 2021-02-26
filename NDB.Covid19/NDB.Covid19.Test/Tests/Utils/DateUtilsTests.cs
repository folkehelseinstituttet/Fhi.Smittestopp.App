using System;
using System.Globalization;
using System.Threading;
using NDB.Covid19.Configuration;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

using Xunit;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class DateUtilsTests
    {
        private static DateTime testDateTime => new DateTime(2020, 7, 15, 13, 37, 00, CultureInfo.InvariantCulture.Calendar);

        public DateUtilsTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public void GetDateFromDateTime_DateTimeNull()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nn-NO");

            // Act
            string actual = DateUtils.GetDateFromDateTime(null, "d");

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_Nb()
        {
            LocalPreferencesHelper.SetAppLanguage("nb");
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");
            DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat; // Expected format to follow

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = dd.mm.yyyy hh:mm:ss with nb-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_Nn()
        {
            LocalPreferencesHelper.SetAppLanguage("nn");

            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nn-NO");
            DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat; // Expected format to follow

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = dd.mm.yyyy hh:mm:ss with nb-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_En()
        {
            LocalPreferencesHelper.SetAppLanguage("en");

            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DateTimeFormatInfo dateTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat; // Expected format to use

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with en-EN culture
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ".")
                ? "7.15.2020 1.37.00 PM"
                : "7/15/2020 1:37:00 PM"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_Ar()
        {
            LocalPreferencesHelper.SetAppLanguage("ar");

            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
            DateTimeFormatInfo dateTimeFormat = defaultCultureInfo.DateTimeFormat; // Expected format to use based Conf.DEFAULT_LANGUAGE

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with nn-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_Ur()
        {
            LocalPreferencesHelper.SetAppLanguage("ur");

            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
            DateTimeFormatInfo dateTimeFormat = defaultCultureInfo.DateTimeFormat; // Expected format to use based Conf.DEFAULT_LANGUAGE

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with nn-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_Ti()
        {
            LocalPreferencesHelper.SetAppLanguage("ti");

            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
            DateTimeFormatInfo dateTimeFormat = defaultCultureInfo.DateTimeFormat; // Expected format to use based Conf.DEFAULT_LANGUAGE

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with nn-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureIsSupported_LT()
        {
            LocalPreferencesHelper.SetAppLanguage("lt");

            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("lt-LT");
            DateTimeFormatInfo dateTimeFormat = cultureInfo.DateTimeFormat; // Expected format to use based Conf.DEFAULT_LANGUAGE

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with nn-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "-" && dateTimeFormat.TimeSeparator == ":")
                ? "2020.07.15 13:37:00"
                : "2020/07/15 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDateFromDateTime_CultureNotSupport_Th()
        {
            LocalPreferencesHelper.SetAppLanguage("nb");
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");

            CultureInfo defaultCultureInfo = CultureInfo.GetCultureInfo(Conf.DEFAULT_LANGUAGE);
            DateTimeFormatInfo dateTimeFormat = defaultCultureInfo.DateTimeFormat; // Expected format to use based Conf.DEFAULT_LANGUAGE

            // Act
            string actual = DateUtils.GetDateFromDateTime(testDateTime, "G"); // G = mm/dd/yyyy hh:mm:ss with nn-NO culture (default language)
            string expected = (dateTimeFormat.DateSeparator == "." && dateTimeFormat.TimeSeparator == ":")
                ? "15.07.2020 13:37:00"
                : "15/07/2020 13:37:00"; // This check have been made to ensure the expected result follows the separators of the expected culture

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

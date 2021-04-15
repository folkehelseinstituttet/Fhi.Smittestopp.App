using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;
using Xunit;
using static NDB.Covid19.ExposureNotifications.Helpers.UploadDiagnosisKeysHelper;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class TransmissionRiskCalculationTest
    {
        private readonly DateTime _today = DateTime.Today;

        public TransmissionRiskCalculationTest()
        {
            DependencyInjectionConfig.Init();
        }

        private DateTime DateToSetDSOS => _today.AddDays(1);

        private ExposureKeyModel TEK(int days)
        {
            return new ExposureKeyModel(
                new byte[1],
                _today.AddDays(days),
                TimeSpan.FromDays(1),
                RiskLevel.Invalid);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void calculateTransmissionRiskBasedOnDateDifferencePositive(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            // Create keys with different dates
            IEnumerable<ExposureKeyModel> temporaryExposureKeys =
                RangeOfTEKs(2, 11);

            // Process a list of copies
            List<ExposureKeyModel> validKeys =
                CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            List<ExposureKeyModel> resultKeys = SetTransmissionRiskAndDSOS(validKeys, DateToSetDSOS);

            AssertPositiveDaysTEKS(resultKeys);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void calculateTransmissionRiskBasedOnDateDifferenceNegative(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            // Create keys with different dates
            IEnumerable<ExposureKeyModel> negativeDifferenceExposureKeys =
                RangeOfTEKs(-3, 3);

            // Process a list of copies
            List<ExposureKeyModel> validNegativeDifferenceExposureKeys =
                CreateAValidListOfTemporaryExposureKeys(
                    negativeDifferenceExposureKeys);
            List<ExposureKeyModel> resultKeysNegativeDifference =
                SetTransmissionRiskAndDSOS(validNegativeDifferenceExposureKeys, DateToSetDSOS);

            AssertNegativeDaysTEKS(resultKeysNegativeDifference);
        }

        private List<ExposureKeyModel> RangeOfTEKs(int start, int count)
        {
            return Enumerable.Range(start, count)
                .Select(i => TEK(i))
                .ToList();
        }

        private void AssertPositiveDaysTEKS(List<ExposureKeyModel> resultKeys)
        {
            for (int i = 1; i < resultKeys.Count; i++)
            {
                if (i == 0)
                {
                    Assert.Equal("Highest", resultKeys[i].TransmissionRiskLevel.ToString());
                }

                if (i > 0 && i < 3)
                {
                    Assert.Equal("VeryHigh", resultKeys[i].TransmissionRiskLevel.ToString());
                }

                if (i > 2 && i < 5)
                {
                    Assert.Equal("High", resultKeys[i].TransmissionRiskLevel.ToString());
                }

                if (i > 4 && i < 9)
                {
                    Assert.Equal("MediumHigh", resultKeys[i].TransmissionRiskLevel.ToString());
                }

                if (i > 8)
                {
                    Assert.Equal("Medium", resultKeys[i].TransmissionRiskLevel.ToString());
                }
            }
        }

        private void AssertNegativeDaysTEKS(List<ExposureKeyModel> resultKeysNegativeDifference)
        {
            for (int i = 0; i < resultKeysNegativeDifference.Count; i++)
            {
                if (i == 0)
                {
                    Assert.Equal("Lowest", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 1)
                {
                    Assert.Equal("Low", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 2)
                {
                    Assert.Equal("MediumLow", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }
            }
        }
    }
}
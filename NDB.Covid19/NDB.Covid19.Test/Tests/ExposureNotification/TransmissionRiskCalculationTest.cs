using System;
using System.Collections.Generic;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;
using Xunit;
using static NDB.Covid19.ExposureNotifications.Helpers.UploadDiagnosisKeysHelper;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class TransmissionRiskCalculationTest
    {
        private readonly DateTime today = DateTime.Today;
        private DateTime MiBaDate => today.AddDays(1);

        private ExposureKeyModel TEK(int days) =>
            new ExposureKeyModel(
                new byte[1],
                today.AddDays(days),
                TimeSpan.FromDays(1),
                RiskLevel.Invalid);

        [Fact]
        public void calculateTransmissionRiskBasedOnDateDifferencePositive()
        {
            // Create keys with different dates
            IEnumerable<ExposureKeyModel> temporaryExposureKeys =
                new List<ExposureKeyModel>
                {
                    TEK(2),
                    TEK(3),
                    TEK(4),
                    TEK(5),
                    TEK(6),
                    TEK(7),
                    TEK(8),
                    TEK(9),
                    TEK(10),
                    TEK(11),
                    TEK(12)
                };

            // Process a list of copies
            IEnumerable<ExposureKeyModel> processedKeys =
                CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);
            List<ExposureKeyModel> validKeys =
                CreateAValidListOfTemporaryExposureKeys(processedKeys);

            List<ExposureKeyModel> resultKeys = SetTransmissionRiskLevel(validKeys, MiBaDate);

            AssertPositiveDaysTEKS(resultKeys);
        }

        [Fact]
        public void calculateTransmissionRiskBasedOnDateDifferenceNegative()
        {
            // Create keys with different dates
            IEnumerable<ExposureKeyModel> negativeDifferenceExposureKeys =
                new List<ExposureKeyModel>
                {
                    TEK(-3),
                    TEK(-2),
                    TEK(-1)
                };

            // Process a list of copies
            IEnumerable<ExposureKeyModel> processedNegativeDifferenceExposureKeys =
                CreateAValidListOfTemporaryExposureKeys(negativeDifferenceExposureKeys);
            List<ExposureKeyModel> validNegativeDifferenceExposureKeys =
                CreateAValidListOfTemporaryExposureKeys(
                    processedNegativeDifferenceExposureKeys);
            List<ExposureKeyModel> resultKeysNegativeDifference =
                SetTransmissionRiskLevel(validNegativeDifferenceExposureKeys, MiBaDate);

            AssertNegativeDaysTEKS(resultKeysNegativeDifference);
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
            for (int i = 1; i < resultKeysNegativeDifference.Count; i++)
            {
                if (i == 0)
                {
                    Assert.Equal("MediumLow", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 1)
                {
                    Assert.Equal("Low", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 2)
                {
                    Assert.Equal("Lowest", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }
            }
        }
    }
}
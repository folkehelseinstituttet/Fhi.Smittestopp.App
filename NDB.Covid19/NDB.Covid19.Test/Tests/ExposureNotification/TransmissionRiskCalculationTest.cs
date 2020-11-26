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
        // 00:00 1 June 2020 UTC
        private readonly DateTimeOffset june1 = DateTimeOffset.FromUnixTimeSeconds(1590969600);

        private DateTime MiBaDate => new DateTime(2020, 6, 2);

        private ExposureKeyModel TEK(int days) =>
            new ExposureKeyModel(new byte[1], june1.AddDays(days), TimeSpan.FromDays(1), RiskLevel.Invalid);

        [Fact]
        public void calculateTransmissionRiskBasedOnDateDifference()
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

            IEnumerable<ExposureKeyModel> negativeDifferenceExposureKeys =
                new List<ExposureKeyModel>
                {
                    TEK(-3),
                    TEK(-2),
                    TEK(-1)
                };

            // Process a list of copies
            IEnumerable<ExposureKeyModel> processedKeys =
                CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);
            List<ExposureKeyModel> validKeys =
                CreateAValidListOfTemporaryExposureKeys(processedKeys);

            List<ExposureKeyModel> resultKeys = SetTransmissionRiskLevel(validKeys, MiBaDate);

            AssertPositiveDaysTEKS(resultKeys);

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
            for (int i = 1; i < 11; i++)
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
            for (int i = 1; i < 2; i++)
            {
                if (i == 0)
                {
                    Assert.Equal("Medium", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 1)
                {
                    Assert.Equal("MediumLow", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }

                if (i == 2)
                {
                    Assert.Equal("Low", resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;
using Xunit;
using Xunit.Abstractions;
using static NDB.Covid19.ExposureNotifications.Helpers.UploadDiagnosisKeysHelper;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class TransmissionRiskCalculationTest
    {
        private readonly ITestOutputHelper output;

        public TransmissionRiskCalculationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        // 00:00 1 June 2020 UTC
        private readonly DateTimeOffset june1 = DateTimeOffset.FromUnixTimeSeconds(1590969600).ToUniversalTime();

        private DateTime MiBaDate => new DateTime(2020, 6, 2).ToUniversalTime();

        private ExposureKeyModel TEK(int days) =>
            new ExposureKeyModel(new byte[1], june1.AddDays(days).ToUniversalTime(), TimeSpan.FromDays(1),
                RiskLevel.Invalid);

        [Fact]
        public void calculateTransmissionRiskBasedOnDateDifferencePositive()
        {
            output.WriteLine("june1: {0}", june1.ToString());
            output.WriteLine("Mibadate, {0}", MiBaDate.ToString());

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
            output.WriteLine("processedKeys {0}", processedKeys.Count());
            output.WriteLine("validKeys {0}", validKeys.Count());
            output.WriteLine("resultKeys {0}", resultKeys.Count());

            for (int i = 0; i < resultKeys.Count; ++i)
            {
                output.WriteLine("tek[{0}] DaysSinceOnsetOfSymptoms, {1}", i, resultKeys[i].DaysSinceOnsetOfSymptoms);
                output.WriteLine("tek[{0}] RollingDuration, {1}", i, resultKeys[i].RollingDuration.ToString());
                output.WriteLine("tek[{0}] RollingStart, {1}", i, resultKeys[i].RollingStart.ToString());
                output.WriteLine("tek[{0}] TransmissionRiskLevel, {1}", i,
                    resultKeys[i].TransmissionRiskLevel.ToString());
            }

            AssertPositiveDaysTEKS(resultKeys);
        }

        [Fact]
        public void calculateTransmissionRiskBasedOnDateDifferenceNegative()
        {
            output.WriteLine("june1: {0}", june1.ToString());
            output.WriteLine("Mibadate, {0}", MiBaDate.ToString());

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
                SetTransmissionRiskLevel(validNegativeDifferenceExposureKeys, MiBaDate, output);

            output.WriteLine("processedNegativeDifferenceExposureKeys {0}",
                processedNegativeDifferenceExposureKeys.Count());
            output.WriteLine("validNegativeDifferenceExposureKeys {0}", validNegativeDifferenceExposureKeys.Count());
            output.WriteLine("resultKeysNegativeDifference {0}", resultKeysNegativeDifference.Count());

            for (int i = 0; i < resultKeysNegativeDifference.Count; ++i)
            {
                output.WriteLine("ntek[{0}] DaysSinceOnsetOfSymptoms, {1}", i,
                    resultKeysNegativeDifference[i].DaysSinceOnsetOfSymptoms);
                output.WriteLine("ntek[{0}] RollingDuration, {1}", i,
                    resultKeysNegativeDifference[i].RollingDuration.ToString());
                output.WriteLine("ntek[{0}] RollingStart, {1}", i,
                    resultKeysNegativeDifference[i].RollingStart.ToString());
                output.WriteLine("ntek[{0}] TransmissionRiskLevel, {1}", i,
                    resultKeysNegativeDifference[i].TransmissionRiskLevel.ToString());
            }

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
using System;
using System.Collections.Generic;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class TransmissionRiskCalculationTest
    {
        public TransmissionRiskCalculationTest()
        {
        }

        // 00:00 1 June 2020 UTC
        private readonly DateTimeOffset june1 = DateTimeOffset.FromUnixTimeSeconds(1590969600);

        private DateTime MiBaDate => new DateTime(2020, 6, 2);

        // TODO: This test needs to be fixed
        [Fact (Skip = "Early development, test failing on some agents")]
        public async void calculateTransmissionRiskbasedOnDateDifference()
        {
            // Create keys with different dates
            ExposureKeyModel tekminus3 = new ExposureKeyModel(new byte[1], june1.AddDays(-3), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tekminus2 = new ExposureKeyModel(new byte[1], june1.AddDays(-2), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tekminus1 = new ExposureKeyModel(new byte[1], june1.AddDays(-1), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[1], june1.AddDays(2), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[1], june1.AddDays(3), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek4 = new ExposureKeyModel(new byte[1], june1.AddDays(4), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek5 = new ExposureKeyModel(new byte[1], june1.AddDays(5), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek6 = new ExposureKeyModel(new byte[1], june1.AddDays(6), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek7 = new ExposureKeyModel(new byte[1], june1.AddDays(7), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek8 = new ExposureKeyModel(new byte[1], june1.AddDays(8), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek9 = new ExposureKeyModel(new byte[1], june1.AddDays(9), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek10 = new ExposureKeyModel(new byte[1], june1.AddDays(10), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek11 = new ExposureKeyModel(new byte[1], june1.AddDays(11), TimeSpan.FromDays(1), RiskLevel.Invalid);
            ExposureKeyModel tek12 = new ExposureKeyModel(new byte[1], june1.AddDays(12), TimeSpan.FromDays(1), RiskLevel.Invalid);


            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() {tek2, tek3, tek4, tek5, tek6, tek7, tek8, tek9, tek10, tek11, tek12 };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);
            List<ExposureKeyModel> validKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(processedKeys);

            List <ExposureKeyModel> resultKeys = UploadDiagnosisKeysHelper.SetTransmissionRiskLevel(validKeys, MiBaDate);

            for (int i=1; i < 11; i++)
            {
                if (i == 0) {
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

            IEnumerable<ExposureKeyModel> negativeDifferenceExposureKeys= new List<ExposureKeyModel>() { tekminus1, tekminus2, tekminus3 };
            IEnumerable<ExposureKeyModel> processedNegativeDifferenceExposureKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(negativeDifferenceExposureKeys);
            List<ExposureKeyModel> validNegativeDifferenceExposureKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(processedNegativeDifferenceExposureKeys);
            List<ExposureKeyModel> resultKeysNegativeDifference = UploadDiagnosisKeysHelper.SetTransmissionRiskLevel(validNegativeDifferenceExposureKeys, MiBaDate);

            for (int i = 1; i < 11; i++)
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

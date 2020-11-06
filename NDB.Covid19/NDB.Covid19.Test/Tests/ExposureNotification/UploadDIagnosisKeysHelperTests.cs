using System;
using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models;
using Xamarin.ExposureNotifications;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class UploadDIagnosisKeysHelperTests
    {
        public UploadDIagnosisKeysHelperTests()
        {
            DependencyInjectionConfig.Init();
        }

        // 00:00 1 June 2020 UTC
        private readonly DateTimeOffset june1 = DateTimeOffset.FromUnixTimeSeconds(1590969600);

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_HaveMultipleWithSameDate_OnlyOneWithEachDateShouldBeKept()
        {
            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], june1, TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], june1, TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], june1.AddDays(1), TimeSpan.FromDays(1), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // The only difference should be that tek2 is not contained in the result
            Assert.Equal(2, processedKeys.Count());
            Assert.True(ContainsTek(tek1, processedKeys));
            Assert.True(ContainsTek(tek3, processedKeys));
        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_HaveDateGap_OnlyNewestShouldBeKept()
        {
            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], june1, TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], june1.AddDays(1), TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], june1.AddDays(3), TimeSpan.FromDays(1), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // Only tek3 should be left
            Assert.Single(processedKeys);
            Assert.True(ContainsTek(tek3, processedKeys));
        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_Have15Keys_Only14ShouldBeKept()
        {
            // Create a list of 15 keys
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>();
            for (int i = 0; i < 15; i++)
            {
                temporaryExposureKeys = temporaryExposureKeys.Append(new ExposureKeyModel(new byte[i + 1], june1.AddDays(i), TimeSpan.FromDays(1), RiskLevel.Medium));
            }

            // Process them
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // There should be 14 left
            Assert.Equal(14, processedKeys.Count());
        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_HaveBadRollingDurations_RollingDurationsShouldBeSetToOneDay()
        {
            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], june1, TimeSpan.FromDays(0), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], june1.AddDays(1), TimeSpan.FromDays(10), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], june1.AddDays(2), TimeSpan.FromDays(-10), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // RollingDurations should be 1 day
            foreach (ExposureKeyModel tek in processedKeys)
            {
                Assert.Equal(TimeSpan.FromDays(1), tek.RollingDuration);
            }
        }

        // True iff. container contains a key with same value as the given tek. Not looking at object addresses
        private bool ContainsTek(ExposureKeyModel tek, IEnumerable<ExposureKeyModel> teks)
        {
            foreach (ExposureKeyModel containedTek in teks)
            {
                if (TeksAreEqual(containedTek, tek))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TeksAreEqual(ExposureKeyModel tek1, ExposureKeyModel tek2)
        {
            return
                tek1.Key == tek2.Key &&
                tek1.RollingDuration == tek2.RollingDuration &&
                tek1.RollingStart == tek2.RollingStart &&
                tek1.TransmissionRiskLevel == tek2.TransmissionRiskLevel;
        }

        private ExposureKeyModel CopyTek(ExposureKeyModel tek)
        {
            return new ExposureKeyModel(tek.Key, tek.RollingStart, tek.RollingDuration, tek.TransmissionRiskLevel);
        }
    }
}

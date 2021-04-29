using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData.SQLite;
using Xamarin.ExposureNotifications;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class UploadDIagnosisKeysHelperTests
    {
        private ILoggingManager _logManager;

        public UploadDIagnosisKeysHelperTests()
        {
            DependencyInjectionConfig.Init();
            _logManager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            _logManager.DeleteAll();
        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_HaveMultipleWithSameDate_AllShouldBeKept()
        {
            SystemTime.ResetDateTime();

            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], SystemTime.Now(), TimeSpan.FromDays(0.7), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], SystemTime.Now(), TimeSpan.FromDays(0.3), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], SystemTime.Now().AddDays(-1), TimeSpan.FromDays(1), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // No keys should be filtered out
            Assert.True(ContainsTek(tek1, processedKeys));
            Assert.True(ContainsTek(tek2, processedKeys));
            Assert.True(ContainsTek(tek3, processedKeys));
        }

        [Fact]
        public async void createAValidListOfTemporaryExposureKeys_HaveDateGap_AllShouldBeKept()
        {
            SystemTime.ResetDateTime();

            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], SystemTime.Now(), TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], SystemTime.Now().AddDays(-1), TimeSpan.FromDays(1), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], SystemTime.Now().AddDays(-3), TimeSpan.FromDays(1), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // No keys should be filtered out
            Assert.Equal(processedKeys.Count(), temporaryExposureKeys.Count());
            Assert.True(ContainsTek(tek3, processedKeys));
            Assert.False((await _logManager.GetLogs(10)).Any());

        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_Have15Keys_Only14ShouldBeKept()
        {
            SystemTime.ResetDateTime();

            // Create a list of 15 keys
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>();
            for (int i = 0; i < 15; i++)
            {
                temporaryExposureKeys = temporaryExposureKeys.Append(new ExposureKeyModel(new byte[i + 1], SystemTime.Now().AddDays(0 - i), TimeSpan.FromDays(1), RiskLevel.Medium));
            }

            // Process them
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // There should be 14 left
            Assert.Equal(14, processedKeys.Count());
        }

        [Fact]
        public void createAValidListOfTemporaryExposureKeys_HaveBadRollingDurations_RollingDurationsShouldBeSetToOneDay()
        {
            SystemTime.ResetDateTime();

            // Create keys
            ExposureKeyModel tek1 = new ExposureKeyModel(new byte[1], SystemTime.Now(), TimeSpan.FromDays(0), RiskLevel.Medium);
            ExposureKeyModel tek2 = new ExposureKeyModel(new byte[2], SystemTime.Now().AddDays(-1), TimeSpan.FromDays(10), RiskLevel.Medium);
            ExposureKeyModel tek3 = new ExposureKeyModel(new byte[3], SystemTime.Now().AddDays(-2), TimeSpan.FromDays(-10), RiskLevel.Medium);

            // Process a list of copies
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>() { CopyTek(tek1), CopyTek(tek2), CopyTek(tek3) };
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // RollingDurations should be 1 day
            foreach (ExposureKeyModel tek in processedKeys)
            {
                Assert.Equal(TimeSpan.FromDays(1), tek.RollingDuration);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(9)]
        public async void createAValidListOfTemporaryExposureKeys_GeneratesProperLogIfKeysAreFiltered(int ExtraKeys)
        {
            SystemTime.ResetDateTime();
            await _logManager.DeleteAll();

            // Create a list of 15 keys
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>();
            for (int i = 0; i < 14 + ExtraKeys; i++)
            {
                temporaryExposureKeys = temporaryExposureKeys.Append(new ExposureKeyModel(new byte[i + 1], SystemTime.Now().AddDays(0 - i), TimeSpan.FromDays(1), RiskLevel.Medium));
            }

            // Process them
            IEnumerable<ExposureKeyModel> processedKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // Check if log is generated
            string logStatement = _logManager.GetLogs(1).Result.ElementAt(0).Description;
            Assert.Equal(ExtraKeys.ToString(), logStatement.Last().ToString());
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

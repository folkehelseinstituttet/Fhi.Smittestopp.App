using System;
using System.Collections.Generic;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Test.Mocks;
using Newtonsoft.Json;
using Xamarin.ExposureNotifications;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class ExposureDetectedTests
    {
        private static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();
        private static SecureStorageService _secureStorageService => ServiceLocator.Current.GetInstance<SecureStorageService>();

        public ExposureDetectedTests()
        {
            DependencyInjectionConfig.Init();
            _preferences.Clear();
            _secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryBucketsAreAboveThresholds()
        {
            //Given the summary returned by EN API has 30 min in low attenuation bucket and 15 min in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(30);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(15);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_returnsFalseWhenSummaryBucketsAreBelowThresholds()
        {
            //Given the summary returned by EN API has 10 min in mid attenuation bucket and 100 min in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(100);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message will not be shown
            Assert.False(isOverThreshold);
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryHas30MinOnlyInMiddleBucket()
        {
            //Given the summary returned by EN API has 30 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(30);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryHas10MinInLowBucketAnd10MinInMiddleBucket()
        {
            //Given the summary returned by EN API has 10 min in low attenuation bucket and 10 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_IgnoresAnyValueInTheHighBucket()
        {
            //Given the summary returned by EN API has 10 min in low attenuation bucket and a a lot of minutes in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(9999999);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message won't be shown
            Assert.False(isOverThreshold);
        }

        [Fact]
        public void IsAttenuationDurationOverThreshold_returnsFalseWhenSummaryHas10MinInLowBucketAnd8MinInMiddleBucket()
        {
            //Given the summary returned by EN API has 10 min in low attenuation bucket and 8 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(8);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new TimeSpan[] { lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration }, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message won't be shown
            Assert.False(isOverThreshold);
        }

        [Fact]
        public void RiskInDailySummaryAboveThreshold_ReturnsTrueWhenAboveConfiguredLimit()
        {
            LocalPreferencesHelper.MaximumScoreThreshold = 900;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(901, 0, 0);
            DailySummary dailySummary = new DailySummary(DateTime.Now, dailySummaryReport, new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.True(isAboveThreshold);
        }

        [Fact]
        public void RiskInDailySummaryAboveThreshold_ReturnsTrueWhenAtConfiguredLimit()
        {
            LocalPreferencesHelper.MaximumScoreThreshold = 900;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(900, 0, 0);
            DailySummary dailySummary = new DailySummary(SystemTime.Now(), dailySummaryReport, new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.True(isAboveThreshold);
        }

        [Fact]
        public void RiskInDailySummaryAboveThreshold_ReturnsFalseWhenBelowConfiguredLimit()
        {
            LocalPreferencesHelper.MaximumScoreThreshold = 900;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(899, 0, 0);
            DailySummary dailySummary = new DailySummary(SystemTime.Now(), dailySummaryReport, new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.False(isAboveThreshold);
        }

        [Fact]
        public void HasNotShownExposureNotificationForDate_ReturnsTrueIfTimeStampHasNotYetBeenSaved()
        {
            SystemTime.ResetDateTime();
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(0).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-1).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-2).Date);


            bool savedBefore = ExposureDetectedHelper.HasNotShownExposureNotificationForDate(SystemTime.Now().AddDays(-1), previouslySavedDates);
            bool neverSaved = ExposureDetectedHelper.HasNotShownExposureNotificationForDate(SystemTime.Now().AddDays(-3), previouslySavedDates);
            Assert.False(savedBefore);
            Assert.True(neverSaved);
        }

        [Fact]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_DeletesDaysOlderThan14Days()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> previouslySavedDates = createListOfDateTimesUpToXDaysInThePast(20);
            _secureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Equal(14, validDates.Count);
        }

        [Fact]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_DeletesNoDaysOlderThan14DaysOld()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> previouslySavedDates = createListOfDateTimesUpToXDaysInThePast(14);
            _secureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Equal(14, validDates.Count);
        }

        [Fact]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_ReturnsEmptyListWhenNothingIsSavedYet()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Empty(validDates);
        }

        [Fact]
        public async void UpdateDatesOfExposures_AddsAllNewDates_WhenNothingIsSavedYet()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(_secureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

            Assert.Equal(datesToSave.Count, savedDates.Count);
        }

        [Fact]
        public async void UpdateDatesOfExposures_AddsAllNewDatesToTheOld_WhenSomeDatesAreSavedInSecureStorage()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(-10).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-8).Date);
            _secureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(_secureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

            Assert.Equal(datesToSave.Count + previouslySavedDates.Count, savedDates.Count);
        }

        [Fact]
        public async void ExposureStateUpdatedAsync_DeletesOldDatesAndAddsNewDates()
        {
            SystemTime.ResetDateTime();
            _secureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(-20).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-18).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-8).Date);
            _secureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            Assert.Single(ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList());

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(_secureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

            Assert.Equal(datesToSave.Count + 1, savedDates.Count);
        }

        private List<DateTime> createListOfDateTimesUpToXDaysInThePast(int daysInThePast)
        {
            List<DateTime> previouslySavedDates = new List<DateTime>();

            for (int i = 0 - daysInThePast; i <= 0; i++)
            {
                previouslySavedDates.Add(SystemTime.Now().AddDays(i).Date);
            }

            return previouslySavedDates;
        }
    }
}
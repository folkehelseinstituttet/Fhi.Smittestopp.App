using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CommonServiceLocator;
using FluentAssertions;
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
        public ExposureDetectedTests()
        {
            DependencyInjectionConfig.Init();
            Preferences.Clear();
            SecureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        private static IPreferences Preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        private static SecureStorageService SecureStorageService =>
            ServiceLocator.Current.GetInstance<SecureStorageService>();

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryBucketsAreAboveThresholds(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            //Given the summary returned by EN API has 30 min in low attenuation bucket and 15 min in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(30);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(15);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_returnsFalseWhenSummaryBucketsAreBelowThresholds(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            //Given the summary returned by EN API has 10 min in mid attenuation bucket and 100 min in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(100);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message will not be shown
            Assert.False(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryHas30MinOnlyInMiddleBucket(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            //Given the summary returned by EN API has 30 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(30);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_returnsTrueWhenSummaryHas10MinInLowBucketAnd10MinInMiddleBucket(
            string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            //Given the summary returned by EN API has 10 min in low attenuation bucket and 10 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get true from the call so the message will be shown
            Assert.True(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_IgnoresAnyValueInTheHighBucket(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            //Given the summary returned by EN API has 10 min in low attenuation bucket and a a lot of minutes in high attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(0);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(9999999);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message won't be shown
            Assert.False(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void IsAttenuationDurationOverThreshold_returnsFalseWhenSummaryHas10MinInLowBucketAnd8MinInMiddleBucket(
            string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            //Given the summary returned by EN API has 10 min in low attenuation bucket and 8 min in mid attenuation bucket
            TimeSpan lowAttenuationDuration = TimeSpan.FromMinutes(10);
            TimeSpan mediumAttenuationDuration = TimeSpan.FromMinutes(8);
            TimeSpan highAttenuationDuration = TimeSpan.FromMinutes(0);
            ExposureDetectionSummary summary = new ExposureDetectionSummary(
                3, 100, 2, new[] {lowAttenuationDuration, mediumAttenuationDuration, highAttenuationDuration}, 512);

            //When the IsAttenuationDurationOverThreshold is called
            bool isOverThreshold = ExposureDetectedHelper.IsAttenuationDurationOverThreshold(summary);

            //We will get false from the call so the message won't be shown
            Assert.False(isOverThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void RiskInDailySummaryAboveThreshold_ReturnsTrueWhenAboveConfiguredLimit(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            LocalPreferencesHelper.ScoreSumThreshold = 780;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(0, 781, 0);
            DailySummary dailySummary = new DailySummary(SystemTime.Now(), dailySummaryReport,
                new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.True(isAboveThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void RiskInDailySummaryAboveThreshold_ReturnsTrueWhenAtConfiguredLimit(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            LocalPreferencesHelper.ScoreSumThreshold = 780;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(0, 780, 0);
            DailySummary dailySummary = new DailySummary(SystemTime.Now(), dailySummaryReport,
                new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.True(isAboveThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void RiskInDailySummaryAboveThreshold_ReturnsFalseWhenBelowConfiguredLimit(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            LocalPreferencesHelper.ScoreSumThreshold = 780;
            DailySummaryReport dailySummaryReport = new DailySummaryReport(0, 779, 0);
            DailySummary dailySummary = new DailySummary(SystemTime.Now(), dailySummaryReport,
                new Dictionary<ReportType, DailySummaryReport>());
            bool isAboveThreshold = ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary);

            Assert.False(isAboveThreshold);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void HasNotShownExposureNotificationForDate_ReturnsTrueIfTimeStampHasNotYetBeenSaved(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(0).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-1).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-2).Date);

            bool savedBefore =
                ExposureDetectedHelper.HasNotShownExposureNotificationForDate(SystemTime.Now().AddDays(-1),
                    previouslySavedDates);
            bool neverSaved =
                ExposureDetectedHelper.HasNotShownExposureNotificationForDate(SystemTime.Now().AddDays(-3),
                    previouslySavedDates);

            Assert.False(savedBefore);
            Assert.True(neverSaved);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_DeletesDaysOlderThan14Days(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> previouslySavedDates = createListOfDateTimesUpToXDaysInThePast(20);
            SecureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Equal(14, validDates.Count);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_DeletesNoDaysOlderThan14DaysOld(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> previouslySavedDates = createListOfDateTimesUpToXDaysInThePast(14);
            SecureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Equal(14, validDates.Count);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public void DeleteDatesOfExposureOlderThan14DaysAndReturnNewList_ReturnsEmptyListWhenNothingIsSavedYet(
            string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> validDates = ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList();

            Assert.Empty(validDates);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public async void UpdateDatesOfExposures_AddsAllNewDates_WhenNothingIsSavedYet(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(
                    SecureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

            datesToSave.Should().BeEquivalentTo(savedDates);
            Assert.Equal(datesToSave.Count, savedDates.Count);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public async void UpdateDatesOfExposures_AddsAllNewDatesToTheOld_WhenSomeDatesAreSavedInSecureStorage(
            string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(-10).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-8).Date);
            SecureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(
                    SecureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

            Assert.Equal(datesToSave.Count + previouslySavedDates.Count, savedDates.Count);
            datesToSave.Concat(previouslySavedDates).Should().BeEquivalentTo(savedDates);
        }

        [Theory]
        [InlineData("th-TH")]
        [InlineData("en-US")]
        [InlineData("pl-PL")]
        [InlineData("ar-SA")]
        public async void ExposureStateUpdatedAsync_DeletesOldDatesAndAddsNewDates(string locale)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);

            SystemTime.ResetDateTime();
            SecureStorageService.Delete(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY);
            List<DateTime> previouslySavedDates = new List<DateTime>();
            previouslySavedDates.Add(SystemTime.Now().AddDays(-20).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-18).Date);
            previouslySavedDates.Add(SystemTime.Now().AddDays(-8).Date);
            SecureStorageService.SaveValue(
                SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY,
                JsonConvert.SerializeObject(previouslySavedDates));

            List<DateTime> datesToSave = createListOfDateTimesUpToXDaysInThePast(2);

            Assert.Single(ExposureDetectedHelper.DeleteDatesOfExposureOlderThan14DaysAndReturnNewList());

            await ExposureDetectedHelper.UpdateDatesOfExposures(datesToSave);

            List<DateTime> savedDates =
                JsonConvert.DeserializeObject<List<DateTime>>(
                    SecureStorageService.GetValue(SecureStorageKeys.DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY));

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
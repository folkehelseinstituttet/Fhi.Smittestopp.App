using System;
using System.Globalization;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices.ExposureNotification;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class PullKeysParamsTest
    {
        static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();
        static string PULL_DATE_KEY => PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME;
        static string PULL_BATCH_KEY => PreferencesKeys.LAST_PULLED_BATCH_NUMBER_SUBMITTED;
        static string PULL_BATCH_TYPE => PreferencesKeys.LAST_PULLED_BATCH_TYPE;

        public PullKeysParamsTest()
        {
            DependencyInjectionConfig.Init();
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_PullFirstTime()
        {
            //Given the app never pulled before (preferences will be empty)
            _preferences.Clear();
            DateTime today = new DateTime(2020, 4, 4, 8, 8, 8);
            SystemTime.SetDateTime(today);

            //When the app pulls for the first time
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //We will request all keys for today
            Assert.Equal(today.Date, pullParams.Date);
            Assert.Equal(1, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_PullFirstTime_ConsentGiven()
        {
            //Given the app never pulled before, but consent was given to pull EU keys.
            _preferences.Clear();
            OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted;

            //When the app pulls for the first time
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //We will request all keys for today but only NO keys (consent was not given yet)
            Assert.Equal(SystemTime.Now().Date, pullParams.Date);
            Assert.Equal(1, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_NotFirstPullOfDay()
        {
            //Given that we previously successfully pulled keys today and the last batch was number 3.
            DateTime lastPullDate = SystemTime.Now().Date;
            _preferences.Clear();
            _preferences.Set(PULL_DATE_KEY, lastPullDate.ToUniversalTime());
            _preferences.Set(PULL_BATCH_KEY, 3);
            _preferences.Set(PULL_BATCH_TYPE, "all");

            //When the app pulls
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //Then the next batch number for today will be requested
            Assert.Equal(lastPullDate, pullParams.Date);
            Assert.Equal(4, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_NotFirstPull()
        {
            //App was just updated to pull EU keys, and the last pull was NO keys.
            DateTime lastPullDate = SystemTime.Now().Date;
            _preferences.Clear();
            _preferences.Set(PULL_DATE_KEY, lastPullDate.ToUniversalTime());
            _preferences.Set(PULL_BATCH_KEY, 3);
            _preferences.Set(PULL_BATCH_TYPE, "no");

            //When the app pulls for the first time
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //We will request all keys for today
            Assert.Equal(SystemTime.Now().Date, pullParams.Date);
            Assert.Equal(1, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_LastPullWasThreeDaysAgo()
        {
            //Given that last time we pulled keys was three days ago and the last batch was number 2 for that day
            DateTime lastPullDate = SystemTime.Now().AddDays(-3).Date;
            _preferences.Clear();
            _preferences.Set(PULL_DATE_KEY, lastPullDate.ToUniversalTime());
            _preferences.Set(PULL_BATCH_KEY, 2);
            _preferences.Set(PULL_BATCH_TYPE, "all");

            //When the app pulls
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //Then the next batch number for three days ago will be requested
            Assert.Equal(lastPullDate, pullParams.Date);
            Assert.Equal(3, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void PullKeysParams_LastPullWas15DaysAgo()
        {
            //Given that last time we pulled keys was 20 days ago and the last batch was number 2 for that day
            DateTime lastPullDate = SystemTime.Now().AddDays(-20).Date;
            _preferences.Clear();
            _preferences.Set(PULL_DATE_KEY, lastPullDate.ToUniversalTime());
            _preferences.Set(PULL_BATCH_KEY, 2);

            //When the app pulls
            PullKeysParams pullParams = PullKeysParams.GenerateParams();

            //Then it pulls only for the last 14 days, incl. today.
            Assert.Equal(SystemTime.Now().AddDays(-13).Date, pullParams.Date);
            Assert.Equal(1, pullParams.BatchNumber);
            Assert.Equal(BatchType.ALL, pullParams.BatchType);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Theory]
        [InlineData("23:30 +2", -1, -1)]
        [InlineData("00:30 +2", 0, -1)]
        [InlineData("02:30 +2", 0, 0)]
        public void PullKeysParams_LastPullWasAtTimes(string time, int daysAgo, int expectedDaysAgo)
        {
            //Given that last time we pulled keys was at given time
            DateTime today = SystemTime.Now();
            string dateString = today.AddDays(daysAgo).ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("nn-NO"));
            DateTime lastPullDate = DateTime.ParseExact($"{dateString} {time}", "yyyy-MM-dd HH:mm z", CultureInfo.GetCultureInfo("nn-NO"));

            _preferences.Clear();
            _preferences.Set(PULL_DATE_KEY, lastPullDate.ToUniversalTime());
            _preferences.Set(PULL_BATCH_TYPE, "all");
            _preferences.Set(PULL_BATCH_KEY, 2);

            //When the app pulls
            PullKeysParams pullParams = PullKeysParams.GenerateParams();
            string pullQueryParams = pullParams.ToBatchFileRequest();

            //The date will be parsed right (UTC time)
            string expectedDateString = today.AddDays(expectedDaysAgo).ToString("yyyy-MM-dd");
            Assert.Equal($"{expectedDateString}_3_all.zip", pullQueryParams);

            //Reset
            SystemTime.ResetDateTime();
        }

        [Theory]
        [InlineData("23:30 +2")]
        [InlineData("00:30 +2")]
        [InlineData("02:30 +2")]
        public void PullKeysParams_PullAtTimes(string time)
        {
            //Given that the current time is as follows, and the app never pulled before
            DateTime today = SystemTime.Now().Date;
            string dateString = today.ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("nn-NO"));
            DateTime newNow = DateTime.ParseExact($"{dateString} {time}", "yyyy-MM-dd HH:mm z", CultureInfo.GetCultureInfo("nn-NO"));
            SystemTime.SetDateTime(newNow);

            _preferences.Clear();

            //When the app pulls
            PullKeysParams pullParams = PullKeysParams.GenerateParams();
            string pullQueryParams = pullParams.ToBatchFileRequest();

            //The date will be parsed right (UTC)
            string expectedDateString = SystemTime.Now().ToUniversalTime().Date.ToString("yyyy-MM-dd");
            Assert.Equal($"{expectedDateString}_1_all.zip", pullQueryParams);

            //Reset
            SystemTime.ResetDateTime();
            
        }
    }
}
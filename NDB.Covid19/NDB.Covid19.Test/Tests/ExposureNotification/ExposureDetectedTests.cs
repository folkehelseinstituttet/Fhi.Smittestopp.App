using System;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotification.Helpers;
using NDB.Covid19.ExposureNotification.Helpers.ExposureDetected;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.SecureStorage;
using NDB.Covid19.Test.Mocks;
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
    }
}
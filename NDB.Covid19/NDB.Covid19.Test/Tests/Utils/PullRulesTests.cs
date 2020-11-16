using System.Collections.Generic;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using Xamarin.Essentials;
using Xunit;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class PullRulesTests
    {
        private readonly IDeveloperToolsService _developerToolsService;
        private IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        public PullRulesTests()
        {
            DependencyInjectionConfig.Init();
            _developerToolsService = ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
        }

        [Fact]
        public void ShouldAbortPull_MobileDataDisabled_Should_ReturnTrue()
        {
            // Given
            PullRules pullRules = new PullRules();
            ConnectivityHelper.MockConnectionProfiles(new List<ConnectionProfile> { ConnectionProfile.Cellular });
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(false);

            // When
            bool shouldAbortPull = pullRules.ShouldAbortPull();

            // Then
            Assert.True(shouldAbortPull);

            ConnectivityHelper.ResetConnectionProfiles();
        }

        [Fact]
        public void ShouldAbortPull_MobileDataEnabled_Should_ReturnFalse()
        {
            // Given
            PullRules pullRules = new PullRules();
            ConnectivityHelper.MockConnectionProfiles(new List<ConnectionProfile> { ConnectionProfile.Cellular });
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(true);

            // When
            bool shouldAbortPull = pullRules.ShouldAbortPull();

            // Then
            Assert.False(shouldAbortPull);

            ConnectivityHelper.ResetConnectionProfiles();
        }

        [Fact]
        public void ShouldAbortPull_MobileDataDisabled_SavedLog()
        {
            // Given
            PullRules pullRules = new PullRules();
            ConnectivityHelper.MockConnectionProfiles(new List<ConnectionProfile> { ConnectionProfile.Cellular });
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(false);

            // When
            string pullKeyInfoPre = _developerToolsService.LastPullHistory;
            pullRules.ShouldAbortPull();
            string pullKeyInfoPost = _developerToolsService.LastPullHistory;

            // Then
            Assert.Empty(pullKeyInfoPre);
            Assert.NotEmpty(pullKeyInfoPost);

            ConnectivityHelper.ResetConnectionProfiles();
        }

        [Fact]
        public void ShouldAbortPull_MobileDataEnabled_NotSavedLog()
        {
            // Given
            PullRules pullRules = new PullRules();
            ConnectivityHelper.MockConnectionProfiles(new List<ConnectionProfile> { ConnectionProfile.Cellular });
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(true);

            // When
            string pullKeyInfoPre = _developerToolsService.LastPullHistory;
            pullRules.ShouldAbortPull();
            string pullKeyInfoPost = _developerToolsService.LastPullHistory;

            // Then
            Assert.Empty(pullKeyInfoPre);
            Assert.Empty(pullKeyInfoPost);

            ConnectivityHelper.ResetConnectionProfiles();
        }

        [Fact]
        public void LastDownloadZipsTooRecent_ShouldReturnFalse()
        {
            // Given
            PullRules pullRules = new PullRules();
            SystemTime.ResetDateTime();

            // When
            _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now().AddHours(-3));
            bool lastDownloadZipsTooRecent = pullRules.LastDownloadZipsTooRecent();

            // Then
            Assert.False(lastDownloadZipsTooRecent);
        }

        [Fact]
        public void LastDownloadZipsTooRecent_ShouldReturnTrue()
        {
            // Given
            PullRules pullRules = new PullRules();
            SystemTime.ResetDateTime();

            // When
            _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now().AddMinutes(-1));
            bool lastDownloadZipsTooRecent = pullRules.LastDownloadZipsTooRecent();

            // Then
            Assert.True(lastDownloadZipsTooRecent);
        }
    }
}

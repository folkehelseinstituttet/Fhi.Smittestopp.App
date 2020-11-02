using NDB.Covid19.PersistedData;
using Xunit;

namespace NDB.Covid19.Test.Tests.PersistedData.Preferences
{
    public class PreferencesTests
    {
        public PreferencesTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public void Preferences_UseMobileData_DefaultIsTrue()
        {
            bool isDownloadWithMobileDataEnabled = LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
            Assert.True(isDownloadWithMobileDataEnabled);
        }

        [Fact]
        public void Preferences_UseMobileData_CustomValueIsSaved_False()
        {
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(false);
            bool isDownloadWithMobileDataEnabled = LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
            Assert.False(isDownloadWithMobileDataEnabled);
        }

        [Fact]
        public void Preferences_UseMobileData_CustomValueIsSaved_True()
        {
            LocalPreferencesHelper.SetIsDownloadWithMobileDataEnabled(true);
            bool isDownloadWithMobileDataEnabled = LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
            Assert.True(isDownloadWithMobileDataEnabled);
        }
    }
}

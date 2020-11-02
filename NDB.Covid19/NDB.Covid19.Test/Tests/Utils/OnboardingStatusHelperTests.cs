using NDB.Covid19.Enums;
using Xunit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.Utils.OnboardingStatusHelper;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class OnboardingStatusHelperTests
    {
        public OnboardingStatusHelperTests()
        {
            DependencyInjectionConfig.Init();
        }

        public enum Pref
        {
            Main,
            Country
        }

        [Theory]
        // Given
        [InlineData(Pref.Main, false)]
        [InlineData(Pref.Main, true)]
        [InlineData(Pref.Country, false)]
        [InlineData(Pref.Country, true)]
        public void LocalPreferences_isCorrect(Pref pref, bool status)
        {
            // When
            if (pref == Pref.Main) IsOnboardingCompleted = status;
            else IsOnboardingCountriesCompleted = status;
            
            // Then
            Assert.Equal(pref == Pref.Main ? IsOnboardingCompleted : IsOnboardingCountriesCompleted, status);
        }

        [Theory]
        // Given
        [InlineData(OnboardingStatus.NoConsentsGiven)]
        [InlineData(OnboardingStatus.OnlyMainOnboardingCompleted)]
        [InlineData(OnboardingStatus.CountriesOnboardingCompleted)]
        public void OnboardingStatus_SetStatus_StatusIsCorrect(OnboardingStatus status)
        {
            // When
            Status = status;

            // Then
            Assert.Equal(Status, status);
        }

        [Theory]
        // Given
        [InlineData(false, false, OnboardingStatus.NoConsentsGiven)]
        [InlineData(true, false, OnboardingStatus.OnlyMainOnboardingCompleted)]
        [InlineData(true, true, OnboardingStatus.CountriesOnboardingCompleted)]
        [InlineData(false, true, OnboardingStatus.CountriesOnboardingCompleted)]
        public void OnboardingStatus_SetLocalPreferences_StatusIsCorrect(bool mainCompleted, bool countriesCompleted, OnboardingStatus status)
        {
            // When
            IsOnboardingCompleted = mainCompleted;
            IsOnboardingCountriesCompleted = countriesCompleted;

            // Then
            Assert.Equal(Status, status);
        }
    }
}

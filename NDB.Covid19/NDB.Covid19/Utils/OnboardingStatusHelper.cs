using NDB.Covid19.Enums;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Utils
{
    public static class OnboardingStatusHelper
    {
        public static OnboardingStatus Status
        {
            get
            {
                if (!IsOnboardingCompleted && !IsOnboardingCountriesCompleted)
                {
                    return OnboardingStatus.NoConsentsGiven;
                }

                if (IsOnboardingCompleted && !IsOnboardingCountriesCompleted)
                {
                    return OnboardingStatus.OnlyMainOnboardingCompleted;
                }

                return OnboardingStatus.CountriesOnboardingCompleted;
            }
            set
            {
                switch (value)
                {
                    case OnboardingStatus.NoConsentsGiven:
                        IsOnboardingCompleted = false;
                        IsOnboardingCountriesCompleted = false;
                        break;
                    case OnboardingStatus.OnlyMainOnboardingCompleted:
                        IsOnboardingCompleted = true;
                        IsOnboardingCountriesCompleted = false;
                        break;
                    case OnboardingStatus.CountriesOnboardingCompleted:
                        IsOnboardingCompleted = true;
                        IsOnboardingCountriesCompleted = true;
                        break;
                }
            }
        }
    }
}
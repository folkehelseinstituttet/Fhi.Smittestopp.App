using System;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using static NDB.Covid19.Utils.OnboardingStatusHelper;

namespace NDB.Covid19.Utils
{
    public static class ConsentsHelper
    {
        private static readonly bool IsReleaseOne = false;

        public static void DoActionWhenOnboarded(Action action)
        {
            if (IsReleaseOne &&
                Status == OnboardingStatus.OnlyMainOnboardingCompleted ||
                Status == OnboardingStatus.CountriesOnboardingCompleted)
            {
                action.Invoke();
            }
        }

        public static bool IsNotFullyOnboarded =>
            !IsReleaseOne && Status == OnboardingStatus.OnlyMainOnboardingCompleted;

        public static OnboardingStatus GetStatusDependingOnRelease() =>
            IsReleaseOne ? OnboardingStatus.OnlyMainOnboardingCompleted : OnboardingStatus.CountriesOnboardingCompleted;
    }
}
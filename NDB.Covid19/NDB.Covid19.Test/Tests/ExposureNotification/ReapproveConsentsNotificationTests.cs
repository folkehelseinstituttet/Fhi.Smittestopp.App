using System;
using System.Threading;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class ReapproveConsentsNotificationTests
    {
        private static readonly LocalNotificationManagerMock LocalNotificationsManager =
            (LocalNotificationManagerMock) NotificationsHelper.LocalNotificationsManager;

        public ReapproveConsentsNotificationTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Theory]
        [InlineData(OnboardingStatus.NoConsentsGiven, false)]
        [InlineData(OnboardingStatus.OnlyMainOnboardingCompleted, true)]
        [InlineData(OnboardingStatus.CountriesOnboardingCompleted, false)]
        public async void OnlyMainOnboardingCompleted_ShouldNotSenReApproveConsentNotification(
            OnboardingStatus status,
            bool result)
        {
            LocalPreferencesHelper.TermsNotificationWasShown = false;
            LocalNotificationsManager.ResetHasBeenCalledMap();
            LocalNotificationsManager.HasBeenCalled[NotificationsEnum.ReApproveConsents] = false;
            OnboardingStatusHelper.Status = status;


            try
            {
                await new FetchExposureKeysHelper().FetchExposureKeyBatchFilesFromServerAsync(null,
                    CancellationToken.None);
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.Equal(result, LocalNotificationsManager.HasBeenCalled[NotificationsEnum.ReApproveConsents]);
            Assert.Equal(result, LocalPreferencesHelper.TermsNotificationWasShown);
        }
    }
}
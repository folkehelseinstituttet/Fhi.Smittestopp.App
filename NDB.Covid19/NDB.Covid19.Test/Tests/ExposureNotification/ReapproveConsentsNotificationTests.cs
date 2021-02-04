using System;
using System.Threading;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using NDB.Covid19.ExposureNotifications.Helpers;
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

        [Theory(Skip = "Not present in this version")]
        [InlineData(OnboardingStatus.NoConsentsGiven, false)]
        [InlineData(OnboardingStatus.OnlyMainOnboardingCompleted, true)]
        [InlineData(OnboardingStatus.CountriesOnboardingCompleted, false)]
        public async void OnlyMainOnboardingCompleted_ShouldNotSenReApproveConsentNotification(
            OnboardingStatus status,
            bool result)
        {
            SystemTime.ResetDateTime();
            LocalPreferencesHelper.LastDateTimeTermsNotificationWasShown = DateTime.MinValue;
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
            Assert.Equal(result, LocalPreferencesHelper.LastDateTimeTermsNotificationWasShown != DateTime.MinValue);
        }

        [Theory(Skip = "Not present in this version")]
        [InlineData(10, 10, 5)]
        [InlineData(3, 3, 6)]
        [InlineData(1, 1, 5)]
        [InlineData(1, 1, 1)]
        public async void UntilNewTermsAreAccepted_NotificationResentNotOftenThan24h(
            int daysToPullForInFuture,
            int numberOfNotificationsExpectedToBeShown,
            int pullingIntervalInHours)
        {
            SystemTime.ResetDateTime();
            LocalPreferencesHelper.LastDateTimeTermsNotificationWasShown = DateTime.MinValue;
            LocalNotificationsManager.ResetHasBeenCalledMap();
            LocalNotificationsManager.HasBeenCalled[NotificationsEnum.ReApproveConsents] = false;
            LocalNotificationsManager.NewConsentsHasBeenCalledCount = 0;
            OnboardingStatusHelper.Status = OnboardingStatus.OnlyMainOnboardingCompleted;

            DateTime inXDays = SystemTime.Now().AddDays(daysToPullForInFuture);

            while(SystemTime.Now() < inXDays)
            {
                try
                {
                    await new FetchExposureKeysHelper().FetchExposureKeyBatchFilesFromServerAsync(null,
                        CancellationToken.None);
                }
                catch (Exception)
                {
                    // ignore
                }
                SystemTime.SetDateTime(SystemTime.Now().AddHours(pullingIntervalInHours));
            }
            
            Assert.True(LocalNotificationsManager.HasBeenCalled[NotificationsEnum.ReApproveConsents]);
            Assert.Equal(numberOfNotificationsExpectedToBeShown, LocalNotificationsManager.NewConsentsHasBeenCalledCount);
        }
    }
}
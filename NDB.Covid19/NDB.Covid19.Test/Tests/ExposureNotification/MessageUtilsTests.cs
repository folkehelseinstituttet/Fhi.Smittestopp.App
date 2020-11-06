using System;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class MessageUtilsTests
    {
        private static SecureStorageService _secureStorageService => ServiceLocator.Current.GetInstance<SecureStorageService>();

        public MessageUtilsTests()
        {
            DependencyInjectionConfig.Init();
            _secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        [Fact]
        public void HasAlertedAboutRiskToday_assertTrueForTodayAndFalseForAnyOtherDay()
        {
            _secureStorageService.SaveValue(SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, DateTime.UtcNow.ToString());
            SystemTime.ResetDateTime();
            Assert.True(MessageUtils.HasCreatedMessageAndNotificationToday());

            _secureStorageService.SaveValue(SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, DateTime.UtcNow.AddDays(-1).ToString());
            SystemTime.ResetDateTime();
            Assert.False(MessageUtils.HasCreatedMessageAndNotificationToday());

            _secureStorageService.SaveValue(SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, DateTime.UtcNow.AddDays(-2).ToString());
            SystemTime.ResetDateTime();
            Assert.False(MessageUtils.HasCreatedMessageAndNotificationToday());

            _secureStorageService.SaveValue(SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, DateTime.UtcNow.AddDays(-10).ToString());
            SystemTime.ResetDateTime();
            Assert.False(MessageUtils.HasCreatedMessageAndNotificationToday());

            _secureStorageService.SaveValue(SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, DateTime.UtcNow.AddDays(1).ToString());
            SystemTime.ResetDateTime();
            Assert.False(MessageUtils.HasCreatedMessageAndNotificationToday());
        }
    }
}

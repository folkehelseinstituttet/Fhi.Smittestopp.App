using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Models.Logging;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using Xunit;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class NTPDateTimeTests
    {
        public NTPDateTimeTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public async void SystemDateTimeIsCorrect_ShouldUseSystemDateTime()
        {
            SystemTime.ResetDateTime();
            DateTime currentTime = DateTime.UtcNow;
            SystemTime.SetDateTime(currentTime);
            
            try
            {
                new FetchExposureKeysHelper()
                    .UpdateLastNTPDateTime();
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }
            
            LogDeviceDetails logModel =
                new LogDeviceDetails(
                    LogSeverity.WARNING,
                    "message",
                    "additionalInfo");
            
            Assert.Equal(currentTime, logModel.ReportedTime);
            
            SystemTime.ResetDateTime();
        }
        
        [Fact]
        public async void SystemDateTimeIsIncorrect_NtpIsLowerThanCurrent_ShouldUseDefault()
        {
            SystemTime.ResetDateTime();
            DateTime currentTime = DateTime.UtcNow.AddYears(-4);
            SystemTime.SetDateTime(currentTime);

            NTPUtcDateTime ntpUtcDateTime =
                Mock.Of<NTPUtcDateTime>(ntp =>
                    ntp.GetNTPUtcDateTime() == Task.FromResult(currentTime.AddYears(-1)));
            
            try
            {
                new FetchExposureKeysHelper().UpdateLastNTPDateTime(ntpUtcDateTime);
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }
            LogDeviceDetails logModel =
                new LogDeviceDetails(
                    LogSeverity.WARNING,
                    "message",
                    "additionalInfo");
            
            Assert.Equal(Conf.DATE_TIME_REPLACEMENT,logModel.ReportedTime);
            
            SystemTime.ResetDateTime();
        }
        
        [Fact]
        public async void SystemDateTimeIsIncorrect_NtpIsEqualToPersisted_ShouldUseDefault()
        {
            SystemTime.ResetDateTime();
            DateTime currentTime = DateTime.UtcNow.AddYears(-4);
            SystemTime.SetDateTime(currentTime);

            NTPUtcDateTime ntpUtcDateTime =
                Mock.Of<NTPUtcDateTime>(ntp =>
                    ntp.GetNTPUtcDateTime() == Task.FromResult(LocalPreferencesHelper.LastNTPUtcDateTime));
            
            try
            {
                new FetchExposureKeysHelper().UpdateLastNTPDateTime(ntpUtcDateTime);
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }
            LogDeviceDetails logModel =
                new LogDeviceDetails(
                    LogSeverity.WARNING,
                    "message",
                    "additionalInfo");
            
            Assert.Equal(Conf.DATE_TIME_REPLACEMENT,logModel.ReportedTime);
            
            SystemTime.ResetDateTime();
        }
        
        [Fact]
        public async void SystemDateTimeIsIncorrect_ShouldUseNTP()
        {
            SystemTime.ResetDateTime();
            DateTime currentTime = DateTime.UtcNow.AddYears(-4);
            SystemTime.SetDateTime(currentTime);
            
            try
            {
                await new FetchExposureKeysHelper()
                    .FetchExposureKeyBatchFilesFromServerAsync(
                        null,
                        CancellationToken.None);
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }
            
            LogDeviceDetails logModel =
                new LogDeviceDetails(
                    LogSeverity.WARNING,
                    "message",
                    "additionalInfo");
            
            Assert.True(currentTime < logModel.ReportedTime);
            
            SystemTime.ResetDateTime();
        }
        
        [Fact]
        public async void SystemDateTimeIsIncorrectInTheFuture_ShouldUseNTP()
        {
            SystemTime.ResetDateTime();
            DateTime currentTime = DateTime.UtcNow.AddYears(3);
            SystemTime.SetDateTime(currentTime);
            
            try
            {
                await new FetchExposureKeysHelper()
                    .FetchExposureKeyBatchFilesFromServerAsync(
                        null,
                        CancellationToken.None);
            }
            catch
            {
                // ignore as ZipDownloader is not mocked in this test
            }
            
            LogDeviceDetails logModel =
                new LogDeviceDetails(
                    LogSeverity.WARNING,
                    "message",
                    "additionalInfo");
            
            Assert.True(currentTime > logModel.ReportedTime);
            
            SystemTime.ResetDateTime();
        }
    }
}
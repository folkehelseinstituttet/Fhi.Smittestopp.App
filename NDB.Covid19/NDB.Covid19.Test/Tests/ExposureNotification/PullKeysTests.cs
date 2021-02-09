using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using NDB.Covid19.WebServices.ExposureNotification;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class PullKeysTests
    {
        private ZipDownloaderHelper _helper;
        private IPreferences _preferences;
        private ILoggingManager _logManager;
        private IDeveloperToolsService _developerTools;

        //Day3 is today
        DateTime day1 = new DateTime(2020, 8, 23, 0, 0, 0, DateTimeKind.Utc).Date;
        DateTime day2 = new DateTime(2020, 8, 24, 0, 0, 0, DateTimeKind.Utc).Date;
        DateTime day3 = new DateTime(2020, 8, 25, 0, 0, 0, DateTimeKind.Utc).Date;
        DateTime forteenDaysAgo = new DateTime(2020, 8, 11, 0, 0, 0, DateTimeKind.Utc).Date;
        DateTime fifteenDaysAgo = new DateTime(2020, 8, 10, 0, 0, 0, DateTimeKind.Utc).Date;
        DateTime sixteenDaysAgo = new DateTime(2020, 8, 9, 0, 0, 0, DateTimeKind.Utc).Date;

        // For unit testing real failed tests conducted by testers
        DateTime day4 = new DateTime(2020, 10, 31, 9, 24, 0, DateTimeKind.Utc).Date;
        DateTime sixteenDaysAgoForGapsTest = new DateTime(2020, 10, 16, 15, 20, 0, DateTimeKind.Utc).Date;

        static string PULL_BATCH_TYPE => PreferencesKeys.LAST_PULLED_BATCH_TYPE;

        public PullKeysTests()
        {
            DependencyInjectionConfig.Init();
            string zipDirectory = ServiceLocator.Current.GetInstance<IFileSystem>().CacheDirectory;
            if (!Directory.Exists(zipDirectory))
            {
                Directory.CreateDirectory(zipDirectory);
            }
            _helper = new ZipDownloaderHelper();

            _preferences = ServiceLocator.Current.GetInstance<IPreferences>();
            _logManager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            _developerTools = ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
            _preferences.Clear();
            _logManager.DeleteAll();
            _developerTools.ClearAllFields();
           
        }

        [Theory]
        [InlineData("2020-08-25 05:30 +2")]
        [InlineData("2020-08-26 00:30 +2")] //This is still the 25th in UTC.
        public async void PullKeys_FetchZipsForMultipleDays_NoErrors(string todayString)
        {
            int lastBatchNumFromHeader = 5;

            //Given there are 3 batches for day1, 3 batches for day 2, 3 batches for day3
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 4).HttpStatusCode(204),

                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(200).WithLastBatchHeader(lastBatchNumFromHeader).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });

            //Given today is day3
            DateTime newToday = DateTime.ParseExact(todayString, "yyyy-MM-dd HH:mm z", CultureInfo.GetCultureInfo("nn-NO"));
            SystemTime.SetDateTime(newToday);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day3, batch2.
            _helper.SetLastPulledDate(day1, 2);

            //When pulling keys
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull the rest from day 1, all from day 2, and all from day 3
            Assert.Equal(7, zipLocations.Count);

            //The last batch number is saved from header
            Assert.Equal(lastBatchNumFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);

            //And no errors were logged
            Assert.False((await _logManager.GetLogs(10)).Any());

            //The history is stored for dev tools:
            string expected = $"Pulled the following keys (batches) at {newToday.ToUniversalTime().ToString("yyyy-MM-dd HH:mm")} UTC:\n" +
                $"* 2020-08-23_3_all.zip: 200 OK\n" +
                $"* 2020-08-24_1_all.zip: 200 OK\n" +
                $"* 2020-08-24_2_all.zip: 200 OK\n" +
                $"* 2020-08-24_3_all.zip: 200 OK\n" +
                $"* 2020-08-25_1_all.zip: 200 OK\n" +
                $"* 2020-08-25_2_all.zip: 200 OK\n" +
                $"* 2020-08-25_3_all.zip: 200 OK";

            Assert.Equal(expected, _developerTools.LastPullHistory);
            Assert.Equal(expected, _developerTools.AllPullHistory);

        }

        [Fact]
        public async void PullKeys_FetchZipsTwoDays()
        {
            //Given there are 3 batches for day1, 3 batches for day 2, 3 batches for day3
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 4).HttpStatusCode(204),

            });

            //Given today is day1
            SystemTime.SetDateTime(day1);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day1, batch1.
            _helper.SetLastPulledDate(day1, 1);

            //When pulling keys
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull the rest from day 1
            Assert.Equal(2, zipLocations.Count);

            //The last batch number is saved from header
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);

            //And no errors were logged
            Assert.False((await _logManager.GetLogs(10)).Any());
            //The history is stored for dev tools:
            string expected = $"Pulled the following keys (batches) at {day1.ToString("yyyy-MM-dd HH:mm")} UTC:\n" +
                $"* 2020-08-23_2_all.zip: 200 OK\n" +
                $"* 2020-08-23_3_all.zip: 200 OK";

            Assert.Equal(expected, _developerTools.LastPullHistory);
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();

            //Now it's the next day
            SystemTime.SetDateTime(day2);

            //And I pull again
            _developerTools.StartPullHistoryRecord();
            zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull everything from day 2
            Assert.Equal(3, zipLocations.Count);

            //The last batch number is saved from header
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);

            //And no errors were logged
            Assert.False((await _logManager.GetLogs(10)).Any());
            string expected2 = $"Pulled the following keys (batches) at {day2.ToString("yyyy-MM-dd HH:mm")} UTC:\n" +
                $"* 2020-08-24_1_all.zip: 200 OK\n" +
                $"* 2020-08-24_2_all.zip: 200 OK\n" +
                $"* 2020-08-24_3_all.zip: 200 OK";

            Assert.Equal(expected2, _developerTools.LastPullHistory);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(400)]
        [InlineData(401)]
        [InlineData(404)]
        public async void PullKeys_FetchZipsForMultipleDays_UnknownErrorFromServer(int errorCode)
        {
            //Given there are 3 batches for day3
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(errorCode),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });

            //Given today is day3
            SystemTime.SetDateTime(day3);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day3, batch2.
            _helper.SetLastPulledDate(day3, 1);

            //When pulling keys
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull 1 good zip before the last one fails
            Assert.Single(zipLocations);

            //The last successful batch number is saved from header
            Assert.Equal(2, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);

            //No error was logged because the service layer will handle that.
            Assert.False((await _logManager.GetLogs(10)).Any());
            //But the error is printed to developer tools
            Assert.Contains("Server Error", _developerTools.LastPullHistory);

            //Asuming submission of keys went well:
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();

            //When pulling the next time, where no errors are returned.
            ExposureNotificationWebService mockedService2 = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations2 = (await new ZipDownloader().PullNewKeys(mockedService2, new CancellationToken())).ToList();

            //Only the missing batches are fetched
            Assert.Single(zipLocations2);
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);

            //No errors
            Assert.False((await _logManager.GetLogs(10)).Any());
            Assert.Contains("200 OK", _developerTools.LastPullHistory);

            //The history is stored for dev tools:
            string day3string = day3.ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
            string lastPullHistoryExpected = $"Pulled the following keys (batches) at {day3string} UTC:\n" +
                $"* 2020-08-25_3_all.zip: 200 OK";
            Assert.Equal(lastPullHistoryExpected, _developerTools.LastPullHistory);

            string AllPullHistoryExpected = $"Pulled the following keys (batches) at {day3string} UTC:\n" +
                $"* 2020-08-25_2_all.zip: 200 OK\n" +
                $"* 2020-08-25_3_all.zip: {errorCode} Server Error\n\n" +
                $"Pulled the following keys (batches) at {day3string} UTC:\n" +
                $"* 2020-08-25_3_all.zip: 200 OK";
            Assert.Equal(AllPullHistoryExpected, _developerTools.AllPullHistory);
        }

        [Fact]
        public async void PullKeys_FetchRestOfKeysFromYesterday_NoErrors()
        {
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 3).HttpStatusCode(204),
            });

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given today is day2
            SystemTime.SetDateTime(day2);

            //Given last time we pulled was day3, batch2.
            _helper.SetLastPulledDate(day1, 2);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull the rest from day 1, and all from day 2
            Assert.Equal(3, zipLocations.Count);
            Assert.Equal(2, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged

            //Asuming submission of keys went well:
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();

            //The next day, when pulling again
            ExposureNotificationWebService mockedService2 = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 4).HttpStatusCode(204),

                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });

            SystemTime.SetDateTime(day3);
            List<string> zipLocations2 = (await new ZipDownloader().PullNewKeys(mockedService2, new CancellationToken())).ToList();

            //It will fetch the rest of day 2 (batch 3) and all from day 3 (batch 1-3).
            Assert.Equal(4, zipLocations2.Count);
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged

        }

        [Fact]
        public async void PullKeys_FetchRestOfKeysFromYesterday_AlreadyHadAllFromYesterday()
        {
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 3).HttpStatusCode(204),
            });

            //Given today is day2
            SystemTime.SetDateTime(day2);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day3, batch2.
            _helper.SetLastPulledDate(day1, 2);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then we pull the rest from day 1, and all from day 2
            Assert.Equal(3, zipLocations.Count);
            Assert.Equal(2, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged

            //Asuming submission of keys went well:
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();

            //The next day, when pulling again
            ExposureNotificationWebService mockedService2 = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 3).HttpStatusCode(204),

                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });

            SystemTime.SetDateTime(day3);
            List<string> zipLocations2 = (await new ZipDownloader().PullNewKeys(mockedService2, new CancellationToken())).ToList();

            //There were no more keys to fetch from day 2, but it will fetch all from day 3 (batch 1-3).
            Assert.Equal(3, zipLocations2.Count);
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged

        }

        [Fact]
        public async void PullKeys_FetchKeys_NoNewKeysForToday()
        {
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),
            });

            //Given today is day1
            SystemTime.SetDateTime(day1);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day, batch3.
            _helper.SetLastPulledDate(day1, 3);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //There were no new keys
            Assert.Empty(zipLocations);

            //A warning was logged
            List<LogSQLiteModel> logs = await _logManager.GetLogs(10);
            Assert.Single(logs);
            Assert.Contains("204 No Content", logs[0].Description);
            Assert.Equal(LogSeverity.WARNING.ToString(), logs[0].Severity);
            Assert.Contains("204 No Content", _developerTools.LastPullHistory);
        }

        [Fact]
        public async void PullKeys_MissingOrBadHeader()
        {
            //Given that the MoreBatchesExist header is missing (batch 3)
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),
            });

            //Given today is day1
            SystemTime.SetDateTime(day1);

            _preferences.Set(PULL_BATCH_TYPE, "all");

            //Given last time we pulled was day, batch3.
            _helper.SetLastPulledDate(day1, 1);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then the missing header is considered a failed pull, only batch 2 was successfull
            Assert.Single(zipLocations);
            Assert.Equal(2, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header

            //An error was logged
            List<LogSQLiteModel> logs = await _logManager.GetLogs(10);
            Assert.Single(logs);
            string expectedErrorMessage = $"Failed to parse {ZipDownloader.MoreBatchesExistHeader} or {ZipDownloader.LastBatchReturnedHeader} header.";
            Assert.Contains(expectedErrorMessage, logs[0].Description);
            Assert.Equal(LogSeverity.ERROR.ToString(), logs[0].Severity);
            Assert.Contains(expectedErrorMessage, _developerTools.LastPullHistory);

            //Prepare for new pull
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();
            await _logManager.DeleteAll();
            _developerTools.ClearAllFields();

            //If LastBatchHeader is missing
            ExposureNotificationWebService mockedService2 = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),
            });
            
            //When pulling keys
            List<string> zipLocations2 = (await new ZipDownloader().PullNewKeys(mockedService2, new CancellationToken())).ToList();

            //Then the missing header is considered a failed pull, there were no successfull new pulls
            Assert.Empty(zipLocations2);
            Assert.Equal(2, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is not updated

            //An error was logged
            logs = await _logManager.GetLogs(10);
            Assert.Single(logs);
            Assert.Contains(expectedErrorMessage, logs[0].Description);
            Assert.Equal(LogSeverity.ERROR.ToString(), logs[0].Severity);
            Assert.Contains(expectedErrorMessage, _developerTools.LastPullHistory);

        }

        [Fact]
        public async void PullKeys_FirstTime()
        {
            int lastBatchNumberFromHeader = 6;

            //Keys exist for more than 14 days back
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(fifteenDaysAgo, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(fifteenDaysAgo, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(fifteenDaysAgo, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(fifteenDaysAgo, 4).HttpStatusCode(204),

                new PullKeysMockData(forteenDaysAgo, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(forteenDaysAgo, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(forteenDaysAgo, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(forteenDaysAgo, 4).HttpStatusCode(204),

                new PullKeysMockData(day1, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 4).HttpStatusCode(204),

                new PullKeysMockData(day2, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 4).HttpStatusCode(204),

                new PullKeysMockData(day3, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 3).HttpStatusCode(200).WithLastBatchHeader(lastBatchNumberFromHeader).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 4).HttpStatusCode(204),
            });

            //Given today is day3 and there has never been pulled before.
            SystemTime.SetDateTime(day3);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then only today is pulled
            Assert.Equal(3, zipLocations.Count);
            Assert.Equal(lastBatchNumberFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged
        }

        [Fact]
        public async void PullKeys_LastFetch15DaysAgo()
        {
            //Keys exist for more than 14 days back
            ExposureNotificationWebService mockedService = _helper.MockedService(SixteenDaysOfKeys());

            //Given today is day3
            SystemTime.SetDateTime(day3);

            //Given last time we pulled was 14 days ago
            _helper.SetLastPulledDate(fifteenDaysAgo, 1);

            //When pulling keys
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();

            //Then only pull the last 14 days' keys
            Assert.Equal(14*3, zipLocations.Count);
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged
        }

        [Fact]
        public async void PullKeys_LastFetch16DaysAgoWithGaps()
        {
            await _logManager.DeleteAll();
            // Fetch one time successfully 16 days ago
            ExposureNotificationWebService mockedService1 = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(sixteenDaysAgoForGapsTest, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(sixteenDaysAgoForGapsTest, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(sixteenDaysAgoForGapsTest, 3, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(sixteenDaysAgoForGapsTest, 4, BatchType.ALL).HttpStatusCode(204),
            });
            SystemTime.SetDateTime(sixteenDaysAgoForGapsTest);
            List<string> zipLocations1 = (await new ZipDownloader().PullNewKeys(mockedService1, new CancellationToken())).ToList();
            // Assert three files were downloaded 15 days ago
            Assert.Equal(3, zipLocations1.Count);

            // Update local preferenced Last pull suceeded time to simulate successful submission to EN API
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();

            //Keys exist for more than 14 days back and there are gaps
            ExposureNotificationWebService mockedService2 = _helper.MockedService(SixteenDaysOfKeysWithGaps());

            //Given today is day4
            SystemTime.SetDateTime(day4);

            //When pulling keys
            List<string> zipLocations2 = (await new ZipDownloader().PullNewKeys(mockedService2, new CancellationToken())).ToList();

            //Then only pull the last 14 days' keys
            Assert.Equal(14 * 3 / 2, zipLocations2.Count);
            Assert.Equal(3, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted); //The last batch number is saved from header
            Assert.False((await _logManager.GetLogs(10)).Any()); //And no errors were logged
        }

        [Theory]
        [InlineData("2020-08-24")]
        [InlineData("2020-08-25")]
        public async void PullKeys_FetchZipsForMultipleDays_FirstFileOfTodaysDateReturns204(string todayDateString)
        {
            string todayString = $"{todayDateString} 01:30 +1";
            int lastBatchNumFromHeader = 4;
            OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted;
            LocalPreferencesHelper.LastPulledBatchType = BatchType.ALL;
            //Given there are 4 batches for day1, 0 batches for day 2
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 4, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(4).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 5, BatchType.ALL).HttpStatusCode(204),
                new PullKeysMockData(day2, 1, BatchType.ALL).HttpStatusCode(204),
                new PullKeysMockData(day3, 1, BatchType.ALL).HttpStatusCode(204),
            });
            //Given today is day2 or day3
            DateTime newToday = DateTime.ParseExact(todayString, "yyyy-MM-dd HH:mm z", CultureInfo.GetCultureInfo("dk"));
            SystemTime.SetDateTime(newToday);
            //Given last time we pulled was day1, batch2.
            _helper.SetLastPulledDate(day1, 2);
            //When pulling keys
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();
            //Then we pull the rest from day 1, none from day 2
            Assert.Equal(2, zipLocations.Count);
            //The last batch number is saved as one since today's first pull ended up in 204
            Assert.Equal(lastBatchNumFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);
            //And one warning is logged
            List<LogSQLiteModel> log = await _logManager.GetLogs(10);
            Assert.Single(log); // Should be one log entry indicating 204 on today's date
            //The history is stored for dev tools:
            string expected = $"Pulled the following keys (batches) at {newToday.ToUniversalTime():yyyy-MM-dd HH:mm} UTC:\n" +
                $"* 2020-08-23_3_all.zip: 200 OK\n" +
                $"* 2020-08-23_4_all.zip: 200 OK\n" +
                $"* {todayDateString}_1_all.zip: 204 No Content - No new keys";
            Assert.Equal(expected, _developerTools.LastPullHistory);
            Assert.Equal(expected, _developerTools.AllPullHistory);

            // Emulate successful submission of the keys to EN API
            _developerTools.AddToPullHistoryRecord("Zips were successfully submitted to EN API.");
            if (LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204)
            {
                LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted = 0;
            }
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();
            LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204 = false;

            Assert.Equal(0, LocalPreferencesHelper.LastPullKeysBatchNumberSuccessfullySubmitted);

            // Emulate pull in couple of hours that ends up in 200 OK
            SystemTime.SetDateTime(SystemTime.Now().AddHours(5));
            lastBatchNumFromHeader = 2;
            mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day2, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 3, BatchType.ALL).HttpStatusCode(204),
                new PullKeysMockData(day3, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day3, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day3, 3, BatchType.ALL).HttpStatusCode(204),
            });
            zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();
            //Then we pull the rest from day 1, none from day 2 or day 3
            Assert.Equal(2, zipLocations.Count);
            //The last batch number is saved as one since today's first pull ended up in 204
            Assert.Equal(lastBatchNumFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);
            // Cleanup log
            await _logManager.DeleteAll();
        }

        [Fact]
        public async void PullKeys_FetchZipsForMultipleDaysWithEUFilesWithFreshInstall_FirstFileOfTodaysDateReturns204()
        {
            string todayString = "2020-08-24 01:30 +1";
            int lastBatchNumFromHeader = 4;
            OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted;
            //Given there are 4 batches for day1, 0 batches for day 2
            ExposureNotificationWebService mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day1, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 3, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day1, 4, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(4).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day1, 5, BatchType.ALL).HttpStatusCode(204),
                new PullKeysMockData(day2, 1, BatchType.ALL).HttpStatusCode(204),
            });
            //Given today is day2
            DateTime newToday = DateTime.ParseExact(todayString, "yyyy-MM-dd HH:mm z", CultureInfo.GetCultureInfo("nn-NO"));
            SystemTime.SetDateTime(newToday);
            //Given last time we pulled was day1, batch2.
            _helper.SetLastPulledDate(day1, 2);
            //When pulling keys
            _developerTools.StartPullHistoryRecord();
            List<string> zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();
            //Then we pull the rest from day 1, none from day 2
            Assert.Equal(4, zipLocations.Count);
            //The last batch number is saved as one since today's first pull ended up in 204
            Assert.Equal(lastBatchNumFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);
            //And 1 warning was logged
            List<LogSQLiteModel> log = await _logManager.GetLogs(10);
            Assert.Single(log); // Should be one log entry indicating 204 on today's date
            //The history is stored for dev tools:
            string expected = $"Pulled the following keys (batches) at {newToday.ToUniversalTime().ToString("yyyy-MM-dd HH:mm")} UTC:\n" +
                $"* 2020-08-23_1_all.zip: 200 OK\n" +
                $"* 2020-08-23_2_all.zip: 200 OK\n" +
                $"* 2020-08-23_3_all.zip: 200 OK\n" +
                $"* 2020-08-23_4_all.zip: 200 OK\n" +
                $"* 2020-08-24_1_all.zip: 204 No Content - No new keys";
            Assert.Equal(expected, _developerTools.LastPullHistory);
            Assert.Equal(expected, _developerTools.AllPullHistory);
            // Emulate successful submission of the keys to EN API
            _developerTools.AddToPullHistoryRecord("Zips were successfully submitted to EN API.");
            if (LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204)
            {
                LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted = 0;
            }
            LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();
            LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204 = false;

            Assert.Equal(0, LocalPreferencesHelper.LastPullKeysBatchNumberSuccessfullySubmitted);

            // Emulate pull in couple of hours that ends up in 200 OK
            SystemTime.SetDateTime(SystemTime.Now().AddHours(5));
            lastBatchNumFromHeader = 2;
            mockedService = _helper.MockedService(new List<PullKeysMockData>
            {
                new PullKeysMockData(day2, 1, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true),
                new PullKeysMockData(day2, 2, BatchType.ALL).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(false),
                new PullKeysMockData(day2, 3, BatchType.ALL).HttpStatusCode(204),
            });
            zipLocations = (await new ZipDownloader().PullNewKeys(mockedService, new CancellationToken())).ToList();
            //Then we pull the rest from day 1, none from day 2
            Assert.Equal(2, zipLocations.Count);
            //The last batch number is saved as one since today's first pull ended up in 204
            Assert.Equal(lastBatchNumFromHeader, LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted);
            //Clean up log
            await _logManager.DeleteAll();
        }

        List<PullKeysMockData> SixteenDaysOfKeys()
        {
            List<PullKeysMockData> keys = new List<PullKeysMockData>();
            DateTime date = sixteenDaysAgo;
            for (int i=0; i<17; i++)
            {
                keys.Add(new PullKeysMockData(date, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true));
                keys.Add(new PullKeysMockData(date, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true));
                keys.Add(new PullKeysMockData(date, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false));
                keys.Add(new PullKeysMockData(date, 4).HttpStatusCode(204));
                date = date.AddDays(1);
            }

            return keys;
        }

        List<PullKeysMockData> SixteenDaysOfKeysWithGaps()
        {
            List<PullKeysMockData> keys = new List<PullKeysMockData>();
            DateTime date = sixteenDaysAgoForGapsTest;
            for (int i = 0; i < 17; i++)
            {
                if (i % 2 == 1) // Make gap every second date
                {
                    keys.Add(new PullKeysMockData(date, 1).HttpStatusCode(200).WithLastBatchHeader(1).WithMoreBatchesExistHeader(true));
                    keys.Add(new PullKeysMockData(date, 2).HttpStatusCode(200).WithLastBatchHeader(2).WithMoreBatchesExistHeader(true));
                    keys.Add(new PullKeysMockData(date, 3).HttpStatusCode(200).WithLastBatchHeader(3).WithMoreBatchesExistHeader(false));
                }
                keys.Add(new PullKeysMockData(date, (i % 2 == 1) ? 4 : 1).HttpStatusCode(204));
                date = date.AddDays(1);
            }

            return keys;
        }
    }
}
using System;
using System.Globalization;
using System.Threading;
using CommonServiceLocator;

using NDB.Covid19.Config;
using NDB.Covid19.ExposureNotification.Helpers;
using NDB.Covid19.Utils;
using NDB.Covid19.Interfaces;
using Xunit;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class MigrationHelperTest
    {
        private static IPreferences preferences => ServiceLocator.Current.GetInstance<IPreferences>();
        private static DateTime testStartDate => new DateTime(2020, 7, 8, 13, 37, 00, DateTimeKind.Utc);


        public MigrationHelperTest()
        {
            DependencyInjectionConfig.Init();
        }

        // Migration to version 1

        // LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF

        [Fact]
        public void MigrateSucessfullyFromDanishToDanish_LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            preferences.Clear();
            SystemTime.SetDateTime(testStartDate);

            DateTime oldSavedDate = SystemTime.Now();
            string oldSavedDateString = oldSavedDate.ToString();
            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, oldSavedDateString);


            // Act
            DateTime migrationTime = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.TrimMilliseconds();
            var actual = preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
        }

        [Fact]
        public void MigrateSucessfullyFromEnglishToDanish_LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            preferences.Clear();
            SystemTime.SetDateTime(testStartDate);

            DateTime oldSavedDate = SystemTime.Now();
            string oldSavedDateString = oldSavedDate.ToString();
            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, oldSavedDateString);


            // Act
            DateTime migrationTime = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.TrimMilliseconds();
            var actual = preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
        }

        [Fact]
        public void MigrateSucessfullyFromThaiToThai_LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            preferences.Clear();
            SystemTime.SetDateTime(testStartDate);


            DateTime oldSavedDate = SystemTime.Now();
            string oldSavedDateString = oldSavedDate.ToString();
            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, oldSavedDateString);


            // Act
            DateTime migrationTime = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.TrimMilliseconds();
            var actual = preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
        }

        [Fact]
        public void MigrateToDefaultFromArabicToArabic_LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF()
        {
            if (Environment.OSVersion.ToString() != "Unix 19.5.0.0") //This test fails on Mac, but still runs find in CI tests
            {
                //Arrange
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
                preferences.Clear();
                SystemTime.SetDateTime(testStartDate);

                DateTime oldSavedDate = SystemTime.Now();
                string oldSavedDateString = oldSavedDate.ToString();
                preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, oldSavedDateString);


                // Act
                DateTime migrationTime = oldSavedDate.AddDays(2);
                SystemTime.SetDateTime(migrationTime);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
                MigrationService migrationService = new MigrationService();
                migrationService.CurrentMigrationVersion = 1;
                migrationService.Migrate();

                // Assert
                var expected = oldSavedDate.TrimMilliseconds();
                var actual = preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
                Assert.Equal(expected, actual);
                Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
            }
        }

        [Fact]
        public void MigrateToFutureDateFromThaiToDanish_LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            preferences.Clear();

            // Set time to now
            SystemTime.SetDateTime(testStartDate);
            DateTime oldSavedDate = SystemTime.Now();

            string oldSavedDateString = oldSavedDate.ToString();
            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, oldSavedDateString);


            // Act
            DateTime migrationTime = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.TrimMilliseconds();
            var actual = preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.True(expected < actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
        }


        // CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF


        [Fact]
        public void MigrateSuccessfullyFromToDanishToDanish_CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            preferences.Clear();

            // Set time to now
            SystemTime.SetDateTime(testStartDate);
            DateTime oldSavedDate = SystemTime.Now();

            string oldSavedDateString = oldSavedDate.Date.ToString("yyyy-MM-dd");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, oldSavedDateString);

            // Act
            SystemTime.SetDateTime(oldSavedDate.AddDays(2)); // some time passes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.Date;
            var actual = preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300));
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
        }

        [Fact]
        public void MigrateSuccessfullyFromToEnglishToDanish_CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-DK");
            preferences.Clear();

            // Set time to now
            SystemTime.SetDateTime(testStartDate);
            DateTime oldSavedDate = SystemTime.Now();

            string oldSavedDateString = oldSavedDate.Date.ToString("yyyy-MM-dd");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, oldSavedDateString);

            // Act
            SystemTime.SetDateTime(oldSavedDate.AddDays(2)); // some time passes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.Date;
            var actual = preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300));
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
        }

        [Fact]
        public void MigrateSuccessfullyFromArabicToArabic_CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            preferences.Clear();

            // Set time to now
            SystemTime.SetDateTime(testStartDate);
            DateTime oldSavedDate = SystemTime.Now();
            string oldSavedDateString = oldSavedDate.Date.ToString("yyyy-MM-dd");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, oldSavedDateString);


            // Act
            DateTime timeOfMigration = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(timeOfMigration); // some time passes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.Date;
            var actual = preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
        }

        [Fact]
        public void MigrateToFutureDateFromThaiToDanish_CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            preferences.Clear();

            // Set time to now
            SystemTime.SetDateTime(testStartDate);
            DateTime oldSavedDate = SystemTime.Now();
            string oldSavedDateString = oldSavedDate.Date.ToString("yyyy-MM-dd");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, oldSavedDateString);


            // Act
            DateTime migrationTime = oldSavedDate.AddDays(2);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.Date;
            var actual = preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.True(expected < actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
        }



        [Fact]
        public void Update_MIGRATION_COUNT_To1()
        {
            //Arrange
            preferences.Clear();

            // Act
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

            // Assert
            var expected = 1;
            var actual = preferences.Get(PreferencesKeys.MIGRATION_COUNT,0);
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
        }

        [Fact]
        public void DoesNotPerformMigrationFrom1to1()
        {
            //Arrange
            preferences.Clear();
            preferences.Set(PreferencesKeys.MIGRATION_COUNT, 1);

            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, "value");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, "value");

            // Act
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 1;
            migrationService.Migrate();

           
            // Assert
            Assert.True(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
            Assert.True(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF));

        }

        [Fact]
        public void MigrateSucessfullyFrom1To2_DateOfTheLastSuccessfulPullFromOld_CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF_ToNew_LAST_PULL_KEYS_SUCCEEDED_DATE_TIME()
        {
            //Arrange
            preferences.Clear();
            SystemTime.SetDateTime(testStartDate);

            DateTime oldSavedDateInPreferences = SystemTime.Now().Date;
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, oldSavedDateInPreferences);
            preferences.Set(PreferencesKeys.MIGRATION_COUNT, 1);

            // Act
            DateTime migrationTime = oldSavedDateInPreferences.AddDays(3);
            SystemTime.SetDateTime(migrationTime);
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 2;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDateInPreferences.TrimMilliseconds();
            var actual = preferences.Get(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DOWNLOAD_DAY_BATCH_PREF));
        }

        [Fact]
        public void MigrateSucessfullyFrom0To2_DateOfTheLastSuccessfulPullFromOld_LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF_ToNew_LAST_PULL_KEYS_SUCCEEDED_DATE_TIME()
        {
            //Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            preferences.Clear();
            SystemTime.SetDateTime(testStartDate);

            DateTime oldSavedDate = SystemTime.Now();

            string oldSavedDateString = oldSavedDate.Date.ToString("yyyy-MM-dd");
            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, oldSavedDateString);

            // Act
            DateTime migrationTime = oldSavedDate.AddDays(3);
            SystemTime.SetDateTime(migrationTime);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 2;
            migrationService.Migrate();

            // Assert
            var expected = oldSavedDate.Date;
            var actual = preferences.Get(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, SystemTime.Now().AddDays(-300)).TrimMilliseconds();
            Assert.Equal(expected, actual);
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.CURRENT_DOWNLOAD_DAY_BATCH_PREF));
        }

        public void DoesNotPerformMigrationFrom2to2()
        {
            //Arrange
            preferences.Clear();
            preferences.Set(PreferencesKeys.MIGRATION_COUNT, 2);

            preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, "value");
            preferences.Set(PreferencesKeys.CURRENT_DOWNLOAD_DAY_BATCH_PREF, "value");
            preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, "value");

            // Act
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 2;
            migrationService.Migrate();


            // Assert
            Assert.True(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF));
            Assert.True(preferences.ContainsKey(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF));
            Assert.True(preferences.ContainsKey(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF));
            Assert.False(preferences.ContainsKey(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME));
        }
    }
}

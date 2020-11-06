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

        [Fact]
        public void DoesNotPerformMigrationFrom0to0()
        {
            //Arrange
            preferences.Clear();

            // Act
            MigrationService migrationService = new MigrationService();
            migrationService.CurrentMigrationVersion = 0;
            migrationService.Migrate();

            // Assert
            Assert.Equal(0, preferences.Get(PreferencesKeys.MIGRATION_COUNT, 0));
        }
    }
}

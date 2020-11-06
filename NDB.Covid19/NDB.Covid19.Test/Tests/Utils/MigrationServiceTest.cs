using System;
using CommonServiceLocator;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
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

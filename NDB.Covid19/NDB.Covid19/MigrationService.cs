using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19
{
    public class MigrationService
    {
        public int CurrentMigrationVersion = 0; // No need for migration, manually increment if migration is needed.
        private static IPreferences _preferences => CommonServiceLocator.ServiceLocator.Current.GetInstance<IPreferences>();

        public MigrationService()
        {
        }

        public void Migrate()
        {
            int LastVersionMigratedTo = LocalPreferencesHelper.MigrationCount;

            while (LastVersionMigratedTo < CurrentMigrationVersion)
            {
                DoTheMigrationToVersion(LastVersionMigratedTo + 1);
                LocalPreferencesHelper.MigrationCount = ++LastVersionMigratedTo;
            }
        }

        private void DoTheMigrationToVersion(int versionToMigrateTo)
        {
            switch (versionToMigrateTo)
            {
                case 1:
                // Add method to call for migration, e.g.,
                // MigrateToVersion1()
                default:
                    break;
            }
        }

        //Add migration code to run below. Use migrateToVersionX signature.
        private void MigrateToVersion1()
        {
        }
    }
}

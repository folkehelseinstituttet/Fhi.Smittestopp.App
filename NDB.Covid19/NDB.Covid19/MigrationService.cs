using System;
using NDB.Covid19.Config;
using NDB.Covid19.ExposureNotification.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19
{
    public class MigrationService
    {
        public int CurrentMigrationVersion = 2; // Increment this when migration needs to be run
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
                    MigrateToVersion1();
                    break;
                case 2:
                    MigrateToVersion2();
                    break;
                // Add more cases to call the functions if needed here.
                default:
                    break;
            }
        }

        //Add migration code to run below. Use migrateToVersionX signature.
        private void MigrateToVersion1()
        {
            DateTime newValue;

            try
            {
                string stringValue = _preferences.Get(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF, "defaultvalue");
                DateTime dateTimeValue = DateTime.Parse(stringValue);
                newValue = dateTimeValue;
            }
            catch (Exception)
            {
                // Conversion failed because of wrong format
                newValue = SystemTime.Now().AddDays(-14);
            }

            _preferences.Set(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF, newValue);
            _preferences.Remove(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_PREF);

            try
            {
                string stringValue = _preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF, "defaultvalue");
                DateTime dateTimeValue = DateTime.Parse(stringValue);
                newValue = dateTimeValue;

            }
            catch (Exception)
            {
                // Conversion failed because of wrong format
                newValue = SystemTime.Now().Date;
            }

            _preferences.Set(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, newValue);
            _preferences.Remove(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_PREF);
        }

        private void MigrateToVersion2()
        {
            DateTime lastDatePulledSucceeded = _preferences.Get(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF, DateTime.MinValue);

            if (lastDatePulledSucceeded != DateTime.MinValue)
            {
                _preferences.Set(PreferencesKeys.LAST_PULL_KEYS_SUCCEEDED_DATE_TIME, lastDatePulledSucceeded);
            }

            _preferences.Remove(PreferencesKeys.LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF);
            _preferences.Remove(PreferencesKeys.CURRENT_DAY_TO_DOWNLOAD_KEYS_FOR_UTC_DATETIME_PREF);
            _preferences.Remove(PreferencesKeys.CURRENT_DOWNLOAD_DAY_BATCH_PREF);
        }
    }
}

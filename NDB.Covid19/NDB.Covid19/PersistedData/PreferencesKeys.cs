namespace NDB.Covid19.PersistedData
{
    public class PreferencesKeys
    {
        public static readonly string MIGRATION_COUNT = "MIGRATION_COUNT";

        public static readonly string MESSAGES_LAST_UPDATED_PREF = "MESSAGES_LAST_UPDATED_PREF";

        public static readonly string IS_ONBOARDING_COMPLETED_PREF = "isOnboardingCompleted";
        public static readonly string IS_ONBOARDING_COUNTRIES_COMPLETED_PREF = "isOnboardingCountriesCompleted";

        public static readonly string USE_MOBILE_DATA_PREF = "USE_MOBILE_DATA_PREF";

        // The difference between LAST_DOWNLOAD_ZIPS_* and LAST_PULLED_KEYS_SUCCEEDED_DATE_TIME is that the first one is the date of the last try and
        // the second one is the last day when it succeeded
        public static readonly string LAST_PULL_KEYS_SUCCEEDED_DATE_TIME = "LAST_PULL_KEYS_SUCCEEDED_DATE_TIME";

        public static readonly string LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED = "LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED";
        public static readonly string LAST_PULLED_BATCH_NUMBER_SUBMITTED = "LAST_PULLED_BATCH_NUMBER_SUBMITTED";
        public static readonly string LAST_PULLED_BATCH_TYPE = "LAST_PULLED_BATCH_TYPE";
        
        public static readonly string APP_LANGUAGE = "APP_LANGUAGE";

        public static readonly string DEV_TOOLS_PULL_KEYS_HISTORY = "DEV_TOOLS_PULL_KEYS_HISTORY";
        public static readonly string DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD = "DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD";
        
        public static readonly string TERMS_NOTIFICATION_WAS_SENT = "TERMS_NOTIFICATION_WAS_SENT";
        
        public static readonly string LAST_MESSAGE_DATE_TIME = "LAST_MESSAGE_DATE_TIME";

        public static readonly string LAST_PERMISSIONS_NOTIFICATION_DATE_TIME = "LAST_PERMISSIONS_NOTIFICATION_DATE_TIME";
        public static readonly string EXPOSURE_TIME_THRESHOLD = "EXPOSURE_TIME_THRESHOLD";
        public static readonly string LOW_ATTENUATION_DURATION_MULTIPLIER = "LOW_ATTENUATION_DURATION_MULTIPLIER";
        public static readonly string MIDDLE_ATTENUATION_DURATION_MULTIPLIER = "MIDDLE_ATTENUATION_DURATION_MULTIPLIER";
        public static readonly string HIGH_ATTENUATION_DURATION_MULTIPLIER = "HIGH_ATTENUATION_DURATION_MULTIPLIER";
        public static readonly string COUNTRY_CONSENTS_GIVEN = "COUNTRTY_CONSENTS_GIVEN";

    }
}

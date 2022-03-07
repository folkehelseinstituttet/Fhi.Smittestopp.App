using System;

namespace NDB.Covid19.Configuration
{
    public class Conf
    {
#if APPCENTER
        public static readonly string APPCENTER_DIAGNOSTICS_TOKEN = "";
#endif
        public static readonly string BASE_URL = "http://localhost:9095/";
        public static string AUTHORIZATION_HEADER => "INJECTED_IN_APP_CENTER_DURING_BUILD";
        public static string USER_AGENT_HEADER => "INJECTED_IN_APP_CENTER_DURING_BUILD";

#if RELEASE
        public static bool UseDeveloperTools => false;
#else
        public static bool UseDeveloperTools => true;
#endif

        // Minimum hours between pulling keys
        // Different intervals are automatically injected for test and production builds by App Center
        public static readonly TimeSpan FETCH_MIN_HOURS_BETWEEN_PULL = TimeSpan.FromMinutes(2);

        public static readonly int APIVersion = 3;

        //It takes around 25 seconds to download a zip file with 100.000 keys if you have 3G connection.
        //The timeout value takes this into consideration.
        public static int DEFAULT_TIMEOUT_SERVICECALLS_SECONDS => 40;
        public static string DEFAULT_LANGUAGE = "nn"; //In case the device is set to use an unsupported language
        public static string[] SUPPORTED_LANGUAGES = new string[] { "en", "nb", "nn", "pl", "so", "ti", "ar", "ur", "lt" };

        public static int MESSAGE_RETENTION_TIME_IN_MINUTES_SHORT => 15; 
        public static int MESSAGE_RETENTION_TIME_IN_MINUTES_LONG => 14 * 24 * 60;
        public static int MAX_MESSAGE_RETENTION_TIME_IN_MINUTES = MESSAGE_RETENTION_TIME_IN_MINUTES_LONG;
        public static int HOURS_UNTIL_RESEND_MESSAGES = 48;
        public static int HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND_BEGIN = 8;
        public static int HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND_END = 22;

        // Year replacement in case of wrong of no DateTime on device
        public static DateTime DATE_TIME_REPLACEMENT = new DateTime(2021, 1, 1);

        // --- Urls ---
        public static string URL_PREFIX => $"{BASE_URL}v{APIVersion}/";
        public static string URL_LOG_MESSAGE => URL_PREFIX + "logging/logMessages";
        public static string URL_PUT_UPLOAD_DIAGNOSIS_KEYS => URL_PREFIX + "diagnostickeys";
        public static string URL_GET_EXPOSURE_CONFIGURATION => URL_PREFIX + "diagnostickeys/exposureconfiguration";
        public static string URL_GET_DAILY_SUMMARY_CONFIGURATION => URL_PREFIX + "diagnostickeys/dailysummaryconfiguration";
        public static string URL_GET_DIAGNOSIS_KEYS => URL_PREFIX + "diagnostickeys";
        public static string URL_GET_COUNTRY_LIST => URL_PREFIX + "countries";
        public static string URL_GET_FHI_DATA => URL_PREFIX + "covidstatistics";
        public static string URL_GET_IMPORTANT_MESSAGE => URL_PREFIX + "importantinfo";

        // --- Gateway Stub ---
        public static string URL_GATEWAY_STUB_UPLOAD => URL_PREFIX + "diagnosiskeys/upload";

        // --- ExposureNotificationHandler variables ---

        // Repeat interval of the periodic work request that pulls on Android
        // (The minimum is 15 minutes: PeriodicWorkRequest.MinPeriodicIntervalMillis is 900000)
        public static readonly TimeSpan BACKGROUND_FETCH_REPEAT_INTERVAL_ANDROID = TimeSpan.FromHours(4);
        
        // For ZipDownloader.RetryIfInvalidResponse
        public static readonly int FETCH_MAX_ATTEMPTS = 1;

        public static readonly Tuple<int, int>[] DAYS_SINCE_ONSET_FOR_TRANSMISSION_RISK_CALCULATION =
        {
            // "-" represents days BEFORE the symptom onset, positive number is for days AFTER symptom onset 
            Tuple.Create(int.MinValue, int.MaxValue), // For array index 1 (Represents Lowest risk, should be set for all the keys)
            Tuple.Create(-3, -3),                     // For array index 2
            Tuple.Create(-2, -2),                     // For array index 3
            Tuple.Create(-1, 2),                      // For array index 4
            Tuple.Create(3, 6),                       // For array index 5
            Tuple.Create(7, 8),                       // For array index 6
            Tuple.Create(9, 10),                      // For array index 7
            Tuple.Create(11, 12)                      // For array index 8
        };

        // Parameters for setting meaningful attenuation bucket weights
        // Multiplier (weight) of each minute in the low attenuation bucket
        public static readonly double LOW_ATTENUATION_DURATION_MULTIPLIER = 1.0;
        // Multiplier (weight) of each minute in the middle attenuation bucket
        public static readonly double MIDDLE_ATTENUATION_DURATION_MULTIPLIER = 0.5;
        // Multiplier (weight) of each minute in the high attenuation bucket
        public static readonly double HIGH_ATTENUATION_DURATION_MULTIPLIER = 0.0;
        // Meaningful exposure time threshold towards which the weighted time from the summary is evaluated
        public static readonly double EXPOSURE_TIME_THRESHOLD = 15.0;

        // EN API v2: Default score sum threshold for DailySummaryReport
        // If score sum for a particular day is higher than this threshold,
        // the app should generate Exposure Notification. The actual threshold is fetched together
        // with DailySummaryConfiguration on each pull of the keys from server
        public static readonly double SCORE_SUM_THRESHOLD = 780;

        public static readonly string[] SUPPORTED_REGIONS = { "no" }; 

        public static string DB_NAME => "Smittestopp1.db3";

        //Links for Apples app store and Googles Play store
        public static string GooglePlayAppLink = "https://play.google.com/store/apps/details?id=no.fhi.smittestopp_exposure_notification";
        public static string IOSAppstoreAppLink = "itms-apps://itunes.apple.com/app/1540967575";
    }
}

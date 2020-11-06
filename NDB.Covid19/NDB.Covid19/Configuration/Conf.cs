using System;

namespace NDB.Covid19.Configuration
{
    public class Conf
    {
#if TEST
        public static readonly string BaseUrl = "BaseUrl_TEST"; // TODO
        public static string AuthorizationHeader => "AuthorizationHeader"; // TODO
        public static bool UseDeveloperTools => true;
        // Minimum hours between pulling keys
        public static readonly TimeSpan FETCH_MIN_HOURS_BETWEEN_PULL = TimeSpan.FromMinutes(0);

#elif UNIT_TEST
        public static readonly string BaseUrl = "http://localhost:9095/"; // TODO
        public static string AuthorizationHeader => "AuthorizationHeader"; // TODO
        public static bool UseDeveloperTools => true;
        public static readonly TimeSpan FETCH_MIN_HOURS_BETWEEN_PULL = TimeSpan.FromMinutes(120);

#elif APPCENTER
        //TEST
        public static readonly string BaseUrl = "BaseUrl_TEST"; // TODO
        public static string AuthorizationHeader => "AuthorizationHeader"; // TODO

        //PREPROD
        //public static readonly string BaseUrl = "BaseUrl_PREPROD"; // TODO
        //public static string AuthorizationHeader => "AuthorizationHeader"; // TODO

        //Both
        public static bool UseDeveloperTools => true;
        public static readonly TimeSpan FETCH_MIN_HOURS_BETWEEN_PULL = TimeSpan.FromMinutes(2);

#elif RELEASE
        public static readonly string BaseUrl = "BaseUrl_PROD"; // TODO
        public static string AuthorizationHeader => "AuthorizationHeader"; // TODO
        public static bool UseDeveloperTools => false;
        public static readonly TimeSpan FETCH_MIN_HOURS_BETWEEN_PULL = TimeSpan.FromMinutes(120);
#endif

        public static readonly int APIVersion = 2;

        //It takes around 25 seconds to download a zip file with 100.000 keys if you have 3G connection.
        //The timeout value takes this into consideration.
        public static int DEFAULT_TIMEOUT_SERVICECALLS_SECONDS => 40;
        public static string DEFAULT_LANGUAGE = "nb"; //In case the device is set to use an unsupported language
        public static string[] SUPPORTED_LANGUAGES = new string[] { "en", "nb", "nn" }; // TODO

        public static int MESSAGE_RETENTION_TIME_IN_MINUTES_SHORT => 15; 
        public static int MESSAGE_RETENTION_TIME_IN_MINUTES_LONG => 14 * 24 * 60;
        public static int MAX_MESSAGE_RETENTION_TIME_IN_MINUTES = MESSAGE_RETENTION_TIME_IN_MINUTES_LONG;
        public static int HOURS_UNTIL_RESEND_MESSAGES = 48;
        public static int HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND = 21;

        // --- Urls ---
        public static string URL_PREFIX => $"{BaseUrl}v{APIVersion}/";
        public static string URL_LOG_MESSAGE => URL_PREFIX + "logging/logMessages";
        public static string URL_PUT_UPLOAD_DIAGNOSIS_KEYS => URL_PREFIX + "diagnostickeys";
        public static string URL_GET_EXPOSURE_CONFIGURATION => URL_PREFIX + "diagnostickeys/exposureconfiguration";
        public static string URL_GET_DIAGNOSIS_KEYS => URL_PREFIX + "diagnostickeys";
        public static string URL_GET_COUNTRY_LIST => URL_PREFIX + "countries";

        // --- Gateway Stub ---
        public static string URL_GATEWAY_STUB_UPLOAD => URL_PREFIX + "diagnosiskeys/upload";

        // --- ExposureNotificationHandler variables ---

        // Repeat interval of the periodic work request that pulls on Android
        // (The minimum is 15 minutes: PeriodicWorkRequest.MinPeriodicIntervalMillis is 900000)
        public static readonly TimeSpan BACKGROUND_FETCH_REPEAT_INTERVAL_ANDROID = TimeSpan.FromHours(1);
        
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

        // Any ExposureDetectionSummary whose MaximumRiskScore is >= this value will be counted as having "high infection risk"
        public static readonly int RISK_SCORE_THRESHOLD_FOR_HIGH_RISK = 512;

        // Parameters for setting meaningful attenuation bucket weights
        // Multiplier (weight) of each minute in the low attenuation bucket
        public static readonly double LOW_ATTENUATION_DURATION_MULTIPLIER = 1.0;
        // Multiplier (weight) of each minute in the middle attenuation bucket
        public static readonly double MIDDLE_ATTENUATION_DURATION_MULTIPLIER = 0.5;
        // Multiplier (weight) of each minute in the high attenuation bucket
        public static readonly double HIGH_ATTENUATION_DURATION_MULTIPLIER = 0.0;
        // Meaningful exposure time threshold towards which the weighted time from the summary is evaluated
        public static readonly double EXPOSURE_TIME_THRESHOLD = 15.0;

        public static readonly string[] SUPPORTED_REGIONS = { "no" }; 

        public static string DB_NAME => "Smittestopp1.db3";

        //Links for Apples app store and Googles Play store
        public static string GooglePlayAppLink = ""; // TODO
        public static string IOSAppstoreAppLink = ""; // TODO

    }
}

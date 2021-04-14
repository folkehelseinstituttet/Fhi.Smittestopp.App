using System.Collections.Generic;
using System.Linq;

namespace NDB.Covid19.PersistedData.SecureStorage
{
    // All SecureStorage keys should be accessed as a static string field in this class because
    // CrossSecureStorage doesn't have a RemoveAll function. DeviceUtils.CleanDataFromDevice
    // needs to be able to delete all SecureStorage values by getting all keys with SecureStorageKeys.GetAllKeys()
    public static class SecureStorageKeys
    {
        public static readonly string LAST_HIGH_RISK_ALERT_UTC_KEY = "LAST_HIGH_RISK_ALERT_UTC_KEY";
        public static readonly string LAST_SENT_NOTIFICATION_UTC_KEY = "LAST_SENT_NOTIFICATION_UTC_KEY";
        public static readonly string LAST_SUMMARY_KEY = "LAST_SUMMARY_KEY";
        public static readonly string DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY = "DAILY_SUMMARIES_OVER_THRESHOLD_TIMESTAMP_KEY";

        public static IEnumerable<string> GetAllKeysForCleaningDevice()
        {
            IEnumerable<object> fieldValues = typeof(SecureStorageKeys).GetFields().Select(field => field.GetValue(typeof(SecureStorageKeys)));
            IEnumerable<string> stringFieldValues = fieldValues
                .Where(field => field.GetType() == typeof(string))
                .Select(field => field.ToString());
            return stringFieldValues;
        }
    }
}

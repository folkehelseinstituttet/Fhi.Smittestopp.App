using System;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
#if !UNIT_TEST
using Xamarin.Essentials;
#endif

namespace NDB.Covid19.Models.Logging
{
    public class LogDeviceDetails
    {
        public LogSeverity Severity { get; private set; }
        public string Description { get; private set; }
        public DateTime ReportedTime { get; private set; }
        public int ApiVersion { get; private set; }
        public string BuildVersion { get; private set; }
        public string BuildNumber { get; private set; }
        public string DeviceOSVersion { get; private set; }
        public string AdditionalInfo { get; set; }

        public LogDeviceDetails(LogSeverity severity, string logMessage, string additionalInfo = "")
        {
            Severity = severity;
            Description = Anonymizer.RedactText(logMessage);

            DateTime reportedUtcDateTime = SystemTime.Now().ToUniversalTime();
            DateTime lastNTPDateTime = LocalPreferencesHelper.LastNTPUtcDateTime;
            ReportedTime = reportedUtcDateTime == null ||
                           (lastNTPDateTime - reportedUtcDateTime).Duration().Days >= 365 * 2
                ? LocalPreferencesHelper.LastNTPUtcDateTime
                : reportedUtcDateTime;
            
            ApiVersion = Conf.APIVersion;

            string addInfoPostfix = ServiceLocator.Current.GetInstance<IApiDataHelper>().GetBackGroundServicVersionLogString();
            AdditionalInfo = Anonymizer.RedactText(additionalInfo) + addInfoPostfix;

#if UNIT_TEST
            BuildNumber = "23";
            BuildVersion = "1.1";
            DeviceOSVersion = "13.4";
#else
            BuildNumber = AppInfo.BuildString;
            BuildVersion = AppInfo.VersionString;
            DeviceOSVersion = DeviceInfo.VersionString;
#endif

        }
    }
}

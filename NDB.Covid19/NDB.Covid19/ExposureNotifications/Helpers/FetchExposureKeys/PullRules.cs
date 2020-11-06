using System;
using System.Linq;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using Xamarin.Essentials;

namespace NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys
{
    /// <summary>
    /// Rules for when to pull keys and check for exposures and when not to.
    /// This method is called approx. every 2 hours in the background if the EN API is enabled.
    /// </summary>
    public class PullRules
    {
        IDeveloperToolsService _developerTools => ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
        IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        /// <summary>
        /// Checks if pulling keys should be aborted due to a set of business rules
        /// </summary>
        public bool ShouldAbortPull()
        {
            string logPrefix = $"{nameof(PullRules)}.{nameof(ShouldAbortPull)}: ";

            if (!LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled() && !ConnectivityHelper.ConnectionProfiles.Contains(ConnectionProfile.WiFi) )
            {
                return PrepareAbortMessage(logPrefix,
                    "Pull aborted. Mobile connectivity has been disabled in general settings and WiFi connection is not available. ",
                    $"Last pull: {LocalPreferencesHelper.GetLastPullKeysSucceededDateTime()}");
            }

            if (LastDownloadZipsTooRecent())
            {
                return PrepareAbortMessage(logPrefix,
                    "Pull aborted. The last time we ran DownloadZips was too recent. ",
                    $"Last pull: {LocalPreferencesHelper.GetLastPullKeysSucceededDateTime()}");
            }

            return false;
        }

        private bool PrepareAbortMessage(string logPrefix, string msg, string additionalData = null)
        {
            string errorMsg = logPrefix + msg;
            if (additionalData != null)
            {
                errorMsg += additionalData;
            }
            _developerTools.AddToPullHistoryRecord(errorMsg);
            LogUtils.LogMessage(Enums.LogSeverity.WARNING, errorMsg);
            return true;
        }

        /// <summary>
        /// Return true if the last call to DownloadZips was < FETCH_MIN_HOURS_BETWEEN_PULL hours ago
        /// </summary>
        public bool LastDownloadZipsTooRecent()
        {
            DateTime lastDownloadZipsDateTime = LocalPreferencesHelper.GetLastPullKeysSucceededDateTime();
            if (lastDownloadZipsDateTime.Equals(DateTime.MinValue))
            {
                lastDownloadZipsDateTime = DateTime.UtcNow.AddDays(-14).Date;
            }
            TimeSpan timeSinceLastDownloadZips = DateTime.UtcNow - lastDownloadZipsDateTime;

            // If the datetime is in the future (which is posssible if the device's time was set manually)
            // we should return false in order to reset LAST_DOWNLOAD_ZIPS_CALL_UTC_DATETIME_PREF
            if (lastDownloadZipsDateTime > DateTime.UtcNow)
            {
                return false;
            }
            return timeSinceLastDownloadZips < Conf.FETCH_MIN_HOURS_BETWEEN_PULL;
        }
    }
}

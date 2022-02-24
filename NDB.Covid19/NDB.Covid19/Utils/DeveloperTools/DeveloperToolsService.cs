using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ProtoModels;
using Xamarin.ExposureNotifications;
using TemporaryExposureKey = NDB.Covid19.ProtoModels.TemporaryExposureKey;

namespace NDB.Covid19.Utils.DeveloperTools
{

    public sealed class DeveloperToolsService : IDeveloperToolsService
    {
        // These Preferences keys will contain stuff that gets stored by ExposureNotificationHandler sub-functions in
        // order to be read on Developer Tools for debugging purposes.
        //
        // Only read/write to these Preferences keys if not RELEASE.
        public static readonly string DEV_TOOLS_LAST_PROVIDED_FILES_PREF = "DEV_TOOLS_LAST_PROVIDED_FILES_PREF";
        public static readonly string DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF = "DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF";
        public static readonly string DEV_TOOLS_LAST_EXPOSURE_INFOS_PREF = "DEV_TOOLS_LAST_EXPOSURE_INFOS_PREF";
        public static readonly string DEV_TOOLS_LAST_KEY_UPLOAD_INFO = "LastKeyUploadInfo";
        public static readonly string DEV_TOOLS_LAST_USED_CONFIGURATION = "LastUsedConfiguration";
        public static readonly string DEV_TOOLS_LAST_EXPOSURE_WINDOWS_PREF = "DEV_TOOLS_LAST_EXPOSURE_WINDOWS_PREF";
        public static readonly string DEV_TOOLS_LAST_DAILY_SUMMARIES_PREF = "DEV_TOOLS_LAST_DAILY_SUMMARIES_PREF";
        public static readonly string DEV_TOOLS_LAST_FETCHED_IMPORTANT_MESSAGE_PREF = "DEV_TOOLS_LAST_FETCHED_IMPORTANT_MESSAGE_PREF";
        private static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();

        /// <summary>
        /// Information about the last upload of diagnosis keys.
        /// </summary>
        public string LastKeyUploadInfo { get => _preferences.Get(DEV_TOOLS_LAST_KEY_UPLOAD_INFO, ""); set { _preferences.Set(DEV_TOOLS_LAST_KEY_UPLOAD_INFO, value); } }

        /// <summary>
        /// Contains information about the last used configuration,
        /// not what the network call returns,
        /// but what was actually used in the last pull operation.
        /// </summary>
        public string LastUsedConfiguration { get => _preferences.Get(DEV_TOOLS_LAST_USED_CONFIGURATION, ""); set { _preferences.Set(DEV_TOOLS_LAST_USED_CONFIGURATION, value); } }



        /// <summary>
        /// Resets all the fields to an empty string: ""
        /// </summary>
        public void ClearAllFields()
        {
            _preferences.Set(DEV_TOOLS_LAST_KEY_UPLOAD_INFO, "");
            _preferences.Set(DEV_TOOLS_LAST_USED_CONFIGURATION, "");
            _preferences.Set(DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF, "");
            _preferences.Set(DEV_TOOLS_LAST_EXPOSURE_INFOS_PREF, "");
            _preferences.Set(DEV_TOOLS_LAST_PROVIDED_FILES_PREF, "");
            _preferences.Set(DEV_TOOLS_LAST_EXPOSURE_WINDOWS_PREF, "");
            _preferences.Set(DEV_TOOLS_LAST_DAILY_SUMMARIES_PREF, "");
        }

        /// <summary>
        /// Saving exposure info for testing purposes.
        /// </summary>
        ///
        public bool ShouldSaveExposureInfo { get => _preferences.Get(DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF, false); set { _preferences.Set(DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF, value); } }

        public string LastProvidedFilesPref { get => _preferences.Get(DEV_TOOLS_LAST_PROVIDED_FILES_PREF, ""); set { _preferences.Set(DEV_TOOLS_LAST_PROVIDED_FILES_PREF, value); } }
        public string PersistedExposureInfo { get => _preferences.Get(DEV_TOOLS_LAST_EXPOSURE_INFOS_PREF, ""); set { _preferences.Set(DEV_TOOLS_LAST_EXPOSURE_INFOS_PREF, value); } }
        public string PersistedExposureWindows { get => _preferences.Get(DEV_TOOLS_LAST_EXPOSURE_WINDOWS_PREF, ""); set { _preferences.Set(DEV_TOOLS_LAST_EXPOSURE_WINDOWS_PREF, value); } }
        public string PersistedDailySummaries { get => _preferences.Get(DEV_TOOLS_LAST_DAILY_SUMMARIES_PREF, ""); set { _preferences.Set(DEV_TOOLS_LAST_DAILY_SUMMARIES_PREF, value); } }

        /// <summary>
        /// Store response of previously fetched ImportantMessage
        /// </summary>
        public string LastFetchedImportantMessage
        {
            get => _preferences.Get(DEV_TOOLS_LAST_FETCHED_IMPORTANT_MESSAGE_PREF, "");
            set
            {
                _preferences.Set(DEV_TOOLS_LAST_FETCHED_IMPORTANT_MESSAGE_PREF, value);
            }
        }

        // Stores a nice string to Preferences, which shows the content of the files last provided to the EN API,
        // so that this can be displayed on Developer Tools
        public void StoreLastProvidedFiles(IEnumerable<string> localFileUrls)
        {
            string allFilesString = $"TEK batch files downloaded at {SystemTime.Now().ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")} UTC:\n#######\n";

            foreach (string localFileUrl in localFileUrls)
            {
                ZipArchive zipArchive = BatchFileHelper.UrlToZipArchive(localFileUrl);
                TemporaryExposureKeyExport temporaryExposureKeyExport = BatchFileHelper.ZipToTemporaryExposureKeyExport(zipArchive);
                string fileString = TemporaryExposureKeyExportToPrettyString(temporaryExposureKeyExport);
                allFilesString += fileString + "\n";
            }

            allFilesString += "#######";
            Debug.WriteLine(allFilesString);
            LastProvidedFilesPref = allFilesString;
        }

        public async Task SaveLastExposureInfos(Func<Task<IEnumerable<ExposureInfo>>> getExposureInfo)
        {
            bool shouldSaveExposureInfos;

            try
            {
                shouldSaveExposureInfos = ShouldSaveExposureInfo;
            }
            catch (Exception e)
            {
                Debug.Print("ExposureDetectedHelper.DevToolsSaveLastExposureInfos: Couldn't fetch DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF from preferences");
                Debug.Print(e.ToString());
                shouldSaveExposureInfos = false;
            }

            //Should never be called if RELEASE conf. DEV_TOOLS_SHOULD_SAVE_EXPOSURE_INFOS_PREF in preferences will never be true. See ENDeveloperToolsViewModel.PullKeysFromServer.
            if (shouldSaveExposureInfos)
            {
                try
                {
                    ShouldSaveExposureInfo = false;

                    IEnumerable<ExposureInfo> exposureInfos = await getExposureInfo();
                    string exposureInfosString = ExposureInfoJsonHelper.ExposureInfosToJson(exposureInfos);
                    PersistedExposureInfo = exposureInfosString;
                }
                catch (Exception e)
                {
                    LogUtils.LogException(LogSeverity.WARNING, e, "ExposureDetectedHelper.DevToolsSaveLastExposureInfos");
                }
            }
        }

        public void SaveExposureWindows(IEnumerable<ExposureWindow> windows)
        {
            try
            {
                string exposureWindowsString = ExposureWindowJsonHelper.ExposureWindowsToJson(windows);
                PersistedExposureWindows = exposureWindowsString;
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e, "ExposureDetectedHelper.DevToolsSaveExposureWindow");
            }
        }

        public void SaveLastDailySummaries(IEnumerable<DailySummary>? summaries)
        {
            try
            {
                string summaryJson = ExposureDailySummaryJsonHelper.ExposureDailySummariesToJson(summaries);
                PersistedDailySummaries = summaryJson;
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e, "ExposureDetectedHelper.DevToolsSaveLastDailySummaries");
            }
        }

        public string TemporaryExposureKeyExportToPrettyString(TemporaryExposureKeyExport temporaryExposureKeyExport)
        {
            try
            {
                string prettyString = "TEK batch, containing these keys:\n";
                prettyString += $"Regions: {temporaryExposureKeyExport.Region}\n";

                string keyPart = "";
                int i = 0;
                foreach (TemporaryExposureKey tek in temporaryExposureKeyExport.Keys)
                {
                    string separator = keyPart == "" ? "--" : "\n--";
                    keyPart += separator;
                    keyPart += $"[TemporaryExposureKey with KeyData.ToBase64()={tek.KeyData.ToBase64()}, " +
                        $"<In DB format: {EncodingUtils.ConvertByteArrayToString(tek.KeyData.ToByteArray())}> " +
                        $"RollingStartIntervalNumber={DateTimeOffset.FromUnixTimeSeconds(tek.RollingStartIntervalNumber * (60 * 10)).UtcDateTime.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")} UTC and " +
                        $"RollingPeriod={tek.RollingPeriod * 10} minutes], " +
                        $"ReportType={tek.ReportType}, " +
                        $"DaysSinceOnsetOfSymptoms={tek.DaysSinceOnsetOfSymptoms}, " +
                        $"[DEPRECATED]TransmissionRiskLevel={tek.TransmissionRiskLevel}";

                    i++;
                    if (i == 200)
                        break;
                }

                prettyString += keyPart;

                return prettyString;
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "DeveloperToolsService.TemporaryExposureKeyExportToPrettyString");
                return "";
            }
        }

        public string AllPullHistory { get => _preferences.Get(PreferencesKeys.DEV_TOOLS_PULL_KEYS_HISTORY, ""); set { _preferences.Set(PreferencesKeys.DEV_TOOLS_PULL_KEYS_HISTORY, value); } }
        public string LastPullHistory { get => _preferences.Get(PreferencesKeys.DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD, ""); set { _preferences.Set(PreferencesKeys.DEV_TOOLS_PULL_KEYS_HISTORY_LAST_RECORD, value); } }

        public void StartPullHistoryRecord()
        {
            string timeUtc = SystemTime.Now().ToGreGorianUtcString("yyyy-MM-dd HH:mm");
            string historyHeadline = $"Pulled the following keys (batches) at {timeUtc} UTC:";
            Debug.Print(historyHeadline);

            //Reset the Last Pull record because a new pull started.
            LastPullHistory = historyHeadline;

            //Add to total pull history
            string currentPullHistory = AllPullHistory;
            AllPullHistory = string.IsNullOrEmpty(currentPullHistory)
                    ? historyHeadline
                    : currentPullHistory + "\n\n" + historyHeadline;
        }

        public void AddToPullHistoryRecord(string message, string requestUrl = null)
        {
            string appendString = requestUrl == null ?
                $"\n* {message}":
                $"\n* {requestUrl}: {message}";
            AllPullHistory = AllPullHistory + appendString;
            LastPullHistory = LastPullHistory + appendString;
            Debug.Print(appendString);
        }

        public void SaveLastFetchedImportantMessage(ApiResponse response, DateTime dateTime)
        {
            string lastFetchedMessageOutput = "";
            lastFetchedMessageOutput +=
                $"{response.HttpMethod} --> {response.Endpoint}\n" +
                $"Last fetched at: {dateTime.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}\n" +
                $"StatusCode: {response.StatusCode}\n" +
                $"Data: {response.ResponseText}";

            LastFetchedImportantMessage = lastFetchedMessageOutput;
        }
    }
}

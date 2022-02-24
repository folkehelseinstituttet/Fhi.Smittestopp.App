using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NDB.Covid19.Models;
using NDB.Covid19.ProtoModels;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Utils.DeveloperTools
{
    /// <summary>
    /// This class runs in Release mode. If anything is added to DeveloperToolsService, it should also be added to this method.
    /// Consider what should happen in Test mode (DeveloperToolsService) and what should happen in Release mode (ReleaseToolsService).
    /// </summary>
    public class ReleaseToolsService : IDeveloperToolsService
    {
        public ReleaseToolsService()
        {
        }

        //These values are in Test mode saved and fetched from Preferences. In release mode, we will not persist anything.
        public string LastKeyUploadInfo { get => ""; set { } }
        public string LastUsedConfiguration { get => ""; set { } }

        //Don't save exposure info in release mode.
        public bool ShouldSaveExposureInfo { get => false; set { } }

        public string LastProvidedFilesPref { get => ""; set { } }
        public string PersistedExposureInfo { get => ""; set { } }
        public string PersistedExposureWindows { get => ""; set { } }
        public string PersistedDailySummaries { get => ""; set { } }

        public void ClearAllFields()
        {
            //There is nothing to clear in release mode, since nothing is persisted.
        }

        public void StoreLastProvidedFiles(IEnumerable<string> localFileUrls)
        {
            //Dont store any files in release mode.
        }

        public Task SaveLastExposureInfos(Func<Task<IEnumerable<ExposureInfo>>> getExposureInfo)
        {
            return Task.FromResult(true);
        }

        //Not needed without developer tools
        public string TemporaryExposureKeyExportToPrettyString(TemporaryExposureKeyExport temporaryExposureKeyExport)
        {
            return "";
        }

        public string AllPullHistory { get; set; }
        public string LastPullHistory { get; set; }
        public string LastFetchedImportantMessage { get; set; }
        public void StartPullHistoryRecord() { }
        public void AddToPullHistoryRecord(string message, string requestUrl) { }

        public void SaveExposureWindows(IEnumerable<ExposureWindow> windows)
        {
            //Exposure Windows are not saved in release mode for now
        }

        public void SaveLastDailySummaries(IEnumerable<DailySummary>? summaries)
        {
            //Daily Summaries are not saved in release mode for now
        }

        public void SaveLastFetchedImportantMessage(ApiResponse response, DateTime dateTime)
        {
            // Last important message not saved to release for now
        }
    }
}

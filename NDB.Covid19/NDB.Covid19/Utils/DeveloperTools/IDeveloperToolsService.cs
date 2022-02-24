using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NDB.Covid19.Models;
using NDB.Covid19.ProtoModels;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.Utils.DeveloperTools
{
    // When setting build configuration to RELEASE, then ReleaseToolsService is implemented.
    // Otherwise, DeveloperToolsService is implemented
    public interface IDeveloperToolsService
    {

        string LastKeyUploadInfo { get; set; }
        string LastUsedConfiguration { get; set; }
        void ClearAllFields();
        bool ShouldSaveExposureInfo { get; set; }
        string LastProvidedFilesPref { get; set; }
        string PersistedExposureInfo { get; set; }
        string PersistedExposureWindows { get; set; }
        string PersistedDailySummaries { get; set; }
        string LastFetchedImportantMessage { get; set; }

        void StoreLastProvidedFiles(IEnumerable<string> localFileUrls);
        Task SaveLastExposureInfos(Func<Task<IEnumerable<ExposureInfo>>> getExposureInfo);
        void SaveExposureWindows(IEnumerable<ExposureWindow> windows);
        void SaveLastDailySummaries(IEnumerable<DailySummary>? summaries);
        void SaveLastFetchedImportantMessage(ApiResponse response, DateTime dateTime);
        string TemporaryExposureKeyExportToPrettyString(TemporaryExposureKeyExport temporaryExposureKeyExport);

        string LastPullHistory { get; set; }
        string AllPullHistory { get; set; }
        void StartPullHistoryRecord();
        void AddToPullHistoryRecord(string message, string requestUrl = null);

    }
}

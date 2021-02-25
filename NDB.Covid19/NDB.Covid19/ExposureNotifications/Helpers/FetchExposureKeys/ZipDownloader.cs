using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using NDB.Covid19.WebServices.ExposureNotification;

namespace NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys
{
    public class ZipDownloader
    {
        IDeveloperToolsService _developerTools => ServiceLocator.Current.GetInstance<IDeveloperToolsService>();

        private readonly ExposureNotificationWebService _exposureNotificationWebService = new ExposureNotificationWebService();
        private static IPreferences _preferences => ServiceLocator.Current.GetInstance<IPreferences>();
        string _logPrefix = $"{nameof(ZipDownloader)}";

        public static readonly string MoreBatchesExistHeader = "nextBatchExists";
        public static readonly string LastBatchReturnedHeader = "lastBatchReturned";

        /// <summary>
        /// Generates the request to pull keys and fetches the new keys if any.
        /// </summary>
        /// <returns>Paths to the temporary location where the zips are stored</returns>
        public async Task<IEnumerable<string>> DownloadZips(CancellationToken cancellationToken)
        {
            return await PullNewKeys(_exposureNotificationWebService, cancellationToken);
        }

        /// <summary>
        /// Fetches the new keys if any.
        /// </summary>
        /// <returns>Paths to the temporary location where the zips are stored</returns>
        public async Task<IEnumerable<string>> PullNewKeys(ExposureNotificationWebService service, CancellationToken cancellationToken)
        {
            PullKeysParams requestParams = PullKeysParams.GenerateParams();
            LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204 = false;

            List<string> zipLocations = new List<string>();
            bool lastPull = false;
            int? lastBatchReceived = null;
            int? lastReceivedStatusCodeFromRequest = null;

            while (!lastPull)
            {
                string requestUrl = requestParams.ToBatchFileRequest();

                ApiResponse <Stream> response = await service.GetDiagnosisKeys(requestUrl, cancellationToken);
                HttpHeaders headers = response.Headers;
                lastReceivedStatusCodeFromRequest = response.StatusCode;
                bool headersAreValid = true;

                if (response == null || (!response.IsSuccessfull))
                {
                    if (response?.StatusCode == 410)
                    {
                        NotificationsHelper.CreateNotification(NotificationsEnum.ApiDeprecated, 0);
                        string warning = "410 Api was deprecated";
                        _developerTools.AddToPullHistoryRecord(warning, requestUrl);
                        LogUtils.LogMessage(LogSeverity.WARNING, $"{_logPrefix}.{nameof(DownloadZips)}: {warning}");

                    }
                    else
                    {
                        //Failed to fetch new keys due to server error. This is already logged in the webservice.
                        _developerTools.AddToPullHistoryRecord($"{response.StatusCode} Server Error", requestUrl);
                    }
                    break; //Abort pulling
                }

                // If the server says 204: No Content, it means that there were no new keys (I.e. the request batch does not exist)
                if (response.StatusCode == 204)
                {
                    if (requestParams.Date.Date < SystemTime.Now().Date)
                    {
                        //If there were no new keys for a day which is not today, then move on to fetch keys for the next date.
                        requestParams.Date = requestParams.Date.AddDays(1);
                        requestParams.BatchNumber = 1;
                        lastPull = false;
                    }
                    else
                    {
                        //There were no new keys to fetch for today
                        _developerTools.AddToPullHistoryRecord($"204 No Content - No new keys", requestUrl);
                        string warning = $"API {response.Endpoint} returned 204 No Content - No new keys since last pull";
                        LogUtils.LogMessage(LogSeverity.WARNING, $"{_logPrefix}.{nameof(DownloadZips)}: {warning}");
                        lastPull = true;
                    }
                }
                else
                {
                    try
                    {
                        int lastBatchReceivedValue = int.Parse(headers.GetValues(LastBatchReturnedHeader).First());
                        bool moreBatchesExist = bool.Parse(headers.GetValues(MoreBatchesExistHeader).First());

                        //If both headers parse (no exceptions), then save lastBatchValue to be persisted
                        lastBatchReceived = lastBatchReceivedValue;

                        if (moreBatchesExist)
                        {
                            //There are still more batches to fetch for the given date
                            requestParams.BatchNumber = (int)lastBatchReceived + 1;
                            lastPull = false;
                        }
                        else if (requestParams.Date.Date < SystemTime.Now().Date)
                        {
                            //If there were no new keys for a day which is not today, then move on to fetch keys for the next date.
                            requestParams.Date = requestParams.Date.AddDays(1);
                            requestParams.BatchNumber = 1;
                            lastPull = false;
                        }
                        else
                        {
                            //There are no more batches to fetch for today. Try again in some hours.
                            lastPull = true;
                        }
                    }
                    catch (Exception e)
                    {
                        headersAreValid = false;
                        HandleErrorWhenPulling(e, $"Failed to parse {MoreBatchesExistHeader} or {LastBatchReturnedHeader} header.", requestUrl);
                        break; //Abort pulling
                    }
                }

                // Copy the zip stream in the response into a temp file
                if (response.StatusCode == 200 && headersAreValid)
                {
                    try
                    {
                        _developerTools.AddToPullHistoryRecord("200 OK", requestUrl);
                        string tmpFile = Path.Combine(ServiceLocator.Current.GetInstance<IFileSystem>().CacheDirectory, Guid.NewGuid() + ".zip");

                        FileStream tmpFileStream = File.Create(tmpFile);
                        await response.Data.CopyToAsync(tmpFileStream);
                        tmpFileStream.Close();

                        zipLocations.Add(tmpFile);
                    }
                    catch (Exception e)
                    {
                        HandleErrorWhenPulling(e, "Failed to save zip locally", requestUrl);
                        break; //Abort pulling
                    }
                }
            }

            if (zipLocations.Any() && lastBatchReceived != null)
            {
                //Persist the last batch that was fetched, to know which one to fetch next time the background task runs.
                LocalPreferencesHelper.LastPullKeysBatchNumberNotSubmitted = (int) lastBatchReceived;

                //Also save the last batchtype fetched
                LocalPreferencesHelper.LastPulledBatchType = requestParams.BatchType;
            }

            // Edge case for when pulling across multiple days ends up in 204 for the first file
            if (requestParams.Date.Date == SystemTime.Now().Date
                && requestParams.BatchNumber == 1
                && lastReceivedStatusCodeFromRequest == 204)
            {
                LocalPreferencesHelper.DidFirstFileOfTheDayEndedWith204 = true;
            }

            return zipLocations;
        }

        public void HandleErrorWhenPulling(Exception e, string errorMessage, string requestUrl)
        {
            _developerTools.AddToPullHistoryRecord(errorMessage, requestUrl);
            LogUtils.LogException(LogSeverity.ERROR, e, $"{_logPrefix}.{nameof(DownloadZips)}: {errorMessage}");
        }
    }
}

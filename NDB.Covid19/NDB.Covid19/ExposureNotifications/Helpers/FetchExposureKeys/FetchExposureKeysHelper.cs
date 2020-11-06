using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Models.UserDefinedExceptions;
using NDB.Covid19.PersistedData;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;

namespace NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys
{
    public class FetchExposureKeysHelper
    {
        IDeveloperToolsService _developerTools => ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
        static string _logPrefix = $"{nameof(FetchExposureKeysHelper)}";
        private PullRules _pullRules = new PullRules();

        // The purpose of this function is to download .zips from the server and submit them to the EN API with the submitBatches function,
        // but there are cases where nothing should be downloaded and cases where nothing should be submitted
        public async Task FetchExposureKeyBatchFilesFromServerAsync(Func<IEnumerable<string>, Task> submitBatches, CancellationToken cancellationToken)
        {
            _developerTools.StartPullHistoryRecord();

            SendReApproveConsentsNotificationIfNeeded();
            ResendMessageIfNeeded();

            if (_pullRules.ShouldAbortPull())
            {
                return;
            }

            IEnumerable<string> zipsLocation = await new ZipDownloader().DownloadZips(cancellationToken);

            if (await SubmitZips(zipsLocation, submitBatches))
            {
                LocalPreferencesHelper.UpdateLastPullKeysSucceededDateTime();
                _developerTools.AddToPullHistoryRecord("Zips were successfully submitted to EN API.");
            }
            DeleteZips(zipsLocation);
        }

        private async void ResendMessageIfNeeded()
        {
            DateTime nowLocal = TimeZoneInfo.ConvertTimeFromUtc(SystemTime.Now(), TimeZoneInfo.Local);
            DateTime todayUtc = SystemTime.Now().Date;
            DateTime lastReceivedMessageDateTimeUtc =
                MessageUtils.GetDateTimeFromSecureStorageForKey(SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY, nameof(ResendMessageIfNeeded));
            DateTime lastReceivedMessageDateTimeLocal = lastReceivedMessageDateTimeUtc.ToLocalTime();

            if (lastReceivedMessageDateTimeUtc < todayUtc &&
                nowLocal.Date.Subtract(lastReceivedMessageDateTimeLocal.Date).TotalHours >= Conf.HOURS_UNTIL_RESEND_MESSAGES)
            {
                if (nowLocal.Hour >= Conf.HOUR_WHEN_MESSAGE_SHOULD_BE_RESEND)
                {
                    List<MessageSQLiteModel> unreadMessages = await MessageUtils.GetAllUnreadMessages();
                    List<MessageSQLiteModel> unreadMessagesNotOlderThanMsgRetentionTime =
                        unreadMessages.FindAll(message =>
                            {
                                double totalMinutes = SystemTime.Now().Subtract(message.TimeStamp).TotalMinutes;
                                return totalMinutes <
                                       Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES;
                            })
                            .ToList();

                    if (unreadMessagesNotOlderThanMsgRetentionTime.Count > 0)
                    {
                        NotificationsHelper.CreateNotification(NotificationsEnum.NewMessageReceived, 0);
                        MessageUtils.SaveDateTimeToSecureStorageForKey(
                            SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY,
                            SystemTime.Now(),
                            nameof(ResendMessageIfNeeded));

                    }
                }
            }
        }

        private void SendReApproveConsentsNotificationIfNeeded()
        {
            if (OnboardingStatusHelper.Status == OnboardingStatus.OnlyMainOnboardingCompleted &&
                !LocalPreferencesHelper.TermsNotificationWasShown)
            {
                NotificationsHelper.CreateNotificationOnlyIfInBackground(NotificationsEnum.ReApproveConsents);
            }
        }

        private async Task<bool> SubmitZips(IEnumerable<string> zips, Func<IEnumerable<string>, Task> submitBatches)
        {
            if (zips == null || !zips.Any())
            {
                //The DownloadZips method already logged the error and updated the message in dev tools.
                return false;
            }

            // Submit downloaded files
            _developerTools.StoreLastProvidedFiles(zips);
            try
            {
                await submitBatches(zips);
                return true;
            }
            catch (FailedToFetchConfigurationException e)
            {
                string errorMessage = e.Message;
                LogUtils.LogException(Enums.LogSeverity.WARNING, e,
                    $"{_logPrefix}.{nameof(SubmitZips)}: {errorMessage}");
                _developerTools.AddToPullHistoryRecord(errorMessage);
                return false;
            }
            catch (Exception e)
            {
                string errorMessage = "submitBatches() failed when submitting the files to the EN API";
                LogUtils.LogException(Enums.LogSeverity.ERROR, e,
                    $"{_logPrefix}.{nameof(SubmitZips)}: {errorMessage}");
                _developerTools.AddToPullHistoryRecord(errorMessage);
                return false;
            }
        }

        private void DeleteZips(IEnumerable<String> zips)
        {
            foreach (string zip in zips)
            {
                try
                {
                    File.Delete(zip);
                }
                catch (Exception e)
                {
                    LogUtils.LogException(Enums.LogSeverity.WARNING, e,
                        $"{_logPrefix}.{nameof(DeleteZips)}: Caught Exception when deleting temporary zip files");
                }
            }
        }
    }
}

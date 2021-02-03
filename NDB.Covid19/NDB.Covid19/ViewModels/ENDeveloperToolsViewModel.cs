using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MoreLinq;
using NDB.Covid19.WebServices.ExposureNotification;
using System.Collections.Generic;
using Xamarin.ExposureNotifications;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Configuration;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications;
using NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected;
using NDB.Covid19.Utils.DeveloperTools;
using EN = Xamarin.ExposureNotifications;
using Debug = System.Diagnostics.Debug;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class ENDeveloperToolsViewModel
    {
        string _logPrefix = $"{nameof(ENDeveloperToolsViewModel)}: ";

        private static bool _longRetentionTime = true;
        private DateTime _messageDateTime = DateTime.Now;
        public static string PushKeysInfo = "";

        public string DevToolsOutput { get; set; }
        public Action DevToolUpdateOutput;

        static IDeveloperToolsService _devTools => ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
        private IClipboard _clipboard => ServiceLocator.Current.GetInstance<IClipboard>();

        public ENDeveloperToolsViewModel() { }

        public void PullWithDelay(Func<Task<bool>> action)
        {
            Task.Run(async () =>
            {
                LastDateTimeTermsNotificationWasShown = DateTime.MinValue;
                await Task.Delay(1000 * 10);
                if (action != null) await action.Invoke();
            });
        }

        internal static void UpdatePushKeysInfo(ApiResponse response, SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO, JsonSerializerSettings settings)
        {
            PushKeysInfo = $"StatusCode: {response.StatusCode}, Time (UTC): {DateTime.UtcNow.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}\n\n";
            ParseKeys(selfDiagnosisSubmissionDTO, settings, ENOperation.PUSH);
            PutInPushKeyInfoInSharedPrefs();
            Debug.WriteLine($"{PushKeysInfo}");
        }

        static void ParseKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO, JsonSerializerSettings settings, ENOperation varAssignCheck)
        {
            string jsonBody = JsonConvert.SerializeObject(selfDiagnosisSubmissionDTO, settings);

            JObject parsed = JObject.Parse(jsonBody);
            JArray keyArray = (JArray)parsed["keys"];
            JArray visitedCountries = (JArray) parsed["visitedCountries"];
            JArray regions = (JArray) parsed["regions"];

            PushKeysInfo += $"visitedCountries: {visitedCountries}\n";
            PushKeysInfo += $"regions: {regions}\n";

            keyArray?.ForEach(key =>
            {
                String keyData = $"Key: {EncodingUtils.ConvertByteArrayToString((byte[])key["key"])} ,\n" +
                                 $"rollingStart: {key["rollingStart"]},\n" +
                                 $"rollingDuration: {key["rollingDuration"]},\n" +
                                 $"transmissionRiskLevel: {key["transmissionRiskLevel"]}\n\n";
                PushKeysInfo += keyData;
                Debug.WriteLine(keyData);
            });
        }

        private static void PutInPushKeyInfoInSharedPrefs() {
            ServiceLocator.Current.GetInstance<IDeveloperToolsService>().LastKeyUploadInfo = PushKeysInfo;
            Debug.WriteLine(PushKeysInfo);
        }

        public async Task<string> GetPushKeyInfoFromSharedPrefs() {
            string res = "Empty";

            PushKeysInfo = _devTools.LastKeyUploadInfo;
            if (PushKeysInfo != "") res = PushKeysInfo;
            Debug.WriteLine(res);
            await _clipboard.SetTextAsync(res);
            return res;
        }

        public async Task<string> GetFormattedPreferences()
        {
            int migrationCount = MigrationCount;
            int lastPullKeysBatchNumberNotSubmitted = LastPullKeysBatchNumberNotSubmitted;
            int lastPullKeysBatchNumberSuccessfullySubmitted = LastPullKeysBatchNumberSuccessfullySubmitted;
            BatchType lastPulledBatchType = LastPulledBatchType;

            bool isOnboardingCompleted = IsOnboardingCompleted;
            bool isOnboardingCountriesCompleted = IsOnboardingCountriesCompleted;
            bool isDownloadWithMobileDataEnabled = GetIsDownloadWithMobileDataEnabled();
            DateTime updatedDateTime = GetUpdatedDateTime();
            DateTime lastPullKeysSucceededDateTime = GetLastPullKeysSucceededDateTime();
            string appLanguage = GetAppLanguage();

            string formattedString =
                $"EXPOSURE_TIME_THRESHOLD: {ExposureTimeThreshold}\n" +
                $"LOW_ATTENUATION_DURATION_MULTIPLIER: {LowAttenuationDurationMultiplier}\n" +
                $"MIDDLE_ATTENUATION_DURATION_MULTIPLIER: {MiddleAttenuationDurationMultiplier}\n" +
                $"HIGH_ATTENUATION_DURATION_MULTIPLIER: {HighAttenuationDurationMultiplier}\n\n" +
                $"MIGRATION_COUNT: {migrationCount}\n " +
                $"LAST_PULLED_BATCH_NUMBER_NOT_SUBMITTED: {lastPullKeysBatchNumberNotSubmitted}\n " +
                $"LAST_PULLED_BATCH_NUMBER_SUBMITTED: {lastPullKeysBatchNumberSuccessfullySubmitted}\n " +
                $"LAST_PULLED_BATCH_TYPE: {lastPulledBatchType}\n " +
                $"IS_ONBOARDING_COMPLETED_PREF: {isOnboardingCompleted}\n " +
                $"IS_ONBOARDING_COUNTRIES_COMPLETED_PREF: {isOnboardingCountriesCompleted}\n" +
                $"USE_MOBILE_DATA_PREF: {isDownloadWithMobileDataEnabled}\n" +
                $"MESSAGES_LAST_UPDATED_PREF: {updatedDateTime}\n" +
                $"LAST_PULL_KEYS_SUCCEEDED_DATE_TIME: {lastPullKeysSucceededDateTime}\n" +
                $"LAST_TERMS_NOTIFICATION_DATE_TIME: {LastDateTimeTermsNotificationWasShown}\n" +
                $"APP_LANGUAGE: {appLanguage}\n\n";

            await _clipboard.SetTextAsync(formattedString);

            return formattedString;
        }

        public static string GetLastPullResult()
        {
            return _devTools.LastPullHistory;
        }

        public string LastUsedExposureConfigurationAsync()
        {
            string res = _devTools.LastUsedConfiguration;
            _clipboard.SetTextAsync(res);

            return res;
        }

        public async Task<ApiResponse> FakeGateway(string region)
        {
            try
            {
                if (string.IsNullOrEmpty(region))
                {
                    region = "no";
                }

                return await FakeGatewayUtils.PostKeysToFakeGateway(region);
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, _logPrefix + "Fake gateway upload failed");
                await _clipboard.SetTextAsync($"Push keys failed:\n{e}");
                return null;
            }
        }

        public async Task<bool> PullKeysFromServer()
        {
            DevToolsOutput = GetLastPullResult();

            bool processedAnyFiles = false;
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.UpdateKeysFromServer();
            }
            catch (Exception e){
                string error = $"Pull keys failed:\n{e}";
                await _clipboard.SetTextAsync(error);
                ServiceLocator.Current.GetInstance<IDeveloperToolsService>().AddToPullHistoryRecord(error);
            }

            return processedAnyFiles;
        }

        public async Task<bool> PullKeysFromServerAndGetExposureInfo()
        {
            DevToolsOutput = GetLastPullResult();

            bool processedAnyFiles = false;

            _devTools.ShouldSaveExposureInfo = true;
            
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.UpdateKeysFromServer();
            }
            catch (Exception e)
            {
                string error = $"Pull keys failed:\n{e}";
                await _clipboard.SetTextAsync(error);
                ServiceLocator.Current.GetInstance<IDeveloperToolsService>().AddToPullHistoryRecord(error);
            }

            return processedAnyFiles;
        }


        public string GetExposureInfosFromLastPull()
        {
            string exposureInfosString = _devTools.PersistedExposureInfo;
            string result = "";

            if (exposureInfosString == "")
            {
                result = "We have not saved any ExposureInfos yet";
            }
            else {
                try
                {
                    IEnumerable<ExposureInfo> exposureInfos = ExposureInfoJsonHelper.ExposureInfosFromJsonCompatibleString(exposureInfosString);
                    foreach (ExposureInfo exposureInfo in exposureInfos)
                    {
                        string separator = result == "" ? "" : "\n";
                        result += separator;
                        result += "[ExposureInfo with ";
                        result += $"AttenuationValue: {exposureInfo.AttenuationValue},";
                        result += $"Duration: {exposureInfo.Duration},";
                        result += $"Timestamp: {exposureInfo.Timestamp},";
                        result += $"TotalRiskScore: {exposureInfo.TotalRiskScore},";
                        result += $"TransmissionRiskLevel: {exposureInfo.TransmissionRiskLevel}";
                        result += "]";
                    }
                }
                catch (Exception e)
                {
                    LogUtils.LogException(Enums.LogSeverity.WARNING, e, _logPrefix + "GetExposureInfosFromLastPull");
                    result = "Failed at deserializing the saved ExposureInfos";
                }
            }

            string finalResult = $"These are the ExposureInfos we got the last time \"Pull keys and get exposure info\" was clicked:\n{result}";
            _clipboard.SetTextAsync(finalResult);
            return finalResult;
        }

        // Consider: Displaying the exposure configuration in the activities.
        public async Task<string> FetchExposureConfigurationAsync()
        {
            EN.Configuration c = await new ExposureNotificationHandler().GetConfigurationAsync();
            string res = $" AttenuationWeight: {c.AttenuationWeight}, Values: {EnConfArrayString(c.AttenuationScores)} \n" +
                $" DaysSinceLastExposureWeight: {c.DaysSinceLastExposureWeight}, Values: {EnConfArrayString(c.DaysSinceLastExposureScores)} \n" +
                $" DurationWeight: {c.DurationWeight}, Values: {EnConfArrayString(c.DurationScores)} \n" +
                $" TransmissionWeight: {c.TransmissionWeight}, Values: {EnConfArrayString(c.TransmissionRiskScores)} \n" +
                $" MinimumRiskScore: {c.MinimumRiskScore}" +
                $" DurationAtAttenuationThresholds: [{c.DurationAtAttenuationThresholds[0]},{c.DurationAtAttenuationThresholds[1]}]";

            Debug.WriteLine("Exposure Configuration:");
            Debug.WriteLine($" AttenuationWeight: {c.AttenuationWeight}, Values: {EnConfArrayString(c.AttenuationScores)}");
            Debug.WriteLine($" DaysSinceLastExposureWeight: {c.DaysSinceLastExposureWeight}, Values: {EnConfArrayString(c.DaysSinceLastExposureScores)}");
            Debug.WriteLine($" DurationWeight: {c.DurationWeight}, Values: {EnConfArrayString(c.DurationScores)}");
            Debug.WriteLine($" TransmissionWeight: {c.TransmissionWeight}, Values: {EnConfArrayString(c.TransmissionRiskScores)}");
            Debug.WriteLine($" MinimumRiskScore: {c.MinimumRiskScore}");

            DevToolsOutput = res;
            DevToolUpdateOutput?.Invoke();
            await _clipboard.SetTextAsync(res);
            return res;
        }

        private string EnConfArrayString(int[] values)
        {
            string res = "";
            for (int i = 0; i < 8; i++) {
                _ = (i == 7) ? res += values[i] : res += values[i] + ", ";
            }
            return res;
        }

        public string ToggleMessageRetentionTime() {
            if (_longRetentionTime)
            {
                Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES = Conf.MESSAGE_RETENTION_TIME_IN_MINUTES_SHORT;
                _longRetentionTime = false;
                Debug.WriteLine($"Setting retention time to: {Conf.MESSAGE_RETENTION_TIME_IN_MINUTES_SHORT}");
            }
            else
            {
                Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES = Conf.MESSAGE_RETENTION_TIME_IN_MINUTES_LONG;
                _longRetentionTime = true;
                Debug.WriteLine($"Setting retention time to: {Conf.MESSAGE_RETENTION_TIME_IN_MINUTES_LONG}");
            }
            return $"Message retention time minutes: \n{Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES}";
        }

        public string IncementExposureDate()
        {
            _messageDateTime = _messageDateTime.AddDays(1);
            return $"Incremented date for Send Message function: \n{_messageDateTime.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}";
        }

        public string DecrementExposureDate()
        {
            _messageDateTime = _messageDateTime.AddDays(-1);
            return $"Decremented date for Send Message function: \n{_messageDateTime.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}";
        }

        public string PrintLastSymptomOnsetDate()
        {
            PersonalDataModel pd = AuthenticationState.PersonalData;
            return $"Last Symptom Onset Date: {QuestionnaireViewModel.DateLabel}, " +
                $"Selection: {QuestionnaireViewModel.Selection}, " +
                $"MiBaDate:{pd?.Covid19_smitte_start}, " +
                $"Date used for risk calc:{pd?.FinalMiBaDate}";
        }

        public string PrintLastPulledKeysAndTimestamp()
        {
            string savedKeyBatches = _devTools.LastProvidedFilesPref;
            if (savedKeyBatches == "")
                savedKeyBatches = "We have not saved any downloaded keys yet"; 
            string result = $"These are the last TEK batch files provided to the EN API:\n{savedKeyBatches}";
            _clipboard.SetTextAsync(result);
            return result;
        }

        public async Task SimulateExposureMessage(int notificationTriggerInSeconds = 0)
        {
            await Task.Delay(notificationTriggerInSeconds * 1000);
            await MessageUtils.CreateMessage(this, _messageDateTime);
        }

        public async Task SimulateExposureMessageAfter10Sec()
        {
            await SimulateExposureMessage(10);
        }

        public string GetLastExposureSummary()
        {
            string result;
            if (ServiceLocator.Current.GetInstance<SecureStorageService>().KeyExists(SecureStorageKeys.LAST_SUMMARY_KEY))
            {
                result = "Last exposure summary: " + ServiceLocator.Current.GetInstance<SecureStorageService>().GetValue(SecureStorageKeys.LAST_SUMMARY_KEY);
            }
            else
            {
                result = "No summary yet";
            }
            Debug.WriteLine(result);
            _clipboard.SetTextAsync(result);
            return result;
        }

        public string GetPullHistory()
        {
            string pullHistory = _devTools.AllPullHistory;
            
            if (pullHistory == "")
            {
                return "No pull history";
            }
            else
            {
                _clipboard.SetTextAsync(pullHistory);
                return pullHistory;
            }
        }
    }
}
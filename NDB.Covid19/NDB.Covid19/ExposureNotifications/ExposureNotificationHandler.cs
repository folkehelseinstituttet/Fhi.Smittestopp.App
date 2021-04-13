using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models;
using NDB.Covid19.Models.UserDefinedExceptions;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using NDB.Covid19.WebServices.ExposureNotification;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications
{
    public class ExposureNotificationHandler : IExposureNotificationDailySummaryHandler
    {
        //MiBaDate is null if the device has garbage collected, e.g. in background. Throws MiBaDateMissingException if null.
        private DateTime? MiBaDate => AuthenticationState.PersonalData?.FinalMiBaDate;

        private ExposureNotificationWebService exposureNotificationWebService = new ExposureNotificationWebService();

        public Task<Xamarin.ExposureNotifications.Configuration> GetConfigurationAsync()
        {
            Debug.Print("Fetching configuration");

            return Task.Run(async () =>
            {
                Xamarin.ExposureNotifications.Configuration configuration = await exposureNotificationWebService.GetExposureConfiguration();
                if (configuration == null)
                {
                    throw new FailedToFetchConfigurationException("Aborting pull because configuration was not fetched from server. See corresponding server error log");
                }

                string jsonConfiguration = JsonConvert.SerializeObject(configuration);
                ServiceLocator.Current.GetInstance<IDeveloperToolsService>().LastUsedConfiguration = $"V1 config. Time used (UTC): {DateTime.UtcNow.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}\n{jsonConfiguration}";
                return configuration;
            });
        }

        public async Task FetchExposureKeyBatchFilesFromServerAsync(Func<IEnumerable<string>, Task> submitBatches, CancellationToken cancellationToken)
        {
            await new FetchExposureKeysHelper().FetchExposureKeyBatchFilesFromServerAsync(submitBatches, cancellationToken);
        }

        public async Task ExposureDetectedAsync(ExposureDetectionSummary summary, Func<Task<IEnumerable<ExposureInfo>>> getExposureInfo)
        {
            Debug.WriteLine("ExposureDetectedAsync is called");
            await ExposureDetectedHelper.EvaluateRiskInSummaryAndCreateMessage(summary, this);
            await ServiceLocator.Current.GetInstance<IDeveloperToolsService>().SaveLastExposureInfos(getExposureInfo);
            ExposureDetectedHelper.SaveLastSummary(summary);
        }

        public async Task ExposureStateUpdatedAsync(IEnumerable<ExposureWindow> windows, IEnumerable<DailySummary>? summaries)
        {
            Debug.WriteLine("ExposureStateUpdatedAsync is called");
            bool shouldSendMessage = false;
            foreach (DailySummary dailySummary in summaries)
            {
                if (ExposureDetectedHelper.RiskInDailySummaryAboveThreshold(dailySummary)
                    && ExposureDetectedHelper.HasNotShownExposureNotificationForDate(dailySummary.Timestamp.Date))
                {
                    shouldSendMessage = true;
                    // TODO: Mark the summary date as the one for which Exposure Notification has been shown
                }
            }
            if (shouldSendMessage) await MessageUtils.CreateMessage(this);
            ServiceLocator.Current.GetInstance<IDeveloperToolsService>().SaveExposureWindows(windows);
            ServiceLocator.Current.GetInstance<IDeveloperToolsService>().SaveLastDailySummaries(summaries);
        }

        public async Task UploadSelfExposureKeysToServerAsync(IEnumerable<TemporaryExposureKey> tempKeys)
        {
            // Convert to ExposureKeyModel list as it is extended with DaysSinceOnsetOfSymptoms value
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = tempKeys?.Select(key => new ExposureKeyModel(key)) ?? new List<ExposureKeyModel>();
            
            // There is a better behaviour of uploading keys when scanning is Stoped/Started (UIAlert for permission is always shown then),
            // The IF-check just toggles the scanning status on/off or off/on to keep the scanning status
            // the same as it was before method is called

            try
            {
                if (ServiceLocator.Current.GetInstance<IDeviceInfo>().Platform == DevicePlatform.iOS)
                {
                    if (await ExposureNotification.IsEnabledAsync())
                    {
                        await ExposureNotification.StopAsync();
                        await ExposureNotification.StartAsync();
                    }
                    else
                    {
                        await ExposureNotification.StartAsync();
                        await ExposureNotification.StopAsync();
                    }
                }
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(ExposureNotificationHandler), nameof(UploadSelfExposureKeysToServerAsync)))
                {
                    throw e;
                }
            }

            if (FakeGatewayUtils.IsFakeGatewayTest)
            {
                FakeGatewayUtils.LastPulledExposureKeys = temporaryExposureKeys;
                return;
            }

            if (AuthenticationState.PersonalData?.Access_token == null)
            {
                throw new AccessTokenMissingFromIDPortenException("The token from ID Porten is not set");
            }

            if (AuthenticationState.PersonalData?.VisitedCountries == null)
            {
                throw new VisitedCountriesMissingException(
                    "The visited countries list is missing. Possibly garbage collection removed it.");
            }

            if (!MiBaDate.HasValue)
            {
                throw new MiBaDateMissingException("The symptom onset date is not set from the calling view model");
            }

            DateTime MiBaDateAsUniversalTime = MiBaDate.Value.ToUniversalTime();

            List<ExposureKeyModel> validKeys = UploadDiagnosisKeysHelper.CreateAValidListOfTemporaryExposureKeys(temporaryExposureKeys);

            // Here all keys are extended with DaysSinceOnsetOfSymptoms value
            validKeys = UploadDiagnosisKeysHelper.SetTransmissionRiskLevel(validKeys, MiBaDateAsUniversalTime);

            bool success = await exposureNotificationWebService.PostSelfExposureKeys(validKeys);
            if (!success)
            {
                throw new FailedToPushToServerException("Failed to push keys to the server");
            }
        }

        public Task<DailySummaryConfiguration> GetDailySummaryConfigurationAsync()
        {
            Debug.WriteLine("Fetching DailySummaryConfiguration");

            return Task.Run(async () =>
            {
                Xamarin.ExposureNotifications.DailySummaryConfiguration dsc = await exposureNotificationWebService.GetDailySummaryConfiguration();
                if (dsc == null)
                {
                    throw new FailedToFetchConfigurationException("Aborting pull because configuration was not fetched from server. See corresponding server error log");
                }

                // On iOS double-type weights represent percents, so we need to multiply by 100
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    dsc.AttenuationWeights[DistanceEstimate.Immediate] *= 100;
                    dsc.AttenuationWeights[DistanceEstimate.Medium] *= 100;
                    dsc.AttenuationWeights[DistanceEstimate.Near] *= 100;
                    dsc.AttenuationWeights[DistanceEstimate.Other] *= 100;
                    dsc.InfectiousnessWeights[Infectiousness.Standard] *= 100;
                    dsc.InfectiousnessWeights[Infectiousness.High] *= 100;
                    dsc.ReportTypeWeights[ReportType.Recursive] *= 100;
                    dsc.ReportTypeWeights[ReportType.ConfirmedTest] *= 100;
                    dsc.ReportTypeWeights[ReportType.SelfReported] *= 100;
                    dsc.ReportTypeWeights[ReportType.ConfirmedClinicalDiagnosis] *= 100;
                }

                string jsonConfiguration = JsonConvert.SerializeObject(dsc);
                ServiceLocator.Current.GetInstance<IDeveloperToolsService>().LastUsedConfiguration = $"V2 config. Time used (UTC): {DateTime.UtcNow.ToGreGorianUtcString("yyyy-MM-dd HH:mm:ss")}\n{jsonConfiguration}";
                return dsc;
            });
        }

        // This is the explanation that will be displayed to the user when getting ExposureInfo objects on iOS
        // Only used in developer tools
        public string UserExplanation => "Saving ExposureInfos with \"Pull Keys and Save ExposureInfos\" causes the EN API to display this notification (not a bug)";

    }
}

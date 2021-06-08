using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices.ErrorHandlers;
using NDB.Covid19.WebServices.Helpers;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using System.Net.Http;
using System.Text;
using NDB.Covid19.OAuth2;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.WebServices.ExposureNotification
{
    public class ExposureNotificationWebService : BaseWebService
    {
        public async Task<bool> PostSelfExposureKeys(IEnumerable<ExposureKeyModel> temporaryExposureKeys)
        {
            return await PostSelfExposureKeys(new SelfDiagnosisSubmissionDTO(temporaryExposureKeys), temporaryExposureKeys);
        }

        public async Task<bool> PostSelfExposureKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO,
            IEnumerable<ExposureKeyModel> temporaryExposureKeys)
        {
            return await PostSelfExposureKeys(selfDiagnosisSubmissionDTO, temporaryExposureKeys, this);
        }

        public async Task<bool> PostSelfExposureKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO, IEnumerable<ExposureKeyModel> temporaryExposureKeys, BaseWebService service)
        {
            ApiResponse response;
            bool requestedAnonToken;

            if (AuthenticationState.PersonalData?.AnonymousTokensEnabled == true)
            {
                requestedAnonToken = true;
                response = await PostSelfExposureKeysWithAnonTokens(selfDiagnosisSubmissionDTO, temporaryExposureKeys, service);
            }
            else
            {
                requestedAnonToken = false;
                response = await service.Post(selfDiagnosisSubmissionDTO, Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS);
            }
            // HandleErrorsSilently happens even if IsSuccessfull is true other places in the code, but here
            // we have an if-else to avoid having to create the redacted key list if we don't have to
            if (!response.IsSuccessfull)
            {
                string redactedKeysJson = RedactedTekListHelper.CreateRedactedTekList(temporaryExposureKeys);
                HandleErrorsSilently(response, new PostExposureKeysErrorHandler(redactedKeysJson));
            }
            else
            {
                HandleErrorsSilently(response);
            }

            ENDeveloperToolsViewModel.UpdatePushKeysInfo(response, selfDiagnosisSubmissionDTO, JsonSerializerSettings, requestedAnonToken);

            return response.IsSuccessfull;
        }

        private static async Task<ApiResponse> PostSelfExposureKeysWithAnonTokens(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO, IEnumerable<ExposureKeyModel> temporaryExposureKeys, BaseWebService service)
        {
            var tokenService = new AnonymousTokenService(CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1));
            var token = await tokenService.GetAnonymousTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS);
            request.Headers.Add("Authorization", $"Anonymous {token}");
            string jsonBody = JsonConvert.SerializeObject(selfDiagnosisSubmissionDTO, JsonSerializerSettings);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await new HttpClient().SendAsync(request);

            var result = new ApiResponse(Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS, HttpMethod.Post);
            result.StatusCode = (int)response.StatusCode;
            result.ResponseText = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                result.ResponseText = response.ReasonPhrase;
            }
            return result;
        }

        public async Task<Xamarin.ExposureNotifications.Configuration> GetExposureConfiguration()
        {
            ApiResponse<AttenuationBucketsConfigurationDTO> response = await Get<AttenuationBucketsConfigurationDTO>(Conf.URL_GET_EXPOSURE_CONFIGURATION);
            HandleErrorsSilently(response);

            LogUtils.SendAllLogs();

            if (response.IsSuccessfull && response.Data != null && response.Data.Configuration != null)
            {
                if (response.Data.AttenuationBucketsParams != null)
                {
                    LocalPreferencesHelper.ExposureTimeThreshold = response.Data.AttenuationBucketsParams.ExposureTimeThreshold;
                    LocalPreferencesHelper.LowAttenuationDurationMultiplier = response.Data.AttenuationBucketsParams.LowAttenuationBucketMultiplier;
                    LocalPreferencesHelper.MiddleAttenuationDurationMultiplier = response.Data.AttenuationBucketsParams.MiddleAttenuationBucketMultiplier;
                    LocalPreferencesHelper.HighAttenuationDurationMultiplier = response.Data.AttenuationBucketsParams.HighAttenuationBucketMultiplier;
                }
                return response.Data.Configuration;
            }

            return null;
        }

        public async Task<DailySummaryConfiguration> GetDailySummaryConfiguration()
        {
            ApiResponse<DailySummaryConfigurationDTO> response = await Get<DailySummaryConfigurationDTO>(Conf.URL_GET_DAILY_SUMMARY_CONFIGURATION);
            HandleErrorsSilently(response);

            LogUtils.SendAllLogs();

            if (response.IsSuccessfull && response.Data != null && response.Data.DailySummaryConfiguration != null)
            {
                if (response.Data.ScoreSumThreshold.HasValue)
                {
                    LocalPreferencesHelper.ScoreSumThreshold = response.Data.ScoreSumThreshold.Value;
                }
                return response.Data.DailySummaryConfiguration;
            }

            return null;
        }

        /// <summary>
        /// Fetch new keys, if there are any new.
        /// </summary>
        public virtual async Task<ApiResponse<Stream>> GetDiagnosisKeys(string batchRequestString, CancellationToken cancellationToken)
        {
            string url = Conf.URL_GET_DIAGNOSIS_KEYS + "/" + batchRequestString;
            ApiResponse<Stream> response = await GetFileAsStreamAsync(url);
            HandleErrorsSilently(response);

            if (response.IsSuccessfull)
            {
                //In messages, this time will be shown as "last updated", to show the user when we last checked for exposures.
                LocalPreferencesHelper.UpdateLastUpdatedDate();
            }

            return response;
        }
    }
}
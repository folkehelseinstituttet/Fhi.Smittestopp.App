using AnonymousTokens.Client.Protocol;

using NDB.Covid19.AnonymousTokens;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.OAuth2;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices.ErrorHandlers;
using NDB.Covid19.WebServices.Helpers;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NDB.Covid19.WebServices.ExposureNotification
{
    public class ExposureNotificationWebService : BaseWebService
    {
        public async Task<bool> PostSelvExposureKeys(IEnumerable<ExposureKeyModel> temporaryExposureKeys)
        {
            return await PostSelvExposureKeys(new SelfDiagnosisSubmissionDTO(temporaryExposureKeys), temporaryExposureKeys);
        }

        public async Task<bool> PostSelvExposureKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO,
            IEnumerable<ExposureKeyModel> temporaryExposureKeys)
        {
            return await PostSelvExposureKeys(selfDiagnosisSubmissionDTO, temporaryExposureKeys, this);
        }

        public async Task<bool> PostSelvExposureKeys(SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDTO, IEnumerable<ExposureKeyModel> temporaryExposureKeys, BaseWebService service)
        {
            if (AuthenticationState.PersonalData?.AnonymousTokensEnabled == true)
            {
                var ecParameters = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1);
                var publicKeyStore = new PublicKeyStore();
                var publicKey = await publicKeyStore.GetAsync();

                var initiator = new Initiator();
                var init = initiator.Initiate(ecParameters.Curve);
                var t = init.t;
                var r = init.r;
                var P = init.P;

                // call /api/anonymoustokens on Verification with P as payload to generate token. Response will contain Q, proofC, proofZ.
                // var W = initiator.RandomiseToken(ecParameters, publicKey, P, Q, proofC, proofZ, r);
                // call /xx/xx on Backend with t and W as payload to verify the token. Response will be true/false.

                return await Task.FromResult(true);
            }
            else
            {
                ApiResponse response = await service.Post(selfDiagnosisSubmissionDTO, Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS);

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

                ENDeveloperToolsViewModel.UpdatePushKeysInfo(response, selfDiagnosisSubmissionDTO, JsonSerializerSettings);

                return response.IsSuccessfull;
            }

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
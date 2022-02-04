using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static NDB.Covid19.ProtoModels.TemporaryExposureKey.Types;
using Xamarin.Auth;

namespace NDB.Covid19.OAuth2
{
    public class AuthenticationManager
    {
        public EventHandler<AuthenticatorCompletedEventArgs> _completedHandler;
        public EventHandler<AuthenticatorErrorEventArgs> _errorHandler;
        public HttpClient client = new HttpClient();
        public static JsonSerializer JsonSerializer = new JsonSerializer();

        /*
            Client ID – this identifies the client that is making the request, and can be retrieved from the project in the Google API Console.
            Client Secret – this should be null or string.Empty.
            Scope – this identifies the API access being requested by the application, and the value informs the consent screen that is shown to the user. For more information about scopes, see Authorizing API request on Google's website.
            Authorize URL – this identifies the URL where the authorization code will be obtained from.
            Redirect URL – this identifies the URL where the response will be sent. The value of this parameter must match one of the values that appears in the Credentials tab for the project in the Google Developers Console.
            AccessToken Url – this identifies the URL used to request access tokens after an authorization code is obtained.
            GetUserNameAsync Func – an optional Func that will be used to asynchronously retrieve the username of the account after it's been successfully authenticated.
            Use Native UI – a boolean value indicating whether to use the device's web browser to perform the authentication request.
        */
        public AuthenticationManager() { }

        public void Setup(
            EventHandler<AuthenticatorCompletedEventArgs> completedHandler,
            EventHandler<AuthenticatorErrorEventArgs> errorHandler,
            ReportType reportType
            )
        {
            string OAuthScope = reportType switch
            {
                ReportType.ConfirmedTest => OAuthConf.OAUTH2_CONFIRMED_TEST_SCOPE,
                ReportType.SelfReport => OAuthConf.OAUTH2_SELF_DIAGNOSIS_SCOPE,
                _ => throw new ArgumentException("Unsupported report type was specified")
            };

            AuthenticationState.Authenticator = new CustomOAuth2Authenticator(
                OAuthConf.OAUTH2_CLIENT_ID,
                null,
                OAuthScope,
                new Uri(OAuthConf.OAUTH2_AUTHORISE_URL),
                new Uri(OAuthConf.OAUTH2_REDIRECT_URL),
                new Uri(OAuthConf.OAUTH2_ACCESSTOKEN_URL),
                null,
                true);
            AuthenticationState.Authenticator.ClearCookiesBeforeLogin = true;
            AuthenticationState.Authenticator.ShowErrors = true;
            AuthenticationState.Authenticator.AllowCancel = true;

            _completedHandler = completedHandler;
            AuthenticationState.Authenticator.Completed += _completedHandler;

            _errorHandler = errorHandler;
            AuthenticationState.Authenticator.Error += _errorHandler;
        }

        public void Cleanup()
        {
            if (AuthenticationState.Authenticator != null)
            {
                AuthenticationState.Authenticator.Completed -= _completedHandler;
                AuthenticationState.Authenticator.Error -= _errorHandler;
            }
            AuthenticationState.Authenticator = null;
        }

        public PersonalDataModel GetPayloadValidateJWTToken(string accessToken)
        {
            try
            {
                byte[] publicKey = Convert.FromBase64String(GetPublicKey(OAuthConf.OAUTH2_JWKS_URL));

                string jsonPayload = new JwtBuilder()
                    .WithAlgorithm(new RS256Algorithm(new X509Certificate2(publicKey)))
                    .WithValidator(new JwtValidator(new JsonNetSerializer(), new UtcDateTimeProvider(), 5 * 60))
                    .MustVerifySignature()
                    .Decode(accessToken);
                
                System.Diagnostics.Debug.Print(jsonPayload);

                JObject obj = JObject.Parse(jsonPayload);

                PersonalDataModel personalDataModel = new PersonalDataModel();
                if (obj != null)
                {
                    personalDataModel = obj.ToObject<PersonalDataModel>(JsonSerializer);
                    personalDataModel.IsMsisLookupSkipped = obj.GetValue("no-msis")?.ToString() == "true";
                }

                personalDataModel.Access_token = accessToken;

                return personalDataModel;

            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, $"{nameof(AuthenticationManager)}.{nameof(GetPayloadValidateJWTToken)} failed.");
                return null;
            }
        }

        public string GetPublicKey(string url)
        {

            Task<string> vs = client.GetStringAsync(url);
            string json = vs.Result;
            client.Dispose();
            JsonWebKeySet jwks = new JsonWebKeySet(json);
            List<JsonWebKey> keyList = new List<JsonWebKey>(jwks.Keys);
            int lastIndex = keyList.Count - 1;
            string publicKey = keyList[lastIndex].X5c[0];
            return publicKey;
        }
    }

}

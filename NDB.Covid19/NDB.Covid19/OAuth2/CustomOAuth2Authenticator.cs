using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using Newtonsoft.Json;
using Xamarin.Auth;
using PCLCrypto;

namespace NDB.Covid19.OAuth2
{
    public class CustomOAuth2Authenticator : OAuth2Authenticator
    {
        Uri _accessTokenUrl;
        private string _redirectUrl;
        private string _codeVerifier;

        public CustomOAuth2Authenticator(string clientId,
            string scope,
            Uri authorizeUrl,
            Uri redirectUrl,
            GetUsernameAsyncFunc getUsernameAsync = null,
            bool isUsingNativeUI = false) : base(clientId, scope, authorizeUrl, redirectUrl, getUsernameAsync, isUsingNativeUI)
        {
        }

        public CustomOAuth2Authenticator(
            string clientId,
            string clientSecret,
            string scope,
            Uri authorizeUrl,
            Uri redirectUrl,
            Uri accessTokenUrl,
            GetUsernameAsyncFunc getUsernameAsync = null,
            bool isUsingNativeUI = false) : base(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync, isUsingNativeUI)
        {
            _accessTokenUrl = accessTokenUrl;
        }

        public override void OnPageLoading(Uri url)
        {
            base.OnPageLoading(url);
        }

        protected override async void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            // Prepare code verifier and send them back to get access token
            query["code_verifier"] = _codeVerifier;
            query["client_id"] = ClientId;
            query["grant_type"] = "authorization_code";
            query["redirect_uri"] = _redirectUrl;

            // Get Access token before hand by our rule, then the package will not attemp to get the access token anymore later
            try
            {
                var token = await CustomRequestAccessTokenAsync(query);
                foreach (var tokenSegment in token)
                {
                    fragment.Add(tokenSegment);
                }
                base.OnRedirectPageLoaded(url, query, fragment);
            }
            catch (AuthException e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, $"{nameof(CustomOAuth2Authenticator)}-{nameof(OnRedirectPageLoaded)} threw an exception");
                System.Diagnostics.Debug.Print(JsonConvert.SerializeObject(query));

                OnError(e);
            }
        }

        /// <summary>
        /// Asynchronously makes a request to the access token URL with the given parameters.
        /// </summary>
        /// <param name="queryValues">The parameters to make the request with.</param>
        /// <returns>The data provided in the response to the access token request.</returns>
        public async Task<IDictionary<string, string>> CustomRequestAccessTokenAsync(IDictionary<string, string> queryValues)
        {
            // mc++ changed protected to public for extension methods RefreshToken (Adrian Stevens) 
            var content = new FormUrlEncodedContent(queryValues);


            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(_accessTokenUrl, content).ConfigureAwait(false);
            string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    LogUtils.LogMessage(LogSeverity.WARNING, "CustomOAuth2Authenticator failed to refresh token.", "Error from service: " + text);
                    System.Diagnostics.Debug.Print("Error from service: " + text);
                }
            }
            catch { }

            // Parse the response
            var data = text.Contains("{") ? WebEx.JsonDecode(text) : WebEx.FormDecode(text);

            if (data.ContainsKey("error"))
            {
                throw new AuthException("Error authenticating: " + data["error"]);
            }
            //---------------------------------------------------------------------------------------
            /// Pull Request - manually added/fixed
            ///		OAuth2Authenticator changes to work with joind.in OAuth #91
            ///		https://github.com/xamarin/Xamarin.Auth/pull/91
            ///		
            else if (data.ContainsKey(AccessTokenName))
            //---------------------------------------------------------------------------------------
            {
            }
            else
            {
                //---------------------------------------------------------------------------------------
                /// Pull Request - manually added/fixed
                ///		OAuth2Authenticator changes to work with joind.in OAuth #91
                ///		https://github.com/xamarin/Xamarin.Auth/pull/91
                ///		
                //throw new AuthException ("Expected access_token in access token response, but did not receive one.");
                throw new AuthException("Expected " + AccessTokenName + " in access token response, but did not receive one.");
                //---------------------------------------------------------------------------------------
            }

            return data;
        }

        protected override void OnCreatingInitialUrl(IDictionary<string, string> query)
        {
            _redirectUrl = Uri.UnescapeDataString(query["redirect_uri"]);
            _codeVerifier = CreateCodeVerifier();
            query["response_type"] = "code";
            query["nonce"] = Guid.NewGuid().ToString("N");
            query["code_challenge"] = CreateChallenge(_codeVerifier);
            query["code_challenge_method"] = "S256";
            query["prompt"] = "login";

            base.OnCreatingInitialUrl(query);
        }

        private string CreateCodeVerifier()
        {
            var codeBytes = WinRTCrypto.CryptographicBuffer.GenerateRandom(64);
            return Convert.ToBase64String(codeBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private string CreateChallenge(string code)
        {
            var codeVerifier = code;
            var sha256 = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(PCLCrypto.HashAlgorithm.Sha256);
            var challengeByteArray = sha256.HashData(WinRTCrypto.CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(codeVerifier)));
            WinRTCrypto.CryptographicBuffer.CopyToByteArray(challengeByteArray, out byte[] challengeBytes);
            return Convert.ToBase64String(challengeBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

    }
}

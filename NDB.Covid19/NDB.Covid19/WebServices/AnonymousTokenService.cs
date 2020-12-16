using AnonymousTokens.Client.Protocol;
using AnonymousTokens.Core.Services;
using AnonymousTokens.Core.Services.InMemory;
using NDB.Covid19.Configuration;
using NDB.Covid19.OAuth2;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NDB.Covid19.WebServices
{
    public class AnonymousTokenService
    {
        private Initiator _initiator = new Initiator();
        private X9ECParameters _ecParameters = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1);
        private IPublicKeyStore _publicKeyStore = new InMemoryPublicKeyStore();

        public AnonymousTokenService(X9ECParameters ecParameters, IPublicKeyStore publicKeyStore)
        {
            _ecParameters = ecParameters;
            _publicKeyStore = publicKeyStore;
        }

        public async Task<string> GetAnonymousTokenAsync()
        {
            var tokenState = GenerateTokenRequest();
            var tokenResponse = await RequestTokenGeneration(tokenState);
            var token = await RandomizeToken(tokenState, tokenResponse);
            return EncodeToken(tokenState, token);
        }

        public AnonymousTokenState GenerateTokenRequest()
        {
            var init = _initiator.Initiate(_ecParameters.Curve);
            return new AnonymousTokenState
            {
                t = init.t,
                r = init.r,
                P = init.P
            };
        }

        public async Task<ECPoint> RandomizeToken(AnonymousTokenState state, GenerateTokenResponseModel tokenResponse)
        {
            var K = await _publicKeyStore.GetAsync();
            var Q = _ecParameters.Curve.DecodePoint(Hex.Decode(tokenResponse.QAsHex));
            var c = new Org.BouncyCastle.Math.BigInteger(Hex.Decode(tokenResponse.ProofCAsHex));
            var z = new Org.BouncyCastle.Math.BigInteger(Hex.Decode(tokenResponse.ProofZAsHex));

            return _initiator.RandomiseToken(_ecParameters, K, state.P, Q, c, z, state.r);
        }

        string EncodeToken(AnonymousTokenState tokenState, ECPoint W)
        {
            var t = tokenState.t;
            return Convert.ToBase64String(W.GetEncoded()) + "." + System.Convert.ToBase64String(t);
        }


        private async Task<GenerateTokenResponseModel> RequestTokenGeneration(AnonymousTokenState state)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, OAuthConf.OAUTH2_ANONTOKEN_URL);
            request.Headers.Add("Authorization", $"Bearer {AuthenticationState.PersonalData?.Access_token}");
            var tokenRequest = new GenerateTokenRequestModel
            {
                PAsHex = Hex.ToHexString(state.P.GetEncoded())
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
            var response = await new HttpClient().SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Call to " + OAuthConf.OAUTH2_ANONTOKEN_URL + " failed " + response);
            }
            return JsonConvert.DeserializeObject<GenerateTokenResponseModel>(await response.Content.ReadAsStringAsync());
        }
    }

    public class AnonymousTokenState
    {
        public Org.BouncyCastle.Math.BigInteger r { get; internal set; }
        public byte[] t { get; internal set; }
        public ECPoint P { get; internal set; }
    }

    public class GenerateTokenRequestModel
    {
        [JsonProperty("pAsHex")]
        public string PAsHex { get; set; }
    }

    public class GenerateTokenResponseModel
    {
        [JsonProperty("qAsHex")]
        public string QAsHex { get; set; }

        [JsonProperty("proofCAsHex")]
        public string ProofCAsHex { get; set; }

        [JsonProperty("proofZAsHex")]
        public string ProofZAsHex { get; set; }
    }
}

using AnonymousTokens.Client.Protocol;
using NDB.Covid19.Configuration;
using NDB.Covid19.OAuth2;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NDB.Covid19.Models.UserDefinedExceptions;

namespace NDB.Covid19.WebServices
{
    public class AnonymousTokenService
    {
        private Initiator _initiator = new Initiator();
        private X9ECParameters _ecParameters;

        public AnonymousTokenService(X9ECParameters ecParameters)
        {
            _ecParameters = ecParameters;
        }

        public async Task<string> GetAnonymousTokenAsync()
        {
            var tokenState = GenerateTokenRequest();
            var tokenResponse = await RequestTokenGeneration(tokenState);
            var publicKey = await GetPublicKeyAsync(tokenResponse);
            var token = RandomizeToken(tokenState, tokenResponse, publicKey);
            return EncodeToken(tokenState, token, tokenResponse.Kid);
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

        public ECPoint RandomizeToken(AnonymousTokenState state, GenerateTokenResponseModel tokenResponse, ECPoint K)
        {
            var Q = _ecParameters.Curve.DecodePoint(Convert.FromBase64String(tokenResponse.SignedPoint));
            var c = new BigInteger(Convert.FromBase64String(tokenResponse.ProofChallenge));
            var z = new BigInteger(Convert.FromBase64String(tokenResponse.ProofResponse));
            return _initiator.RandomiseToken(_ecParameters, K, state.P, Q, c, z, state.r);
        }

        string EncodeToken(AnonymousTokenState tokenState, ECPoint W, string keyId)
        {
            var t = tokenState.t;
            return Convert.ToBase64String(W.GetEncoded()) + "." + Convert.ToBase64String(t) + "." + keyId;
        }

        private async Task<GenerateTokenResponseModel> RequestTokenGeneration(AnonymousTokenState state)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, OAuthConf.OAUTH2_ANONTOKEN_URL);
            request.Headers.Add("Authorization", $"Bearer {AuthenticationState.PersonalData?.Access_token}");
            var tokenRequest = new GenerateTokenRequestModel
            {
                MaskedPoint = Convert.ToBase64String(state.P.GetEncoded())
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(tokenRequest), Encoding.UTF8, "application/json");
            var response = await new HttpClient().SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebServiceHttpException(response, OAuthConf.OAUTH2_ANONTOKEN_URL);
            }
            return JsonConvert.DeserializeObject<GenerateTokenResponseModel>(await response.Content.ReadAsStringAsync());
        }

        private async Task<ECPoint> GetPublicKeyAsync(GenerateTokenResponseModel tokenResponse)
        {
            string kid = tokenResponse.Kid;
            var request = new HttpRequestMessage(HttpMethod.Get, OAuthConf.OAUTH2_ANONTOKEN_URL + "/atks");
            var response = await new HttpClient().SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new WebServiceHttpException(response, OAuthConf.OAUTH2_ANONTOKEN_URL + "/atks");
            }
            var anonTokenKeyStoreResponse = JsonConvert.DeserializeObject<AnonymousTokenKeyStoreResponseModel>(await response.Content.ReadAsStringAsync());
            return DecodePublicKey(anonTokenKeyStoreResponse.Keys.Single(k => k.Kid == kid));
        }

        private static ECPoint DecodePublicKey(AnonymousTokenKey key)
        {
            var curve = CustomNamedCurves.GetByName(key.Crv);
            return curve.Curve.CreatePoint(
                new BigInteger(Convert.FromBase64String(key.X)),
                new BigInteger(Convert.FromBase64String(key.Y))
            );
        }
    }

    public class AnonymousTokenState
    {
        public BigInteger r { get; internal set; }
        public byte[] t { get; internal set; }
        public ECPoint P { get; internal set; }
    }

    public class GenerateTokenRequestModel
    {
        [JsonProperty("maskedPoint")]
        public string MaskedPoint { get; set; }
    }

    public class GenerateTokenResponseModel
    {
        [JsonProperty("signedPoint")]
        public string SignedPoint { get; set; }

        [JsonProperty("proofChallenge")]
        public string ProofChallenge { get; set; }

        [JsonProperty("proofResponse")]
        public string ProofResponse { get; set; }

        [JsonProperty("kid")]
        public string Kid { get; set; }
    }

    public class AnonymousTokenKeyStoreResponseModel
    {
        public List<AnonymousTokenKey> Keys { get; set; }
    }

    public class AnonymousTokenKey
    {
        [JsonProperty("kid")]
        public string Kid { get; set; }

        [JsonProperty("kty")]
        public string Kty { get; set; }
        
        [JsonProperty("crv")]
        public string Crv { get; set; }
        
        [JsonProperty("x")]
        public string X { get; set; }
        
        [JsonProperty("y")]
        public string Y { get; set; }
        
        [JsonProperty("k")]
        public string K { get; set; }

        [JsonProperty("publicKeyAsHex")]
        public string PublicKeyAsHex { get; set; }
    }

}

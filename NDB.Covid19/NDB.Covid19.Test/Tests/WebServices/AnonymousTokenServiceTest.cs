using AnonymousTokens.Core;
using AnonymousTokens.Core.Services;
using AnonymousTokens.Core.Services.InMemory;
using AnonymousTokens.Server.Protocol;
using FluentAssertions;
using NDB.Covid19.WebServices;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NDB.Covid19.Test.Tests.WebServices
{

    public class AnonymousTokenServiceTest
    {
        private X9ECParameters ecParameters = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1);
        private IPublicKeyStore publicKeyStore = new InMemoryPublicKeyStore();
        private InMemoryPrivateKeyStore inMemoryPrivateKeyStore = new InMemoryPrivateKeyStore();

        [Fact]
        public async Task GenerateToken_Generates_Valid_Token()
        {
            var privateKey = await inMemoryPrivateKeyStore.GetAsync();

            var service = new AnonymousTokenService(ecParameters);
            var tokenState = service.GenerateTokenRequest();

            GenerateTokenResponseModel tokenResponse = await GenerateTokenAsync(privateKey, publicKeyStore, ecParameters, tokenState.P);

            var publicKey = (await publicKeyStore.GetAsync()).Q;
            var token = service.RandomizeToken(tokenState, tokenResponse, publicKey);
            (await VerifyToken(privateKey, ecParameters, tokenState, token)).Should().BeTrue();
        }

        [Fact]
        public async Task RandomizeToken_Should_Reject_Token_From_Rogue_Generator()
        {
            var service = new AnonymousTokenService(ecParameters);
            var tokenState = service.GenerateTokenRequest();

            var privateKey = GeneratePrivateKey();
            GenerateTokenResponseModel tokenResponse = await GenerateTokenAsync(privateKey, publicKeyStore, ecParameters, tokenState.P);

            var publicKey = (await publicKeyStore.GetAsync()).Q;
            Assert.Throws<AnonymousTokensException>(() => service.RandomizeToken(tokenState, tokenResponse, publicKey))
                .Message.Should().Contain("proof is invalid");
        }

        [Fact]
        public async Task Verify_Should_Reject_Forged_Token()
        {
            var service = new AnonymousTokenService(ecParameters);
            var tokenState = service.GenerateTokenRequest();

            GenerateTokenResponseModel tokenResponse = await GenerateTokenAsync(await inMemoryPrivateKeyStore.GetAsync(), publicKeyStore, ecParameters, tokenState.P);

            var privateKey = GeneratePrivateKey();
            var publicKey = (await publicKeyStore.GetAsync()).Q;
            var token = service.RandomizeToken(tokenState, tokenResponse, publicKey);
            (await VerifyToken(privateKey, ecParameters, tokenState, token)).Should().BeFalse();
        }

        private BigInteger GeneratePrivateKey()
        {
            var random = new Random();
            byte[] data = new byte[32];
            random.NextBytes(data);
            return new BigInteger(data);
        }


        private static async Task<bool> VerifyToken(BigInteger privateKey, X9ECParameters ecParameters, AnonymousTokenState tokenState, ECPoint token)
        {
            return await new TokenVerifier(new InMemorySeedStore()).VerifyTokenAsync(privateKey, ecParameters.Curve, tokenState.t, token);
        }

        private static async Task<GenerateTokenResponseModel> GenerateTokenAsync(BigInteger privateKey, IPublicKeyStore publicKeyStore, X9ECParameters ecParameters, ECPoint P)
        {
            var response = new TokenGenerator().GenerateToken(privateKey, (await publicKeyStore.GetAsync()).Q, ecParameters, P);
            return new GenerateTokenResponseModel
            {
                ProofChallenge = Convert.ToBase64String(response.c.ToByteArray()),
                ProofResponse = Convert.ToBase64String(response.z.ToByteArray()),
                SignedPoint = Convert.ToBase64String(response.Q.GetEncoded())
            };
        }
    }
}

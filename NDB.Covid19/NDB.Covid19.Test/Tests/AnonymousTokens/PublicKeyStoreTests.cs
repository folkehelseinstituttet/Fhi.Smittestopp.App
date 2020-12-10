
using NDB.Covid19.AnonymousTokens;
using NDB.Covid19.Configuration;

using Xunit;

namespace NDB.Covid19.Test.Tests.AnonymousTokens
{
    public class PublicKeyStoreTests
    {
        private PublicKeyStore _systemUnderTest;

        public PublicKeyStoreTests()
        {
            _systemUnderTest = new PublicKeyStore();
        }

        [Fact]
        public async void GetAsync_GivenABase64EncodedStringAsPublicKey_ReturnsExpectedPublicKeyAsync()
        {
            // Arrange

            /* Generated with:
             * # generate a private key for a curve
               openssl ecparam -name prime256v1 -genkey -noout -out private-key.pem

               # generate corresponding public key
               openssl ec -in private-key.pem -pubout -out public-key.pem
            */
            AnonymousTokensConfig.ANONYMOUS_TOKENS_PUBLIC_KEY =
@"-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAESwcNFtWPA+ea0dKLy8qu94az50x9
3FO39ogmOgWLhpjKc3wvaMXOpHzJq6BR3hIniaqCJ8UKdw0Kd42RBnYghg==
-----END PUBLIC KEY-----";

            // Act
            var result = await _systemUnderTest.GetAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}

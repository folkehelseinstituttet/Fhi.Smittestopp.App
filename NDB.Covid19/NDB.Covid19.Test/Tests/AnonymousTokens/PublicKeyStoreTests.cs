
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

        [Fact(Skip = "Need clarification of type of key which will be used")]
        public async void GetAsync_GivenABase64EncodedStringAsPublicKey_ReturnsExpectedPublicKey()
        {
            // Arrange
            OAuthConf.OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = "MIIDUjCCAjqgAwIBAgIQcIpPJ6seRKGLgN2br+wpPjANBgkqhkiG9w0BAQsFADAmMSQwIgYDVQQDExt2ZXJpZmlzZXJpbmcuc21pdHRlc3RvcHAubm8wHhcNMjAxMTA3MTQyNzM5WhcNMjExMTA3MTQzNzM5WjAmMSQwIgYDVQQDExt2ZXJpZmlzZXJpbmcuc21pdHRlc3RvcHAubm8wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDDAhFyQIpg06siT1adoDLBZEEhamwteY6crgujR/BbJkUCrcBkBr2PspvG2M4JTEWmmZCMls2aZT7WeqRaTzZxbdq6a1ouNO9BYyIeQGEPJuanYeeCqvHanr0QDsGDQCasmkOGsnxG2ziIrPhykuULXZdlC4RhaIyVC9hDlwaTwDmDLLdDbnBfbQxY00oQGS8tPQrMyPAhaYnXmRpibmVPwUPhn8Mj4Y8a37PuaFo4aN3eZBakbPlPBjKXJwu+ENG3eWZQnj5LJT4yoSL/4qOrbAfKhfi6BmvH1tjjNvtLjeDy8eUSCEWnpfgaKY2F6O8paDGArFH37JDTXbwDHJJZAgMBAAGjfDB6MA4GA1UdDwEB/wQEAwIFoDAJBgNVHRMEAjAAMB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAfBgNVHSMEGDAWgBQ9TP5rRU3U3ycDvaFHdlttc99pvjAdBgNVHQ4EFgQUPUz+a0VN1N8nA72hR3ZbbXPfab4wDQYJKoZIhvcNAQELBQADggEBALnkfjYwn8XU5/XunbcIT0HLGUBYN7yN1QVz9lEQOn4Oj54JcIA0s0iK1mpi12bGGn7QRIq2kpsekyAZmDgKXMZmwclo6do2MOyYdAGd9hBL6ewy0eRSe13OV0H2BvJXfi9+ciRRpImFIXYG217mR4CrRK6pklAkeiDq6zub9GVuz7boLX7qcdX93yVApQ2uBuACAEY/x8DfgKnBkunVuRGdX0hUCnoBAZvm+caD+DpsWNw+4be9IH66LiM0Tg0rSRqKHv21jzCffPeCQQP/nGrykgJL069A+cL+Nx1W/fRczw4jVcz7TJjrktuHQzIqBhwYvfTgSzUXvMCkN6xDOG8=";

            // Act
            var result = await _systemUnderTest.GetAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Tests.IdentityServerMock;
using NDB.Covid19.OAuth2;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace NDB.Covid19.Test.Tests.OAuth2
{
    public class IdentityServerTest : 
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public IdentityServerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("/.well-known/openid-configuration/jwks")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            AuthenticationManager authenticationManager = new AuthenticationManager();
            authenticationManager.client = _factory.CreateClient();
            X509Certificate2 cert = new X509Certificate2(Startup.RsaCertPfxBytes, "12345", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            
            // Act
            //public key from certificate
            //X509Certificate2 certificate is converted to JsonWebKey (with use of JsonWebKeyConverter) from which it is possible 
            //to extract public key X5c. Key is then compared with the one pulled from mocked server. 

            X509SecurityKey rsaSecurityKey = new X509SecurityKey(cert);
            JsonWebKey rsaJwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(rsaSecurityKey);
            string publicKey2 = rsaJwk.X5c[0];
            //response from server
            System.Net.Http.HttpResponseMessage response = await authenticationManager.client.GetAsync(url);
            //public key from endpoint
            string publicKey = authenticationManager.GetPublicKey(url);
            
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(publicKey2, publicKey);
            authenticationManager.client.Dispose();
            _factory.Dispose();
        }
    }
}

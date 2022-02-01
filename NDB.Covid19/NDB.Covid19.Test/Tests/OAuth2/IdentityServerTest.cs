using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Tests.IdentityServerMock;
using NDB.Covid19.OAuth2;
using IdentityModel.Jwk;
using CertificateManager;
using System.IO;

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
            File.WriteAllBytes("rsaCert.pfx", Startup.RsaCertPfxBytes);
            Chilkat.Pfx pfx = new Chilkat.Pfx();
            if (File.Exists("rsaCert.pfx"))
            {
                pfx.LoadPfxFile("rsaCert.pfx", "12345");
            }
           
            File.Delete("rsaCert.pfx");

            // Act
            //public key from certificate
            //Chilkat package is used to process X509 certificate 2 from pfx file to JWKS (Json Web Key Set) from which it is possible 
            //to extract public key X5c. Key is then compared with the one pulled from mocked server. 
            string alias = "my_ecc_key1";
            string password = "secret123";
            
            Chilkat.JavaKeyStore jks = pfx.ToJavaKeyStore(alias, password);
            Chilkat.StringBuilder sbJwkSet = new Chilkat.StringBuilder();
            jks.ToJwkSet(password, sbJwkSet);
            Chilkat.JsonObject jwkSet = new Chilkat.JsonObject();
            jwkSet.LoadSb(sbJwkSet);
            jwkSet.EmitCompact = false;
            var jwksCheck = jwkSet.Emit();
            JsonWebKeySet jwkscheck = new JsonWebKeySet(jwksCheck);
            List<JsonWebKey> keyList2 = new List<JsonWebKey>(jwkscheck.Keys);
            string publicKey2 = keyList2[0].X5c[0];
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

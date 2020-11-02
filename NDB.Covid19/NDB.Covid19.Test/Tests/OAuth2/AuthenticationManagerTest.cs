using NDB.Covid19.OAuth2;
using Xunit;

namespace NDB.Covid19.Test.Tests.OAuth2
{
    public class AuthenticationManagerTest
    {
        public AuthenticationManagerTest() {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public void AuthenticationManager_GetPayloadValidateJWTToken()
        {
            //Valid publicKey, valid token
            AuthenticationManager authenticationManager = new AuthenticationManager();
            //string validToken = ""; //TODO: Generate valid token.
            //string validJson = authenticationManager.GetPayloadValidateJWTToken(validToken);
            //Assert.NotNull(validJson);

            //Valid publickey, Invalid token
            string invalidToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJtSzItbFF2MkNFZDF2MWVraHZrM2FqWWtnUE93ckxJeGFZejV3OUU0cEtZIn0.eyJqdGkiOiIyOTc4NTg1Yi01YjI5LTQ0Y2UtYjNkMC1iMjBkY2E4YzVkZDgiLCJleHAiOjE1OTA0MTc0MDgsIm5iZiI6MCwiaWF0IjoxNTkwNDE3MzQ4LCJpc3MiOiJodHRwczovL29pZGMtdGVzdC5ob3N0ZWQudHJpZm9yay5jb20vYXV0aC9yZWFsbXMvc21pdHRlc3RvcCIsInN1YiI6IjdkMWY4MDYxLWYwZGItNDdlNy05NTk3LTE4ZDZlOTVkYjQ4NSIsInR5cCI6IkJlYXJlciIsImF6cCI6InNtaXR0ZXN0b3AiLCJhdXRoX3RpbWUiOjE1OTA0MTczNDgsInNlc3Npb25fc3RhdGUiOiJmM2NlYjk1Zi1hZmUxLTRkZWYtODhmNi01NmZlNmU1OGJhZDgiLCJhY3IiOiIxIiwic2NvcGUiOiJvcGVuaWQiLCJjb3ZpZDE5X3NtaXR0ZV9zdGFydCI6MTU4OTA2MTYwMCwiY292aWQxOV9ibG9rZXJldCI6ImZhbHNlIiwiY292aWQxOV9zbWl0dGVfc3RvcCI6MTU5MDI3MTIwMCwiY292aWQxOV9zdGF0dXMiOiJwb3NpdGl2In0.EL5JZGpTryktkt_fF_p9VM0oDzIVLjV405XYNc_q9TtPcatuL-LwhXuOqBOgzpV-A7o39PkrYgsdbXx2Ny1JlwLRTct5LRMVLXNMDO75qjvtD9HW02QDa9gAFO4J1fdJ004FPwRV9juOFW9AG6BxM_ffwGstCp0dA96Ft42RvAFyuK4CEWHv_ZbVGnu4tJ6lV7EsaTgedASxkhjFW-4t5Vzoe-jqSBJKg57B2hlaqpqZt36WWEuUXTx5DqbIoyTK5cX6lWoOsi7T8rFBpkfdgdfgNH3mkKQAN1a74MQo4jmgZbGN338hNA5k6cqXUUHV5m1DtKPZvvGePn-OorB8qIl0N31A";
            var invalidJson = authenticationManager.GetPayloadValidateJWTToken(invalidToken);
            Assert.Null(invalidJson);
        }
    }
}

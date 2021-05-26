using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using IdentityServer4Demo.TokenServer;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using NDB.Covid19.OAuth2;
using IdentityModel.Jwk;
using System.IdentityModel.Tokens.Jwt;
using CertificateManager.Models;
using System.Security.Cryptography;
using CertificateManager;
using System.IO;

namespace NDB.Covid19.Test.Tests.OAuth2
{
     public class IdentityServerTest : 
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        static CreateCertificates _cc;
        public static string _path;
        public IdentityServerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        /*public static X509Certificate2 CreateRsaCertificate(
            string dnsName, int validityPeriodInYears)
        {
            var basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            var subjectAlternativeName = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            var x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            var enhancedKeyUsages = new System.Security.Cryptography.OidCollection
                {
                    new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                    new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
                };


            var certificate = _cc.NewRsaSelfSignedCertificate(
                new DistinguishedName { CommonName = dnsName },
                basicConstraints,
                new ValidityPeriod
                {
                    ValidFrom = DateTimeOffset.UtcNow,
                    ValidTo = DateTimeOffset.UtcNow.AddYears(validityPeriodInYears)
                },
                subjectAlternativeName,
                enhancedKeyUsages,
                x509KeyUsageFlags,
                new RsaConfiguration { KeySize = 2048 }
            );

            return certificate;
        }
        public void CreateStoreCert()
        {
            var sp = new ServiceCollection()
                           .AddCertificateManager()
                           .BuildServiceProvider();

            _cc = sp.GetService<CreateCertificates>();

            var rsaCert = CreateRsaCertificate("localhost", 10);

            string password = "1234";
            var iec = sp.GetService<ImportExportCertificate>();
            
            var rsaCertPfxBytes =
                iec.ExportSelfSignedCertificatePfx(password, rsaCert);
            //File.WriteAllBytes("rsaCert.pfx", rsaCertPfxBytes);
            certificate = new X509Certificate2(rsaCertPfxBytes);

            //X509Store store = new X509Store("teststore", StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadWrite);
            //store.Add(certificate1);
            //store.Close();
        }*/
        [Theory]
        [InlineData("/.well-known/openid-configuration/jwks")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            AuthenticationManager authenticationManager = new AuthenticationManager();
            //CreateStoreCert();
            // X509Store store = new X509Store("teststore", StoreLocation.CurrentUser, OpenFlags.OpenExistingOnly);
            //X509Certificate2Collection collection = store.Certificates;
            //X509Certificate2 certificate1 = collection.

            Chilkat.Pfx pfx = new Chilkat.Pfx();
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // This will get the current PROJECT directory
            string projectDirectory2 = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            _path = projectDirectory2 + "\\Tests\\OAuth2\\rsaCert.pfx";
            
            pfx.LoadPfxFile(_path, "1234");
            //string path = Directory.GetCurrentDirectory();
            authenticationManager.client = _factory.CreateClient();
            

            // Act
            //public key from local cert
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
            //public key from endpoint
            var response = await authenticationManager.client.GetAsync(url);
            string publicKey =await authenticationManager.GetPublickey(url);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(publicKey2, publicKey);

        }
    }
}

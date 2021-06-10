using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CertificateManager;
using System.Security.Cryptography.X509Certificates;
using CertificateManager.Models;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Tests.IdentityServerMock
{
    public class Startup
    {
        static CreateCertificates _cc;
        private X509Certificate2 Certificate { get; set; }
        private X509Certificate2 OldCertificate { get; set; }
        public static byte [] RsaCertPfxBytes { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(Data.ResourceManager.Apis)
                    .AddInMemoryClients(Data.ClientManager.Clients);
            CreateCert();
            AddOldKey(services);
            AddKey(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
        public static X509Certificate2 CreateRsaCertificate(
            string dnsName, int validityPeriodInYears)
        {
            BasicConstraints basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            SubjectAlternativeName subjectAlternativeName = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            X509KeyUsageFlags x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            OidCollection enhancedKeyUsages = new System.Security.Cryptography.OidCollection
                {
                    new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                    new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
                };


            X509Certificate2 certificate = _cc.NewRsaSelfSignedCertificate(
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
        public void CreateCert()
        {
            ServiceProvider sp = new ServiceCollection()
                           .AddCertificateManager()
                           .BuildServiceProvider();

            _cc = sp.GetService<CreateCertificates>();

            X509Certificate2 oldRsaCert = CreateRsaCertificate("localhost_IS_test_old", 1);

            X509Certificate2 rsaCert = CreateRsaCertificate("localhost_IS_test", 10);

            string password = "12345";
            ImportExportCertificate iec = sp.GetService<ImportExportCertificate>();

            RsaCertPfxBytes =
                iec.ExportSelfSignedCertificatePfx(password, rsaCert);
            byte[] OldRsaCertPfxBytes = 
                iec.ExportSelfSignedCertificatePfx(password, oldRsaCert);

            Certificate = new X509Certificate2(RsaCertPfxBytes, password);

            OldCertificate = new X509Certificate2(OldRsaCertPfxBytes, password);

        }
        public void AddOldKey(IServiceCollection services)
        {
            IIdentityServerBuilder builder = services.AddIdentityServerBuilder();
            builder.AddValidationKey(OldCertificate);
        }
        public void AddKey(IServiceCollection services)
        {
            IIdentityServerBuilder builder = services.AddIdentityServerBuilder();
            builder.AddValidationKey(Certificate);
        }
    }
}


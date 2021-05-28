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
            AddKey(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)//, IWebHostEnvironment env)
        {
            app.UseIdentityServer();
        }
        public static X509Certificate2 CreateRsaCertificate(
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
        public void CreateCert()
        {
            var sp = new ServiceCollection()
                           .AddCertificateManager()
                           .BuildServiceProvider();

            _cc = sp.GetService<CreateCertificates>();

            var rsaCert = CreateRsaCertificate("localhost_IS_test", 10);

            string password = "12345";
            var iec = sp.GetService<ImportExportCertificate>();

            RsaCertPfxBytes =
                iec.ExportSelfSignedCertificatePfx(password, rsaCert);
            Certificate = new X509Certificate2(RsaCertPfxBytes, password);

        }
        public void AddKey(IServiceCollection services)
        {

            var builder = services.AddIdentityServerBuilder();
            builder.AddValidationKey(Certificate);
        }
    }
}


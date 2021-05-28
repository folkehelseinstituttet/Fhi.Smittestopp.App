using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using System;

namespace Tests.IdentityServerMock
{
    public class Startup
    {
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

            AddKey(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)//, IWebHostEnvironment env)
        {
            app.UseIdentityServer();
        }
        public static void AddKey(IServiceCollection services)
        {

            string workingDirectory = Environment.CurrentDirectory;

            string projectDirectory2 = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory2 + "\\Tests\\OAuth2\\rsaCert.pfx";
            var rsaCertificate = new X509Certificate2(
                path, "1234");
            
            var builder = services.AddIdentityServerBuilder();
            builder.AddValidationKey(rsaCertificate);
        }
    }
}


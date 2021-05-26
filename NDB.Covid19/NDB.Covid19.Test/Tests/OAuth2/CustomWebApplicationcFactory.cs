using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4Demo.TokenServer;

namespace NDB.Covid19.Test.Tests.OAuth2
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.UseUrls("http://localhost:5001/");
            //builder.UseKestrel();
            //builder.UseIISIntegration();
            //builder.Build();
        }
       
    }
}

using IdentityServer4.Models;
using System.Collections.Generic;

namespace Tests.IdentityServerMock.Data
{
    internal static class ResourceManager
    {
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource {
                    Name = "app.api.whatever",
                    DisplayName = "Whatever Apis",
                    ApiSecrets = { new Secret("a75a559d-1dab-4c65-9bc0-f8e590cb388d".Sha256()) }
             
                },
                new ApiResource("app.api.weather","Whatever Apis")
            };
    }
}

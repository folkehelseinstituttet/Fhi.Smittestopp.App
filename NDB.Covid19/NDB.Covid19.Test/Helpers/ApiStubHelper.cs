using NDB.Covid19.Configuration;
using WireMock.Server;
using WireMock.Settings;

namespace NDB.Covid19.Test.Helpers
{
    public class ApiStubHelper
    {
        public static WireMockServer StubServer;
        public static string StubServerUrl { get; private set; }

        public static void StartServer()
        {
            StubServerUrl = Conf.BASE_URL.Remove(Conf.BASE_URL.Length - 1, 1).Replace("/api/", "/");
            StubServer = WireMockServer.Start(new FluentMockServerSettings()
            {
                Urls = new string[] { StubServerUrl }
            });
        }

        public static void StopServer()
        {
            StubServer.Stop();
        }
    }
}

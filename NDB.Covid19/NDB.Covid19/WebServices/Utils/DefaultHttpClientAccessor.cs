using System;
using System.Net;
using System.Net.Http;

namespace NDB.Covid19.WebServices.Utils
{
    public interface IHttpClientAccessor
    {
        HttpClient HttpClient { get; }
        CookieContainer Cookies { get; }
    }

    public class DefaultHttpClientAccessor : IHttpClientAccessor
    {
        public HttpClient HttpClient { get; }
        public CookieContainer Cookies { get; }

        public DefaultHttpClientAccessor()
        {
            Cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = Cookies;

#if (TEST || APPCENTER)
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
#endif

            HttpClient = new HttpClient(handler);
            HttpClient.Timeout = TimeSpan.FromSeconds(10);
        }
    }
}

using System;
using System.Net.Http.Headers;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.Utils;
using NDB.Covid19.Interfaces;
using Xamarin.Essentials;

namespace NDB.Covid19.WebServices.Utils
{
    public class HttpClientManager
    {
        public static string CsrfpTokenCookieName = "Csrfp-Token";
        public static string CsrfpTokenHeader = "csrfp-token";
        public IHttpClientAccessor HttpClientAccessor;
        static HttpClientManager _instance;

        HttpClientManager()
        {
            HttpClientAccessor = new DefaultHttpClientAccessor();
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Authorization_Mobile", Conf.AUTHORIZATION_HEADER);
            HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("User-Agent", Conf.USER_AGENT_HEADER);

            // If running on a platform that is not supported by Xamarin.Essentials (for example if unit testing)
            IDeviceInfo deviceInfo = ServiceLocator.Current.GetInstance<IDeviceInfo>();
            if (deviceInfo.Platform == DevicePlatform.Unknown)
            {
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Manufacturer", "Unknown");
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OSVersion", "Unknown");
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OS", "Unknown");
            }
            else
            {
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("Manufacturer", deviceInfo.Manufacturer);
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OSVersion", deviceInfo.VersionString);
                HttpClientAccessor.HttpClient.DefaultRequestHeaders.Add("OS", DeviceUtils.DeviceType);
            }
            HttpClientAccessor.HttpClient.MaxResponseContentBufferSize = 3000000; 
            HttpClientAccessor.HttpClient.Timeout = TimeSpan.FromSeconds(Conf.DEFAULT_TIMEOUT_SERVICECALLS_SECONDS);
        }

        public static HttpClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    MakeNewInstance();
                }
                return _instance;
            }
        }

        public void AddSecretToHeaderIfMissing()
        {
            HeaderUtils.AddSecretToHeader(HttpClientAccessor);
        }

        public static void MakeNewInstance()
        {
            if (_instance?.HttpClientAccessor?.HttpClient != null)
            {
                _instance.HttpClientAccessor.HttpClient.CancelPendingRequests();
            }
            _instance = new HttpClientManager();
        }

        public bool CheckInternetConnection()
        {
            return ServiceLocator.Current.GetInstance<IConnectivity>().NetworkAccess != NetworkAccess.None;
        }
    }
}

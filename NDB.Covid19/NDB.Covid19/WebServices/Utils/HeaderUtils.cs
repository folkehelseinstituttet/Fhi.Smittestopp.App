using NDB.Covid19.OAuth2;
using NDB.Covid19.WebServices.Utils;

namespace NDB.Covid19.WebServices
{
    public static class HeaderUtils
    {
        public static void AddSecretToHeader(IHttpClientAccessor accessor)
        {
            if (accessor.HttpClient.DefaultRequestHeaders.Contains("Authorization")) {
                accessor.HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            if (AuthenticationState.PersonalData != null && AuthenticationState.PersonalData.Validate())
            {
                string access_token = AuthenticationState.PersonalData?.Access_token;
                accessor.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
            }
        }
    }
}

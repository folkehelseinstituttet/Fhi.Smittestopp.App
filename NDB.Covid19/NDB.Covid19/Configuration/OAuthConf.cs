namespace NDB.Covid19.Configuration
{
    public static class OAuthConf
    {
        public static string OAUTH2_CLIENT_ID = "smittestopp";
        public static string OAUTH2_SCOPE = "openid smittestop";
        public static string OAUTH2_REDIRECT_URL = "no.fhi.smittestopp-exposure-notification:/oauth2redirect";
        public static string OAUTH2_AUTHORISE_URL = "http://localhost:5001/connect/authorize";
        public static string OAUTH2_ACCESSTOKEN_URL = "http://localhost:5001/connect/token";
        public static string OAUTH2_ANONTOKEN_URL = "http://localhost:5001/api/anonymousTokens";
        public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = "INJECTED_IN_APP_CENTER_DURING_BUILD";
    }
}

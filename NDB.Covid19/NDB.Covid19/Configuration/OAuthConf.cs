namespace NDB.Covid19.Configuration
{
    public static class OAuthConf
    {
        public static string OAUTH2_CLIENT_ID = "smittestopp";
        public static string OAUTH2_CONFIRMED_TEST_SCOPE = "openid smittestop";
        public static string OAUTH2_SELF_DIAGNOSIS_SCOPE = "openid smittestop no-msis";
        public static string OAUTH2_REDIRECT_URL = "no.fhi.smittestopp-exposure-notification:/oauth2redirect";

        public static string OAUTH2_BASE_URL = "http://localhost:5001/";
        public static string OAUTH2_AUTHORISE_URL => OAUTH2_BASE_URL + "connect/authorize";
        public static string OAUTH2_ACCESSTOKEN_URL => OAUTH2_BASE_URL + "connect/token";
        public static string OAUTH2_ANONTOKEN_URL => OAUTH2_BASE_URL + "api/anonymousTokens";

        public static string OAUTH2_JWKS_URL => OAUTH2_BASE_URL + ".well-known/openid-configuration/jwks";
    }
}

namespace NDB.Covid19.Config
{
    public static class OAuthConf
    {
        public static string OAUTH2_CLIENT_ID = "smittestopp";
        public static string OAUTH2_SCOPE = "openid";
        public static string OAUTH2_REDIRECT_URL = "no.fhi.smittestopp:/oauth2redirect";

#if TEST
        //Dev links // TODO
        public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO

        ////Test links // TODO
        //public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        //public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        //public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO

        ////Prod links // TODO
        //public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        //public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        //public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO

#elif UNIT_TEST
        //DEV
        public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO

#elif APPCENTER
        //DEV
        public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO

#elif RELEASE
        //Prod
        public static string OAUTH2_AUTHORISE_URL = ""; // TODO
        public static string OAUTH2_ACCESSTOKEN_URL = ""; // TODO
        public static string OAUTH2_VERIFY_TOKEN_PUBLIC_KEY = ""; // TODO
#endif
    }

}

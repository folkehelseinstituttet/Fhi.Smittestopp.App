using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;

namespace NDB.Covid19.OAuth2
{
    public static class AuthenticationState
    {
        public static CustomOAuth2Authenticator Authenticator;
        public static PersonalDataModel PersonalData;
    }
}

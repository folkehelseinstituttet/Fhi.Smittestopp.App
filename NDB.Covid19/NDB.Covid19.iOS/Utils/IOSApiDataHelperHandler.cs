using NDB.Covid19.Interfaces;

namespace NDB.Covid19.iOS.Utils
{
    class IOSApiDataHelperHandler : IApiDataHelper
    {
        public bool IsGoogleServiceEnabled()
        {
            return false;
        }

        public string GetBackGroundServicVersionLogString()
        {
            return "";
        }
    }
}
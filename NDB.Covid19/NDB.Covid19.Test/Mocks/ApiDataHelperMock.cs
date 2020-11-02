using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Test.Mocks
{
    class ApiDataHelperMock : IApiDataHelper
    {

        public bool IsGoogleServiceEnabled()
        {
            return false;
        }

        public string GetBackGroundServicVersionLogString()
        {
            return " (GPS: Mock version)";
        }
    }
}

namespace NDB.Covid19.Interfaces
{
    public interface IApiDataHelper
    {
        bool IsGoogleServiceEnabled();
        string GetBackGroundServicVersionLogString();
    }
}

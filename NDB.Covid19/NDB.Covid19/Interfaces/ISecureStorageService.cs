namespace NDB.Covid19.Interfaces
{
    public interface ISecureStorageService
    {
        Plugin.SecureStorage.Abstractions.ISecureStorage SecureStorage { get; }
    }
}

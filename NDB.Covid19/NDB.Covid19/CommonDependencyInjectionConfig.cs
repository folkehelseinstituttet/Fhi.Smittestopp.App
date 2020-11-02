using NDB.Covid19.Config;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.SecureStorage;
using NDB.Covid19.Utils;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Implementation;
using NDB.Covid19.WebServices;
using NDB.Covid19.WebServices.Utils;
using Unity;

namespace NDB.Covid19
{
    public static class CommonDependencyInjectionConfig
    {
        public static void Init(UnityContainer unityContainer)
        {
            unityContainer.RegisterType<ILoggingManager, LoggingSQLiteManager>();
            unityContainer.RegisterType<IMessagesManager, MessagesManager>();
            unityContainer.RegisterSingleton<SecureStorageService>();

            if (Conf.UseDeveloperTools)
            {
                unityContainer.RegisterType<IDeveloperToolsService, DeveloperToolsService>();
            }
            else
            {
                unityContainer.RegisterType<IDeveloperToolsService, ReleaseToolsService>();
            }
            XamarinEssentialsRegister(unityContainer);
        }

        private static void XamarinEssentialsRegister(UnityContainer unityContainer)
        {
            unityContainer.RegisterType<IConnectivity, ConnectivityImplementation>();
            unityContainer.RegisterType<IAppInfo, AppInfoImplementation>();
            unityContainer.RegisterType<IBrowser, BrowserImplementation>();
            unityContainer.RegisterType<IClipboard, ClipboardImplementation>();
            unityContainer.RegisterType<IDeviceInfo, DeviceInfoImplementation>();
            unityContainer.RegisterType<IFileSystem, FileSystemImplementation>();
            unityContainer.RegisterType<IPreferences, PreferencesImplementation>();
            unityContainer.RegisterType<IShare, ShareImplementation>();
        }
    }
}

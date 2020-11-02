using NDB.Covid19.Utils;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.SecureStorage;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Implementation;
using Unity;
using Unity.Lifetime;
using Unity.ServiceLocation;

namespace NDB.Covid19.Test
{
    public static class DependencyInjectionConfig
    {
        public static UnityContainer unityContainer;

        public static void Init()
        {
            unityContainer = new UnityContainer();
            unityContainer.EnableDebugDiagnostic();
            unityContainer.RegisterSingleton<SecureStorageService>();
            unityContainer.RegisterSingleton<IApiDataHelper, ApiDataHelperMock>();
            unityContainer.RegisterSingleton<ILoggingManager, LoggingManagerMock>();
            unityContainer.RegisterSingleton<IMessagesManager, MessagesManager>();
            unityContainer.RegisterSingleton<ILocalNotificationsManager, LocalNotificationManagerMock>();
            unityContainer.RegisterSingleton<IDeveloperToolsService, DeveloperToolsService>();
            XamarinEssentialsRegister(unityContainer);
            UnityServiceLocator unityServiceLocalter = new UnityServiceLocator(unityContainer);
            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => unityServiceLocalter);
        }

        private static void XamarinEssentialsRegister(UnityContainer unityContainer)
        {
            unityContainer.RegisterSingleton<IConnectivity, TestConnectivityMocks>();
            unityContainer.RegisterSingleton<IPreferences, TestPreferencesMock>();
            unityContainer.RegisterSingleton<IAppInfo, TestsAppInfoMocks>();
            unityContainer.RegisterSingleton<IFileSystem, TestsFileSystemMocks>();

            // TODO Those implementation may be wrong for our tests and may need refactoring
            unityContainer.RegisterSingleton<IBrowser, BrowserImplementation>();
            unityContainer.RegisterSingleton<IClipboard, ClipboardImplementation>();
            unityContainer.RegisterSingleton<IDeviceInfo, DeviceInfoImplementation>();
            unityContainer.RegisterSingleton<IShare, ShareImplementation>();
        }
    }
}

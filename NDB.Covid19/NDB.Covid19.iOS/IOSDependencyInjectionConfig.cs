using NDB.Covid19.iOS.Utils;
using NDB.Covid19.WebServices.ErrorHandlers;
using Unity;
using Unity.ServiceLocation;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Managers;
using NDB.Covid19.iOS.Permissions;
using Unity.Lifetime;

namespace NDB.Covid19.iOS
{
    public static class IOSDependencyInjectionConfig
    {
        public static UnityContainer unityContainer;

        public static void Init()
        {
            unityContainer = new UnityContainer();
            unityContainer.RegisterType<IDialogService, IOSDialogService>();
            unityContainer.RegisterType<ILocalNotificationsManager, iOSLocalNotificationsManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IApiDataHelper, IOSApiDataHelperHandler>();
            unityContainer.RegisterSingleton<IResetViews, IOSResetViews>();
            unityContainer.RegisterType<IPermissionsHelper, IOSPermissionsHelper>();

            CommonDependencyInjectionConfig.Init(unityContainer);
           
            UnityServiceLocator unityServiceLocalter = new UnityServiceLocator(unityContainer);
            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => unityServiceLocalter);
        }
    }
}

using NDB.Covid19.Droid.Utils;
using NDB.Covid19.WebServices.ErrorHandlers;
using Unity;
using Unity.ServiceLocation;
using NDB.Covid19.Droid.Services;
using Unity.Lifetime;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Droid
{
    public static class DroidDependencyInjectionConfig
    {
        public static UnityContainer unityContainer;

        public static void Init()
        {
            unityContainer = new UnityContainer();
            unityContainer.RegisterType<IApiDataHelper, DroidApiDataHelperHandler>();
            unityContainer.RegisterType<IDialogService, DroidDialogService>();
            unityContainer.RegisterType<ILocalNotificationsManager, LocalNotificationsManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterSingleton<IResetViews, DroidResetViews>();
            unityContainer.RegisterType<IPermissionsHelper, PermissionUtils>();

            CommonDependencyInjectionConfig.Init(unityContainer);
            UnityServiceLocator unityServiceLocalter = new UnityServiceLocator(unityContainer);
            CommonServiceLocator.ServiceLocator.SetLocatorProvider(() => unityServiceLocalter);
        }
    }
}

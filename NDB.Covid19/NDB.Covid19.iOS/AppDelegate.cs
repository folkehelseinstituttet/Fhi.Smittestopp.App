using System;
using System.Diagnostics;
using CommonServiceLocator;
using Foundation;
using NDB.Covid19.iOS.Views.MessagePage;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using Plugin.SecureStorage;
using UIKit;
using UserNotifications;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Managers;
using NDB.Covid19.iOS.Views.InfectionStatus;

#if APPCENTER
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace NDB.Covid19.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {

        [Export("window")]
        public UIWindow Window { get; set; }

        private iOSLocalNotificationsManager _notifMgn;

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
#if APPCENTER
            AppCenter.Start(
                Configuration.Conf.APPCENTER_DIAGNOSTICS_TOKEN,
                typeof(Analytics), typeof(Crashes));
#endif

            IOSDependencyInjectionConfig.Init();
            LocalesService.Initialize();
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            new MigrationService().Migrate();

            LogUtils.SendAllLogs();
            AppDomain.CurrentDomain.UnhandledException += LogUtils.OnUnhandledException;

            SecureStorageImplementation.DefaultAccessible = Security.SecAccessible.AfterFirstUnlockThisDeviceOnly;

            HandleLocalNotifications();

            BackgroundServiceHandler.PlatformScheduleFetch();

            UIUserNotificationSettings notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            );
            application.RegisterUserNotificationSettings(notificationSettings);
            application.BeginBackgroundTask("showNotification", () => { });

            return true;
        }
        
        void HandleLocalNotifications()
        {
            // Request notification permissions from the user. Dialog will only show if user has not already answered
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge, (approved, err) => {
                Console.WriteLine("Notifications approve = " + approved.ToString());
            });

            _notifMgn = ServiceLocator.Current.GetInstance<ILocalNotificationsManager>() as iOSLocalNotificationsManager;

            // Watch for notifications while the app is active
            UNUserNotificationCenter.Current.Delegate = _notifMgn;
            _notifMgn.OnNotificationTappedHandler += HandleNotificationTapped;
        }

        /// <summary>
		/// We check for NotificationHasBeenTapped, and if so we segue into the messages module
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void HandleNotificationTapped(object sender, string e)
        {
            UIViewController topController = NavigationHelper.TopController();

            if (OnboardingStatusHelper.Status != Enums.OnboardingStatus.CountriesOnboardingCompleted)
            {
                new IOSResetViews().ResetViews();
                return;
            }

            if (topController is MessagePageViewController && e == iOSLocalNotificationsManager.NewMessageIdentifier)
            {
                return;
            }
            if (topController is InfectionStatusViewController && e == iOSLocalNotificationsManager.NewNotificationIdentifier)
            {
                return;
            }
            
            UINavigationController vc =
                e == iOSLocalNotificationsManager.NewMessageIdentifier
                    ? MessagePageViewController.GetMessagePageControllerInNavigationController()
                    : InfectionStatusViewController.GetInfectionSatusPageControllerInNavigationController();
            topController.PresentViewController(vc, true, null);
        }

        [Export("applicationDidEnterBackground:")]
        public void DidEnterBackground(UIApplication application)
        {
            Debug.WriteLine("DidEnterBackground called");
        }

        [Export("applicationWillEnterForeground:")]
        public void WillEnterForeground(UIApplication application)
        {
            Debug.WriteLine("Den hoppede i forgrunden");
        }
        // UISceneSession Lifecycle

        [Export("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create("Default Configuration", connectingSceneSession.Role);
        }

        [Export("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions(UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }

        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        [Export("application:performFetchWithCompletionHandler:")]
        public void PerformFetch(UIApplication application, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}


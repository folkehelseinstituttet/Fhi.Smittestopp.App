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
using NDB.Covid19.OAuth2;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using NDB.Covid19.Enums;

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
        public static bool ShouldOperateIn12_5Mode
        {
            get
            {
                return !UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
            }
        }

        public static bool DidEnterBackgroundState { get; private set; }


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

            LayoutUtils.OnLayoutDirectionChange();

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

        // Below are AppDelegate application lifecycle-methods that are callen in pre-iOS 13.
        // In  iOS 13 SceneDelegate is handling this.

        /// <summary>
        /// Method that is used before iOS 13 to detect application entering background.
        /// Corresponds to iOS 13+ SceneDelegate's DidEnterBackground(UIScene:) method.
        /// </summary>
        [Export("applicationDidEnterBackground:")]
        public void DidEnterBackground(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.DidEnterBackground called");

            DidEnterBackgroundState = true;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_WILL_ENTER_BACKGROUND);
        }

        /// <summary>
        /// Method that is used before iOS 13 to detect application returning to foreground from background.
        /// Corresponds to iOS 13+ SceneDelegate's WillEnterForeground(UIScene:) method.
        /// </summary>
        [Export("applicationWillEnterForeground:")]
        public void WillEnterForeground(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.WillEnterForeground called");

            LogUtils.LogMessage(LogSeverity.INFO, "The user has opened the app", null, GetCorrelationId());

            DidEnterBackgroundState = false;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND);
        }

        /// <summary>
        /// Method that is used before iOS 13 to detect application has become active.
        /// Corresponds to iOS 13+ SceneDelegate's DidBecomeActive(UIScene:) method.
        /// </summary>
        [Export("applicationDidBecomeActive:")]
        public void DidBecomeActive(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.DidBecomeActive called");

            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
        }

        /// <summary>
        /// Method that is used before iOS 13 to detect application is about to become inactive.
        /// Corresponds to iOS 13+ SceneDelegate's WillResignActive(UIScene:) method.
        /// </summary>
        [Export("applicationWillResignActive:")]
        public void WillResignActive(UIApplication application)
        {
            Debug.WriteLine("AppDelegate.DidBecomeActive called");

            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        /// <summary>
        /// Method that is used before iOS 13 to request application open a resource specified by a URL.
        /// Corresponds to iOS 13+ SceneDelegate's OpenUrlContexts(scene:urlContexts:) method.
        /// </summary>
        [Export("application:openURL:options:")]
        public void OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            Debug.WriteLine("AppDelegate.OpenUrl called");

            try
            {
                Uri uri_netfx = new Uri(url.AbsoluteString);
                AuthenticationState.Authenticator.OnPageLoading(uri_netfx);
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.WARNING, e, $"{nameof(AppDelegate)}.{nameof(OpenUrl)}: Error on redirecting from ID-porten [iOS 12.5 mode]");
            }
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
            return UIInterfaceOrientationMask.All;
        }

        [Export("application:performFetchWithCompletionHandler:")]
        public void PerformFetch(UIApplication application, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        [Export("applicationWillTerminate:")]
        public void AppWillTerminate(UIApplication application)
        {
            string correlationId = GetCorrelationId();
            if(!string.IsNullOrEmpty(correlationId))
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user closed the app", null, correlationId);
            }
            LogUtils.SendAllLogs();
        }
    }
}


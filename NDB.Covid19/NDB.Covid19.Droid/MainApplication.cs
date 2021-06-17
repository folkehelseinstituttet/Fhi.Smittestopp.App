using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.LocalBroadcastManager.Content;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using NDB.Covid19.Droid.Services;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.Droid.Utils.BatteryOptimisationUtils;
#if APPCENTER
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace NDB.Covid19.Droid
{
    [Application]
    class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private BroadcastReceiver _permissionsBroadcastReceiver;
        private FlightModeHandlerBroadcastReceiver _flightModeBroadcastReceiver;
        private IntentFilter _filter;
        private BackgroundNotificationBroadcastReceiver _backgroundNotificationBroadcastReceiver;

        private int _activityReferences = 0;
        private bool _isActivityChangingConfigurations = false;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public MainApplication()
        {
        }

        void Init()
        {
            _filter = new IntentFilter();
            _filter.AddAction("android.bluetooth.adapter.action.STATE_CHANGED");
            _filter.AddAction("android.location.PROVIDERS_CHANGED");

            AppDomain.CurrentDomain.UnhandledException += LogUtils.OnUnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += OnUnhandledAndroidException;

#if APPCENTER
            AppCenter.Start(
                Configuration.Conf.APPCENTER_DIAGNOSTICS_TOKEN,
                typeof(Analytics), typeof(Crashes));
#endif

            DroidDependencyInjectionConfig.Init();
            Xamarin.Essentials.Platform.Init(this);
            Current.Init(this);
            LocalesService.Initialize();

            new MigrationService().Migrate();

            _permissionsBroadcastReceiver = new PermissionsBroadcastReceiver();
            _flightModeBroadcastReceiver = new FlightModeHandlerBroadcastReceiver();
            _backgroundNotificationBroadcastReceiver = new BackgroundNotificationBroadcastReceiver();

            LogUtils.SendAllLogs();

            Xamarin.Essentials.VersionTracking.Track();

            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                BackgroundFetchScheduler.ScheduleBackgroundFetch();
            }

            LogUtils.LogMessage(LogSeverity.INFO, $"The user has opened the app", null);
        }

        private void OnUnhandledAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            if (e?.Exception != null)
            {
                string correlationId = GetCorrelationId();
                if (!string.IsNullOrEmpty(correlationId))
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "The user has experienced native Android crash",
                        null,
                        correlationId);
                }

                string message = $"{nameof(MainApplication)}.{nameof(OnUnhandledAndroidException)}: "
                                 + (!e.Handled
                                     ? "Native unhandled crash"
                                     : "Native unhandled exception - not crashing");

                LogSeverity logLevel = e.Handled
                    ? LogSeverity.WARNING
                    : LogSeverity.ERROR;

                LogUtils.LogException(logLevel, e.Exception, message);
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Init();
            RegisterActivityLifecycleCallbacks(this);
            ManualGarbageCollectionTool();
            RegisterReceiver(_permissionsBroadcastReceiver, _filter);
            RegisterReceiver(_flightModeBroadcastReceiver, new IntentFilter("android.intent.action.AIRPLANE_MODE"));
            LocalBroadcastManager
                .GetInstance(ApplicationContext)
                .RegisterReceiver(
                    _backgroundNotificationBroadcastReceiver,
                    new IntentFilter(LocalNotificationsManager.BroadcastActionName));
            IsAppLaunchedToShowDialog = true;
            IsAppLaunchedToPullKeys = true;
        }

        public override void OnTerminate()
        {
            UnregisterReceiver(_permissionsBroadcastReceiver);
            UnregisterReceiver(_flightModeBroadcastReceiver);
            LocalBroadcastManager
                .GetInstance(ApplicationContext)
                .UnregisterReceiver(_backgroundNotificationBroadcastReceiver);
            base.OnTerminate();
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            AccessibilityUtils.AdjustFontScale(activity);

            MessagingCenter.Subscribe<object>(
                activity,
                MessagingCenterKeys.KEY_FORCE_UPDATE,
                o => OnForceUpdate(activity));
        }

        public void OnActivityDestroyed(Activity activity)
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }

        public void OnActivityPaused(Activity activity)
        {

        }

        public void OnActivityResumed(Activity activity)
        {

        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {

        }

        public void OnActivityStarted(Activity activity)
        {
            ++_activityReferences;
            if (_activityReferences == 1 && !_isActivityChangingConfigurations)
            {
                // Log LoadPageActivity entered foreground after being put into background
                // because onResume() in LoadPageActivity is not called due to the pop-up window on the activity
                if (activity is Views.AuthenticationFlow.LoadingPageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
                }
            }
        }

        public void OnActivityStopped(Activity activity)
        {
            // check if the app entered background
            _isActivityChangingConfigurations = activity.IsChangingConfigurations;
            if (--_activityReferences == 0 && !_isActivityChangingConfigurations)
            {
                if(activity is Views.AuthenticationFlow.CountriesConsentActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Countries Consent", null, GetCorrelationId());
                }
                else if(activity is Views.AuthenticationFlow.InformationAndConsentActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Information and Consent", null);
                }
                else if(activity is Views.AuthenticationFlow.LoadingPageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Loading Page", null, GetCorrelationId());
                }
                else if (activity is Views.AuthenticationFlow.QuestionnaireCountriesSelectionActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire Countries Selection", null, GetCorrelationId());
                }
                else if (activity is Views.AuthenticationFlow.QuestionnairePageActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire", null, GetCorrelationId());
                }
                else if(activity is Views.AuthenticationFlow.RegisteredActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left Registered");
                }
                else if(activity is Views.AuthenticationFlow.ErrorActivities.GeneralErrorActivity)
                {
                    LogUtils.LogMessage(LogSeverity.INFO, "The user left General Error");
                }
            }
        }

        void ManualGarbageCollectionTool()
        {
            #region MANUALLY GC
            // TODO: For memory management purpose this is saved to uncomment when needing constantly garbage collection
            //var constantGC = new System.Timers.Timer()
            //{
            //    Interval = 1000,
            //    AutoReset = true,
            //    Enabled = true
            //};
            //constantGC.Elapsed += GarbageCollect;
            #endregion MANUALLY GC
        }
        
        void OnForceUpdate(Activity activity)
        {
            activity.RunOnUiThread(() =>
            {
                Intent intent = new Intent(this, typeof(ForceUpdateActivity));
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask | ActivityFlags.ClearTop);
                StartActivity(intent);
            });
            activity.Finish();
        }
    }
}
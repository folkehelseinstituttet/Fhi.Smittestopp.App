using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using NDB.Covid19.Droid.Services;
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

            LogUtils.SendAllLogs();

            if (PlayServicesVersionUtils.PlayServicesVersionNumberIsLargeEnough(PackageManager))
            {
                BackgroundFetchScheduler.ScheduleBackgroundFetch();
            }
        }

        private void OnUnhandledAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            if (e?.Exception != null)
            {
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
        }

        public override void OnTerminate()
        {
            UnregisterReceiver(_permissionsBroadcastReceiver);
            UnregisterReceiver(_flightModeBroadcastReceiver);
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

        }

        public void OnActivityStopped(Activity activity)
        {

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
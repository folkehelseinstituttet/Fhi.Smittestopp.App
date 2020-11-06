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
            Init();
        }

        public MainApplication()
        {
            Init();
        }

        void Init()
        {
            _filter = new IntentFilter();
            _filter.AddAction("android.bluetooth.adapter.action.STATE_CHANGED");
            _filter.AddAction("android.location.PROVIDERS_CHANGED");

            AppDomain.CurrentDomain.UnhandledException += LogUtils.OnUnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += OnUnhandledAndroidException;

            
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

            MessagingCenter.Subscribe<object>(activity, MessagingCenterKeys.KEY_FORCE_UPDATE, (object obj) =>
            {
                OnForceUpdate(activity);
            });
        }

        public void OnActivityDestroyed(Activity activity)
        {
            MessagingCenter.Unsubscribe<object>(activity, MessagingCenterKeys.KEY_FORCE_UPDATE);
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

        void OnForceUpdate(Activity fromActivity)
        {
            Intent intent = new Intent(fromActivity, typeof(ForceUpdateActivity));
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            fromActivity.StartActivity(intent);
        }
    }
}
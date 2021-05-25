using Android.App;
using Android.Content;
using Android.OS;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Droid.Services
{
    [Service(Enabled = true, Exported = false)]
    public class RemovedFromRecentDetectorService : Service
    {
        public static bool IsRunning { get; set; }

        public override void OnCreate()
        {
            base.OnCreate();
            IsRunning = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            IsRunning = false;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
            try
            {
                string correlationId = GetCorrelationId();
                if (!string.IsNullOrEmpty(correlationId))
                {
                    LogUtils.LogMessage(
                        LogSeverity.INFO,
                        "The user has closed the app",
                        null,
                        correlationId);
                }

                LogUtils.SendAllLogs();
            }
            catch
            {
                // ignore
            }
        }
    }
}

using Android.App;
using Android.Content;
using Android.OS;
using System;
using Android.Runtime;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.Services
{
    [Service]
    public class TimedReminderForegroundService : Service
    {

        private TimedReminderCountdownTimer _timedReminderCountdownTimer;

        
       public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            
            if (intent.GetBooleanExtra("close", false))
            {
                StopForeground(true);
                StopSelf();
                return base.OnStartCommand(intent, flags, startId); ;
            }
            long ticks = intent.GetLongExtra("ticks", 0);
            Notification notification =
                new LocalNotificationsManager()
                    .CreateNotificationWithExtraLongData(
                        NotificationsEnum.TimedReminder.Data(),
                        ticks);

            // Enlist this instance of the service as a foreground service
            StartForeground((int)NotificationsEnum.TimedReminder, notification);

            

            try
            {
                _timedReminderCountdownTimer = new TimedReminderCountdownTimer(this, ticks, 1000);
                _timedReminderCountdownTimer.Start();
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING,
                    e,
                    $"{nameof(TimedReminderForegroundService)}-{nameof(StartCommandResult)}: Timer exception");
            }

            return base.OnStartCommand(intent, flags, startId);
        }
        public override void OnDestroy()
        {
            _timedReminderCountdownTimer.Cancel();
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                StopForeground(StopForegroundFlags.Remove);
            }
            else
            {
                StopForeground(true);
            }

            StopSelf();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        class TimedReminderCountdownTimer : CountDownTimer
        {
            private readonly LocalNotificationsManager _lnm = new LocalNotificationsManager();
            private readonly Service _service;
            public TimedReminderCountdownTimer(Service self, long millisInFuture, long countDownInterval) :
                base(millisInFuture, countDownInterval)
            {
                _service = self;
            }

            public TimedReminderCountdownTimer(IntPtr javaReference, JniHandleOwnership transfer) :
                base(javaReference, transfer)
            {
            }

            public TimedReminderCountdownTimer(long millisInFuture, long countDownInterval) :
                base(millisInFuture, countDownInterval)
            {
            }

            public override void OnFinish()
            {
                _lnm.GenerateLocalNotification(
                    NotificationsEnum.TimedReminderFinished.Data(), 0);
                _service.StopForeground(true);
                _service.StopSelf();
            }

            public override void OnTick(long millisUntilFinished)
            {
                _lnm.GenerateDelayedNotification(
                    NotificationsEnum.TimedReminder.Data(), millisUntilFinished);
            }
        }

    }
}
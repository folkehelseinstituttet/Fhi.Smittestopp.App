using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NDB.Covid19.Utils;
using NDB.Covid19.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDB.Covid19.Droid.Utils
{
    public class ForegroundServiceHelper
    {
        public static void StartForegroundServiceCompat<T>(Context context, Bundle args = null) where T : Service
        {
            try
            {
                Intent intent = new Intent(context, typeof(T));
                if (args != null)
                {
                    intent.PutExtras(args);
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    context.StartForegroundService(intent);
                }
                else
                {
                    context.StartService(intent);
                }

            }
            catch (Exception ex)
            {
                LogUtils.LogException(LogSeverity.ERROR, ex,
                    $"Failed to start service {nameof(ForegroundServiceHelper)}.{nameof(StartForegroundServiceCompat)}");
            }
            
        }

        public static void StopForegroundServiceCompat<T>(Context context) where T : Service
        {
            Intent intent = new Intent(context, typeof(T));
            context.StopService(intent);
        }
    }
}
using System;
using NDB.Covid19.Utils;

namespace NDB.Covid19
{
    public static class ExceptionExtensions
    {
        /// <returns>True if the exception is due to EN API not being supported on Android</returns>
        public static bool HandleExposureNotificationException(this Exception e, string className, string methodName)
        {
            if (e.ToString().Contains("Android.Gms.Common.Apis.ApiException: 17"))
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e,
                    className + "." + methodName + ": EN API was not available");
                return true;
            }

            if (e.ToString().Contains("com.google.android.gms.common.api.UnsupportedApiCallException"))
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e,
                    className + "." + methodName + ": EN API call is not supported");
                return true;
            }

            return false;
        }

    }
}

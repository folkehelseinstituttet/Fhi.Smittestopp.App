using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureWindowJsonHelper
    {
        public static string ExposureWindowsToJson(IEnumerable<ExposureWindow> exposureWindows)
        {
            IEnumerable<JsonCompatibleExposureWindow> jsonCompatibleExposureWindows
                = exposureWindows.Select(exposureWindow => new JsonCompatibleExposureWindow(exposureWindow));
            return JsonConvert.SerializeObject(jsonCompatibleExposureWindows);
        }
    }
}

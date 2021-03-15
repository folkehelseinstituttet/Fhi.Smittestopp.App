using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public abstract class ExposureWindowJsonHelper
    {
        public static string ExposureWindowToJson (IEnumerable<ExposureWindow> exposureWindows)
        {
            IEnumerable<JsonCompatibleExposureWindow> jsonCompatibleExposureWindows
                = exposureWindows.Select(exposureWindow => new JsonCompatibleExposureWindow(exposureWindow));
            return JsonConvert.SerializeObject(jsonCompatibleExposureWindows);
        }

        public static IEnumerable<ExposureWindow> ExposureWindowsFromJsonCompatibleString(string jsonCompatibleExposureWindowsJson)
        {
            IEnumerable<JsonCompatibleExposureWindow> jsonCompatibleExposureWindows
                = JsonConvert.DeserializeObject<IEnumerable<JsonCompatibleExposureWindow>>(jsonCompatibleExposureWindowsJson);
            return jsonCompatibleExposureWindows.Select(jsonCompatibleExposureWindow => new ExposureWindow(
                calibrationConfidence: jsonCompatibleExposureWindow.CalibrationConfidence,
                timestamp: jsonCompatibleExposureWindow.Timestamp,
                infectiousness: jsonCompatibleExposureWindow.Infectiousness,
                reportType: jsonCompatibleExposureWindow.ReportType,
                scanInstances: jsonCompatibleExposureWindow.ScanInstances));
        }
    }
}

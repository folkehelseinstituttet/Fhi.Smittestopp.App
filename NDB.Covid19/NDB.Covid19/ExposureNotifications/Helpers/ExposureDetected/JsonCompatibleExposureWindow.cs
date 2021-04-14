using System;
using System.Collections.Generic;
using Xamarin.ExposureNotifications;
namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public class JsonCompatibleExposureWindow
    {
        public CalibrationConfidence CalibrationConfidence { get; set; }
        public DateTime Timestamp { get; set; }
        public Infectiousness Infectiousness { get; set; } 
        public ReportType ReportType { get; set; }
        public IReadOnlyList<ScanInstance> ScanInstances { get; set; }

        public JsonCompatibleExposureWindow()
        {
        }

        public JsonCompatibleExposureWindow(ExposureWindow exposureWindow)
        {
            CalibrationConfidence = exposureWindow.CalibrationConfidence;
            Timestamp = exposureWindow.Timestamp;
            Infectiousness = exposureWindow.Infectiousness;
            ReportType = exposureWindow.ReportType;
            ScanInstances = exposureWindow.ScanInstances;
        }

    }
}

using System;
using System.Collections.Generic;
using Xamarin.ExposureNotifications;
namespace NDB.Covid19.ExposureNotifications.Helpers.ExposureDetected
{
    public class JsonCompatibleExposureDailySummary
    {
        public Dictionary<ReportType, DailySummaryReport?>? Reports { get; set; }
        public DateTime Timestamp { get; set; }
        public DailySummaryReport? Summary { get; set; }

        public JsonCompatibleExposureDailySummary()
        {
        }

        public JsonCompatibleExposureDailySummary(DailySummary dailySummary)
        {
            Timestamp = dailySummary.Timestamp;
            Summary = dailySummary.GetReport(ReportType.ConfirmedTest);
            Dictionary<ReportType, DailySummaryReport> summaryBasedOnReportType = new Dictionary<ReportType, DailySummaryReport>();
            summaryBasedOnReportType.Add(ReportType.ConfirmedTest, Summary);
            Reports = summaryBasedOnReportType;
        }
    }
}

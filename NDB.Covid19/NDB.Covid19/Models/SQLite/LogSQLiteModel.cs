using System;
using NDB.Covid19.Models.Logging;
using SQLite;

namespace NDB.Covid19.Models.SQLite
{
    /// <summary>
    /// All log items stored in SQLite to report at a later time.
    /// 
    /// The ApiVersion and build information is stored in SQLite because it's
    /// important to persist the versions that were when the incident happened
    /// - not when it's reported which might be long after.
    /// </summary>
    public class LogSQLiteModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        //Parameters that are always filled out
        public DateTime ReportedTime { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public int ApiVersion { get; set; }
        public string BuildVersion { get; set; }
        public string BuildNumber { get; set; }
        public string DeviceOSVersion { get; set; }

        //Used for exceptions
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string InnerExceptionType { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string InnerExceptionStackTrace { get; set; }

        //Used for the authentication/submission flow
        public string CorrelationId { get; set; }

        //Used for API errors
        public string Api { get; set; }
        public Nullable<int> ApiErrorCode { get; set; }
        public string ApiErrorMessage { get; set; }

        public string AdditionalInfo { get; set; }

        public LogSQLiteModel() { }

        public LogSQLiteModel(LogDeviceDetails info, LogApiDetails apiDetails = null, LogExceptionDetails e = null, string correlationId = null)
        {
            ReportedTime = info.ReportedTime;
            Severity = info.Severity.ToString();
            Description = info.Description;
            ApiVersion = info.ApiVersion;
            BuildVersion = info.BuildVersion;
            BuildNumber = info.BuildNumber;
            DeviceOSVersion = info.DeviceOSVersion;
            AdditionalInfo = info.AdditionalInfo;

            if (apiDetails != null)
            {
                Api = apiDetails.Api;
                ApiErrorCode = apiDetails.ApiErrorCode;
                ApiErrorMessage = apiDetails.ApiErrorMessage;
            }

            if (e != null)
            {
                ExceptionType = e.ExceptionType;
                ExceptionMessage = e.ExceptionMessage;
                ExceptionStackTrace = e.ExceptionStackTrace;
                InnerExceptionType = e.InnerExceptionType;
                InnerExceptionMessage = e.InnerExceptionMessage;
                InnerExceptionStackTrace = e.InnerExceptionStackTrace;
            }

            if (correlationId != null)
            {
                CorrelationId = correlationId;
            }
        }

        public override string ToString()
        {
            return $"{Severity} Log: " + Description;
        }

    }
}

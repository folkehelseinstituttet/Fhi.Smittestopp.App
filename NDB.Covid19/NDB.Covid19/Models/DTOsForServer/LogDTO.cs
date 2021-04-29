using System;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class LogDTO
    {
        //Parameters that are always filled out
        public DateTime ReportedTime { get; private set; }
        public string Severity { get; private set; }
        public string Description { get; private set; }
        public int ApiVersion { get; private set; }
        public string BuildVersion { get; private set; }
        public string BuildNumber { get; private set; }
        public string DeviceOSVersion { get; private set; }
        public string DeviceCorrelationId { get; private set; }
        public string DeviceType { get; private set; }
        public string DeviceDescription { get; private set; }

        //Used for exceptions
        public string ExceptionType { get; private set; }
        public string ExceptionMessage { get; private set; }
        public string ExceptionStackTrace { get; private set; }
        public string InnerExceptionType { get; private set; }
        public string InnerExceptionMessage { get; private set; }
        public string InnerExceptionStackTrace { get; private set; }

        //Used for API errors
        public string Api { get; set; }
        public Nullable<int> ApiErrorCode { get; private set; }
        public string ApiErrorMessage { get; private set; }

        public string AdditionalInfo { get; private set; }

        //Used for the authentication/submission flow
        public string CorrelationId { get; private set; }

        public LogDTO(LogSQLiteModel log)
        {
#if UNIT_TEST
            DeviceType = "123";
            DeviceDescription = "123";
#else
            DeviceType = DeviceUtils.DeviceType;
            DeviceDescription = DeviceUtils.DeviceModel;
#endif
            ReportedTime = log.ReportedTime;
            Severity = log.Severity;
            Description = log.Description;
            ApiVersion = log.ApiVersion;
            BuildVersion = log.BuildVersion;
            BuildNumber = log.BuildNumber;
            DeviceOSVersion = log.DeviceOSVersion;
            DeviceCorrelationId = "";
            ExceptionType = log.ExceptionType;
            ExceptionMessage = log.ExceptionMessage;
            ExceptionStackTrace = log.ExceptionStackTrace;
            InnerExceptionType = log.InnerExceptionType;
            InnerExceptionMessage = log.InnerExceptionMessage;
            InnerExceptionStackTrace = log.InnerExceptionStackTrace;
            Api = log.Api;
            ApiErrorCode = log.ApiErrorCode;
            ApiErrorMessage = log.ApiErrorMessage;
            AdditionalInfo = log.AdditionalInfo;
            CorrelationId = log.CorrelationId;
        }

        public override string ToString()
        {
            return $"{Severity} Log: " + Description;
        }
    }
}

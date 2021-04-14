using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Models.Logging;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.WebServices;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.Utils
{
    public static class LogUtils
    {
        // Number of logs at a time when we do "send logs to server", "delete them", repeat
        private static readonly int _numLogsToSendAtATime = 100;
        // Max number of persisted logs we allow to remain saved on the device after sending to the server fails
        private static readonly int _maxNumOfPersistedLogsOnSendError = 200;

        public static void LogMessage(LogSeverity severity, string message, string additionalInfo = "", string correlationId = null)
        {
            LogDeviceDetails logModel = new LogDeviceDetails(severity, message, additionalInfo);
            LogSQLiteModel dbModel = new LogSQLiteModel(logModel, null, null, correlationId);
            ServiceLocator.Current.GetInstance<ILoggingManager>().SaveNewLog(dbModel);
        }

        public static void LogException(LogSeverity severity, Exception e, string contextDescription, string additionalInfo = "", string correlationId = null)
        {
            LogDeviceDetails logModel = new LogDeviceDetails(severity, contextDescription, additionalInfo);
            LogExceptionDetails eModel = new LogExceptionDetails(e);
            LogSQLiteModel dbModel = new LogSQLiteModel(logModel, null, eModel, correlationId);
            ServiceLocator.Current.GetInstance<ILoggingManager>().SaveNewLog(dbModel);
        }

        public static void LogApiError(LogSeverity severity, ApiResponse apiResponse, bool erroredSilently,
            string additionalInfo = "", string overwriteMessage = null)
        {
            string errorMessage = overwriteMessage ?? apiResponse.ErrorLogMessage;
            string message = errorMessage
                + (erroredSilently ? " (silent)" : " (error shown)");
            LogDeviceDetails logModel = new LogDeviceDetails(severity, message, additionalInfo);
            LogApiDetails apiModel = new LogApiDetails(apiResponse);
            LogExceptionDetails eModel = null;
            if (apiResponse.Exception != null)
            {
                eModel = new LogExceptionDetails(apiResponse.Exception);
            }
            LogSQLiteModel dbModel = new LogSQLiteModel(logModel, apiModel, eModel);
            ServiceLocator.Current.GetInstance<ILoggingManager>().SaveNewLog(dbModel);
        }

        public static async void SendAllLogs()
        {
            UpdateCorrelationId(null);
            ILoggingManager manager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            try
            {
                bool allLogsSent = false;
                while(!allLogsSent)
                {
                    List<LogSQLiteModel> logs = await manager.GetLogs(_numLogsToSendAtATime);
                    if (logs == null || !logs.Any())
                    {
                        break;
                    }

                    // Try to post logs to the server
                    List<LogDTO> dto = logs.Select(l => new LogDTO(l)).ToList();
                    bool success = await (new LoggingService()).PostAllLogs(dto);

                    // If posting succeeded, delete them
                    if (success)
                    {
                        await manager.DeleteLogs(logs);
                    }
                    // Else, we must try to send them another time.
                    // Also make sure we don't store too many logs because sending keeps failing
                    else
                    {
                        DeleteLogsIfTooMany();
                        break;
                    }

                    allLogsSent = logs.Count < _numLogsToSendAtATime;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print($"{nameof(LogUtils)}.{nameof(SendAllLogs)}: Failed to send logs. Wiping DB to prevent deadlock");
                System.Diagnostics.Debug.Print(e.ToString());
                await manager.DeleteAll();
            }
        }

        private static async void DeleteLogsIfTooMany()
        {
            ILoggingManager manager = ServiceLocator.Current.GetInstance<ILoggingManager>();

            bool tooManyPersistedLogs = true;
            while (tooManyPersistedLogs)
            {
                // GetLogs gives the earliest logs
                List<LogSQLiteModel> logs = await manager.GetLogs(_maxNumOfPersistedLogsOnSendError * 2);
                    
                tooManyPersistedLogs = logs.Count() > _maxNumOfPersistedLogsOnSendError;
                if (tooManyPersistedLogs)
                {
                    int atLeastNLogsTooMany = logs.Count() - _maxNumOfPersistedLogsOnSendError;
                    List<LogSQLiteModel> logsToDelete = logs.Take(atLeastNLogsTooMany).ToList();
                    await manager.DeleteLogs(logsToDelete);
                }
            }
        }

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e?.ExceptionObject != null && e?.ExceptionObject is Exception exception)
            {
                string message = $"{nameof(LogUtils)}.{nameof(OnUnhandledException)}: "
                    + (e.IsTerminating
                    ? "Native unhandled crash"
                    : "Native unhandled exception - not crashing");

                LogSeverity logLevel = e.IsTerminating
                    ? LogSeverity.ERROR
                    : LogSeverity.WARNING;

                LogUtils.LogException(logLevel, exception, message);
            }
        }

        public static string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

using System;
using System.Linq;
using CommonServiceLocator;
using Xunit;
using NDB.Covid19.Utils;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.Models.SQLite;
using System.Collections.Generic;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Models;
using NDB.Covid19.WebServices.Helpers;
using Xamarin.ExposureNotifications;
using Newtonsoft.Json;
using static NDB.Covid19.Utils.Anonymizer;
using NDB.Covid19.WebServices;
using System.Net.Http;
using NDB.Covid19.Configuration;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class LogUtilsTests
    {
        public LogUtilsTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public async void LogMessage_CreateModelsStoreInDatabasePullFromDatabaseConvertToDtos_DtosShouldContainCorrectValues()
        {
            DependencyInjectionConfig.Init();

            // Clear the mock database
            ILoggingManager logManager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            await logManager.DeleteAll();

            // Log models to the mock database
            LogUtils.LogMessage(Enums.LogSeverity.INFO, "My message");
            LogUtils.LogMessage(Enums.LogSeverity.WARNING, "My message", "Additional info");
            LogUtils.LogMessage(Enums.LogSeverity.ERROR, "");

            //// Wait for async IO operations
            //await Task.Delay(1000);

            // Pull models from the mock database
            List<LogSQLiteModel> logSqliteModels = await logManager.GetLogs(10);
            Assert.Equal(3, logSqliteModels.Count);

            // Convert models to DTOs
            IEnumerable<LogDTO> logDtos = logSqliteModels.Select(x => new LogDTO(x));

            //Check that DTOs correspond with arguments to LogMessage()
            foreach (LogDTO logDto in logDtos)
            {
                if (logDto.Severity == Enums.LogSeverity.INFO.ToString())
                {
                    Assert.Equal("My message", logDto.Description);
                }
                else if (logDto.Severity == Enums.LogSeverity.WARNING.ToString())
                {
                    Assert.Equal("My message", logDto.Description);
                    Assert.Equal("Additional info (GPS: Mock version)", logDto.AdditionalInfo);

                }
                else if (logDto.Severity == Enums.LogSeverity.ERROR.ToString())
                {
                    Assert.Equal("", logDto.Description);
                }
                else
                {
                    throw new Exception("At least one of the ifs is expected to be true");
                }
            }
        }

        [Fact]
        public async void LogException_CreateModelsStoreInDatabasePullFromDatabaseConvertToDtos_DtosShouldContainCorrectValues()
        {
            // Clear the mock database
            ILoggingManager logManager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            await logManager.DeleteAll();

            // Log models to the mock database
            LogUtils.LogException(Enums.LogSeverity.INFO, new Exception("Hello"),
                "My context description");
            LogUtils.LogException(Enums.LogSeverity.WARNING, new Exception("Goodbye"),
                "Context decription", "Additional info");

            // Pull models from the mock database
            List<LogSQLiteModel> logSqliteModels = await logManager.GetLogs(10);
            Assert.Equal(2, logSqliteModels.Count);

            
            // Convert models to DTOs
            IEnumerable<LogDTO> logDtos = logSqliteModels.Select(x => new LogDTO(x));

            // Check that DTOs correspond with arguments to LogMessage()
            foreach (LogDTO logDto in logDtos)
            {
                if (logDto.Severity == Enums.LogSeverity.INFO.ToString())
                {
                    Assert.Equal("Hello", logDto.ExceptionMessage);
                }
                else if (logDto.Severity == Enums.LogSeverity.WARNING.ToString())
                {
                    // (GPS: Mock version) is added in the Logdetails constructor
                    Assert.Equal("Additional info (GPS: Mock version)", logDto.AdditionalInfo);

                }
                else
                {
                    throw new Exception("At least one of the ifs is expected to be true");
                }
            }
        }

        [Fact]
        public async void LogApiError_CreateModelsStoreInDatabasePullFromDatabaseConvertToDtos_DtosShouldContainCorrectValues()
        {
            // Clear the mock database
            ILoggingManager logManager = ServiceLocator.Current.GetInstance<ILoggingManager>();
            await logManager.DeleteAll();

            // Log apiResponse1

            string urlPrefix = Conf.URL_PREFIX;

            ApiResponse apiResponse1 = new ApiResponse(urlPrefix + "HELLO!", HttpMethod.Get);
            apiResponse1.StatusCode = 200;
            IEnumerable<ExposureKeyModel> temporaryExposureKeys = new List<ExposureKeyModel>()
                {
                    new ExposureKeyModel(new byte[5], DateTimeOffset.FromUnixTimeSeconds(1234), TimeSpan.FromHours(24), RiskLevel.Medium),
                    new ExposureKeyModel(new byte[5], DateTimeOffset.FromUnixTimeSeconds(1234), TimeSpan.FromHours(24), RiskLevel.Medium),
                    new ExposureKeyModel(new byte[5], DateTimeOffset.FromUnixTimeSeconds(1234), TimeSpan.FromHours(24), RiskLevel.Medium),
                };
             
            string redactedKeysJson = RedactedTekListHelper.CreateRedactedTekList(temporaryExposureKeys);
            LogUtils.LogApiError(Enums.LogSeverity.INFO, apiResponse1,
                false, redactedKeysJson);

            //Added Mockversion to test if version is set correctly in the Logdevicedetails
            redactedKeysJson += " (GPS: Mock version)";

            // Log apiResponse2

            ApiResponse apiResponse2 = new ApiResponse(urlPrefix + "GOODBYE!", HttpMethod.Get);
            apiResponse2.StatusCode = 201;
            LogUtils.LogApiError(Enums.LogSeverity.WARNING, apiResponse2, true);

            // Pull the two log models from the mock database
            List<LogSQLiteModel> logSqliteModels = await logManager.GetLogs(10);
            Assert.Equal(2, logSqliteModels.Count);

            // Convert the models to DTOs
            IEnumerable<LogDTO> logDtos = logSqliteModels.Select(x => new LogDTO(x));

            // Check that the DTOs contain the expected values
            foreach (LogDTO logDto in logDtos)
            {
                // apiReponse1
                string apiVersion = $"/v{Conf.APIVersion}/";

                if (logDto.Severity == Enums.LogSeverity.INFO.ToString())
                {
                    Assert.Equal(apiVersion + "HELLO!", logDto.Api);
                    Assert.Equal(200, logDto.ApiErrorCode);

                    // We expect AdditionalInfo to consist of redactedKeysJson, after it's been processed with RedactText
                    Assert.Equal(RedactText(redactedKeysJson), logDto.AdditionalInfo);

                    // This part is just to double check redaction of key data works 
                    // Before redaction, the key data is a byte array of size 5, which is shown as "AAAAAAA="
                    // After redaction we do not want this to be in the string
                    string nonRedactedKeysJson = JsonConvert.SerializeObject(temporaryExposureKeys, BaseWebService.JsonSerializerSettings);
                    Assert.Contains("AAAAAAA=", nonRedactedKeysJson);
                    Assert.DoesNotContain("AAAAAAA=", redactedKeysJson);
                }

                // apiResponse2

                else if (logDto.Severity == Enums.LogSeverity.WARNING.ToString())
                {
                    Assert.Equal(apiVersion + "GOODBYE!", logDto.Api);
                    Assert.Equal(201, logDto.ApiErrorCode);
                }

                // Should never be reached
                else
                {
                    throw new Exception("At least one of the ifs is expected to be true");
                }
            }
        }
    }
}

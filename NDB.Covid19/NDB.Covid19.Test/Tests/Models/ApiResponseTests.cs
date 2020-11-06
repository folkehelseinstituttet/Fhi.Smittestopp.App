using System;
using Xunit;
using NDB.Covid19.Models;
using System.Net;
using NDB.Covid19.Test.Tests.Models.utils;
using System.Net.Http;
using NDB.Covid19.Configuration;

namespace NDB.Covid19.Test.Tests.Models
{
    public class ApiResponseTests
    {
        public ApiResponseTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Theory]
        [InlineData("response text1")]
        public void ResponseText_SetValueAndGetValue_ShouldReturnCorrectValue(string responseTxt)
        {
            ApiResponse res = new ApiResponse("url", HttpMethod.Get);
            res.ResponseText = responseTxt;
            Assert.Equal(responseTxt, res.ResponseText);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void StatusCode_SetValueAndGetValue_ShouldReturnCorrectValue(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("url", HttpMethod.Get);
            res.StatusCode = (int) httpStatus;
            Assert.Equal((int) httpStatus, res.StatusCode);
        }

        [Theory]
        [ClassData(typeof(ExceptionObj))]
        public void Exception_SetValueAndGetValue_ShouldReturnCorrectValue(Exception exception)
        {
            ApiResponse res = new ApiResponse("url", HttpMethod.Get);
            res.Exception = exception;
            Assert.Equal(exception, res.Exception);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        public void isSuccessful_HaveStatusCreatedOrOK_ShouldReturnTrue(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("url", HttpMethod.Get);
            res.StatusCode = (int) httpStatus;
            Assert.True(res.IsSuccessfull);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.BadGateway)]
        public void isSuccessful_HaveStatusNotEqualCreatedAndOK_ShouldReturnFalse(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("Url", HttpMethod.Get);
            res.StatusCode = (int) httpStatus;
            Assert.False(res.IsSuccessfull);
        }

        [Theory]
        [InlineData("profiles/register")]
        [InlineData("encounters")]
        [InlineData("encounters/average")]
        [InlineData("profiles/withdrawconsent")]
        [InlineData("logging/logMessages")]
        [InlineData("diagnostickeys/2020-09-03_1_no.zip")]
        public void ApiResponse_CalculateEndpoint(string endpoint)
        {
            string completeUrl = $"{Conf.URL_PREFIX}{endpoint}";
            ApiResponse res = new ApiResponse(completeUrl, HttpMethod.Get);

            string apiVersion = $"v{Conf.APIVersion}/";
            Assert.Equal(apiVersion + endpoint, res.Endpoint);
        }

        [Theory]
        [InlineData("profiles/register", "profiles/register")]
        [InlineData("diagnostickeys", "diagnostickeys")]
        [InlineData("diagnostickeys/2020-09-03_1_no.zip", "diagnostickeys")]
        public void ApiResponse_ErrorLogMessage(string endpoint, string expectedEndpointInMessage)
        {
            string completeUrl = $"{Conf.URL_PREFIX}{endpoint}";
            ApiResponse res = new ApiResponse(completeUrl, HttpMethod.Get);
            res.StatusCode = 204;

            string expectedMessage = $"API GET /v{Conf.APIVersion}/{expectedEndpointInMessage} failed";
            Assert.Equal(expectedMessage, res.ErrorLogMessage);

            res.Exception = new ArgumentNullException("Something was null");

            Assert.Equal(expectedMessage + " with ArgumentNullException", res.ErrorLogMessage);
               
            res.StatusCode = 500;

            Assert.Equal(expectedMessage + " with HttpStatusCode 500", res.ErrorLogMessage);
        }

    }
}

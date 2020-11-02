using CommonServiceLocator;
using FluentAssertions;
using Moq;
using NDB.Covid19.Models;
using NDB.Covid19.SecureStorage;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Interfaces;
using NDB.Covid19.WebServices;
using System;
using System.Net;
using System.Threading.Tasks;
using Unity;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NDB.Covid19.Test.Tests.WebServices
{
    public class WebServiceTests : IDisposable
    {
        public WebServiceTests()
        {
            DependencyInjectionConfig.Init();
            var secureStorageService = ServiceLocator.Current.GetInstance<SecureStorageService>();
            secureStorageService.SetSecureStorageInstance(new SecureStorageMock());

            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            var mockConnectivity = new Mock<IConnectivity>();
            mockConnectivity.Setup(x => x.NetworkAccess).Returns(Xamarin.Essentials.NetworkAccess.Internet);
            container.RegisterInstance(mockConnectivity.Object);

            ApiStubHelper.StartServer();
        }

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        public async Task Get_SendGetRequest_ReturnSuccessResponseWithCorrectTextResult()
        {
            var testApiPath = "/testGetText";
            var testApiResult = @"
{
	""Test"": ""Test String"",
	""TestBool"": false
}
";
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(testApiPath).UsingGet())
                .RespondWith(
                  Response.Create()
                    .WithStatusCode(System.Net.HttpStatusCode.OK)
                    .WithBody(testApiResult)
                );
            var baseService = new BaseWebService();
            ApiResponse<TestModelToParse> response = await baseService.Get<TestModelToParse>(ApiStubHelper.StubServerUrl + testApiPath);
            var returnedSuccessResponseWithCorrectValue = response.IsSuccessfull
                && response.StatusCode == (int)HttpStatusCode.OK
                && response.Data.Test == "Test String"
                && response.Data.TestBool == false;
            returnedSuccessResponseWithCorrectValue.Should().BeTrue();
        }

        class TestModelToParse
        {
            public string Test { get; set; }
            public bool TestBool { get; set; }
        }

    }
}

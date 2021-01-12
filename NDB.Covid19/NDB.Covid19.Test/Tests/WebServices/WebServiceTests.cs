using CommonServiceLocator;
using FluentAssertions;
using Moq;
using NDB.Covid19.Models;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Interfaces;
using NDB.Covid19.WebServices;
using System;
using System.Net;
using System.Threading.Tasks;
using NDB.Covid19.PersistedData.SecureStorage;
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
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.IsSuccessfull.Should().BeTrue();
            response.Data.Test.Should().Be("Test String");
            response.Data.TestBool.Should().BeFalse();
        }

        class TestModelToParse
        {
            public string Test { get; set; }
            public bool TestBool { get; set; }
        }

    }
}

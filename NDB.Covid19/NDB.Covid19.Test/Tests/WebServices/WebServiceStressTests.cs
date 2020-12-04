using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using FluentAssertions;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.WebServices;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NDB.Covid19.Test.Tests.WebServices
{
    public class WebServiceStressTests : IDisposable
    {
        public WebServiceStressTests()
        {
            DependencyInjectionConfig.Init();
            var secureStorageService = ServiceLocator.Current.GetInstance<SecureStorageService>();
            secureStorageService.SetSecureStorageInstance(new SecureStorageMock());

            ApiStubHelper.StartServer();
        }

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        public async Task Post_Null_ReturnWrongStatusShouldBeUnsuccessfulAndReturnException()
        {
            string testApiPath = "/profiles/withdrawconsent";

            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(testApiPath).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(0)
                        .WithBodyAsJson("Successfully send POST request")
                );
            BaseWebService baseService = new BaseWebService();

            ApiResponse response = await baseService.Post(ApiStubHelper.StubServerUrl + testApiPath);
            var returnedFailResponseWithException = !response.IsSuccessfull && response.Exception != null;
            returnedFailResponseWithException.Should().BeTrue();
        }
    }
}
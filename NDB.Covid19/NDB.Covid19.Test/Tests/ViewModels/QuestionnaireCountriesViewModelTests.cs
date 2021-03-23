using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace NDB.Covid19.Test.Tests.ViewModels
{
    public class QuestionnaireCountriesViewModelTests : IDisposable
    {
        private readonly QuestionnaireCountriesViewModel _viewModel;

        public QuestionnaireCountriesViewModelTests()
        {
            DependencyInjectionConfig.Init();
            _viewModel = new QuestionnaireCountriesViewModel();
            ApiStubHelper.StartServer();
        }

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        public async void  CountryListService_EndpointWorks()
        {
            var testApiPath = "/countries";

            var mockedData = new CountryListDTO
            {
                CountryCollection = new List<CountryDetailsDTO>
                {
                    new CountryDetailsDTO
                    {
                        TranslatedName = "Norge",
                        Code = "no"
                    }
                }
            };

            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(testApiPath).UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(System.Net.HttpStatusCode.OK)
                        .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(mockedData))
                );
            var baseService = new BaseWebService();

            ApiResponse<CountryListDTO> response = await baseService.Get<CountryListDTO>(ApiStubHelper.StubServerUrl + testApiPath);
            Assert.True(response.IsSuccessfull);
            Assert.Equal(mockedData.CountryCollection.Count, response.Data.CountryCollection.Count);
            Assert.Equal(mockedData.CountryCollection[0].Code, response.Data.CountryCollection[0].Code);
            Assert.Equal(mockedData.CountryCollection[0].TranslatedName, response.Data.CountryCollection[0].TranslatedName);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void QuestionnaireCountriesViewModel_InvokeNextButtonClick_UpdatesList(params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = new PersonalDataModel
            {
                TokenExpiration = DateTime.Now.AddHours(1),
                Covid19_smitte_start = "2021-03-08"
            };

            _viewModel.InvokeNextButtonClick(null, null, list.ToList());

            Assert.NotNull(AuthenticationState.PersonalData.VisitedCountries);
            Assert.True(AuthenticationState.PersonalData.VisitedCountries.SequenceEqual(list.Select(c => c.Code).ToList()));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async void QuestionnaireCountriesViewModel_InvokeNextButtonClick_OnSuccessCalled(params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = new PersonalDataModel
            {
                TokenExpiration = DateTime.Now.AddHours(1),
                Covid19_smitte_start = "2021-03-08"
            };

            TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

            _viewModel.InvokeNextButtonClick(() =>
            {
                _tcs.SetResult(true);
            }, null, list.ToList());

            Assert.True(await _tcs.Task);
            Assert.NotNull(AuthenticationState.PersonalData.VisitedCountries);
            Assert.True(AuthenticationState.PersonalData.VisitedCountries.SequenceEqual(list.Select(c => c.Code).ToList()));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async void QuestionnaireCountriesViewModel_InvokeNextButtonClick_OnFailCalled(params CountryDetailsViewModel[] list)
        {
            AuthenticationState.PersonalData = null;

            TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

            _viewModel.InvokeNextButtonClick(null,() =>
            {
                _tcs.SetResult(true);
            }, list.ToList());

            Assert.True(await _tcs.Task);
            Assert.Null(AuthenticationState.PersonalData?.VisitedCountries);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    new CountryDetailsViewModel{Checked = true, Code = "nb", Name = "Denmark"}
                },
                new object[]
                {
                    new CountryDetailsViewModel{Checked = true, Code = "nb", Name = "Denmark"},
                    new CountryDetailsViewModel{Checked = true, Code = "en", Name = "England"}
                },
                new object[]
                {
                    new CountryDetailsViewModel{Checked = true, Code = "nb", Name = "Denmark"},
                    new CountryDetailsViewModel{Checked = true, Code = "en", Name = "England"},
                    new CountryDetailsViewModel{Checked = true, Code = "pl", Name = "Poland"}
                },
                new object[]
                {
                    new CountryDetailsViewModel{Checked = true, Code = "nb", Name = "Denmark"},
                    new CountryDetailsViewModel{Checked = true, Code = "en", Name = "England"},
                    new CountryDetailsViewModel{Checked = true, Code = "pl", Name = "Poland"},
                    new CountryDetailsViewModel{Checked = true, Code = "de", Name = "Germany"},
                }
            };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CommonServiceLocator;
using Moq;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Test.Helpers;
using NDB.Covid19.Test.Tests.Utils;
using NDB.Covid19.Utils.DeveloperTools;
using NDB.Covid19.WebServices.ExposureNotification;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xamarin.ExposureNotifications;
using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class PushKeysTests : IDisposable
    {
        private readonly IDeveloperToolsService _developerToolsService;
        private readonly ExposureNotificationWebService _exposureNotificationWebService;

        public PushKeysTests()
        {
            DependencyInjectionConfig.Init();
            ApiStubHelper.StartServer();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new JsonConverter[] { new JsonMockConverter() }
            };
            _developerToolsService = ServiceLocator.Current.GetInstance<IDeveloperToolsService>();
            _exposureNotificationWebService = new ExposureNotificationWebService();
        }

        public void Dispose()
        {
            ApiStubHelper.StopServer();
        }

        [Fact]
        public async void LastKeyUploadInfoIsUpdated()
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess =
                await _exposureNotificationWebService.PostSelfExposureKeys(
                    GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys),
                    temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.True(isSuccess);
            Assert.NotEqual(lastKeyUploadInfoBeforePush, lastKeyUploadInfoAfterPush);
        }

        [Fact]
        public async void IfServerResponseIsEmptyLastKeyUploadInfoIsNotUpdated()
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithNotFound()
                        .WithFault(FaultType.EMPTY_RESPONSE)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess = await _exposureNotificationWebService.PostSelfExposureKeys(
                GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys),
                temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.False(isSuccess);
            Assert.True(lastKeyUploadInfoAfterPush != null && lastKeyUploadInfoAfterPush.Contains("StatusCode: 404"));
        }

        [Fact]
        public async void LastKeyUploadInfoAPIIsDeprecated()
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(410)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess =
                await _exposureNotificationWebService.PostSelfExposureKeys(
                    GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys),
                    temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.False(isSuccess);
            Assert.NotEqual(lastKeyUploadInfoBeforePush, lastKeyUploadInfoAfterPush);
        }

        [Theory]
        [InlineData("no")]
        [InlineData("no", "en")]
        [InlineData("no", "en", "pl")]
        public async void LastKeyUploadInfoIsUpdated_withListOfCountries(params string[] countriesArray)
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess =
                await _exposureNotificationWebService.PostSelfExposureKeys(
                    GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys, countriesArray.ToList()),
                    temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.True(isSuccess);
            Assert.NotEqual(lastKeyUploadInfoBeforePush, lastKeyUploadInfoAfterPush);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(14)]
        public async void LastKeyUploadInfoIsUpdated_withDaysSinceSymptoms(int daysSinceSymptoms)
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium, daysSinceSymptoms)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess =
                await _exposureNotificationWebService.PostSelfExposureKeys(
                    GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys),
                    temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.True(isSuccess);
            Assert.NotEqual(lastKeyUploadInfoBeforePush, lastKeyUploadInfoAfterPush);
        }

        [Theory]
        [InlineData(0, "no")]
        [InlineData(7, "no")]
        [InlineData(14, "no")]
        [InlineData(0, "no", "en")]
        [InlineData(7, "no", "en")]
        [InlineData(14, "no", "en")]
        public async void LastKeyUploadInfoIsUpdated_withListOfCountries_And_DaysSinceSymptoms(int daysSinceSymptoms, params string[] countriesArray)
        {
            //Given
            string url = Conf.URL_PUT_UPLOAD_DIAGNOSIS_KEYS.Replace(ApiStubHelper.StubServerUrl, "");

            List<DateTime> keyDates = new List<DateTime> { new DateTime(2020, 6, 1) };
            List<ExposureKeyModel> temporaryExposureKeys = keyDates.Select(
                (key, i) =>
                    new ExposureKeyModel(new byte[i], key, TimeSpan.FromDays(1), RiskLevel.Medium, daysSinceSymptoms)).ToList();
            ApiStubHelper.StubServer
                .Given(Request.Create().WithPath(url).UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            // When
            string lastKeyUploadInfoBeforePush = _developerToolsService.LastKeyUploadInfo;
            bool isSuccess =
                await _exposureNotificationWebService.PostSelfExposureKeys(
                    GetMockedSelDiagnosisSubmissionDTO(temporaryExposureKeys, countriesArray.ToList()),
                    temporaryExposureKeys);
            string lastKeyUploadInfoAfterPush = _developerToolsService.LastKeyUploadInfo;

            // Then
            Assert.Empty(lastKeyUploadInfoBeforePush);
            Assert.True(isSuccess);
            Assert.NotEqual(lastKeyUploadInfoBeforePush, lastKeyUploadInfoAfterPush);
        }

        private SelfDiagnosisSubmissionDTO GetMockedSelDiagnosisSubmissionDTO(
            IEnumerable<ExposureKeyModel> temporaryExposureKeys, List<string> countriesList = null)
        {
            Mock<SelfDiagnosisSubmissionDTO> mock = new Mock<SelfDiagnosisSubmissionDTO>()
                .RegisterForJsonSerialization();
            SelfDiagnosisSubmissionDTO selfDiagnosisSubmissionDto = mock.Object;
            selfDiagnosisSubmissionDto.Keys = temporaryExposureKeys;
            if (!countriesList.IsNullOrEmpty())
            {
                selfDiagnosisSubmissionDto.VisitedCountries = countriesList;
            }
            selfDiagnosisSubmissionDto.ComputePadding();

            return selfDiagnosisSubmissionDto;
        }
    }
}
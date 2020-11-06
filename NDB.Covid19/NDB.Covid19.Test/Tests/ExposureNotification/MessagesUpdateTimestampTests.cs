using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using Moq;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Models;
using NDB.Covid19.Test.Mocks;
using Xunit;
using System.Net.Http;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.WebServices.ExposureNotification;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class MessagesUpdateTimestampTests
    {
        public MessagesUpdateTimestampTests()
        {
            DependencyInjectionConfig.Init();
            var secureStorageService = ServiceLocator.Current.GetInstance<SecureStorageService>();
            secureStorageService.SetSecureStorageInstance(new SecureStorageMock());
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task StatusCode_PullKeys_ShouldUpdateOrNotLastUpdatedTimestamp(HttpStatusCode httpStatus)
        {
            ApiResponse<Stream> apiResponse = new ApiResponse<Stream>("test", HttpMethod.Get)
            {
                StatusCode = (int) httpStatus,
            };

            ExposureNotificationWebService notificationWebService =
                Mock.Of<ExposureNotificationWebService>(b =>
                    b.GetFileAsStreamAsync(It.IsAny<string>()) == Task.FromResult(apiResponse));

            Mock.Get(notificationWebService).CallBase = true;

            ApiResponse<Stream> response =
                await notificationWebService.GetDiagnosisKeys("dummyDate", CancellationToken.None);

            List<HttpStatusCode> errorStatusCodesList = new List<HttpStatusCode>
                {HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest};

            if (errorStatusCodesList.Contains(httpStatus))
            {
                //MessageLastUpdateDateTime should not be updated
                Assert.Equal(DateTime.MinValue.ToLocalTime(), MessagesViewModel.LastUpdateDateTime);
            }
            else
            {
                //MessageLastUpdateDateTime must be updated
                Assert.NotEqual(DateTime.MinValue.ToLocalTime(), MessagesViewModel.LastUpdateDateTime);
            }
        }
    }
}
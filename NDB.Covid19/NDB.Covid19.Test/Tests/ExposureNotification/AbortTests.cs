using Moq;

using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers.FetchExposureKeys;
using NDB.Covid19.Models;
using NDB.Covid19.Test.Mocks;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices;
using NDB.Covid19.WebServices.ExposureNotification;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace NDB.Covid19.Test.Tests.ExposureNotification
{
    public class AbortTests
    {
        private static readonly LocalNotificationManagerMock _localNotificationsManager =
            (LocalNotificationManagerMock)NotificationsHelper.LocalNotificationsManager;

        public AbortTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Theory]
        [InlineData(200, false)]
        [InlineData(204, false)]
        [InlineData(404, false)]
        [InlineData(410, true)]
        [InlineData(500, false)]
        public async void DownloadZips_ShouldReturnProperState(int statusCode, bool isNotificationShown)
        {
            _localNotificationsManager.HasBeenCalled[NotificationsEnum.ApiDeprecated] = false;

            ExposureNotificationWebService exposureNotificationWebService = Mock.Of<ExposureNotificationWebService>(
                service => service.GetDiagnosisKeys(
                               It.IsAny<string>(),
                               It.IsAny<CancellationToken>())
                           == Task.FromResult(new ApiResponse<Stream>("", HttpMethod.Get)
                           {
                               Data = null,
                               Endpoint = "",
                               Exception = null,
                               Headers = null,
                               ResponseText = null,
                               StatusCode = statusCode
                           }));

            await MockZipDownloader().PullNewKeys(exposureNotificationWebService, CancellationToken.None);

            Assert.Equal(isNotificationShown, _localNotificationsManager.HasBeenCalled[NotificationsEnum.ApiDeprecated]);

            _localNotificationsManager.HasBeenCalled[NotificationsEnum.ApiDeprecated] = false;
        }

        [Fact]
        public async void PostSelvExposureKeys_ShouldInformAboutDeprecatedAPI()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            PrepareMessagingCenter(() => tcs.SetResult(true));

            BaseWebService baseWebService = Mock.Of<BaseWebService>(
                service => service.Post(It.IsAny<SelfDiagnosisSubmissionDTO>(), It.IsAny<string>()) == Task.FromResult(
                    new ApiResponse("", HttpMethod.Post)
                    {
                        Endpoint = "something.zip",
                        Exception = null,
                        Headers = null,
                        ResponseText = null,
                        StatusCode = 410
                    }));
            Mock.Get(baseWebService).CallBase = true;

            bool postSelvExposureKeys = await new ExposureNotificationWebService()
                .PostSelvExposureKeys(
                    new SelfDiagnosisSubmissionDTO
                    {
                        AppPackageName = "",
                        Keys = new List<ExposureKeyModel>(),
                        Padding = "",
                        Platform = "",
                        Regions = new List<string>(),
                        VisitedCountries = new List<string>()
                    },
                    new List<ExposureKeyModel>(),
                    baseWebService);


            using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                Task completedTask =
                    await Task.WhenAny(tcs.Task, Task.Delay(2000, timeoutCancellationTokenSource.Token));
                if (completedTask == tcs.Task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    Assert.True(await tcs.Task);
                }
                else
                {
                    Assert.True(false, "Timeout");
                }
            }

            Assert.False(postSelvExposureKeys);

            UnsubscribeMessagingCenter();
        }

        private void PrepareMessagingCenter(Action action)
        {
            MessagingCenter.Subscribe(this, MessagingCenterKeys.KEY_FORCE_UPDATE,
                (object obj) => { action?.Invoke(); });
        }

        private void UnsubscribeMessagingCenter()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }

        private ZipDownloader MockZipDownloader()
        {
            ZipDownloader zipDownloader = Mock.Of<ZipDownloader>();
            Mock.Get(zipDownloader).CallBase = true;
            return zipDownloader;
        }
    }
}
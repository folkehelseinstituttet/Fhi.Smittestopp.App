using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Configuration;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils.DeveloperTools;

namespace NDB.Covid19.WebServices
{
    public class ImportantMessageService : BaseWebService
    {
        public async Task<ImportantMessage> GetImportantMessage()
        {
            ApiResponse<ImportantMessage> response = await Get<ImportantMessage>($"{Conf.URL_GET_IMPORTANT_MESSAGE}?lang={LocalPreferencesHelper.GetAppLanguage()}");
            HandleErrorsSilently(response);

            ServiceLocator.Current.GetInstance<IDeveloperToolsService>().SaveLastFetchedImportantMessage(response, SystemTime.Now());

            return response?.Data;
        }
    }
}

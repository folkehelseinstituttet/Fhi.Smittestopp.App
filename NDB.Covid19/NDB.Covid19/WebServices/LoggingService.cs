using System.Collections.Generic;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;

namespace NDB.Covid19.WebServices
{
    public class LoggingService : BaseWebService
    {
        public async Task<bool> PostAllLogs(List<LogDTO> dtos)
        {
            object body = new
            {
                Logs = dtos.ToArray()
            };

            ApiResponse response = await Post(body, Conf.URL_LOG_MESSAGE);
            return response.IsSuccessfull;
        }
    }
}

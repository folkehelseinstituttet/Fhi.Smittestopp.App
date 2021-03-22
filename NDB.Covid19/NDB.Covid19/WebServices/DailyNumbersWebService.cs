using System;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;
namespace NDB.Covid19.WebServices
{
    public class DailyNumbersWebService : BaseWebService
    {
        public async Task<DailyNumbersDTO> GetFHIData(DateTime packageDate)
        {
            ApiResponse<DailyNumbersDTO> response = await Get<DailyNumbersDTO>($"{Conf.URL_GET_FHI_DATA}?packageDate={packageDate.ToString("dd'-'MM'-'yyyy")}");
            HandleErrorsSilently(response);
            return response?.Data;
        }

        public async Task<DailyNumbersDTO> GetFHIData()
        {
            ApiResponse<DailyNumbersDTO> response = await Get<DailyNumbersDTO>($"{Conf.URL_GET_FHI_DATA}");
            HandleErrorsSilently(response);
            return response?.Data;
        }
    }
}
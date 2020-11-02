using System.Threading.Tasks;
using NDB.Covid19.Config;
using NDB.Covid19.Models;
using NDB.Covid19.Models.DTOsForServer;

namespace NDB.Covid19.WebServices
{
    public class CountryListService : BaseWebService
    {
        public async Task<CountryListDTO> GetCountryList()
        {
            ApiResponse<CountryListDTO> response = await Get<CountryListDTO>(Conf.URL_GET_COUNTRY_LIST);
            HandleErrorsSilently(response);

            return response?.Data;
        }
    }
}

using System.Collections.Generic;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class CountryListDTO
    {
        public List<CountryDetailsDTO> CountryCollection { get; set; }
    }
}
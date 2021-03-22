using System;
namespace NDB.Covid19.Models.DTOsForServer
{
    public class DailyNumbersDTO
    {
        public CovidStatisticsDTO CovidStatistics { get; set; }
        public AppStatisticsDTO ApplicationStatistics { get; set; }
    }
}
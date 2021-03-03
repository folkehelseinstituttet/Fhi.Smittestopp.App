using System;
namespace NDB.Covid19.Models.DTOsForServer
{
    public class DailyNumbersDTO
    {
        public FHIStatisticsDTO SSIStatistics { get; set; }
        public AppStatisticsDTO ApplicationStatistics { get; set; }
    }
}
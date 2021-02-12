using System;
namespace NDB.Covid19.Models.DTOsForServer
{
    public class DailyNumbersDTO
    {
        public FHIStatisticsDTO FHIStatistics { get; set; }
        public AppStatisticsDTO AppStatistics { get; set; }
    }
}
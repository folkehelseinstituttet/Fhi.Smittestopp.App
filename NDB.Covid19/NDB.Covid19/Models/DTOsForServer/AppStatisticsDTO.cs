using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class AppStatisticsDTO
    {
        public DateTime EntryDate { get; set; }
        public int PositiveResultsLast7Days { get; set; }
        public int PositiveTestsResultsTotal { get; set; }
        public int SmittestopDownloadsTotal { get; set; }
    }
}
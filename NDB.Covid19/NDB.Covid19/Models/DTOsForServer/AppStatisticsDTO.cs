using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class AppStatisticsDTO
    {
        public DateTime EntryDate { get; set; }
        public int NumberOfPositiveTestsResultsLast7Days { get; set; }
        public int NumberOfPositiveTestsResultsTotal { get; set; }
        public int SmittestoppDownloadsTotal { get; set; }
    }
}
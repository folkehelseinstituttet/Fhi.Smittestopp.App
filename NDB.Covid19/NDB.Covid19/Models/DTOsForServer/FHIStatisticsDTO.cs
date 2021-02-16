using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class FHIStatisticsDTO
    {
        public DateTime EntryDate { get; set; }
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int DeathsToday { get; set; }
        public int DeathsTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedToday { get; set; }
        public int PatientsIntensiveCare { get; set; }
        public double ReproductionsNumber { get; set; }
        public int VaccinationsDoseOneToday { get; set; }
        public int VaccinationsDoseTwoToday { get; set; }
        public int VaccinationsDoseOneTotal { get; set; }
        public int VaccinationsDoseTwoTotal { get; set; }
    }
}
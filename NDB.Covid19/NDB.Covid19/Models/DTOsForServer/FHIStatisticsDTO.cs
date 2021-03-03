using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class FHIStatisticsDTO
    {
        public DateTime Date { get; set; }
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedToday { get; set; }
        public int PatientsIntensiveCare { get; set; }
        public int VaccinationsDoseOneToday { get; set; }
        public int VaccinationsDoseTwoToday { get; set; }
        public int VaccinatedFirstDose { get; set; }
        public int VaccinatedSecondDose { get; set; }
    }
}
using System;

namespace NDB.Covid19.Models.DTOsForServer
{
    public class CovidStatisticsDTO
    {
        public DateTime Date { get; set; }
        
        public int ConfirmedCasesTotal { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedTotal { get; set; }
        public int IcuAdmittedTotal { get; set; }
        public int VaccinatedFirstDoseTotal { get; set; }
        public int VaccinatedSecondDoseTotal { get; set; }
        public int DeathsTotal { get; set; }
    }
}
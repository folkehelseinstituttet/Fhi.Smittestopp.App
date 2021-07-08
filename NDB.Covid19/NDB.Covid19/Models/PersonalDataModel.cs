using NDB.Covid19.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NDB.Covid19.Models
{
    public class PersonalDataModel
    {
        //Set by ID Porten authentication
        public string Covid19_smitte_start { get; set; }
        public string Covid19_blokeret { get; set; }
        public string Covid19_smitte_stop { get; set; }
        public string Covid19_status { get; set; }
        public string Covid19_anonymous_token { get; set; }

        [JsonIgnore]
        public string Access_token { get; set; }
        [JsonIgnore]
        public DateTime? TokenExpiration { get; set; }

        //Set from questionnaire
        [JsonIgnore]
        public DateTime? FinalDateToSetDSOS { get; set; }
        [JsonIgnore]
        public List<string> VisitedCountries { get; set; } = new List<string>();

        [JsonIgnore]
        public bool IsBlocked => Covid19_blokeret == "true";
        [JsonIgnore]
        public bool IsNotInfected => Covid19_status == "negativ";
        [JsonIgnore]
        public bool UnknownStatus => Covid19_status == "ukendt";
        [JsonIgnore]
        public bool AnonymousTokensEnabled => Covid19_anonymous_token == "v1_enabled";


        public bool Validate()
        {
            string logPrefix = nameof(PersonalDataModel) + "." + nameof(Validate) + ": ";

            bool startDateOK = !String.IsNullOrEmpty(Covid19_smitte_start);
            if (!startDateOK)
            {
                LogUtils.LogMessage(Enums.LogSeverity.ERROR, logPrefix + "Covid19_smitte_start value was null or empty");
            }

            bool notExpired = TokenExpiration != null && TokenExpiration > DateTime.Now;
            if (!notExpired)
            {
                LogUtils.LogMessage(Enums.LogSeverity.ERROR, logPrefix + "Access token was expired");
            }

            return startDateOK && notExpired;
        }
    }
}

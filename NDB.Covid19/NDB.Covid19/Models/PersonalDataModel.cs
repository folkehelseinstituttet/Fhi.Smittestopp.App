using System;
using System.Collections.Generic;
using NDB.Covid19.Utils;
using Newtonsoft.Json;

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
        public string Role { get; set; }

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
        public bool IsMsisLookupSkipped { get; set; }

        [JsonIgnore]
        public bool IsBlocked => Covid19_blokeret == "true";
        [JsonIgnore]
        public bool IsNotInfected => Covid19_status == "negativ";
        [JsonIgnore]
        public bool UnknownStatus => Covid19_status == "ukendt";
        [JsonIgnore]
        public bool AnonymousTokensEnabled => Covid19_anonymous_token == "v1_enabled";
        [JsonIgnore]
        public bool CanUploadKeys => Role == "upload-approved";
        [JsonIgnore]
        public bool IsUnderaged => Role == "underaged";


        public bool Validate()
        {
            return ValidateStartDate() && ValidateAccessToken();
        }

        public bool ValidateAccessToken()
        {
            string logPrefix = $"{nameof(PersonalDataModel)}.{nameof(ValidateAccessToken)}: ";

            bool notExpired = TokenExpiration != null && TokenExpiration > DateTime.Now;
            if (!notExpired)
            {
                LogUtils.LogMessage(Enums.LogSeverity.ERROR, logPrefix + "Access token was expired");
            }

            return notExpired;
        }

        public bool ValidateStartDate()
        {
            string logPrefix = $"{nameof(PersonalDataModel)}.{nameof(ValidateStartDate)}: ";

            bool startDateOK = !String.IsNullOrEmpty(Covid19_smitte_start);
            if (!startDateOK)
            {
                LogUtils.LogMessage(Enums.LogSeverity.ERROR, logPrefix + "Covid19_smitte_start value was null or empty");
            }

            return startDateOK;
        }
    }
}

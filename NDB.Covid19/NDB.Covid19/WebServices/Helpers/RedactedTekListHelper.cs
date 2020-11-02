using System.Collections.Generic;
using System.Linq;
using NDB.Covid19.Models;
using Newtonsoft.Json;

namespace NDB.Covid19.WebServices.Helpers
{
    public abstract class RedactedTekListHelper
    {
        public static string CreateRedactedTekList(IEnumerable<ExposureKeyModel> teks)
        {
            IEnumerable<ExposureKeyModel> teksRedacted =
                teks.Select(tek => new ExposureKeyModel(new byte[1], tek.RollingStart, tek.RollingDuration, tek.TransmissionRiskLevel));
            return JsonConvert.SerializeObject(teksRedacted, BaseWebService.JsonSerializerSettings);
        }
    }
}

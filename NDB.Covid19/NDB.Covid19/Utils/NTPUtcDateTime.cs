using System;
using System.Threading.Tasks;
using NDB.Covid19.Enums;
using NDB.Covid19.PersistedData;
using Yort.Ntp;

namespace NDB.Covid19.Utils
{
    public class NTPUtcDateTime
    {
        private readonly NtpClient _client;
        public NTPUtcDateTime()
        {
            _client = new NtpClient(KnownNtpServers.PoolOrg);
        }

        public virtual async Task<DateTime> GetNTPUtcDateTime()
        {
            try
            {
                return (await _client.RequestTimeAsync()).NtpTime;
            }
            catch (Exception e)
            {
                LogUtils.LogException(
                    LogSeverity.WARNING,
                    e,
                    $"{nameof(NTPUtcDateTime)}-{nameof(GetNTPUtcDateTime)} threw an exception");
                return LocalPreferencesHelper.LastNTPUtcDateTime;
            }
        }
    }
}

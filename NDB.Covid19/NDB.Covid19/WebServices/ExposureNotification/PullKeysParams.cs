using System;
using System.Globalization;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotification.Helpers;
using NDB.Covid19.Interfaces;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

namespace NDB.Covid19.WebServices.ExposureNotification
{
    /// <summary>
    /// The get request parameters used to pull new keys from the server
    /// </summary>
    public class PullKeysParams
    {
        public DateTime Date { get; set; }
        public BatchType BatchType { get; set; }
        public int BatchNumber { get; set; }

        /// <summary>
        /// The app will request a file with the following format.
        /// </summary>
        /// <returns>"[date]_[batchnumber]_[dk/all].zip", e.g. "2020-08-13_1_dk.zip"</returns>
        public string ToBatchFileRequest()
        {
            return $"{Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"_{BatchNumber}" +
                $"_{BatchType.ToTypeString()}" +
                $".zip";
        }

        /// <summary>
        /// Generates the parameters used to pull keys, based on the last successful background task that was performed.
        /// </summary>
        public static PullKeysParams GenerateParams()
        {
            DateTime today = SystemTime.Now().ToUniversalTime().Date;

            //BatchType: We will pull all EU keys if consent is given to do so.
            bool consentToPullEUKeysIsGiven = OnboardingStatusHelper.Status == OnboardingStatus.CountriesOnboardingCompleted;

            BatchType batchType = consentToPullEUKeysIsGiven ? BatchType.ALL : BatchType.DK;

            //Date: Request data for the last successful background task
            DateTime lastPullDate = LocalPreferencesHelper.GetLastPullKeysSucceededDateTime();
            if (lastPullDate.Date.Equals(DateTime.MinValue.Date))
            {
                lastPullDate = today;
            }

            //BatchNumber: Request the next batch number after the last successful one.
            int num = LocalPreferencesHelper.LastPullKeysBatchNumberSuccessfullySubmitted;
            num += 1;

            //If more than 14 days ago, then only pull for 14 days
            if (lastPullDate.Date <= today.AddDays(-14))
            {
                lastPullDate = today.AddDays(-13);
                num = 1;
            }

            //If terms were just approved and last pull was DK keys, then start over for today with EU keys.
            if (consentToPullEUKeysIsGiven && LocalPreferencesHelper.LastPulledBatchType == BatchType.DK)
            {
                batchType = BatchType.ALL;
                num = 1;
            }

            return new PullKeysParams
            {
                BatchType = batchType,
                Date = lastPullDate,
                BatchNumber = num
            };
        }
    }
}

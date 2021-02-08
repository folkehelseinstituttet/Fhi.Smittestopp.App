using System;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class DailyNumbersViewModel
    {
        public static string DAILY_NUMBERS_HEADER => "DAILY_NUMBERS_HEADER".Translate();
        public static string DAILY_NUMBERS_SUBHEADER => "DAILY_NUMBERS_SUBHEADER".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_NEW => "KEY_FEATURE_ONE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_ALL => "KEY_FEATURE_ONE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_ONE_LABEL => "KEY_FEATURE_ONE_LABEL".Translate();
        public static string KEY_FEATURE_TWO_UPDATE_NEW => "KEY_FEATURE_TWO_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_TWO_UPDATE_ALL => "KEY_FEATURE_TWO_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_TWO_LABEL => "KEY_FEATURE_TWO_LABEL".Translate();
        public static string KEY_FEATURE_THREE_UPDATE_NEW => "KEY_FEATURE_THREE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_THREE_UPDATE_ALL => "KEY_FEATURE_THREE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_THREE_LABEL => "KEY_FEATURE_THREE_LABEL".Translate();
        public static string KEY_FEATURE_FOUR_UPDATE_NEW => "KEY_FEATURE_FOUR_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_FOUR_LABEL => "KEY_FEATURE_FOUR_LABEL".Translate();
        public static string KEY_FEATURE_FIVE_UPDATE_NEW => "KEY_FEATURE_FIVE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_FIVE_UPDATE_ALL => "KEY_FEATURE_FIVE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_FIVE_LABEL => "KEY_FEATURE_FIVE_LABEL".Translate();
        public static string KEY_FEATURE_SIX_UPDATE_ALL => "KEY_FEATURE_SIX_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_SIX_LABEL => "KEY_FEATURE_SIX_LABEL".Translate();
        public static string DAILY_NUMBERS_SUBSUBHEADER => "DAILY_NUMBERS_SUBSUBHEADER".Translate();
        private static readonly DailyNumbersWebService WebService;

        static DailyNumbersViewModel()
        {
            WebService = new DailyNumbersWebService();
        }

        public static DateTime LastUpdateFHINumbersDateTime => FHILastUpdateDateTime.ToLocalTime();
        public static DateTime LastUpdateDownloadsNumbersDateTime => APPDownloadNumberLastUpdateDateTime.ToLocalTime();

        public static string LastUpdateStringSubHeader => LastUpdateFHINumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBHEADER, $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "t")}")
            : "";

        public static string LastUpdateStringSubSubHeader => LastUpdateDownloadsNumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBSUBHEADER, $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "t")}")
            : "";

        public static async Task<bool> UpdateFHIDataAsync()
        {
            try
            {
                // TODO: Uncomment when backend is ready
                //DailyNumbersDTO fhiData = await (WebService ?? new DailyNumbersWebService()).GetFHIData();
                // Sample data for developer convenience. TODO: Delete when backend is ready.
                DailyNumbersDTO fhiData = new DailyNumbersDTO
                {
                    FHIStatistics = new FHIStatisticsDTO
                    {
                        ConfirmedCasesToday = 10,
                        ConfirmedCasesTotal = 123,
                        DeathsToday = 2,
                        DeathsTotal = 68,
                        TestsConductedToday = 396,
                        TestsConductedTotal = 3095,
                        EntryDate = DateTime.Now,
                        PatientsAdmittedToday = 44
                    },
                    AppStatistics = new AppStatisticsDTO
                    {
                        EntryDate = DateTime.Now,
                        NumberOfPositiveTestsResultsLast7Days = 33,
                        NumberOfPositiveTestsResultsTotal = 359,
                        SmittestoppDownloadsTotal = 159000
                    }
                };

                if (fhiData?.FHIStatistics == null || fhiData.AppStatistics == null)
                {
                    return false;
                }
                DailyNumbers.UpdateAll(fhiData);
                MessagingCenter.Send(new object(), MessagingCenterKeys.KEY_UPDATE_DAILY_NUMBERS);
                return true;
            }
            catch (NullReferenceException e)
            {
                LogUtils.LogException(LogSeverity.WARNING, e, $"{nameof(DailyNumbersViewModel)}.{nameof(UpdateFHIDataAsync)}: Failed to fetch the data.");
            }
            catch (Exception e)
            {
                LogUtils.LogException(LogSeverity.ERROR, e, $"{nameof(DailyNumbersViewModel)}.{nameof(UpdateFHIDataAsync)}: Unidentified exception.");
            }

            return false;
        }

        public static string ConfirmedCasesToday => string.Format(KEY_FEATURE_ONE_UPDATE_NEW, $"{DailyNumbers.FHIConfirmedCasesToday:N0}");
        public static string ConfirmedCasesTotal => string.Format(KEY_FEATURE_ONE_UPDATE_ALL, $"{DailyNumbers.FHIConfirmedCasesTotal:N0}");
        public static string DeathsToday => string.Format(KEY_FEATURE_TWO_UPDATE_NEW, $"{DailyNumbers.FHIDeathsToday:N0}");
        public static string DeathsTotal => string.Format(KEY_FEATURE_TWO_UPDATE_ALL, $"{DailyNumbers.FHIDeathsTotal:N0}");
        public static string TestsConductedToday => string.Format(KEY_FEATURE_THREE_UPDATE_NEW, $"{DailyNumbers.FHITestsConductedToday:N0}");
        public static string TestsConductedTotal => string.Format(KEY_FEATURE_THREE_UPDATE_ALL, $"{DailyNumbers.FHITestsConductedTotal:N0}");
        public static string PatientsAdmittedToday => string.Format(KEY_FEATURE_FOUR_UPDATE_NEW, $"{DailyNumbers.FHIPatientsAdmittedToday:N0}");
        public static string NumberOfPositiveTestsResultsLast7Days => string.Format(KEY_FEATURE_FIVE_UPDATE_NEW, $"{DailyNumbers.APPNumberOfPositiveTestsResultsLast7Days:N0}");
        public static string NumberOfPositiveTestsResultsTotal => string.Format(KEY_FEATURE_FIVE_UPDATE_ALL, $"{DailyNumbers.APPNumberOfPositiveTestsResultsTotal:N0}");
        public static string SmittestopDownloadsTotal => string.Format(KEY_FEATURE_SIX_UPDATE_ALL, $"{DailyNumbers.APPSmittestopDownloadsTotal:N0}");
    }
}
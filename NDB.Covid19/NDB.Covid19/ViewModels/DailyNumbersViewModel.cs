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
        public static string DAILY_NUMBERS_ACCESSIBILITY_BACK_BUTTON => "DAILY_NUMBERS_ACCESSIBILITY_BACK_BUTTON".Translate();
        public static string DAILY_NUMBERS_HEADER => "DAILY_NUMBERS_HEADER".Translate();
        public static string DAILY_NUMBERS_TITLE_ONE => "DAILY_NUMBERS_TITLE_ONE".Translate();
        public static string DAILY_NUMBERS_SUBHEADER => "DAILY_NUMBERS_SUBHEADER".Translate();
        public static string DAILY_NUMBERS_TITLE_TWO => "DAILY_NUMBERS_TITLE_TWO".Translate();
        public static string DAILY_NUMBERS_SUBTEXT_TWO => "DAILY_NUMBERS_SUBTEXT_TWO".Translate();
        public static string DAILY_NUMBERS_TITLE_THREE => "DAILY_NUMBERS_TITLE_THREE".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_NEW => "KEY_FEATURE_ONE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_ALL => "KEY_FEATURE_ONE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_ONE_LABEL => "KEY_FEATURE_ONE_LABEL".Translate();
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
        public static string KEY_FEATURE_SEVEN_LABEL => "KEY_FEATURE_SEVEN_LABEL".Translate();
        public static string KEY_FEATURE_SEVEN_UPDATE_ALL => "KEY_FEATURE_SEVEN_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_NINE_LABEL => "KEY_FEATURE_NINE_LABEL".Translate();
        public static string KEY_FEATURE_NINE_ACCESSIBILITY_LABEL => "KEY_FEATURE_NINE_ACCESSIBILITY_LABEL".Translate();
        public static string KEY_FEATURE_NINE_UPDATE_NEW => "KEY_FEATURE_NINE_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_NINE_UPDATE_ALL => "KEY_FEATURE_NINE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_TEN_LABEL => "KEY_FEATURE_TEN_LABEL".Translate();
        public static string KEY_FEATURE_TEN_ACCESSIBILITY_LABEL => "KEY_FEATURE_TEN_ACCESSIBILITY_LABEL".Translate();
        public static string KEY_FEATURE_TEN_UPDATE_NEW => "KEY_FEATURE_TEN_UPDATE_NEW".Translate();
        public static string KEY_FEATURE_TEN_UPDATE_ALL => "KEY_FEATURE_TEN_UPDATE_ALL".Translate();
        public static string DAILY_NUMBERS_SUBSUBHEADER => "DAILY_NUMBERS_SUBSUBHEADER".Translate();
        public static string KEY_FEAUTRE_SEVEN_LABEL => "KEY_FEATURE_SEVEL_LABEL".Translate();
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

        public static string LastUpdateStringSubTextTwo => LastUpdateFHINumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBTEXT_TWO, $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "t")}")
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
                        TestsConductedToday = 396,
                        TestsConductedTotal = 3095,
                        EntryDate = DateTime.Now,
                        PatientsAdmittedToday = 44,
                        PatientsIntensiveCare = 2,
                        VaccinationsDoseOneToday = 1230,
                        VaccinationsDoseTwoToday = 143,
                        VaccinationsDoseOneTotal = 50477,
                        VaccinationsDoseTwoTotal = 148

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
        public static string TestsConductedToday => string.Format(KEY_FEATURE_THREE_UPDATE_NEW, $"{DailyNumbers.FHITestsConductedToday:N0}");
        public static string TestsConductedTotal => string.Format(KEY_FEATURE_THREE_UPDATE_ALL, $"{DailyNumbers.FHITestsConductedTotal:N0}");
        public static string PatientsAdmittedToday => string.Format(KEY_FEATURE_FOUR_UPDATE_NEW, $"{DailyNumbers.FHIPatientsAdmittedToday:N0}");
        public static string NumberOfPositiveTestsResultsLast7Days => string.Format(KEY_FEATURE_FIVE_UPDATE_NEW, $"{DailyNumbers.APPNumberOfPositiveTestsResultsLast7Days:N0}");
        public static string NumberOfPositiveTestsResultsTotal => string.Format(KEY_FEATURE_FIVE_UPDATE_ALL, $"{DailyNumbers.APPNumberOfPositiveTestsResultsTotal:N0}");
        public static string SmittestopDownloadsTotal => string.Format(KEY_FEATURE_SIX_UPDATE_ALL, $"{DailyNumbers.APPSmittestopDownloadsTotal:N0}");
        public static string PatientsIntensiveCare => string.Format(KEY_FEATURE_SEVEN_UPDATE_ALL, $"{DailyNumbers.FHIPatientsIntensiveCare:N0}");
        public static string VaccinationsDoseOneToday => string.Format(KEY_FEATURE_NINE_UPDATE_NEW, $"{DailyNumbers.FHIVaccinationsDoseOneToday:N0}");
        public static string VaccinationsDoseOneTotal => string.Format(KEY_FEATURE_NINE_UPDATE_ALL, $"{DailyNumbers.FHIVaccinationsDoseOneTotal:N0}");
        public static string VaccinationsDoseTwoToday => string.Format(KEY_FEATURE_TEN_UPDATE_NEW, $"{DailyNumbers.FHIVaccinationsDoseTwoToday:N0}");
        public static string VaccinationsDoseTwoTotal => string.Format(KEY_FEATURE_TEN_UPDATE_ALL, $"{DailyNumbers.FHIVaccinationsDoseTwoTotal:N0}");
    }
}
using System;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class DailyNumbersViewModel
    {
        public static string BACK_BUTTON_ACCESSIBILITY_TEXT => "BACK_BUTTON_ACCESSIBILITY_TEXT".Translate();
        public static string DAILY_NUMBERS_HEADER => "DAILY_NUMBERS_HEADER".Translate();
        public static string DAILY_NUMBERS_TITLE_ONE => "DAILY_NUMBERS_TITLE_ONE".Translate();
        public static string DAILY_NUMBERS_SUBHEADER => "DAILY_NUMBERS_SUBHEADER".Translate();
        public static string DAILY_NUMBERS_TITLE_TWO => "DAILY_NUMBERS_TITLE_TWO".Translate();
        public static string DAILY_NUMBERS_SUBTEXT_TWO => "DAILY_NUMBERS_SUBTEXT_TWO".Translate();
        public static string DAILY_NUMBERS_TITLE_THREE => "DAILY_NUMBERS_TITLE_THREE".Translate();
        public static string KEY_FEATURE_ONE_UPDATE_ALL => "KEY_FEATURE_ONE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_ONE_LABEL => "KEY_FEATURE_ONE_LABEL".Translate();
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
        public static string KEY_FEATURE_EIGHT_LABEL => "KEY_FEATURE_EIGHT_LABEL".Translate();
        public static string KEY_FEATURE_EIGHT_UPDATE_ALL => "KEY_FEATURE_EIGHT_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_NINE_LABEL => "KEY_FEATURE_NINE_LABEL".Translate();
        public static string KEY_FEATURE_NINE_ACCESSIBILITY_LABEL => "KEY_FEATURE_NINE_ACCESSIBILITY_LABEL".Translate();
        public static string KEY_FEATURE_NINE_UPDATE_ALL => "KEY_FEATURE_NINE_UPDATE_ALL".Translate();
        public static string KEY_FEATURE_TEN_LABEL => "KEY_FEATURE_TEN_LABEL".Translate();
        public static string KEY_FEATURE_TEN_ACCESSIBILITY_LABEL => "KEY_FEATURE_TEN_ACCESSIBILITY_LABEL".Translate();
        public static string KEY_FEATURE_TEN_UPDATE_ALL => "KEY_FEATURE_TEN_UPDATE_ALL".Translate();
        public static string DAILY_NUMBERS_SUBSUBHEADER => "DAILY_NUMBERS_SUBSUBHEADER".Translate();
        private static readonly DailyNumbersWebService WebService;

        static DailyNumbersViewModel()
        {
            WebService = new DailyNumbersWebService();
        }

        public static DateTime LastUpdateFHINumbersDateTime => FHILastUpdateDateTime.ToLocalTime();
        public static DateTime LastUpdateDownloadsNumbersDateTime => APPDownloadNumberLastUpdateDateTime.ToLocalTime();

        public static string LastUpdateStringSubHeader => LastUpdateFHINumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBHEADER, $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "t", true, "so", "en")}")
            : "";

        public static string LastUpdateStringSubTextTwo => LastUpdateFHINumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBTEXT_TWO, $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateFHINumbersDateTime, "t", true, "so", "en")}")
            : "";

        public static string LastUpdateStringSubSubHeader => LastUpdateDownloadsNumbersDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(DAILY_NUMBERS_SUBSUBHEADER, $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateDownloadsNumbersDateTime, "t", true, "so", "en")}")
            : "";

        public static async void RequestFHIDataUpdate(Action onFinished = null)
        {
            // Update only if the date of the last update is different than today
            // to limit the requests
            if (FHILastUpdateDateTime.Date != SystemTime.Now().ToLocalTime().Date)
            {
                await UpdateFHIDataAsync();
                onFinished?.Invoke();
            }
        }
        
        public static async Task<bool> UpdateFHIDataAsync()
        {
            try
            {
                DailyNumbersDTO fhiData = await (WebService ?? new DailyNumbersWebService()).GetFHIData();

                if (fhiData?.CovidStatistics == null || fhiData.ApplicationStatistics == null)
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

        public static string ConfirmedCasesTotal => string.Format(KEY_FEATURE_ONE_UPDATE_ALL, $"{DailyNumbers.FHIConfirmedCasesTotal:N0}");
        public static string TestsConductedTotal => string.Format(KEY_FEATURE_THREE_UPDATE_ALL, $"{DailyNumbers.FHITestsConductedTotal:N0}");
        public static string PatientsAdmittedTotal => string.Format(KEY_FEATURE_FOUR_UPDATE_NEW, $"{DailyNumbers.FHIPatientsAdmittedTotal:N0}");
        public static string NumberOfPositiveTestsResultsLast7Days => string.Format(KEY_FEATURE_FIVE_UPDATE_NEW, $"{DailyNumbers.APPNumberOfPositiveTestsResultsLast7Days:N0}");
        public static string NumberOfPositiveTestsResultsTotal => string.Format(KEY_FEATURE_FIVE_UPDATE_ALL, $"{DailyNumbers.APPNumberOfPositiveTestsResultsTotal:N0}");
        public static string SmittestopDownloadsTotal => string.Format(KEY_FEATURE_SIX_UPDATE_ALL, $"{DailyNumbers.APPSmittestopDownloadsTotal:N0}");
        public static string PatientsIntensiveCareTotal => string.Format(KEY_FEATURE_SEVEN_UPDATE_ALL, $"{DailyNumbers.FHIPatientsIntensiveCareTotal:N0}");
        public static string DeathsTotal => string.Format(KEY_FEATURE_EIGHT_UPDATE_ALL, $"{DailyNumbers.FHIDeathsTotal:N0}");
        public static string VaccinationsDoseOneTotal => string.Format(KEY_FEATURE_NINE_UPDATE_ALL, $"{DailyNumbers.FHIVaccinationsDoseOneTotal:N0}");
        public static string VaccinationsDoseTwoTotal => string.Format(KEY_FEATURE_TEN_UPDATE_ALL, $"{DailyNumbers.FHIVaccinationsDoseTwoTotal:N0}");
    }
}
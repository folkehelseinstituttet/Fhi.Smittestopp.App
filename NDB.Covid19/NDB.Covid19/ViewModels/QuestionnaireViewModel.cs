using System;
using System.Globalization;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Utils;
using NDB.Covid19.WebServices.ErrorHandlers;
using static NDB.Covid19.OAuth2.AuthenticationState;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.ViewModels
{
    public class QuestionnaireViewModel
    {
        public static string REGISTER_QUESTIONAIRE_HEADER => "REGISTER_QUESTIONAIRE_HEADER".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_TEXT => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP".Translate();
        public static string REGISTER_QUESTIONAIRE_SYMPTOMONSET_HELP => "REGISTER_QUESTIONAIRE_SYMPTOMONSET_HELP".Translate();
        public static string REGISTER_QUESTIONAIRE_NEXT => "REGISTER_QUESTIONAIRE_NEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_SHARING_TEXT => "REGISTER_QUESTIONAIRE_SHARING_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_SHARING_ANSWER_YES => "REGISTER_QUESTIONAIRE_SHARING_ANSWER_YES".Translate();
        public static string REGISTER_QUESTIONAIRE_SHARING_ANSWER_NO => "REGISTER_QUESTIONAIRE_SHARING_ANSWER_NO".Translate();
        public static string REGISTER_QUESTIONAIRE_SHARING_ANSWER_SKIP => "REGISTER_QUESTIONAIRE_SHARING_ANSWER_SKIP".Translate();
        public static string REGISTER_QUESTIONAIRE_SUBMIT => "REGISTER_QUESTIONAIRE_SUBMIT".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_HEADER => "REGISTER_QUESTIONAIRE_RECEIPT_HEADER".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_TEXT => "REGISTER_QUESTIONAIRE_RECEIPT_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION => "REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_DISMISS => "REGISTER_QUESTIONAIRE_RECEIPT_DISMISS".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER => "REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE => "REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE".Translate();
        public static string REGISTER_QUESTIONAIRE_RECEIPT_LINK => "REGISTER_QUESTIONAIRE_RECEIPT_LINK".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_1_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_1_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_2_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_2_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_3_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_3_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_4_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_4_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_DATEPICKER_TEXT => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_DATEPICKER_TEXT".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_DATE_INFO_BUTTON => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_DATE_INFO_BUTTON".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE => "REGISTER_QUESTIONAIRE_ACCESSIBILITY_LOADING_PAGE_TITLE".Translate();
        public static string REGISTER_QUESTIONAIRE_ACCESSIBILITY_DATEPICKER => "REGISTER_QUESTIONAIRE_CHOOSE_DATE_POP_UP".Translate();
        public static string REGISTER_QUESTIONAIRE_DATE_LABEL_FORMAT => "REGISTER_QUESTIONAIRE_DATE_LABEL_FORMAT".Translate();
        public static string CHOOSE_TYPE_OF_TEST_HEADER => "CHOOSE_TYPE_OF_TEST_HEADER".Translate();
        public static string CHOOSE_CONFIRMED_TEST_BUTTON_TEXT => "CHOOSE_CONFIRMED_TEST_BUTTON_TEXT".Translate();
        public static string CHOOSE_SELF_TEST_BUTTON_TEXT => "CHOOSE_SELF_TEST_BUTTON_TEXT".Translate();

        public DialogViewModel CloseDialogViewModel => new DialogViewModel
        {
            Title = ErrorViewModel.REGISTER_LEAVE_HEADER,
            Body = ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
            OkBtnTxt = ErrorViewModel.REGISTER_LEAVE_CONFIRM,
            CancelbtnTxt = ErrorViewModel.REGISTER_LEAVE_CANCEL
        };

        public static string DateLabel => _selectedDateUTC == DateTime.MinValue
            ? REGISTER_QUESTIONAIRE_DATE_LABEL_FORMAT
            : DateUtils.GetDateFromDateTime(_localSelectedDate, "d");

        public static bool DateHasBeenSet;
        static DateTime _selectedDateUTC { get; set; }
        static DateTime _localSelectedDate => DateTime.SpecifyKind(_selectedDateUTC, DateTimeKind.Utc).ToLocalTime();
        public static QuestionaireSelection Selection { get; private set; } = QuestionaireSelection.Skip;
        public DateTime MinimumDate => SystemTime.Now().Date.AddDays(-20);
        public DateTime MaximumDate => SystemTime.Now().Date;

        public string RadioButtonAccessibilityDatepicker => REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES + ". " + REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_DATEPICKER_TEXT + ". " + REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_1_TEXT;
        public string RadioButtonAccessibilityYesDontRemember => REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT + ". " + REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_2_TEXT;
        public string RadioButtonAccessibilityNo => REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO + "\n " + REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_3_TEXT;
        public string RadioButtonAccessibilitySkip => REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP + ". " + REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_4_TEXT;

        public string ReceipetPageReadMoreButtonAccessibility => $"{REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER}. {REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE}";

        public void SetSelectedDateUTC(DateTime newDate)
        {
            _selectedDateUTC = newDate;
            DateHasBeenSet = true;
        }

        public static DateTime GetLocalSelectedDate()
        {
            return _localSelectedDate;
        }

        public void SetSelection(QuestionaireSelection selection)
        {
            Selection = selection;
        }

        private DateTime? GetMSISDateFromPersonalData()
        {
            if (!IsReportingSelfTest && PersonalData.Validate())
            {
                try
                {
                    return Convert.ToDateTime(PersonalData.Covid19_smitte_start, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    LogUtils.LogMessage(LogSeverity.ERROR, e.StackTrace, "MSIS data can't be parsed into DateTime");
                }
            }
            return null;
        }

        public void InvokeNextButtonClick(
            Action onSuccess,
            Action onFail,
            Action onValidationFail,
            PlatformDialogServiceArguments platformDialogServiceArguments = null)
        {
            DateTime? dateInMSIS = GetMSISDateFromPersonalData();

            // If we are not able to get infection date from MSIS on a confirmed test, this should result in an error
            if (dateInMSIS == null && !IsReportingSelfTest)
            {
                onFail?.Invoke();
                return;
            }

            if (Selection == QuestionaireSelection.YesSince)
            {
                if (_selectedDateUTC == DateTime.MinValue)
                {
                    ServiceLocator.Current
                        .GetInstance<IDialogService>()
                        .ShowMessageDialog(
                            null,
                            "REGISTER_QUESTIONAIRE_CHOOSE_DATE_POP_UP".Translate(),
                            "ERROR_OK_BTN".Translate(),
                            platformDialogServiceArguments);

                    onValidationFail?.Invoke();

                    return;
                }

                if (IsReportingSelfTest)
                {
                    PersonalData.FinalDateToSetDSOS = _localSelectedDate < SystemTime.Now().Date ? _localSelectedDate : SystemTime.Now().Date;
                }
                else
                {
                    PersonalData.FinalDateToSetDSOS = dateInMSIS < _localSelectedDate ? dateInMSIS : _localSelectedDate;
                }
            }
            else
            {
                if (dateInMSIS != null)
                {
                    PersonalData.FinalDateToSetDSOS = dateInMSIS;
                }
                else
                {
                    PersonalData.FinalDateToSetDSOS = SystemTime.Now().Date;
                }
            }
            onSuccess?.Invoke();
        }
    }
}

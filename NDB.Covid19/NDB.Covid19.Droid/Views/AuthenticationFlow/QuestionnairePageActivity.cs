﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using I18NPortable;
using MoreLinq.Extensions;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.QuestionnaireViewModel;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using Object = Java.Lang.Object;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    class QuestionnairePageActivity : AppCompatActivity
    {
        private DatePickerDialog _datePicker;
        private TextView _datePickerTextView;
        private RadioButton _firstRadioButton;
        private RadioButton _secondRadioButton;
        private RadioButton _thirdRadioButton;
        private RadioButton _fourthRadioButton;
        private ImageButton _infoButton;
        private bool _isChangedFromDatePicker;
        private Button _questionnaireButton;
        private Button _closeButton;
        private QuestionnaireViewModel _questionnaireViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = REGISTER_QUESTIONAIRE_HEADER;
            SetContentView(Resource.Layout.questionnaire_page);
            Init();
        }

        private void Init()
        {

            _questionnaireViewModel = new QuestionnaireViewModel();

            TextView questionnaireTitle = FindViewById<TextView>(Resource.Id.questionnaire_title);
            questionnaireTitle.Text = REGISTER_QUESTIONAIRE_HEADER;
            questionnaireTitle.ContentDescription = REGISTER_QUESTIONAIRE_HEADER;
            questionnaireTitle.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            TextView questionnaireSubtitle = FindViewById<TextView>(Resource.Id.questionnaire_subtitle);
            ISpanned questionnaireSubtitleTextFormatted =
                HtmlCompat.FromHtml(REGISTER_QUESTIONAIRE_SYMPTOMONSET_TEXT, HtmlCompat.FromHtmlModeLegacy);
            questionnaireSubtitle.TextFormatted = questionnaireSubtitleTextFormatted;
            questionnaireSubtitle.ContentDescriptionFormatted = questionnaireSubtitleTextFormatted;

            _questionnaireButton = FindViewById<Button>(Resource.Id.questionnaire_button);
            _questionnaireButton.Text = REGISTER_QUESTIONAIRE_NEXT;
            _questionnaireButton.ContentDescription = REGISTER_QUESTIONAIRE_NEXT;
            _questionnaireButton.Click += OnNextButtonClick;

            _infoButton = FindViewById<ImageButton>(Resource.Id.questionnaire_info_button);
            _infoButton.ContentDescription = REGISTER_QUESTIONAIRE_ACCESSIBILITY_DATE_INFO_BUTTON;
            _infoButton.Click += OnInfoButtonPressed;

            _closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);
            _closeButton.ContentDescription = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            _closeButton.Click +=
                new SingleClick((o, ev) => ShowAreYouSureToExitDialog()).Run;

            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing the Questionnaire page");

            PrepareRadioButtons();
        }

        private void PrepareRadioButtons()
        {
            _firstRadioButton = FindViewById<RadioButton>(Resource.Id.firstRadioButton);
            _secondRadioButton = FindViewById<RadioButton>(Resource.Id.secondRadioButton);
            _thirdRadioButton = FindViewById<RadioButton>(Resource.Id.thirdRadioButton);
            _fourthRadioButton = FindViewById<RadioButton>(Resource.Id.fourthRadioButton);

            _fourthRadioButton.Checked = true;

            _datePickerTextView = FindViewById<TextView>(Resource.Id.date_picker);
            _datePickerTextView.Text = DateLabel;
            _datePickerTextView.ContentDescription = REGISTER_QUESTIONAIRE_ACCESSIBILITY_RADIO_BUTTON_DATEPICKER_TEXT;
            _firstRadioButton.Text = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES;
            _secondRadioButton.Text = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT;
            _thirdRadioButton.Text = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO;
            _fourthRadioButton.Text = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP;

            _firstRadioButton.ContentDescriptionFormatted = GetFormattedText;
            _secondRadioButton.ContentDescription = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT;
            _thirdRadioButton.ContentDescription = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO;
            _fourthRadioButton.ContentDescription = REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP;

            List<RadioButton> radioButtons = new List<RadioButton>
            {
                _firstRadioButton, _secondRadioButton, _thirdRadioButton, _fourthRadioButton
            };
            radioButtons.ForEach(((button, i) => button.SetOnClickListener(new CustomRadioOnClickListener(radioButtons, i, OnCheckChange))));
            _datePickerTextView.SetOnClickListener(new CustomRadioOnClickListener(radioButtons, 0, OnDateEditTextClick));
        }

        class CustomRadioOnClickListener : Object, View.IOnClickListener
        {
            private readonly Action<RadioButton> _action;
            private readonly List<RadioButton> _radioButtonList;
            private readonly int _radioButtonToCheck;

            public CustomRadioOnClickListener(List<RadioButton> radioButtons, int radioButtonToCheck, Action<RadioButton> onCheckChange)
            {
                _action = onCheckChange;
                _radioButtonList = radioButtons;
                _radioButtonToCheck = radioButtonToCheck;
            }

            public void OnClick(View v)
            {
                _radioButtonList[_radioButtonToCheck].Checked = true;

                _radioButtonList.Where(((button, i) => i != _radioButtonToCheck)).ForEach(button => button.Checked = false);
                _action?.Invoke(_radioButtonList[_radioButtonToCheck]);
            }
        }

        ISpanned GetFormattedText =>
            HtmlCompat.FromHtml(
                $"{REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES} <input type=\"date\">{_datePickerTextView.ContentDescription}</input>",
                HtmlCompat.FromHtmlModeLegacy);

        private void OnCheckChange(RadioButton radioButton)
        {
            int radioButtonsCheckedRadioButtonId = radioButton.Id;
            switch (radioButtonsCheckedRadioButtonId)
            {
                case Resource.Id.firstRadioButton:
                    _questionnaireViewModel.SetSelection(QuestionaireSelection.YesSince);
                    if (!_isChangedFromDatePicker)
                    {
                        ShowDatePickerDialog();
                    }
                    else if (_isChangedFromDatePicker)
                    {
                        _isChangedFromDatePicker = false;
                    }
                    break;
                case Resource.Id.secondRadioButton:
                    _questionnaireViewModel.SetSelection(QuestionaireSelection.YesBut);
                    if (_isChangedFromDatePicker)
                    {
                        _isChangedFromDatePicker = false;
                    }
                    break;
                case Resource.Id.thirdRadioButton:
                    _questionnaireViewModel.SetSelection(QuestionaireSelection.No);
                    if (_isChangedFromDatePicker)
                    {
                        _isChangedFromDatePicker = false;
                    }
                    break;
                case Resource.Id.fourthRadioButton:
                    _questionnaireViewModel.SetSelection(QuestionaireSelection.Skip);
                    if (_isChangedFromDatePicker)
                    {
                        _isChangedFromDatePicker = false;
                    }
                    break;
            }
        }

        private void OnDateEditTextClick(RadioButton button)
        {
            ShowDatePickerDialog();
            _isChangedFromDatePicker = true;
            OnCheckChange(button);
        }

        private void ShowDatePickerDialog()
        {
            DateTime previousSelection = GetLocalSelectedDate();
            int day = DateHasBeenSet ? previousSelection.Day : DateTime.Now.Day;
            int month = DateHasBeenSet ? previousSelection.Month - 1 : DateTime.Now.Month - 1; // DatePicker uses 0-based month indexing, DateTime does not.
            int year = DateHasBeenSet ? previousSelection.Year : DateTime.Now.Year;

            _datePicker = new DatePickerDialog(Current.Activity,
                ((sender, args) =>
                {
                    _questionnaireViewModel.SetSelectedDateUTC(args.Date.ToUniversalTime());
                    _firstRadioButton.Checked = true;
                    _firstRadioButton.ContentDescriptionFormatted = GetFormattedText;
                    _datePickerTextView.Text = DateLabel;
                    _datePickerTextView.Ellipsize = TextUtils.TruncateAt.End;
                }), year, month, day);

            _datePicker.DatePicker.MinDate = DateTimeToAndroidDatePickerLong(_questionnaireViewModel.MinimumDate.ToLocalTime());
            _datePicker.DatePicker.MaxDate = DateTimeToAndroidDatePickerLong(DateTime.Now);
            _datePicker.Show();
        }

        private long DateTimeToAndroidDatePickerLong(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
        }

        public override void OnBackPressed()
        {
            ShowAreYouSureToExitDialog();
        }

        private async void ShowAreYouSureToExitDialog()
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(
                this,
                ErrorViewModel.REGISTER_LEAVE_HEADER,
                ErrorViewModel.REGISTER_LEAVE_DESCRIPTION,
                ErrorViewModel.REGISTER_LEAVE_CONFIRM,
                ErrorViewModel.REGISTER_LEAVE_CANCEL);
            if (isOkPressed)
            {
                GoToInfectionStatusPage();
            }
        }

        private void OnNextButtonClick(object o, EventArgs args)
        {
            _questionnaireViewModel.InvokeNextButtonClick(
                GoToLoadingPage, null, null);
        }

        private void OnInfoButtonPressed(object o, EventArgs args)
        {
            DialogUtils.DisplayBubbleDialog(this, REGISTER_QUESTIONAIRE_SYMPTOMONSET_HELP, "ERROR_OK_BTN".Translate());
        }

        // Left for future release when we will support other countries
        private void GoToQuestionnaireCountriesSelectionPage() =>
            StartActivity(new Intent(this, typeof(QuestionnaireCountriesSelectionActivity)));

        private void GoToLoadingPage() =>
            StartActivity(new Intent(this, typeof(LoadingPageActivity)));
        
        private void GoToInfectionStatusPage() => NavigationHelper.GoToResultPageAndClearTop(this);
    }
}
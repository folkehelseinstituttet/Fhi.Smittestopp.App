using System;
using Foundation;
using I18NPortable;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices.ErrorHandlers;
using UIKit;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class QuestionnaireViewController : BaseViewController
    {
        public QuestionnaireViewController(IntPtr handle) : base(handle)
        {
        }

        public static QuestionnaireViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("Questionnaire", null);
            QuestionnaireViewController vc = storyboard.InstantiateInitialViewController() as QuestionnaireViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        QuestionnaireViewModel _viewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new QuestionnaireViewModel();

            SetTexts();
            SetStyling();
            UpdateUIWhenSelectionChanges();
            SetAccessibilityAttributes();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing the Questionnaire page");
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            DatePicker.ValueChanged += DatePickerChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            DatePicker.ValueChanged -= DatePickerChanged;
        }

        void SetTexts()
        {
            TitleLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_HEADER);
            SubtitleLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_TEXT);
            YesLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YES, StyleUtil.FontType.FontBold);
            YesButLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_YESBUT, StyleUtil.FontType.FontBold);
            NoLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_NO, StyleUtil.FontType.FontBold);
            SkipLbl.SetAttributedText(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_ANSWER_SKIP, StyleUtil.FontType.FontBold);
            UpdateDateLbl(QuestionnaireViewModel.DateLabel);
        }

        void UpdateDateVisibility()
        {
            DatePicker.Superview.Hidden = !(QuestionnaireViewModel.Selection == QuestionaireSelection.YesSince);

            if (QuestionnaireViewModel.Selection == QuestionaireSelection.YesSince)
            {
                DateTime valueToShow = QuestionnaireViewModel.DateHasBeenSet ? QuestionnaireViewModel.GetLocalSelectedDate() : DateTime.Now.Date;
                _viewModel.SetSelectedDateUTC(valueToShow); // Set the default date value when user enter pick date mode
                UpdateDateLbl(QuestionnaireViewModel.DateLabel);

                UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, DatePicker);
            }
        }

        void SetStyling()
        {
            DateContainer.Layer.CornerRadius = 8;

            DatePicker.Superview.Layer.CornerRadius = 12;
            DatePicker.MinimumDate = (NSDate)DateTime.SpecifyKind(_viewModel.MinimumDate, DateTimeKind.Utc);
            DatePicker.MaximumDate = (NSDate)DateTime.SpecifyKind(_viewModel.MaximumDate, DateTimeKind.Utc);

            NextBtn.SetTitle(QuestionnaireViewModel.REGISTER_QUESTIONAIRE_NEXT, UIControlState.Normal);
        }

        void UpdateDateLbl(string text)
        {
            string sizeCategory = UIApplication.SharedApplication.PreferredContentSizeCategory;
            StyleUtil.InitMonospacedLabel(DateLbl, StyleUtil.FontType.FontMonospaced, DateUtils.ReplaceAndInsertNewlineiOS(text, sizeCategory), 1.28, 12, 18);
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            DialogHelper.ShowDialog(this, _viewModel.CloseDialogViewModel, CloseConfirmed, UIAlertActionStyle.Destructive);
        }

        void CloseConfirmed(UIAlertAction obj)
        {
            NavigationController?.DismissViewController(true, AfterDismissed);
        }

        void AfterDismissed()
        {
            YesSinceBtn.Dispose();
            YesButBtn.Dispose();
            NoBtn.Dispose();
            SkipBtn.Dispose();
        }

        partial void OnYesSinceLayoutTapped(UIButton sender)
        {
            HandleRadioBtnChange(QuestionaireSelection.YesSince, sender);
        }

        partial void OnYesButLayoutTapped(UIButton sender)
        {
            HandleRadioBtnChange(QuestionaireSelection.YesBut, sender);
        }

        partial void OnNoLayoutTapped(UIButton sender)
        {
            HandleRadioBtnChange(QuestionaireSelection.No, sender);
        }

        partial void OnSkipLayoutTapped(UIButton sender)
        {
            HandleRadioBtnChange(QuestionaireSelection.Skip, sender);
        }

        void HandleRadioBtnChange(QuestionaireSelection selection, UIButton sender)
        {
            if (QuestionnaireViewModel.Selection == selection)
            {
                return;
            }

            _viewModel.SetSelection(selection);
            UpdateUIWhenSelectionChanges();
        }

        partial void NextBtnTapped(CustomSubclasses.DefaultBorderButton sender)
        {
            NextBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);
            _viewModel.InvokeNextButtonClick(OnSuccess, OnFail, OnValidationFail,
                new PlatformDialogServiceArguments()
                {
                    Context = this
                });
        }

        void OnValidationFail()
        {
            NextBtn.HideSpinner();
        }

        void OnFail()
        {
            NextBtn.HideSpinner();
        }

        void OnSuccess()
        {
            NextBtn.HideSpinner();
            NavigationController?.PushViewController(QuestionnaireCountriesViewController.Create(), true);
        }

        void DatePickerChanged(object sender, EventArgs e)
        {
            _viewModel.SetSelectedDateUTC(NSDateToDateTime(DatePicker.Date));
            UpdateDateLbl(QuestionnaireViewModel.DateLabel);
        }

        static DateTime NSDateToDateTime(NSDate date)
        {
            return DateTime.SpecifyKind((DateTime)date, DateTimeKind.Unspecified);
        }

        void UpdateUIWhenSelectionChanges()
        {
            YesSinceBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.YesSince;
            YesButBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.YesBut;
            NoBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.No;
            SkipBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.Skip;

            YesSinceLargeBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.YesSince;
            YesButLargeBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.YesBut;
            NoLargeBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.No;
            SkipLargeBtn.Selected = QuestionnaireViewModel.Selection == QuestionaireSelection.Skip;

            UpdateDateVisibility();
            SetAccessibilityAttributes();
        }

        void SetAccessibilityAttributes()
        {
            TitleLbl.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_HEADER;
            CloseButton.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
            InfoButton.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_DATE_INFO_BUTTON;

            //Labels
            DatepickerStackView.AccessibilityElementsHidden = true;
            YesButLbl.AccessibilityElementsHidden = true;
            NoLbl.AccessibilityElementsHidden = true;
            SkipLbl.AccessibilityElementsHidden = true;

            //Radiobutton
            YesSinceBtn.AccessibilityElementsHidden = true;
            YesButBtn.AccessibilityElementsHidden = true;
            NoBtn.AccessibilityElementsHidden = true;
            SkipBtn.AccessibilityElementsHidden = true;

            //Buttons ontop used for accessibility
            YesSinceLargeBtn.AccessibilityLabel = _viewModel.RadioButtonAccessibilityDatepicker;
            YesButLargeBtn.AccessibilityLabel = _viewModel.RadioButtonAccessibilityYesDontRemember;
            NoLargeBtn.AccessibilityLabel = _viewModel.RadioButtonAccessibilityNo;
            SkipLargeBtn.AccessibilityLabel = _viewModel.RadioButtonAccessibilitySkip;
        }

        partial void InfoButton_TouchUpInside(UIButton sender)
        {
            HelpCustomView.HelpCustomView.Create(View, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_HELP, "ERROR_OK_BTN".Translate(), sender);
        }
    }
}
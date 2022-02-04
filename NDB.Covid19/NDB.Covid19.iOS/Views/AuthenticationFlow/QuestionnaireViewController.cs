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
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class QuestionnaireViewController : BaseViewController, IUIAccessibilityContainer
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

            // Generate and set the correlation id for current authentication flow
            UpdateCorrelationId(LogUtils.GenerateCorrelationId());

            SetTexts();
            SetStyling();
            UpdateUIWhenSelectionChanges();
            SetAccessibilityAttributes();
            AddObservers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing the Questionnaire", null, GetCorrelationId());
            DatePicker.ValueChanged += DatePickerChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
            DatePicker.ValueChanged -= DatePickerChanged;
        }

        void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Questionnaire", null, GetCorrelationId());
            });

            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user left Questionnaire", null, GetCorrelationId());
            });
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
            }
        }

        void SetStyling()
        {
            DateContainer.Layer.CornerRadius = 8;
            DateContainer.BackgroundColor = UIColor.Clear;
            DateContainer.Layer.BorderWidth = 1;
            DateContainer.Layer.BorderColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND.CGColor;

            DatePicker.Superview.Layer.CornerRadius = 6;
            if (!AppDelegate.ShouldOperateIn12_5Mode) DatePicker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            DatePicker.MinimumDate = (NSDate)DateTime.SpecifyKind(_viewModel.MinimumDate, DateTimeKind.Utc);
            DatePicker.MaximumDate = (NSDate)DateTime.SpecifyKind(_viewModel.MaximumDate, DateTimeKind.Utc);

            StyleUtil.InitButtonStyling(NextBtn, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_NEXT);
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
            LogUtils.LogMessage(LogSeverity.INFO, "The user is returning to Infection Status", null, GetCorrelationId());
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
            if(QuestionnaireViewModel.Selection == QuestionaireSelection.Skip)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user does not want to provide health information", null, GetCorrelationId());
            }
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
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireViewController)}.{nameof(OnFail)}: " +
                $"AuthenticationState.personaldata is not valid");
        }

        void OnSuccess()
        {
            NextBtn.HideSpinner();
            if (IsReportingSelfTest)
            {
                NavigationController?.PushViewController(LoadingPageViewController.Create(), true);
            }
            else
            {
                NavigationController?.PushViewController(CountriesConsentViewController.Create(), true);
            }
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
            TitleLbl.AccessibilityTraits = UIAccessibilityTrait.Header;

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

            if (UIAccessibility.IsVoiceOverRunning)
            {
                this.SetAccessibilityElements(NSArray.FromNSObjects(ScrollView, CloseButton));
                PostAccessibilityNotificationAndReenableElement(CloseButton, TitleLbl);
            }
        }

        partial void InfoButton_TouchUpInside(UIButton sender)
        {
            HelpCustomView.HelpCustomView.Create(View, QuestionnaireViewModel.REGISTER_QUESTIONAIRE_SYMPTOMONSET_HELP, "ERROR_OK_BTN".Translate(), sender);
        }
    }
}
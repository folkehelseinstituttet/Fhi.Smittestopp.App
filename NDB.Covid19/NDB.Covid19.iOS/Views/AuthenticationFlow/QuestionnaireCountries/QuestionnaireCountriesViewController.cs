using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.WebServices.ErrorHandlers;
using System;
using System.Collections.Generic;
using UIKit;
using NDB.Covid19.Models.DTOsForServer;
using NDB.Covid19.Enums;
using System.Linq;

namespace NDB.Covid19.iOS
{
    public partial class QuestionnaireCountriesViewController : BaseViewController
    {
        public QuestionnaireCountriesViewController(IntPtr handle) : base(handle)
        {
        }

        public static QuestionnaireCountriesViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("QuestionnaireCountries", null);
            QuestionnaireCountriesViewController vc = storyboard.InstantiateInitialViewController() as QuestionnaireCountriesViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        UIActivityIndicatorView _spinner;
        QuestionnaireCountriesViewModel _viewModel;
        List<CountryDetailsViewModel> _countryList;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new QuestionnaireCountriesViewModel();

            SetStyling();
            SetAccessibility();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SetupTableView();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        void SetStyling()
        {
            StyleUtil.InitLabelWithSpacing(TitleLbl, StyleUtil.FontType.FontBold, QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_HEADER_TEXT, 1.14, 24, 38);
            StyleUtil.InitLabelWithSpacing(SubtitleLbl, StyleUtil.FontType.FontRegular, QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_INFORMATION_TEXT, 1.28, 16, 20);
            StyleUtil.InitLabelWithSpacing(ListExplainLbl, StyleUtil.FontType.FontRegular, QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_FOOTER, 1.28, 16, 20);
            NextBtn.SetTitle(QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_BUTTON_TEXT, UIControlState.Normal);
            ButtonView.Alpha = 0.9f;
            ButtonView.BackgroundColor = "#001F34".ToUIColor();
        }

        async void SetupTableView()
        {
            _spinner = StyleUtil.ShowSpinner(View, UIActivityIndicatorViewStyle.WhiteLarge);
            _countryList = await _viewModel.GetListOfCountriesAsync();

            if (!_countryList.Any())
            {
                _spinner?.RemoveFromSuperview();
                OnServerError();
                return;
            }

            InvokeOnMainThread(() =>
            {
                TableViewHeightConstraint.Constant = _countryList.Count * CountryTableCell.ROW_HEIGHT;
                _spinner?.RemoveFromSuperview();
                CountryTableView.RegisterNibForCellReuse(CountryTableCell.Nib, CountryTableCell.Key);
                CountryTableView.Source = new CountryTableViewSource(_countryList);
                CountryTableView.ReloadData();
            });
        }

        void SetAccessibility()
        {
            ListExplainLbl.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_FOOTER);
            CloseButton.AccessibilityLabel = QuestionnaireViewModel.REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            DialogHelper.ShowDialog(this, _viewModel.CloseDialogViewModel, CloseConfirmed, UIAlertActionStyle.Destructive);
        }

        void CloseConfirmed(UIAlertAction obj)
        {
            NavigationController?.DismissViewController(true, null);
        }

        

        partial void NextBtnTapped(DefaultBorderButton sender)
        {
            NextBtn.ShowSpinner(View, UIActivityIndicatorViewStyle.White);
            _viewModel.InvokeNextButtonClick(OnSuccess, OnFail, _countryList);
        }

        void OnSuccess()
        {
            NextBtn.HideSpinner();
            NavigationController?.PushViewController(LoadingPageViewController.Create(), true);
        }

        //If the server fails, then we just skip this page.
        void OnServerError()
        {
            LogUtils.LogMessage(LogSeverity.ERROR,
                $"{nameof(QuestionnaireCountriesViewController)}.{nameof(OnServerError)}: " +
                $"Skipping language selection because countries failed to be fetched. (IOS)");
            _countryList = new List<CountryDetailsViewModel>();
            NextBtnTapped(null);
        }

        //Is only invoked if data was garbage collected.
        void OnFail()
        {
            NextBtn.HideSpinner();
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesViewController)}.{nameof(OnFail)}: " +
                $"AuthenticationState.personaldata was garbage collected (IOS)");
        }
    }
}
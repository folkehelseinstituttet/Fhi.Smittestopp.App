﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.AuthenticationFlow.QuestionnaireAdapters;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;


namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class QuestionnaireCountriesSelectionActivity : Activity
    {
        private readonly QuestionnaireCountriesViewModel _viewModel = new QuestionnaireCountriesViewModel();
        private List<CountryDetailsViewModel> _countries = new List<CountryDetailsViewModel>();
        private TextView _title;
        private TextView _subtitle;
        private Button _nextButton;
        private ProgressBar _progressBar;
        private RecyclerView _recyclerView;
        private Button _closeButton;
        private TextView _footer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_HEADER_TEXT;
           
            InitView();
        }

        private async void InitView()
        {
            SetContentView(Resource.Layout.questionnaire_countries);

            _title = FindViewById<TextView>(Resource.Id.countries_title);
            _subtitle = FindViewById<TextView>(Resource.Id.countries_subtitle);
            _footer = FindViewById<TextView>(Resource.Id.questionnaire_countries_footer);
            _nextButton = FindViewById<Button>(Resource.Id.countries_button);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.countries_list);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            _closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);

            //Accessibility
            _closeButton.ContentDescription = InformationAndConsentViewModel.CLOSE_BUTTON_ACCESSIBILITY_LABEL;
            _title.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            RunOnUiThread(() => ShowSpinner(true));

            _countries.AddRange(await _viewModel.GetListOfCountriesAsync() ?? new List<CountryDetailsViewModel>());
            if (!_countries.Any())
            {
                RunOnUiThread(() => ShowSpinner(false));
                OnServerError();
                return;
            }
            RunOnUiThread(() => ShowSpinner(false));

            _closeButton.Click += new StressUtils.SingleClick(OnExitClick).Run;
            _nextButton.Click += new StressUtils.SingleClick(OnNextButtonClick).Run;

            _title.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_HEADER_TEXT;
            _subtitle.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_INFORMATION_TEXT;
            _footer.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_FOOTER;
            _nextButton.Text = QuestionnaireCountriesViewModel.COUNTRY_QUESTIONAIRE_BUTTON_TEXT;
           

            var adapter = new QuestionnaireCountriesSelectionAdapter(_countries);

            var layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(layoutManager);

            _recyclerView.SetAdapter(adapter);
        }

        private async void OnExitClick(object sender, EventArgs args)
        {
            bool isOkPressed = await DialogUtils.DisplayDialogAsync(this, _viewModel.CloseDialogViewModel);
            if (isOkPressed)
            {
                GoToInfectionStatusPage();
            }
        }

        private void GoToInfectionStatusPage() => NavigationHelper.GoToResultPageAndClearTop(this);

        private void GoToLoadingPage() =>
            StartActivity(new Intent(this, typeof(LoadingPageActivity)));

        private void OnNextButtonClick(object sender, EventArgs args)
        {
            _viewModel.InvokeNextButtonClick(GoToLoadingPage, OnFail, _countries);
        }

        //If the server fails, then we just skip this page.
        void OnServerError()
        {
            LogUtils.LogMessage(LogSeverity.ERROR,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnServerError)}: " +
                $"Skipping language selection because countries failed to be fetched. (Android)");
            _countries = new List<CountryDetailsViewModel>();
            ShowSpinner(false);
            OnNextButtonClick(null, null);
        }

        //Is only invoked if data was garbage collected.
        void OnFail()
        {
            ShowSpinner(false);
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, null,
                $"{nameof(QuestionnaireCountriesSelectionActivity)}.{nameof(OnFail)}: " +
                $"AuthenticationState.personaldata was garbage collected (Android)");
        }

        private void ShowSpinner(bool show)
        {
            if (show)
            {
                _nextButton.Enabled = false;
                _nextButton.Visibility = ViewStates.Invisible;
                _progressBar.Visibility = ViewStates.Visible;
                _recyclerView.Visibility = ViewStates.Invisible;
            }
            else
            {
                _nextButton.Enabled = true;
                _nextButton.Visibility = ViewStates.Visible;
                _progressBar.Visibility = ViewStates.Gone;
                _recyclerView.Visibility = ViewStates.Visible;
            }
        }
    }
}
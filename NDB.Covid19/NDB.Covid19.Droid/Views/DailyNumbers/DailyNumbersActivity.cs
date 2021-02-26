using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.ViewModels.DailyNumbersViewModel;

namespace NDB.Covid19.Droid.Views.DailyNumbers
{
    [Activity(
        Theme = "@style/AppTheme",
        ParentActivity = typeof(InfectionStatusActivity),
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class DailyNumbersActivity : AppCompatActivity
    {
        private static readonly DailyNumbersViewModel ViewModel;
        private ViewGroup _closeButton;

        static DailyNumbersActivity()
        {
            ViewModel = new DailyNumbersViewModel();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = DAILY_NUMBERS_HEADER;
            SetContentView(Resource.Layout.activity_daily_numbers);
            Init();
        }

        private void Init()
        {
            TextView _dailyNumbersHeader = FindViewById<TextView>(Resource.Id.daily_numbers_header_textView);
            _dailyNumbersHeader.Text = DAILY_NUMBERS_HEADER;
            TextView _dailyNumbersSubHeaderStatistics = FindViewById<TextView>(Resource.Id.daily_numbers_statistics_header_textView);
            _dailyNumbersSubHeaderStatistics.Text = DAILY_NUMBERS_TITLE_ONE;
            FindViewById<TextView>(Resource.Id.daily_numbers_statistics_text_textview).Text = LastUpdateStringSubHeader;
            TextView diseaseRateSubSub = FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_text_textview);

            ISpanned formattedDescription = HtmlCompat.FromHtml(LastUpdateStringSubSubHeader, HtmlCompat.FromHtmlModeLegacy);
            diseaseRateSubSub.TextFormatted = formattedDescription;
            diseaseRateSubSub.ContentDescriptionFormatted = formattedDescription;
            diseaseRateSubSub.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;
            Color linkColor = new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primaryText));
            diseaseRateSubSub.SetLinkTextColor(linkColor);

            FindViewById<TextView>(Resource.Id.daily_numbers_infected_header_text).Text = KEY_FEATURE_ONE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_infected_number_text).Text = ConfirmedCasesToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_infected_total_text).Text = ConfirmedCasesTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_death_header_text).Text = KEY_FEATURE_TWO_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_death_number_text).Text = DeathsToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_death_total_text).Text = DeathsTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_tested_header_text).Text = KEY_FEATURE_THREE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_tested_number_text).Text = TestsConductedToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_tested_total_text).Text = TestsConductedTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_hospitalized_header_text).Text = KEY_FEATURE_FOUR_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_hospitalized_number_text).Text = PatientsAdmittedToday;

            FindViewById<TextView>(Resource.Id.daily_numbers_intensive_header_text).Text = KEY_FEATURE_SEVEN_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_intensive_number_text).Text = PatientsIntensiveCare;

            TextView _dailyNumbersSubHeaderVaccinations = FindViewById<TextView>(Resource.Id.daily_numbers_vaccinations_header_textView);
            _dailyNumbersSubHeaderVaccinations.Text = DAILY_NUMBERS_TITLE_TWO;

            FindViewById<TextView>(Resource.Id.daily_numbers_vaccinations_text_textview).Text = LastUpdateStringSubTextTwo;

            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_header_text).Text = KEY_FEATURE_NINE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_number_text).Text = VaccinationsDoseOneToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose1_total_text).Text = VaccinationsDoseOneTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_header_text).Text = KEY_FEATURE_TEN_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_number_text).Text = VaccinationsDoseTwoToday;
            FindViewById<TextView>(Resource.Id.daily_numbers_vaccination_dose2_total_text).Text = VaccinationsDoseTwoTotal;

            TextView _dailyNumbersSubHeaderSmittestopp = FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_header_textView);
            _dailyNumbersSubHeaderSmittestopp.Text = DAILY_NUMBERS_TITLE_THREE;

            //Added newline for the UI to align.
            FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_downloads_header_text).Text = $"{KEY_FEATURE_SIX_LABEL} \n";
            FindViewById<TextView>(Resource.Id.daily_numbers_smittestopp_downloads_number_text).Text = SmittestopDownloadsTotal;

            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_header_text).Text = KEY_FEATURE_FIVE_LABEL;
            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_number_text).Text = NumberOfPositiveTestsResultsLast7Days;
            FindViewById<TextView>(Resource.Id.daily_numbers_positive_shared_total_text).Text = NumberOfPositiveTestsResultsTotal;

            // CONTENT DESCRIPTIONS OF HEADERS
            _dailyNumbersHeader.ContentDescription = DailyNumbersViewModel.DAILY_NUMBERS_HEADER.ToLower();
            _dailyNumbersSubHeaderStatistics.ContentDescription = DailyNumbersViewModel.DAILY_NUMBERS_TITLE_ONE.ToLower();
            _dailyNumbersSubHeaderVaccinations.ContentDescription = DailyNumbersViewModel.DAILY_NUMBERS_TITLE_TWO.ToLower();
            _dailyNumbersSubHeaderSmittestopp.ContentDescription = DailyNumbersViewModel.DAILY_NUMBERS_TITLE_THREE.ToLower();
            _dailyNumbersHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderStatistics.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderVaccinations.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeaderSmittestopp.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            _closeButton = FindViewById<ViewGroup>(Resource.Id.daily_numbers_close_cross_btn);
            _closeButton.Click += new StressUtils.SingleClick(OnCloseBtnClicked).Run;
            _closeButton.ContentDescription = MessagesViewModel.MESSAGES_ACCESSIBILITY_CLOSE_BUTTON;

        }

        private void OnCloseBtnClicked(object arg1, EventArgs arg2)
        {
            GoToInfectionStatusActivity();
        }

        public override void OnBackPressed() => GoToInfectionStatusActivity();

        private void GoToInfectionStatusActivity() => NavigationHelper.GoToResultPageAndClearTop(this);

    }
}
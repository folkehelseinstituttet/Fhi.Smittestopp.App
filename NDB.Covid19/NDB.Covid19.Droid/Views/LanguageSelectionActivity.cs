using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views
{
    [Activity(Label = "", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.FullUser, Theme = "@style/AppTheme")]
    public class LanguageSelectionActivity : Activity
    {

        private Button BokmalButton;
        private Button NynorskButton;
        private Button EnglishButton;
        private Button ArabicButton;
        private Button LithuanianButton;
        private Button PolishButton;
        private Button SomaliButton;
        private Button TigrinyaButton;
        private Button UrduButton;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.language_selection);

            InitButtons();
        }

        private void InitButtons()
        {
            BokmalButton = FindViewById<Button>(Resource.Id.bokmal_button);
            NynorskButton = FindViewById<Button>(Resource.Id.nynorsk_button);
            EnglishButton = FindViewById<Button>(Resource.Id.english_button);
            ArabicButton = FindViewById<Button>(Resource.Id.arabic_button);
            LithuanianButton = FindViewById<Button>(Resource.Id.lithuanian_button);
            PolishButton = FindViewById<Button>(Resource.Id.polish_button);
            SomaliButton = FindViewById<Button>(Resource.Id.somali_button);
            TigrinyaButton = FindViewById<Button>(Resource.Id.tigrinya_button);
            UrduButton = FindViewById<Button>(Resource.Id.urdu_button);

            BokmalButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_NB;
            NynorskButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_NN;
            EnglishButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_EN;
            ArabicButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_AR;
            LithuanianButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_LT;
            PolishButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_PL;
            SomaliButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_SO;
            TigrinyaButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_TI;
            UrduButton.Text = LanguageSelectionViewModel.LANGUAGE_SELECTION_UR;

            BokmalButton.Click += new SingleClick(LanguageButton_Click).Run;
            NynorskButton.Click += new SingleClick(LanguageButton_Click).Run;
            EnglishButton.Click += new SingleClick(LanguageButton_Click).Run;
            ArabicButton.Click += new SingleClick(LanguageButton_Click).Run;
            LithuanianButton.Click += new SingleClick(LanguageButton_Click).Run;
            PolishButton.Click += new SingleClick(LanguageButton_Click).Run;
            SomaliButton.Click += new SingleClick(LanguageButton_Click).Run;
            TigrinyaButton.Click += new SingleClick(LanguageButton_Click).Run;
            UrduButton.Click += new SingleClick(LanguageButton_Click).Run;
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            switch (senderButton.Id)
            {
                case Resource.Id.bokmal_button:
                    LocalPreferencesHelper.SetAppLanguage("nb");
                    break;
                case Resource.Id.nynorsk_button:
                    LocalPreferencesHelper.SetAppLanguage("nn");
                    break;
                case Resource.Id.english_button:
                    LocalPreferencesHelper.SetAppLanguage("en");
                    break;
                case Resource.Id.arabic_button:
                    LocalPreferencesHelper.SetAppLanguage("ar");
                    break;
                case Resource.Id.lithuanian_button:
                    LocalPreferencesHelper.SetAppLanguage("lt");
                    break;
                case Resource.Id.polish_button:
                    LocalPreferencesHelper.SetAppLanguage("pl");
                    break;
                case Resource.Id.somali_button:
                    LocalPreferencesHelper.SetAppLanguage("so");
                    break;
                case Resource.Id.tigrinya_button:
                    LocalPreferencesHelper.SetAppLanguage("ti");
                    break;
                case Resource.Id.urdu_button:
                    LocalPreferencesHelper.SetAppLanguage("ur");
                    break;

            }
            LocalesService.Initialize();
            NavigationHelper.GoToOnBoarding(this, true);
        }
    }
}

using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Initializer
{
    public partial class LanguageSelectionViewController : BaseViewController
    {
        public LanguageSelectionViewController(IntPtr handle) : base(handle) 
        {
        }

        public override void ViewDidLoad()
        { 
            base.ViewDidLoad();
            InitButtons();
        }

        private void InitButtons()
        {
            StyleUtil.InitButtonStyling(BokmalButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_NB);
            StyleUtil.InitButtonStyling(NynorskButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_NN);
            StyleUtil.InitButtonStyling(EnglishButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_EN);
            StyleUtil.InitButtonStyling(ArabicButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_AR);
            StyleUtil.InitButtonStyling(LithuanianButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_LT);
            StyleUtil.InitButtonStyling(PolishButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_PL);
            StyleUtil.InitButtonStyling(SomaliButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_SO);
            StyleUtil.InitButtonStyling(TigrinyaButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_TI);
            StyleUtil.InitButtonStyling(UrduButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_UR);

            StyleUtil.InitLanguageSelectionButtonStyling(BokmalButton);
            StyleUtil.InitLanguageSelectionButtonStyling(NynorskButton);
            StyleUtil.InitLanguageSelectionButtonStyling(EnglishButton);
            StyleUtil.InitLanguageSelectionButtonStyling(ArabicButton);
            StyleUtil.InitLanguageSelectionButtonStyling(LithuanianButton);
            StyleUtil.InitLanguageSelectionButtonStyling(PolishButton);
            StyleUtil.InitLanguageSelectionButtonStyling(SomaliButton);
            StyleUtil.InitLanguageSelectionButtonStyling(TigrinyaButton);
            StyleUtil.InitLanguageSelectionButtonStyling(UrduButton);
        }

        private void SetAppLanguageAndContinue(string appLanguage)
        {
            LocalPreferencesHelper.SetAppLanguage(appLanguage);
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            NavigationHelper.GoToOnboardingPage(this);
        }

        partial void BokmalButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("nb");
        }

        partial void NynorskButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("nn");
        }

        partial void EnglishButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("en");
        }

        partial void ArabicButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("ar");
        }

        partial void LithuanianButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("lt");
        }

        partial void PolishButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("pl");
        }

        partial void SomaliButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("so");
        }

        partial void TigrinyaButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("ti");
        }

        partial void UrduButton_TouchUpInside(NSObject sender)
        {
            SetAppLanguageAndContinue("ur");
        }
    }
}


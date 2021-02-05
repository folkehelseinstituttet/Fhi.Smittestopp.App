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
            StyleUtil.InitButtonStyling(PolishButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_PL);
            StyleUtil.InitButtonStyling(SomaliButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_SO);
            StyleUtil.InitButtonStyling(TigrinyaButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_TI);
            StyleUtil.InitButtonStyling(UrduButton, LanguageSelectionViewModel.LANGUAGE_SELECTION_UR);

            StyleUtil.InitLanguageSelectionButtonStyling(BokmalButton);
            StyleUtil.InitLanguageSelectionButtonStyling(NynorskButton);
            StyleUtil.InitLanguageSelectionButtonStyling(EnglishButton);
            StyleUtil.InitLanguageSelectionButtonStyling(ArabicButton);
            StyleUtil.InitLanguageSelectionButtonStyling(PolishButton);
            StyleUtil.InitLanguageSelectionButtonStyling(SomaliButton);
            StyleUtil.InitLanguageSelectionButtonStyling(TigrinyaButton);
            StyleUtil.InitLanguageSelectionButtonStyling(UrduButton);
        }

        partial void BokmalButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("nb");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void NynorskButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("nn");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void EnglishButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("en");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void ArabicButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("ar");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void PolishButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("pl");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void SomaliButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("so");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void TigrinyaButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("ti");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        partial void UrduButton_TouchUpInside(NSObject sender)
        {
            LocalPreferencesHelper.SetAppLanguage("ur");
            LocalesService.Initialize();
            LayoutUtils.OnLayoutDirectionChange();
            Continue();
        }

        private void Continue()
        {
            NavigationHelper.GoToOnboardingPage(this);
        }
    }
}


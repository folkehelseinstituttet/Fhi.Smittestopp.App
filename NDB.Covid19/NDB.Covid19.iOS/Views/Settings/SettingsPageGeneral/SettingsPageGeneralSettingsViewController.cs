using System;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.PersistedData;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;


namespace NDB.Covid19.iOS.Views.Settings.SettingsPageGeneral
{
    public partial class SettingsPageGeneralSettingsViewController : BaseViewController
    {
        private SettingsGeneralViewModel _viewModel;
        private UITapGestureRecognizer _gestureRecognizer;
        private readonly IResetViews _resetViews = ServiceLocator.Current.GetInstance<IResetViews>();
        
        public SettingsPageGeneralSettingsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupStyling();
            
            _viewModel = new SettingsGeneralViewModel();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            switchButton.ValueChanged += SwitchValueChanged;
            SetupSwitchButton();
            SetupLinkButton();
            SetupRadioButtons();
            PostAccessibilityNotificationAndReenableElement(BackButton, HeaderLabel);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            switchButton.ValueChanged -= SwitchValueChanged;
            SmittestopLinkButtonStackView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        void SetupStyling()
        {
            InitLabel(Header, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_TITLE, 24, 28);
            InitLabel(HeaderLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_EXPLANATION_ONE, 26,
                28);
            InitLabel(ContentLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_MOBILE_DATA_HEADER, 18,
                28);
            InitLabel(ContentLabelOne, FontType.FontRegular, SettingsGeneralViewModel.SETTINGS_GENERAL_EXPLANATION_TWO,
                16, 28);
            InitLabel(DescriptionLabel, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_MOBILE_DATA_DESC, 14, 28);
            InitLabel(ChooseLanguageHeaderLbl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_CHOOSE_LANGUAGE_HEADER, 16, 28);
            InitLabel(BokmalLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_NB, 16, 28);
            InitLabel(NynorskLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_NN, 16, 28);
            InitLabel(EnglishLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_EN, 16, 28);
            InitLabel(LithuanianLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_LT, 16, 28);
            InitLabel(PolishLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_PL, 16, 28);
            InitLabel(SomaliLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_SO, 16, 28);
            InitLabel(TigrinyaLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_TI, 16, 28);
            InitLabel(ArabicLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_AR, 16, 28);
            InitLabel(UrduLabel, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_UR, 16, 28);


            InitLabel(RestartAppLabl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_RESTART_REQUIRED_TEXT, 14, 28);
            InitUnderlinedLabel(SmittestopLinkButtonLbl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT, 16, 28);

            Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            SmittestopLinkButtonLbl.TextColor = ColorHelper.LINK_COLOR;

            Header.AccessibilityTraits = UIAccessibilityTrait.Header;
            ChooseLanguageHeaderLbl.AccessibilityTraits = UIAccessibilityTrait.Header;

            //Implemented for correct voiceover due to Back button 
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            //Implemented for correct voiceover due to last paragraph and link
            SmittestopLinkButtonLbl.AccessibilityLabel =
                SettingsGeneralViewModel.SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT;

            //Implemented for correct voiceover due to smitte|stop, removing pronunciation of lodretstreg
            ContentLabel.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(SettingsGeneralViewModel
                    .SETTINGS_GENERAL_MOBILE_DATA_HEADER);

        }


        void SetupSwitchButton()
        {
            if (LayoutUtils.GetTextAlignment() == UITextAlignment.Right)
            {
                switchButton.SemanticContentAttribute = UISemanticContentAttribute.ForceRightToLeft;
            }
            switchButton.On = LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
        }

        void SetupLinkButton()
        {
            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OnSmittestopLinkButtonStackViewTapped(_gestureRecognizer));
            SmittestopLinkButtonStackView.AddGestureRecognizer(_gestureRecognizer);
            SmittestopLinkButtonStackView.AccessibilityTraits = UIAccessibilityTrait.Link;
        }

        void SetupRadioButtons()
        {
            SettingsLanguageSelection appLanguage = SettingsLanguageSelectionExtensions.FromString(LocalPreferencesHelper.GetAppLanguage());
            _viewModel.SetSelection(appLanguage);

            BokmalButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Bokmal;
            NynorskButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Nynorsk;
            EnglishButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
            LithuanianButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Lithuanian;
            PolishButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Polish;
            SomaliButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Somali;
            TigrinyaButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Tigrinya;
            ArabicButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Arabic;
            UrduButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Urdu;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
            BokmalButton.Enabled = false;
            NynorskButton.Enabled = false;
            EnglishButton.Enabled = false;
            LithuanianButton.Enabled = false;
            PolishButton.Enabled = false;
            SomaliButton.Enabled = false;
            TigrinyaButton.Enabled = false;
            ArabicButton.Enabled = false;
            UrduButton.Enabled = false;
        }

        public void SwitchValueChanged(object sender, EventArgs e)
        {
            if (!switchButton.On)
            {
                DialogHelper.ShowDialog(
                    this,
                    SettingsGeneralViewModel.AreYouSureDialogViewModel,
                    action => { _viewModel.OnCheckedChange(switchButton.On); },
                    UIAlertActionStyle.Default,
                    action =>
                    {
                        switchButton.On = true;
                        _viewModel.OnCheckedChange(switchButton.On);
                    });
            }
            else
            {
                _viewModel.OnCheckedChange(switchButton.On);
            }
        }

        void OnSmittestopLinkButtonStackViewTapped(UITapGestureRecognizer recognizer)
        {
            SettingsGeneralViewModel.OpenSmitteStopLink();
        }

        void HandleRadioBtnChange(SettingsLanguageSelection selection, UIButton sender)
        {
            if (SettingsGeneralViewModel.Selection == selection)
            {
                BokmalButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Bokmal;
                NynorskButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Nynorsk;
                EnglishButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
                LithuanianButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Lithuanian;
                PolishButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Polish;
                SomaliButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Somali;
                TigrinyaButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Tigrinya;
                ArabicButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Arabic;
                UrduButton.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Urdu;
                return;
            }

            DialogHelper.ShowDialog(this, SettingsGeneralViewModel.GetChangeLanguageViewModel,
                Action =>
                {
                    _resetViews.ResetViews();
                }
            );

            LocalPreferencesHelper.SetAppLanguage(SettingsLanguageSelectionExtensions.ToString(selection));

            LayoutUtils.OnLayoutDirectionChange();

            LocalesService.SetInternationalization();
            SetupRadioButtons();
        }

        partial void BokmalButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Bokmal, sender);
        }

        partial void NynorskButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Nynorsk, sender);
        }

        partial void EnglishButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.English, sender);
        }

        partial void ArabicButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Arabic, sender);
        }

        partial void LithuanianButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Lithuanian, sender);
        }

        partial void PolishButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Polish, sender);
        }

        partial void SomaliButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Somali, sender);
        }

        partial void TigrinyaButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Tigrinya, sender);
        }

        partial void UrduButton_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Urdu, sender);
        }
    }
}
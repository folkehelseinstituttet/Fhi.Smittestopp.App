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
            InitLabel(RadioButton1Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_NB, 16, 28);
            InitLabel(RadioButton2Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_NN, 16, 28);
            InitLabel(RadioButton3Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_EN, 16, 28);
            InitLabel(RadioButton4Lbl, FontType.FontBold, SettingsGeneralViewModel.SETTINGS_GENERAL_PL, 16, 28);

            InitLabel(RestartAppLabl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_RESTART_REQUIRED_TEXT, 14, 28);
            InitUnderlinedLabel(SmittestopLinkButtonLbl, FontType.FontRegular,
                SettingsGeneralViewModel.SETTINGS_GENERAL_MORE_INFO_BUTTON_TEXT, 16, 28);

            Header.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;

            //Implemented for correct voiceover due to Back button 
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            //Implemented for correct voiceover due to last paragraph and link
            SmittestopLinkButtonLbl.AccessibilityLabel =
                SettingsGeneralViewModel.SETTINGS_GENERAL_ACCESSIBILITY_MORE_INFO_BUTTON_TEXT;

            //Implemented for correct voiceover due to smitte|stop, removing pronunciation of lodretstreg
            ContentLabel.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(SettingsGeneralViewModel
                    .SETTINGS_GENERAL_MOBILE_DATA_HEADER);

            _viewModel = new SettingsGeneralViewModel();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            switchButton.ValueChanged += SwitchValueChanged;
            SetupSwitchButton();
            SetupLinkButton();
            SetupRadioButtons();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            switchButton.ValueChanged -= SwitchValueChanged;
            SmittestopLinkButtonStackView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        void SetupSwitchButton()
        {
            switchButton.On = LocalPreferencesHelper.GetIsDownloadWithMobileDataEnabled();
        }

        void SetupLinkButton()
        {
            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OnSmittestopLinkButtonStackViewTapped(_gestureRecognizer));
            SmittestopLinkButtonStackView.AddGestureRecognizer(_gestureRecognizer);
        }

        void SetupRadioButtons()
        {
            string appLanguage = LocalPreferencesHelper.GetAppLanguage();

            switch (appLanguage)
            {
                case "en":
                    _viewModel.SetSelection(SettingsLanguageSelection.English);
                    break;
                case "nn":
                    _viewModel.SetSelection(SettingsLanguageSelection.Nynorsk);
                    break;
                case "pl":
                    _viewModel.SetSelection(SettingsLanguageSelection.Polish);
                    break;
                default:
                    _viewModel.SetSelection(SettingsLanguageSelection.Bokmal);
                    break;
            }

            RadioButton1.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Bokmal;
            RadioButton2.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Nynorsk;
            RadioButton3.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
            RadioButton4.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Polish;

        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
            RadioButton1.Enabled = false;
            RadioButton2.Enabled = false;
            RadioButton3.Enabled = false;
            RadioButton4.Enabled = false;
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
                RadioButton1.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Bokmal;
                RadioButton2.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Nynorsk;
                RadioButton3.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.English;
                RadioButton4.Selected = SettingsGeneralViewModel.Selection == SettingsLanguageSelection.Polish;
                return;
            }

            DialogHelper.ShowDialog(this, SettingsGeneralViewModel.GetChangeLanguageViewModel,
                Action =>
                {
                    _resetViews.ResetViews();
                }
            );

            LocalPreferencesHelper.SetAppLanguage(SettingsLanguageSelectionExtensions.GetStringValue(selection));

            LocalesService.SetInternationalization();
            SetupRadioButtons();
        }

        partial void RadioButton1_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Bokmal, sender);
        }

        partial void RadioButton2_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Nynorsk, sender);
        }

        partial void RadioButton3_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.English, sender);
        }

        partial void RadioButton4_TouchUpInside(CustomRadioButton sender)
        {
            HandleRadioBtnChange(SettingsLanguageSelection.Polish, sender);
        }
    }
}
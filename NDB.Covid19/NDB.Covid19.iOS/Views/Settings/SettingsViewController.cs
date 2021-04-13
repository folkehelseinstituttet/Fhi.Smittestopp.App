using System;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings
{
    public partial class SettingsViewController : BaseViewController, IUIAccessibilityContainer
    {
        SettingsViewModel _viewModel;

        SettingsTableViewSource _tableSource
        {
            get => (SettingsTableViewSource)SettingsItemsTable.Source;
            set => SettingsItemsTable.Source = value;
        }

        public static SettingsViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("Settings", null);
            SettingsViewController vc = (SettingsViewController)storyboard.InstantiateViewController(nameof(SettingsViewController));
            return vc;
        }

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new SettingsViewModel();
            _tableSource = new SettingsTableViewSource(_viewModel.SettingItemList);
            SetupAccessibility();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _tableSource.OnCellTapped += OnCellTapped;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _tableSource.OnCellTapped -= OnCellTapped;
        }

        private void SetupAccessibility()
        {
            CloseBtn.AccessibilityLabel = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            fhiLogo.AccessibilityLabel = InfectionStatusViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            if (UIAccessibility.IsVoiceOverRunning)
            {
                this.SetAccessibilityElements(NSArray.FromNSObjects(fhiLogo, SettingsItemsTable, CloseBtn));
            }
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            LeaveController();
        }

        void OnCellTapped(object sender, SettingItemType type)
        {
            switch (type)
            {
                case SettingItemType.Intro:              
                    UIViewController WelcomeViewController = NavigationHelper.ViewControllerByStoryboardName("Welcome");
                    NavigationController?.PushViewController(WelcomeViewController, true);
                    break;
                case SettingItemType.HowItWorks:
                    UIViewController SettingsPage2ViewController = NavigationHelper.ViewControllerByStoryboardName("SettingsPage2");
                    NavigationController?.PushViewController(SettingsPage2ViewController, true);
                    break;
                case SettingItemType.Consent:
                    UIViewController settingsConsetnController = NavigationHelper.ViewControllerByStoryboardName("SettingsPage3");
                    NavigationController?.PushViewController(settingsConsetnController, true);
                    break;
                case SettingItemType.Help:
                    UIViewController SettingsPage4ViewController = NavigationHelper.ViewControllerByStoryboardName("SettingsPage4");
                    NavigationController?.PushViewController(SettingsPage4ViewController, true);
                    break;
                case SettingItemType.Settings:
                    UIViewController SettingsPageGeneralSettingsViewController = NavigationHelper.ViewControllerByStoryboardName("SettingsPageGeneralSettings");
                    NavigationController?.PushViewController(SettingsPageGeneralSettingsViewController, true);
                    break;
                case SettingItemType.About:
                    UIViewController SettingsPage5ViewController = NavigationHelper.ViewControllerByStoryboardName("SettingsPage5");
                    NavigationController?.PushViewController(SettingsPage5ViewController, true);
                    break;
                case SettingItemType.Debug:
                    NavigationHelper.GoToDeveloperToolsPage(this, NavigationController);
                    break;
            }
        }
    }
}
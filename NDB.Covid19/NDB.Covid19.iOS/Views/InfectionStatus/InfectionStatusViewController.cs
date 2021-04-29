using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreFoundation;
using NDB.Covid19.iOS.Permissions;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.iOS.Views.DailyNumbers;
using NDB.Covid19.iOS.Views.Settings;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using NDB.Covid19.Enums;

namespace NDB.Covid19.iOS.Views.InfectionStatus
{

    public partial class InfectionStatusViewController : BaseViewController
    {
        public InfectionStatusViewController(IntPtr handle) : base(handle)
        {
        }

        public static InfectionStatusViewController Create(bool comingFromOnboarding)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("InfectionStatus", null);
            InfectionStatusViewController vc = storyboard.InstantiateInitialViewController() as InfectionStatusViewController;
            vc._comingFromOnboarding = comingFromOnboarding;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public static UINavigationController GetInfectionSatusPageControllerInNavigationController()
        {
            UIViewController vc = InfectionStatusViewController.Create(false);
            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return navigationController;
        }

        bool _comingFromOnboarding;

        InfectionStatusViewModel _viewModel;

        UIButton _dailyNumbersButton;
        UIButton _messageViewBtn;
        UIButton _areYouInfectedBtn;

        IOSPermissionManager _permissionManager;

        UIFocusGuide _focusGuide = new UIFocusGuide ();
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = new InfectionStatusViewModel();
            View.AddLayoutGuide(_focusGuide);
            
            _focusGuide.LeadingAnchor.ConstraintEqualTo(MenuIcon.LeadingAnchor).Active = true;
            _focusGuide.TrailingAnchor.ConstraintEqualTo(MenuIcon.TrailingAnchor).Active = true;
            _focusGuide.TopAnchor.ConstraintEqualTo(OnOffBtn.TopAnchor).Active = true;
            _focusGuide.BottomAnchor.ConstraintEqualTo(OnOffBtn.BottomAnchor).Active = true;

            MenuLabel.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                OnMenubtnTapped(MenuIcon);
            }));
            MenuLabel.IsAccessibilityElement = false;
           
            SetupStyling();
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED, OnMessageStatusChanged);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND, OnAppReturnsFromBackground);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_CONSENT_MODAL_IS_CLOSED, OnConsentModalIsClosed);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DAILY_NUMBERS, OnAppDailyNumbersChanged);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UpdateCorrelationId(null);
            LogUtils.LogMessage(LogSeverity.INFO, "User opened InfectionStatus", null);
        }

        private void OnAppDailyNumbersChanged(object _ = null)
        {
            DispatchQueue.MainQueue.DispatchAsync(() => {
                _dailyNumbersButton.AccessibilityLabel = _viewModel.NewDailyNumbersAccessibilityText;
                StyleUtil.InitLabelWithSpacing(dailyNumbersUpdatedLbl, StyleUtil.FontType.FontRegular, InfectionStatusViewModel.INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_TEXT, 1.14, 12, 16);
            });
        }

        public override void DidUpdateFocus (UIFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
        {
            base.DidUpdateFocus (context, coordinator);

            UIView nextFocusableItem = context.NextFocusedView;

            if (nextFocusableItem == null) return;
            
            if (nextFocusableItem.Equals(OnOffBtn)) {
                _focusGuide.PreferredFocusedView = MenuIcon;
            } else if (nextFocusableItem.Equals(MenuIcon)) {
                _focusGuide.PreferredFocusedView = OnOffBtn;
            } else {
                _focusGuide.PreferredFocusedView = null;
            }
        }

        public override void ViewDidUnload()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_CONSENT_MODAL_IS_CLOSED);
            base.ViewDidUnload();
        }

        private void OnMessageStatusChanged(object _ = null)
        {
            InvokeOnMainThread(() => _viewModel.UpdateNotificationDot());
        }

        private void OnConsentModalIsClosed(object obj)
        {
            UpdateUI();
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetPermissionManager();
            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;

            _dailyNumbersButton.TouchUpInside += OnDailyNumbersBtnTapped;
            _messageViewBtn.TouchUpInside += OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside += OnAreYouInfectedBtnTapped;

            OnAppReturnsFromBackground(null);

            if (_comingFromOnboarding)
            {
                StartIfStopped();
                _comingFromOnboarding = false;
            }
        }


        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
            _messageViewBtn.TouchUpInside -= OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside -= OnAreYouInfectedBtnTapped;
            _dailyNumbersButton.TouchUpInside -= OnDailyNumbersBtnTapped;

            ResetStatusBar();
        }

        private void ResetStatusBar()
        {
            UIView statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            statusBar.BackgroundColor = ColorHelper.DEFAULT_BACKGROUND_COLOR;
        }

        void OnAppReturnsFromBackground(object obj)
        {
            _viewModel.CheckIfAppIsRestricted(UpdateUI);
            UpdateUI();
            OnMessageStatusChanged();
        }

        void SetPermissionManager()
        {
            if (_permissionManager == null)
            {
                _permissionManager = new IOSPermissionManager();
            }
        }

        void StartIfStopped()
        {
            Task.Run(async () =>
            {
                if (await _viewModel.IsRunning() == false)
                {
                    await _viewModel.StartENService();

                    /// If EN is disabled, then the OS dialog will pop up.
                    /// Calling the StartENService method again seems to make the EN api properly update its state, before we update our UI.
                    /// This is not an optimal solution, but seems to work fine as a workaround for now.
                    /// A better way would be for us to be able to listen to state changes in the EN api.
                    if (await _viewModel.IsEnabled() == false)
                    {
                        await _viewModel.StartENService();
                    }

                    if (await _permissionManager.PoweredOff())
                    {
                        DialogHelper.ShowBluetoothTurnedOffDialog(this);
                    }

                    InvokeOnMainThread(() =>
                    {
                        UpdateUI();
                    });
                }
            });
        }

        async void UpdateUI()
        {
            fhiLogo.AccessibilityLabel = InfectionStatusViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            appLogo.AccessibilityLabel = InfectionStatusViewModel.SMITTESPORING_APP_LOGO_ACCESSIBILITY;
            _areYouInfectedBtn.AccessibilityLabel = _viewModel.NewRegistrationAccessibilityText;
            _messageViewBtn.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(_viewModel.NewMessageAccessibilityText);
            _dailyNumbersButton.AccessibilityLabel = _viewModel.NewDailyNumbersAccessibilityText;
            ActivityExplainerLbl.Text = await _viewModel.StatusTxtDescription();
            SetOnOffBtnState(await _viewModel.IsRunning());
            SetStatusContainerState(await _viewModel.IsRunning());
            UpdateNewIndicatorView();
        }

        async void SetStatusContainerState(bool isRunning)
        {
            UIView statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            bool modalClosed = true;
            if (ModalViewController != null)
            {
                modalClosed = ModalViewController.IsBeingDismissed;
            }
            if (NavigationController?.TopViewController is InfectionStatusViewController && modalClosed)
            {
                statusBar.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;
            }
            else
            {
                statusBar.BackgroundColor = ColorHelper.DEFAULT_BACKGROUND_COLOR;
            }

            ScrollDownBackgroundView.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;

            StatusContainer.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;
            StatusText.Text = await _viewModel.StatusTxt();
            StatusText.AccessibilityLabel = InfectionStatusViewModel.INFECTION_STATUS_APP_TITLE_TEXT + " " + await _viewModel.StatusTxt();
        }

        void SetOnOffBtnState(bool isRunning)
        {
            if (isRunning)
            {
                string accessibilityText = InfectionStatusViewModel.INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT;
                string buttonText = InfectionStatusViewModel.INFECTION_STATUS_STOP_BUTTON_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(accessibilityText);
                OnOffBtn.BackgroundColor = UIColor.Clear;
                OnOffBtn.Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
                OnOffBtn.Layer.BorderWidth = 1;
                OnOffBtn.SetTitleColor(ColorHelper.PRIMARY_COLOR, UIControlState.Normal);
                OnOffBtn.SetTitle(buttonText, UIControlState.Normal);
            }
            else
            {
                string accessibilityText = InfectionStatusViewModel.INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT;
                string buttonText = InfectionStatusViewModel.INFECTION_STATUS_START_BUTTON_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(accessibilityText);
                OnOffBtn.BackgroundColor = ColorHelper.PRIMARY_COLOR;
                OnOffBtn.Layer.BorderColor = UIColor.Clear.CGColor;
                OnOffBtn.Layer.BorderWidth = 0;
                OnOffBtn.SetTitleColor(ColorHelper.TEXT_COLOR_ON_PRIMARY, UIControlState.Normal);
                OnOffBtn.SetTitle(buttonText, UIControlState.Normal);
            }
        }

        void SetupStyling()
        {
            ActivityExplainerLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontMedium, 18, 22);
            ActivityExplainerLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            MenuIcon.AccessibilityLabel = InfectionStatusViewModel.INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            MenuLabel.Text = InfectionStatusViewModel.INFECTION_STATUS_MENU_TEXT;
            MenuLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 18f, 18f);
            MenuIcon.SizeToFit();
            StatusText.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16f, 20f);
            OnOffBtn.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 22f, 28f);
            OnOffBtn.ContentEdgeInsets = new UIEdgeInsets(12, 12, 12, 12);
            OnOffBtn.Layer.CornerRadius = 8;

            SetupEncounterAndInfectedButtons();
        }

        void SetupEncounterAndInfectedButtons()
        {
            DailyNumbersView.Subviews[0].Layer.CornerRadius = 12;
            DailyNumbersView.Subviews[0].Layer.BorderWidth = 1;
            DailyNumbersView.Subviews[0].Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            MessageView.Subviews[0].Layer.CornerRadius = 12;
            MessageView.Subviews[0].Layer.BorderWidth = 1;
            MessageView.Subviews[0].Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            AreYouInfectetView.Subviews[0].Layer.CornerRadius = 12;
            AreYouInfectetView.Subviews[0].Layer.BorderWidth = 1;
            AreYouInfectetView.Subviews[0].Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;

            dailyNumbersLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 18, 22);
            dailyNumbersLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_DAILY_NUMBERS_HEADER_TEXT;
            dailyNumbersUpdatedLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            dailyNumbersUpdatedLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_TEXT;

            MessageLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 18, 22);
            MessageLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            NewRegistrationLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;

            AreYouInfectetLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 18, 22);
            AreYouInfectetLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            LogInAndRegisterLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            LogInAndRegisterLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;

            // We take the fairly complicated UIViews from the storyboard and embed them into UIButtons
            _messageViewBtn = new UIButton();
            _messageViewBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(MessageView, _messageViewBtn);

            _areYouInfectedBtn = new UIButton();
            _areYouInfectedBtn.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(AreYouInfectetView, _areYouInfectedBtn);

            _dailyNumbersButton = new UIButton();
            _dailyNumbersButton.TranslatesAutoresizingMaskIntoConstraints = false;
            StyleUtil.EmbedViewInsideButton(DailyNumbersView, _dailyNumbersButton);

        }

        void UpdateNewIndicatorView()
        {
            InvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = _viewModel.ShowNewMessageIcon ? 1 : 0;

                MessageIcon.Image = _viewModel.ShowNewMessageIcon ? UIImage.FromBundle("notification_active") : UIImage.FromBundle("notification_inactive");
                NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;
                _messageViewBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(_viewModel.NewMessageAccessibilityText);
            });
        }

        void OnNewMessagesIconVisibilityChanged(object sender, EventArgs e)
        {
            UpdateNewIndicatorView();
        }

        partial void OnMenubtnTapped(UIButton sender)
        {
            UIViewController vc = SettingsViewController.Create();
            NavigationController?.PushViewController(vc, true);
        }


        async partial void OnOffBtnTapped(UIButton sender)
        {
            if (_viewModel.IsAppRestricted)
            {
                DialogHelper.ShowDialog(
                    this,
                    _viewModel.PermissionViewModel,
                    (UIAlertAction action) =>
                    {
                        NavigationHelper.GoToAppSettings();
                    }
                    );
                return;
            }
            if (await _viewModel.IsRunning() && await _viewModel.IsEnabled())
            {
                DialogHelper.ShowDialog(
                    this,
                    _viewModel.OffDialogViewModel,
                    OnStopScannerChosen
                    );
            }
            else
            {
                if (await _permissionManager.PoweredOn())
                {
                    DialogHelper.ShowDialog(
                    this,
                    _viewModel.OnDialogViewModel,
                    OnStartScannerChosen
                    );
                }
                else if (await _permissionManager.PermissionUnknown())
                {
                    // We do nothing. The OS will throw a dialog by itself
                    Debug.WriteLine("GetStatusAsync() == Status.Unknown");
                }
                else if (await _permissionManager.PoweredOff())
                {
                    DialogHelper.ShowBluetoothTurnedOffDialog(this);
                }
                else
                {
                    // Then it must be because we don't have permission
                    DialogHelper.ShowDialog(
                        this,
                        _viewModel.PermissionViewModel,
                        (UIAlertAction action) =>
                        {
                            NavigationHelper.GoToAppSettings();
                        });
                }
            }
        }

        void OnStartScannerChosen(UIAlertAction obj)
        {
            // If dialog is confimed start exposure notifications through this async method: _viewModel.StartEN(); 
            StartIfStopped();
            UpdateUI();
        }

        async void OnStopScannerChosen(UIAlertAction obj)
        {
            // If dialog is dismissed stop exposure notifications through this async method: _viewModel.StopEN(); 
            await _viewModel.StopENService();
            UpdateUI();
        }

        void OnDailyNumbersBtnTapped(object sender, EventArgs e)
        {
            OpenDailyNumbersPage();
        }

        void OnMessageBtnTapped(object sender, EventArgs e)
        {
            OpenMessagesPage();
        }

        async void OnAreYouInfectedBtnTapped(object sender, EventArgs e)
        {
            if (await _viewModel.IsRunning())
            {
                UINavigationController navigationController = new UINavigationController(InformationAndConsentViewController.GetInformationAndConsentViewController());
                navigationController.SetNavigationBarHidden(true, false);
                navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                PresentViewController(navigationController, true, null);
            }
            else
            {
                DialogHelper.ShowDialog(this, _viewModel.ReportingIllDialogViewModel, null);
            }
        }

        void OpenDailyNumbersPage()
        {
            NavigationController?.PushViewController(DailyNumbersLoadingViewController.Create(), true);
        }

        void OpenMessagesPage()
        {
            UIViewController vc = NavigationHelper.ViewControllerByStoryboardName("MessagePage");
            NavigationController?.PushViewController(vc, true);
        }
    }
}
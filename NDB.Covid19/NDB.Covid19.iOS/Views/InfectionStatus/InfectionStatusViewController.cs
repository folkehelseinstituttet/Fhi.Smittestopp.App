using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NDB.Covid19.iOS.Permissions;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.iOS.Views.Settings;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

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

        bool _comingFromOnboarding;

        InfectionStatusViewModel _viewModel;

        UIButton _messageViewBtn;
        UIButton _areYouInfectedBtn;
        PulseAnimationView _pulseAnimationView;

        IOSPermissionManager _permissionManager;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = new InfectionStatusViewModel();
            SetupStyling();
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED, OnMessageStatusChanged);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND, OnAppReturnsFromBackground);
        }

        public override void ViewDidUnload()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            base.ViewDidUnload();
        }

        private void OnMessageStatusChanged(object _ = null)
        {
            InvokeOnMainThread(() => _viewModel.UpdateNotificationDot());
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetPermissionManager();
            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;

            _messageViewBtn.TouchUpInside += OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside += OnAreYouInfectedBtnTapped;

            OnAppReturnsFromBackground(null);

            if (_comingFromOnboarding)
            {
                StartIfStopped();
                _comingFromOnboarding = false;
            }
            _pulseAnimationView?.RestartAnimation();
        }


        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
            _messageViewBtn.TouchUpInside -= OnMessageBtnTapped;
            _areYouInfectedBtn.TouchUpInside -= OnAreYouInfectedBtnTapped;
            ResetStatusBar();
        }

        private void ResetStatusBar()
        {
            UIView statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            statusBar.BackgroundColor = ColorHelper.DEFAULT_BACKGROUND_COLOR;
            UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
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
            _areYouInfectedBtn.AccessibilityLabel = _viewModel.NewRegistrationAccessibilityText;
            _messageViewBtn.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(_viewModel.NewMessageAccessibilityText);
            ActivityExplainerLbl.Text = await _viewModel.StatusTxtDescription();
            SetOnOffBtnState(await _viewModel.IsRunning());
            SetStatusContainerState(await _viewModel.IsRunning());
            UpdateNewIndicatorView();
        }

        async void SetStatusContainerState(bool isRunning)
        {
            UIView statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
            if (NavigationController.TopViewController is SettingsViewController)
            {
                statusBar.BackgroundColor = ColorHelper.DEFAULT_BACKGROUND_COLOR;
            }
            else
            {
                statusBar.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;
            }
            UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);

            ScrollDownBackgroundView.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;

            StatusContainer.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;
            StatusText.Text = await _viewModel.StatusTxt();
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
            NewIndicatorView.Layer.CornerRadius = NewIndicatorView.Layer.Frame.Height / 2;
            ActivityExplainerLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontMedium, 18, 22);
            ActivityExplainerLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            MenuIcon.AccessibilityLabel = InfectionStatusViewModel.INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            MenuIcon.TitleEdgeInsets = new UIEdgeInsets(0, -8, 0, 0);
            MenuIcon.SetTitle(InfectionStatusViewModel.INFECTION_STATUS_MENU_TEXT, UIControlState.Normal);
            MenuIcon.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 18f, 18f);
            MenuIcon.SizeToFit();
            StatusText.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16f, 20f);
            OnOffBtn.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 22f, 28f);
            OnOffBtn.ContentEdgeInsets = new UIEdgeInsets(12, 12, 12, 12);
            OnOffBtn.Layer.CornerRadius = 8;

            SetupEncounterAndInfectedButtons();
        }

        void SetupEncounterAndInfectedButtons()
        {
            MessageView.Subviews[0].Layer.CornerRadius = 12;
            MessageView.Subviews[0].Layer.BorderWidth = 1;
            MessageView.Subviews[0].Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            AreYouInfectetView.Subviews[0].Layer.CornerRadius = 12;
            AreYouInfectetView.Subviews[0].Layer.BorderWidth = 1;
            AreYouInfectetView.Subviews[0].Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;

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

        }

        void UpdateNewIndicatorView()
        {
            InvokeOnMainThread(() =>
            {
                NewIndicatorView.Hidden = !_viewModel.ShowNewMessageIcon;

                UIApplication.SharedApplication.ApplicationIconBadgeNumber = NewIndicatorView.Hidden ? 0 : 1;

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

        void OpenMessagesPage()
        {
            UIViewController vc = NavigationHelper.ViewControllerByStoryboardName("MessagePage");
            NavigationController?.PushViewController(vc, true);
        }
    }
}
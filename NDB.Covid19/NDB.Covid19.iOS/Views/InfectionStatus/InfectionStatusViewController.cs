using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NDB.Covid19.ViewModels;
using NDB.Covid19.iOS.Permissions;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views;
using NDB.Covid19.Utils;
using UIKit;
using NDB.Covid19.iOS.Views.InfectionStatus;
using NDB.Covid19.iOS.Views.MessagePage;

namespace NDB.Covid19.iOS
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

            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND, OnAppReturnsFromBackground);
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
        }

        void OnAppReturnsFromBackground(object obj)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000); // Wait 1 sec before update the notification to wait for any status change
                BeginInvokeOnMainThread(_viewModel.UpdateNotificationDot);
            });
            UpdateUI();
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
            ActivityStatusLbl.Text = await _viewModel.StatusTxt();
            ActivityExplainerLbl.Text = await _viewModel.StatusTxtDescription();
            SetOnOffBtnState(await _viewModel.IsRunning());
            UpdateNewIndicatorView();
        }

        void SetOnOffBtnState(bool isRunning)
        {
            if (_pulseAnimationView == null)
            {
                CreatePulseAnimation(OnOffBtn);
            }

            if (isRunning)
            {
                OnOffBtn.BackgroundColor = UIColor.FromRGB(86, 197, 104);
                OnOffBtn.SetImage(UIImage.FromBundle("pauseIcon"), UIControlState.Normal);
                string text = InfectionStatusViewModel.INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
                OnOffBtn.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
                _pulseAnimationView.Hidden = false;
            }
            else
            {
                OnOffBtn.BackgroundColor = UIColor.FromRGB(235, 87, 87);
                OnOffBtn.SetImage(UIImage.FromBundle("playIcon"), UIControlState.Normal);
                string text = InfectionStatusViewModel.INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT;
                OnOffBtn.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(text);
                OnOffBtn.ImageEdgeInsets = new UIEdgeInsets(0, 5, 0, 0);
                _pulseAnimationView.Hidden = true;
            }
        }

        void CreatePulseAnimation(UIView view)
        {
            _pulseAnimationView = new PulseAnimationView();
            OnOffBtnContainer.InsertSubview(_pulseAnimationView, 0);
            _pulseAnimationView.TranslatesAutoresizingMaskIntoConstraints = false;
            _pulseAnimationView.TopAnchor.ConstraintEqualTo(view.TopAnchor).Active = true;
            _pulseAnimationView.BottomAnchor.ConstraintEqualTo(view.BottomAnchor).Active = true;
            _pulseAnimationView.LeadingAnchor.ConstraintEqualTo(view.LeadingAnchor).Active = true;
            _pulseAnimationView.TrailingAnchor.ConstraintEqualTo(view.TrailingAnchor).Active = true;
            _pulseAnimationView.BackgroundColor = UIColor.FromRGB(86, 197, 104);
        }

        void SetupStyling()
        {
            NewIndicatorView.Layer.CornerRadius = NewIndicatorView.Layer.Frame.Height / 2;

            OnOffBtn.Layer.CornerRadius = OnOffBtn.Layer.Frame.Width / 2;

            ActivityStatusLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 17, 24);

            ActivityExplainerLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 20);
            ActivityExplainerLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            MenuIcon.AccessibilityLabel = InfectionStatusViewModel.INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            SetupEncounterAndInfectedButtons();
        }

        void SetupEncounterAndInfectedButtons()
        {
            MessageView.Subviews[0].Layer.CornerRadius = 12;
            AreYouInfectetView.Subviews[0].Layer.CornerRadius = 12;

            MessageLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 20);
            MessageLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            NewRegistrationLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 12, 16);
            NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;

            AreYouInfectetLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 20);
            AreYouInfectetLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            LogInAndRegisterLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 12, 16);
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
            if (await _viewModel.IsRunning())
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
            UINavigationController vc = MessagePageViewController.GetMessagePageControllerInNavigationController();
            PresentViewController(vc, true, null);
        }
    }
}
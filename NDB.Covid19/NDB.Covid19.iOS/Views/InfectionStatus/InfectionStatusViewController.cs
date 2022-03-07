using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommonServiceLocator;
using CoreFoundation;
using MoreLinq;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Permissions;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.DailyNumbers;
using NDB.Covid19.iOS.Views.Settings;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using UserNotifications;
using static NDB.Covid19.ViewModels.InfectionStatusViewModel;
using NDB.Covid19.iOS.Views.SelftestOption;
using NDB.Covid19.Models;

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

        UIFocusGuide _focusGuide = new UIFocusGuide();
        UITapGestureRecognizer _bannerGestureRecognizer;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = new InfectionStatusViewModel();
            _viewModel.RequestImportantMessageAsync((ImportantMessage message) => DispatchQueue.MainQueue.DispatchAsync(() => UpdateImportantMessage(message)));
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

        public override void DidUpdateFocus(UIFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
        {
            base.DidUpdateFocus(context, coordinator);

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

            TopBar.BackgroundColor = isRunning ? ColorHelper.STATUS_ACTIVE : ColorHelper.STATUS_INACTIVE;

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
            InformationBannerLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontMedium, 18, 22);
            InformationBannerLbl.TextAlignment = UITextAlignment.Center;
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

        void SetupInformationBannerLink()
        {
            _bannerGestureRecognizer = new UITapGestureRecognizer();
            _bannerGestureRecognizer.AddTarget(() => OnInformationBannerViewTapped(_bannerGestureRecognizer));
            InformationBannerView.AddGestureRecognizer(_bannerGestureRecognizer);
            InformationBannerView.UserInteractionEnabled = true;
            InformationBannerLbl.AccessibilityTraits = UIAccessibilityTrait.Button;
        }

        private void UpdateImportantMessage(ImportantMessage message)
        {
            if (message != null)
            {
                UIView.Transition(withView: View, duration: 0.2, options: UIViewAnimationOptions.CurveEaseInOut, animation: () => InformationBannerView.Hidden = false, completion: null);                
                InformationBannerView.BackgroundColor = message.BannerColor.ToUIColor();
                InformationBannerLbl.Text = message.Text;
                InformationBannerLbl.AccessibilityLabel = message.Text;

                if (message.IsClickable)
                {
                    SetupInformationBannerLink();
                }
                else
                {
                    InformationBannerLbl.UserInteractionEnabled = false;
                }
            }
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
            dailyNumbersLbl.TextAlignment = LayoutUtils.GetTextAlignment();
            dailyNumbersUpdatedLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            dailyNumbersUpdatedLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_TEXT;
            dailyNumbersUpdatedLbl.TextAlignment = LayoutUtils.GetTextAlignment();

            MessageLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 18, 22);
            MessageLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            MessageLbl.TextAlignment = LayoutUtils.GetTextAlignment();
            NewRegistrationLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            NewRegistrationLbl.Text = _viewModel.NewMessageSubheaderTxt;
            NewRegistrationLbl.TextAlignment = LayoutUtils.GetTextAlignment();

            AreYouInfectetLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 18, 22);
            AreYouInfectetLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            AreYouInfectetLbl.TextAlignment = LayoutUtils.GetTextAlignment();
            LogInAndRegisterLbl.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 14, 18);
            LogInAndRegisterLbl.Text = InfectionStatusViewModel.INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;
            LogInAndRegisterLbl.TextAlignment = LayoutUtils.GetTextAlignment();

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
                        action => ShowPickerController()
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
            string[] requestID = new[] { NotificationsEnum.TimedReminderFinished.ToString() };
            UNUserNotificationCenter.Current.RemoveDeliveredNotifications(
                requestID);
            UNUserNotificationCenter.Current.RemovePendingNotificationRequests(
                requestID);
            // If dialog is confimed start exposure notifications through this async method: _viewModel.StartEN(); 
            StartIfStopped();
            UpdateUI();
        }

        async void OnStopScannerChosen()
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
                UINavigationController navigationController = new UINavigationController(SelftestOptionViewController.GetSelftestOptionViewController());
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
        void SetRecursiveInteraction(UIView parentView, bool isEnabled)
        {
            InvokeOnMainThread(() =>
            {
                parentView?.Subviews.ForEach(view =>
                {
                    if (!view.Equals(SpinnerMainView) ||
                        !view.Equals(Picker) ||
                        !view.Equals(SpinnerDialogTitle) ||
                        !view.Equals(SpinnerDialogMessage) ||
                        !view.Equals(SpinnerDialogButton))
                    {
                        view.AccessibilityElementsHidden = !isEnabled;
                        view.UserInteractionEnabled = isEnabled;
                    }
                });
            });
        }
        async void ShowPickerController()
        {
            SetRecursiveInteraction(View, false);
            View.AddSubview(SpinnerMainView);
            // Make sure accessibility is enabled for the whole view
            SpinnerMainView.AccessibilityElementsHidden = false;
            SpinnerMainView.UserInteractionEnabled = true;

            // Disable accessibility for the subviews to prevent focus stealing
            SpinnerDialogButton.AccessibilityElementsHidden = true;
            SpinnerDialogButton.UserInteractionEnabled = false;
            Picker.AccessibilityElementsHidden = true;
            Picker.UserInteractionEnabled = false;
            SpinnerDialogTitle.AccessibilityElementsHidden = true;
            SpinnerDialogTitle.UserInteractionEnabled = false;
            SpinnerDialogMessage.AccessibilityElementsHidden = true;
            SpinnerDialogMessage.UserInteractionEnabled = false;

            StyleUtil.InitLabelWithSpacing(
                SpinnerDialogTitle,
                StyleUtil.FontType.FontBold,
                INFECTION_STATUS_PAUSE_DIALOG_TITLE,
                1.14,
                24,
                38,
                true);
            StyleUtil.InitLabelWithSpacing(
                SpinnerDialogMessage,
                StyleUtil.FontType.FontRegular,
                INFECTION_STATUS_PAUSE_DIALOG_MESSAGE,
                1.28,
                16,
                20,
                true);
            SpinnerDialogButton.SetTitle(
                INFECTION_STATUS_PAUSE_DIALOG_OK_BUTTON,
                UIControlState.Normal);

            SpinnerDialogTitle.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_PAUSE_DIALOG_TITLE);
            SpinnerDialogMessage.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_PAUSE_DIALOG_MESSAGE);
            SpinnerDialogButton.AccessibilityAttributedLabel =
                AccessibilityUtils.RemovePoorlySpokenSymbols(INFECTION_STATUS_PAUSE_DIALOG_OK_BUTTON);

            Picker.Model = new HoursPickerModel(
                new List<string> {
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_NO_REMINDER,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_ONE_HOUR,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_TWO_HOURS,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_FOUR_HOURS,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_EIGHT_HOURS
                });

            SpinnerMainView.Hidden = false;
            SetFocusTo(SpinnerDialogTitle, () =>
            {
                SpinnerDialogButton.AccessibilityElementsHidden = false;
                SpinnerDialogButton.UserInteractionEnabled = true;
                Picker.AccessibilityElementsHidden = false;
                Picker.UserInteractionEnabled = true;
                SpinnerDialogTitle.AccessibilityElementsHidden = false;
                SpinnerDialogTitle.UserInteractionEnabled = true;
                SpinnerDialogMessage.AccessibilityElementsHidden = false;
                SpinnerDialogMessage.UserInteractionEnabled = true;
            });
        }
        void SetFocusTo(UIView view, Action onAccessibilityPostFinished = null)
        {
            DispatchQueue.MainQueue.DispatchAfter(
                new DispatchTime(DispatchTime.Now, 10000000L),
                () =>
                {
                    UIAccessibility.PostNotification(
                        UIAccessibilityPostNotification.LayoutChanged,
                        view);
                    onAccessibilityPostFinished?.Invoke();
                });
        }

        partial void OnSpinnerDialogButton_TouchUpInside(UIButton sender)
        {
            InvokeOnMainThread(() =>
            {
                SpinnerMainView.Hidden = true;
                switch (HoursPickerModel.SelectedOptionByUser)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        DisplayNotificationAfterTime((int)Math.Pow(2, HoursPickerModel.SelectedOptionByUser - 1));
                        break;
                }
                OnStopScannerChosen();
                SetRecursiveInteraction(View, true);
                SetFocusTo(View);
                SpinnerMainView.AccessibilityElementsHidden = true;
            });
        }

        void OnInformationBannerViewTapped(UITapGestureRecognizer recognizer)
        {
            _viewModel.OpenInformationBannerLink();
        }

        private void DisplayNotificationAfterTime(int hourMultiplier)
        {
            long ticks = 60 * 60 * hourMultiplier;
#if DEBUG
            ticks /= 60;
#endif
            ServiceLocator.Current
                .GetInstance<ILocalNotificationsManager>()
                .GenerateDelayedNotification(
                    NotificationsEnum.TimedReminderFinished.Data(),
                    ticks);
        }


    }
}
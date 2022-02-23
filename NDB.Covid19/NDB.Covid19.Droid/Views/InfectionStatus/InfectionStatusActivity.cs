using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using CommonServiceLocator;
using NDB.Covid19.Droid.Services;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.AuthenticationFlow;
using NDB.Covid19.Droid.Views.DailyNumbers;
using NDB.Covid19.Droid.Views.Messages;
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xamarin.ExposureNotifications;
using Xamarin.Essentials;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.ViewModels.InfectionStatusViewModel;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using static NDB.Covid19.Droid.Utils.BatteryOptimisationUtils;
using AlertDialog = Android.App.AlertDialog;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.Models;

namespace NDB.Covid19.Droid.Views.InfectionStatus
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class InfectionStatusActivity : AppCompatActivity
    {
        private ImageView _fhiLogo;
        private ImageView _appLogo;
        private InfectionStatusViewModel _viewModel;
        private TextView _activityStatusText;
        private TextView _activityStatusDescription;
        private TextView _dailyNumbersHeader;
        private TextView _dailyNumbersSubHeader;
        private TextView _messeageHeader;
        private TextView _messageSubHeader;
        private TextView _registrationHeader;
        private TextView _registrationSubheader;
        private TextView _menuText;
        private TextView _messageBannerText;
        private Button _onOffButton;
        private ImageView _notificationDot;
        private RelativeLayout _dailyNumbersRelativeLayout;
        private RelativeLayout _messageRelativeLayout;
        private RelativeLayout _registrationRelativeLayout;
        private LinearLayout _toolbarLinearLayout;
        private LinearLayout _statusLinearLayout;
        private LinearLayout _messageBannerLinearLayout;
        private ImageButton _menuIcon;
        private Button _dailyNumbersCoverButton;
        private Button _messageCoverButton;
        private Button _registrationCoverButton;
        private Button _positiveButton;
        private Button _negativeButton;
        private Button _dontShowButton;        
        private NumberPicker _picker;
        private bool _dialogDisplayed;
        private bool _lockUnfocusedDialogs;
        
        private readonly SemaphoreSlim _semaphoreSlim =
            new SemaphoreSlim(1, 1);

        private readonly PermissionUtils _permissionUtils =
            ServiceLocator.Current.GetInstance<PermissionUtils>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.infection_status);
            _viewModel = new InfectionStatusViewModel();
            InitLayout();
            UpdateMessagesStatus();
            _viewModel.RequestImportantMessageAsync((ImportantMessage message) => RunOnUiThread(() => UpdateImportantMessage(message)));;
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED,
                OnMessageStatusChanged);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_UPDATE_DAILY_NUMBERS, OnAppDailyNumbersChanged);

            if (IsAppLaunchedToPullKeys)
            {
                UpdateKeys();
                IsAppLaunchedToPullKeys = false;
            }
            
            CheckIfShowBackgroundActivityDialog();

            if (!GetIsBackgroundActivityDialogShowEnableNewUser()
                && GetIsBackgroundActivityDialogShowEnable()
                && !CheckIsEnableBatteryOptimizations()
                && IsAppLaunchedToShowDialog
                && (SystemTime.Now().Date != DialogLastShownDate))
            {
                ShowBackgroundActivityDialog();
                DialogLastShownDate = SystemTime.Now().Date;                
            }

        }

        private void OnAppDailyNumbersChanged(object _ = null)
        {
            RunOnUiThread(() => {
                _dailyNumbersSubHeader.Text = INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_TEXT;
                _dailyNumbersCoverButton.ContentDescription =
                $"{INFECTION_STATUS_DAILY_NUMBERS_HEADER_TEXT} {INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_ACCESSIBILITY_TEXT}";
            });
        }

        protected override void OnDestroy()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            base.OnDestroy();
        }

        private async Task<bool> IsRunning() =>
            await _viewModel.IsRunning() && _permissionUtils.IsLocationEnabled();

        private void OnMessageStatusChanged(object _ = null)
        {
            RunOnUiThread(() => _viewModel.UpdateNotificationDot());
        }

        protected override void OnResume()
        {
            base.OnResume();

            UpdateCorrelationId(null);
            LogUtils.LogMessage(LogSeverity.INFO, $"User opened InfectionStatus", null);

            _permissionUtils.SubscribePermissionsMessagingCenter(this,
                o =>
                {
                    if (!_permissionUtils.AreAllPermissionsGranted())
                    {
                        PreventMultiplePermissionsDialogsForAction(
                            _permissionUtils.HasPermissions);
                    }
                });

            UpdateUI();

            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;
            OnMessageStatusChanged();

            if (!(_positiveButton is null))
            {
                AdjustLines(_dontShowButton, _positiveButton, _negativeButton);
            }

        }

        private void ShowPermissionsDialogIfTheyHavChangedWhileInIdle() =>
            RunOnUiThread(() =>
                PreventMultiplePermissionsDialogsForAction(
                    _permissionUtils.CheckPermissionsIfChangedWhileIdle));

        private async void PreventMultiplePermissionsDialogsForAction(Func<Task<bool>> action)
        {
            if (_dialogDisplayed || _lockUnfocusedDialogs) return;
            await _semaphoreSlim.WaitAsync();
            _dialogDisplayed = true;
            if (!await IsRunning())
            {
                if (action != null) await action.Invoke();
                // wait until BT state change will be completed
                await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
                UpdateUI();
            }

            _dialogDisplayed = false;
            _semaphoreSlim.Release();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _permissionUtils.UnsubscribePermissionsMessagingCenter(this);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
        }

        private async void UpdateKeys()
        {
            if (await _viewModel.IsRunning())
            {
                await _viewModel.PullKeysFromServer();
            }
        }
        private async void InitLayout()
        {
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();

            // Header
            _toolbarLinearLayout = FindViewById<LinearLayout>(Resource.Id.infection_status_activity_toolbar_layout);
            _statusLinearLayout = FindViewById<LinearLayout>(Resource.Id.infection_status_activity_status_layout);
            _messageBannerLinearLayout = FindViewById<LinearLayout>(Resource.Id.infection_status_important_message_banner);

            //TextViews
            _activityStatusText = FindViewById<TextView>(Resource.Id.infection_status_activity_status_textView);
            _activityStatusDescription =
                FindViewById<TextView>(Resource.Id.infection_status_activivity_status_description_textView);
            _dailyNumbersHeader = FindViewById<TextView>(Resource.Id.infection_status_daily_numbers_text_textView);
            _dailyNumbersSubHeader = FindViewById<TextView>(Resource.Id.infection_status_daily_numbers_updated_textView);
            _messeageHeader = FindViewById<TextView>(Resource.Id.infection_status_message_text_textView);
            _messageSubHeader = FindViewById<TextView>(Resource.Id.infection_status_new_message_text_textView);
            _registrationHeader = FindViewById<TextView>(Resource.Id.infection_status_registration_text_textView);
            _registrationSubheader =
                FindViewById<TextView>(Resource.Id.infection_status_registration_login_text_textView);
            _menuText = FindViewById<TextView>(Resource.Id.infection_status_menu_text_view);
            _menuText.TextAlignment = Android.Views.TextAlignment.ViewEnd;
            _messageBannerText = FindViewById<TextView>(Resource.Id.infection_status_important_message_banner_text);

            //Buttons
            _onOffButton = FindViewById<Button>(Resource.Id.infection_status_on_off_button);
            _dailyNumbersRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.infection_status_daily_numbers_button_relativeLayout);
            _messageRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_messages_button_relativeLayout);
            _registrationRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_registration_button_relativeLayout);
            _menuIcon = FindViewById<ImageButton>(Resource.Id.infection_status_menu_icon_relativeLayout);
            _dailyNumbersCoverButton = FindViewById<Button>(Resource.Id.infection_status_daily_numbers_button_relativeLayout_button);
            _messageCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_messages_button_relativeLayout_button);
            _registrationCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_registration_button_relativeLayout_button);

            //ImageViews
            _notificationDot = FindViewById<ImageView>(Resource.Id.infection_status_message_bell_imageView);
            _fhiLogo = FindViewById<ImageView>(Resource.Id.infection_status_app_icon_imageView);
            _appLogo = FindViewById<ImageView>(Resource.Id.infection_status_app_logo);
            ImageView arrowImageView1 = FindViewById<ImageView>(Resource.Id.infection_status_registration_arrow_imageView);
            ImageView arrowImageView2 = FindViewById<ImageView>(Resource.Id.infection_status_message_arrow_imageView);
            Drawable arrowImage = ContextCompat.GetDrawable(this, Resource.Drawable.ic_arrow_right);
            arrowImage.AutoMirrored = true;
            arrowImageView1.SetImageDrawable(arrowImage);
            arrowImageView2.SetImageDrawable(arrowImage);

            //Text initialization
            _activityStatusText.Text = INFECTION_STATUS_ACTIVE_TEXT;
            _activityStatusDescription.Text = INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            _dailyNumbersHeader.Text = INFECTION_STATUS_DAILY_NUMBERS_HEADER_TEXT;
            _dailyNumbersSubHeader.Text = INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_TEXT;
            _messeageHeader.Text = INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            _messageSubHeader.Text = INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT;
            _registrationHeader.Text = INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            _registrationSubheader.Text = INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;
            _menuText.Text = INFECTION_STATUS_MENU_TEXT;

            _messeageHeader.TextAlignment = TextAlignment.ViewStart;
            _messageSubHeader.TextAlignment = TextAlignment.ViewStart;
            _registrationHeader.TextAlignment = TextAlignment.ViewStart;
            _registrationSubheader.TextAlignment = TextAlignment.ViewStart;

            //Accessibility
            _messageSubHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _messeageHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _registrationHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _dailyNumbersSubHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _menuIcon.ContentDescription = INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            _dailyNumbersCoverButton.ContentDescription = $"{INFECTION_STATUS_DAILY_NUMBERS_HEADER_TEXT} {INFECTION_STATUS_DAILY_NUMBERS_LAST_UPDATED_ACCESSIBILITY_TEXT}";
            _messageCoverButton.ContentDescription =
                $"{INFECTION_STATUS_MESSAGE_HEADER_TEXT} {INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT}";
            _registrationCoverButton.ContentDescription =
                $"{INFECTION_STATUS_REGISTRATION_HEADER_TEXT} {INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT}";
            _fhiLogo.ContentDescription = SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            _appLogo.ContentDescription = SMITTESPORING_APP_LOGO_ACCESSIBILITY;

            //Button click events
            _onOffButton.Click += new SingleClick(StartStopButton_Click, 500).Run;
            _dailyNumbersRelativeLayout.Click += new SingleClick(DailyNumbersLayoutButton_Click, 500).Run;
            _dailyNumbersCoverButton.Click += new SingleClick(DailyNumbersLayoutButton_Click, 500).Run;
            _messageRelativeLayout.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _messageCoverButton.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _registrationRelativeLayout.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _registrationCoverButton.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _menuIcon.Click += new SingleClick((sender, e) => NavigationHelper.GoToSettingsPage(this), 500).Run;
            _menuText.Click += new SingleClick((sender, e) => NavigationHelper.GoToSettingsPage(this), 500).Run;
            if (!await IsRunning())
            {
                _onOffButton.PerformClick();
            }

            UpdateUI();
            FlightModeHandlerBroadcastReceiver.OnFlightModeChange += UpdateUI;
        }

        private void UpdateUI()
        {
            RunOnUiThread(async () =>
            {
                bool isRunning = await IsRunning();
                _activityStatusText.Text = await _viewModel.StatusTxt(isRunning);
                _activityStatusDescription.Text = await _viewModel.StatusTxtDescription(isRunning);

                Color enabledColor =
                    new Color(ContextCompat.GetColor(this, Resource.Color.infectionStatusButtonOnGreen));
                Color disabledColor =
                    new Color(ContextCompat.GetColor(this, Resource.Color.infectionStatusButtonOffRed));
                _statusLinearLayout.SetBackgroundColor(isRunning ? enabledColor : disabledColor);
                _toolbarLinearLayout.SetBackgroundColor(isRunning ? enabledColor : disabledColor);
                Window.SetStatusBarColor(isRunning ? enabledColor : disabledColor);

                //Accessibility
                _activityStatusText.ContentDescription =
                    SMITTESPORING_APP_TITLE_ACCESSIBILITY + await _viewModel.StatusTxt(isRunning);

                if (isRunning)
                {
                    _onOffButton.Text = INFECTION_STATUS_STOP_BUTTON_TEXT;
                    _onOffButton.ContentDescription = INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT;
                    _onOffButton.Background = ContextCompat.GetDrawable(this, Resource.Drawable.ic_secondary_button);
                    _onOffButton.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.primaryText)));
                }
                else
                {
                    _onOffButton.Text = INFECTION_STATUS_START_BUTTON_TEXT;
                    _onOffButton.ContentDescription = INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT;
                    _onOffButton.Background = ContextCompat.GetDrawable(this, Resource.Drawable.ic_default_button);
                    _onOffButton.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.secondaryText)));
                }
            });
        }

        private void UpdateMessagesStatus()
        {
            RunOnUiThread(() =>
            {
                _notificationDot.SetImageDrawable(
                    _viewModel.ShowNewMessageIcon
                        ? ContextCompat.GetDrawable(this, Resource.Drawable.ic_notification_active)
                        : ContextCompat.GetDrawable(this, Resource.Drawable.ic_notification_inactive)
                );

                _messageSubHeader.Text = _viewModel.NewMessageSubheaderTxt;
                _messageCoverButton.ContentDescription =
                    $"{INFECTION_STATUS_MESSAGE_HEADER_TEXT} {_viewModel.NewMessageSubheaderTxt}";
            });
        }

        private void UpdateImportantMessage(ImportantMessage message)
        {          
            RunOnUiThread(() =>
            {
                _messageBannerLinearLayout.Visibility = ViewStates.Visible;
                _messageBannerLinearLayout.SetBackgroundColor(Color.ParseColor(message.BannerColor));
                _messageBannerText.Text = message.Text;
                _messageBannerText.ContentDescription = message.Text;

                if (message.IsClickable)
                {
                    _messageBannerLinearLayout.Click += new SingleClick(ImportantMessageBanner_Click, 500).Run;
                }                
            });
        }

        private void OnNewMessagesIconVisibilityChanged(object sender, EventArgs e)
        {
            UpdateMessagesStatus();
        }

        private async void StartStopButton_Click(object sender, EventArgs e)
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                bool isRunning = await _viewModel.IsEnabled();
                switch (isRunning)
                {
                    case true when !_permissionUtils.AreAllPermissionsGranted():
                        PreventMultiplePermissionsDialogsForAction(_permissionUtils.HasPermissions);
                        break;
                    case true:
                        await DialogUtils.DisplayDialogAsync(
                            this,
                            _viewModel.OffDialogViewModel,
                            ShowPauseDialog);
                        break;
                    default:
                        await DialogUtils.DisplayDialogAsync(
                            this,
                            _viewModel.OnDialogViewModel,
                            StartGoogleAPI);
                        break;
                }

            }
            finally
            {
                _semaphoreSlim.Release();
            }

            UpdateUI();
        }

        public override async void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            
            _lockUnfocusedDialogs = !hasFocus;
            
            if (hasFocus
                && await _viewModel.IsEnabled()
                && (!_permissionUtils.IsBluetoothEnabled()
                    || !_permissionUtils.IsLocationEnabled()))
            {
                ShowPermissionsDialogIfTheyHavChangedWhileInIdle();
            }
            
            if (!(_positiveButton is null))
            {
                AdjustLines(_dontShowButton, _positiveButton, _negativeButton);
            }
           
            UpdateUI();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                if (resultCode == Result.Ok)
                {
                    ExposureNotification.OnActivityResult(requestCode, resultCode, data);
                }
            }
            catch (Exception e)
            {
                _ = e.HandleExposureNotificationException(nameof(InfectionStatusActivity), nameof(OnActivityResult));
            }
            finally
            {
                _permissionUtils.OnActivityResult(requestCode, resultCode, data);
                UpdateUI();
            }
        }

        private async void StartGoogleAPI()
        {
            try
            {
                CloseReminderNotifications();
                await _viewModel.StartENService();
                bool isRunning = await IsRunning();
                if (isRunning)
                {
                    BackgroundFetchScheduler.ScheduleBackgroundFetch();
                    
                }

                if (await _viewModel.IsEnabled() &&
                    !await IsRunning() &&
                    await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI) == BluetoothState.OFF)
                {
                    if (!_permissionUtils.AreAllPermissionsGranted())
                    {
                        PreventMultiplePermissionsDialogsForAction(_permissionUtils.HasPermissions);
                        // wait until BT state change will be completed
                        await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
                    }
                }
            }
            finally
            {
                UpdateUI();
            }
            //showing dialog for new user
            if (GetIsBackgroundActivityDialogShowEnableNewUser()
                && !CheckIsEnableBatteryOptimizations())
            {
                ShowBackgroundActivityDialog();
                DialogLastShownDate = SystemTime.Now().Date;
            }
        }

        private async void StopGoogleAPI()
        {
            try
            {
                await _viewModel.StopENService();
            }
            finally
            {
                UpdateUI();
            }
        }

        private void DailyNumbersLayoutButton_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(DailyNumbersLoadingActivity)));
        }

        private void MessageLayoutButton_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(MessagesActivity)));
        }

        private void ImportantMessageBanner_Click(object sender, EventArgs e)
        {
            _viewModel.OpenInformationBannerLink();
        }

        private void ShowPauseDialog()
        {

            View dialogView = LayoutInflater.Inflate(Resource.Layout.spinner_dialog, null);
            AlertDialog builder = new AlertDialog.Builder(this)
                    .SetView(dialogView)
                    .SetCancelable(false)
                    .Create();
            builder.SetTitle(INFECTION_STATUS_PAUSE_DIALOG_TITLE);
            builder.SetMessage(INFECTION_STATUS_PAUSE_DIALOG_MESSAGE);
            Button okButton = dialogView.FindViewById<Button>(Resource.Id.spinner_dialog_ok_button);
            okButton.Text = INFECTION_STATUS_PAUSE_DIALOG_OK_BUTTON;
            _picker = dialogView.FindViewById(Resource.Id.picker) as NumberPicker;
            _picker.MinValue = 0;
            _picker.MaxValue = 4;
            _picker.DescendantFocusability = DescendantFocusability.BlockDescendants;
            _picker.SetDisplayedValues(
                new[]
                {
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_NO_REMINDER,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_ONE_HOUR,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_TWO_HOURS,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_FOUR_HOURS,
                    INFECTION_STATUS_PAUSE_DIALOG_OPTION_EIGHT_HOURS,
                });

            okButton.Click += new SingleClick((sender, args) =>
            {
                switch (_picker.Value)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        StartReminderService((int)Math.Pow(2, _picker.Value - 1));
                        break;
                }
                StopGoogleAPI();
                builder.Dismiss();
            }).Run;

            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
            {      
                builder.Window.DecorView.LayoutDirection = LayoutUtils.GetLayoutDirection();
                builder.Show();
            }
            else
            {
                builder.Show();
            }


        }
        private void CloseReminderNotifications()
        {
            NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager?.Cancel((int)NotificationsEnum.TimedReminderFinished);
            StopReminderService();
        }
        private void StartReminderService(long hourMultiplier)
        {
            Bundle bundle = new Bundle();
            long ticks = 1000 * 60 * 60 * hourMultiplier;
#if DEBUG
            ticks /= 60;
#endif
            bundle.PutLong("ticks", ticks);
            ForegroundServiceHelper
                .StartForegroundServiceCompat<TimedReminderForegroundService>(this, bundle);
        }

        private void StopReminderService()
        {
            ForegroundServiceHelper
                .StopForegroundServiceCompat<TimedReminderForegroundService>(this);
        }
        private async void RegistrationLayoutButton_Click(object sender, EventArgs e)
        {
            if (!await IsRunning())
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.ReportingIllDialogViewModel);
                return;
            }

            Intent intent = new Intent(this, typeof(QuestionnaireTestOptionActivity));
            StartActivity(intent);
            IsReportingSelfTest = false;
        }

        private void CheckIfShowBackgroundActivityDialog()
        {

            bool firstLaunchEver = VersionTracking.IsFirstLaunchEver;
            if (firstLaunchEver)
            {
                SetIsBackgroundActivityDialogShowEnable(true);
            }
            
        }
        private void AdjustLineBreaks(TextView viewA, TextView viewB)
        {
            if (viewA.LineCount > viewB.LineCount)
            {
                string addition = "";
                for (int i = 0; i < viewA.LineCount - viewB.LineCount; ++i)
                {
                    addition += "\n";
                }

                viewB.Text = viewB.Text + addition;
            }
        }

        private void AdjustLines(TextView viewA, TextView viewB, TextView viewC)
        {
            AdjustLineBreaks(viewA, viewB);
            AdjustLineBreaks(viewA, viewC);
        }
        public async void ShowBackgroundActivityDialog()
        {
            if (await _viewModel.IsRunning())
            {

                string messageCombined = INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_MESSAGE_PART1 + "\n\n" +
                    INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_MESSAGE_PART2 + "\n\n" +
                    INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_MESSAGE_PART3;
                View dialogView = LayoutInflater.Inflate(Resource.Layout.background_activity_dialog, null);
                TextView headerText = dialogView.FindViewById<TextView>(Resource.Id.background_activity_dialog_header);
                TextView messageText = dialogView.FindViewById<TextView>(Resource.Id.background_activity_dialog_message);
                headerText.Text = INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_TITLE;
                messageText.Text = messageCombined;
                _positiveButton = dialogView.FindViewById<Button>(Resource.Id.background_activity_dialog_button_yes);
                _negativeButton = dialogView.FindViewById<Button>(Resource.Id.background_activity_dialog_button_no);
                _dontShowButton = dialogView.FindViewById<Button>(Resource.Id.background_activity_dialog_button_dont_show);
                _positiveButton.Text = INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_OK_BUTTON.ToUpper();
                _negativeButton.Text = INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_NOK_BUTTON.ToUpper();
                _dontShowButton.Text = INFECTION_STATUS_BACKGROUND_ACTIVITY_DIALOG_DONT_SHOW_BUTTON.ToUpper();
                
                AlertDialog builder = new AlertDialog.Builder(this)
                    .SetView(dialogView)
                    .SetCancelable(false)
                    .Create();
                SetIsBackgroundActivityDialogShowEnableNewUser(false);
                _positiveButton.Click += new SingleClick((sender, args) =>
                {
                    StopBatteryOptimizationSetting(this);
                    builder.Dismiss();
                }).Run;
                _negativeButton.Click += new SingleClick((sender, args) =>
                {
                    IsAppLaunchedToShowDialog = false;
                    builder.Dismiss();
                }).Run;

                _dontShowButton.Click += new SingleClick((sender, args) =>
                {
                    SetIsBackgroundActivityDialogShowEnable(false);
                    builder.Dismiss();
                }).Run;
                builder.Show();
                AdjustLines(_dontShowButton, _positiveButton, _negativeButton);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CommonServiceLocator;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Views.Messages;
using NDB.Covid19.Droid.Utils;
using static NDB.Covid19.Droid.Utils.StressUtils;
using NDB.Covid19.Droid.Views.AuthenticationFlow;
using Android.Views.Animations;
using NDB.Covid19.Droid.Services;
using NDB.Covid19.Utils;
using static NDB.Covid19.ViewModels.InfectionStatusViewModel;
using AndroidX.Core.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace NDB.Covid19.Droid.Views.InfectionStatus
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class InfectionStatusActivity : AppCompatActivity
    {
        ImageView _fhiLogo;
        InfectionStatusViewModel _viewModel;
        TextView _activityStatusText;
        TextView _activityStatusDescription;
        TextView _messeageHeader;
        TextView _messageSubHeader;
        TextView _registrationHeader;
        TextView _registrationSubheader;
        TextView _menuText;
        Button _onOffButton;
        ImageView _notificationDot;
        RelativeLayout _messageRelativeLayout;
        RelativeLayout _registrationRelativeLayout;
        LinearLayout _toolbarLinearLayout;
        LinearLayout _statusLinearLayout;
        ImageButton _menuIcon;
        Button _messageCoverButton;
        Button _registrationCoverButton;

        bool _dialogDisplayed;
        readonly PermissionUtils _permissionUtils = ServiceLocator.Current.GetInstance<PermissionUtils>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.infection_status);
            _viewModel = new InfectionStatusViewModel();
            InitLayout();
            UpdateMessagesStatus();
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED, OnMessageStatusChanged);
        }

        protected override void OnDestroy()
        {
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
            base.OnDestroy();
        }

        private void OnMessageStatusChanged(object _ = null)
        {
            RunOnUiThread(() => _viewModel.UpdateNotificationDot());
        }

        protected override void OnResume()
        {
            base.OnResume();

            _permissionUtils.SubscribePermissionsMessagingCenter(this,
                o => PreventMultiplePermissionsDialogsForAction(_permissionUtils.HasPermissions));

            ShowPermissionsDialogIfTheyHavChangedWhileInIdle();

            UpdateUI();

            _viewModel.NewMessagesIconVisibilityChanged += OnNewMessagesIconVisibilityChanged;
            OnMessageStatusChanged();
        }

        private void ShowPermissionsDialogIfTheyHavChangedWhileInIdle() =>
            RunOnUiThread(() =>
                PreventMultiplePermissionsDialogsForAction(_permissionUtils.CheckPermissionsIfChangedWhileIdle));

        private async void PreventMultiplePermissionsDialogsForAction(Func<Task<bool>> action)
        {
            bool isRunning = await _viewModel.IsRunning();
            if ((!isRunning || !_permissionUtils.HasPermissionsWithoutDialogs())
                && _dialogDisplayed == false)
            {
                _dialogDisplayed = true;
                if (action != null) await action.Invoke();
                _dialogDisplayed = false;
                // wait until BT state change will be completed
                await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
            }
            UpdateUI();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _permissionUtils.UnsubscribePErmissionsMessagingCenter(this);
            _viewModel.NewMessagesIconVisibilityChanged -= OnNewMessagesIconVisibilityChanged;
        }

        async void InitLayout()
        {
            // Header
            _toolbarLinearLayout = FindViewById<LinearLayout>(Resource.Id.infection_status_activity_toolbar_layout);
            _statusLinearLayout = FindViewById<LinearLayout>(Resource.Id.infection_status_activity_status_layout);

            //TextViews
            _activityStatusText = FindViewById<TextView>(Resource.Id.infection_status_activity_status_textView);
            _activityStatusDescription =
                FindViewById<TextView>(Resource.Id.infection_status_activivity_status_description_textView);
            _messeageHeader = FindViewById<TextView>(Resource.Id.infection_status_message_text_textView);
            _messageSubHeader = FindViewById<TextView>(Resource.Id.infection_status_new_message_text_textView);
            _registrationHeader = FindViewById<TextView>(Resource.Id.infection_status_registration_text_textView);
            _registrationSubheader =
                FindViewById<TextView>(Resource.Id.infection_status_registration_login_text_textView);
            _menuText = FindViewById<TextView>(Resource.Id.infection_status_menu_text_view);

            //Buttons
            _onOffButton = FindViewById<Button>(Resource.Id.infection_status_on_off_button);
            _messageRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_messages_button_relativeLayout);
            _registrationRelativeLayout =
                FindViewById<RelativeLayout>(Resource.Id.infection_status_registration_button_relativeLayout);
            _menuIcon = FindViewById<ImageButton>(Resource.Id.infection_status_menu_icon_relativeLayout);
            _messageCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_messages_button_relativeLayout_button);
            _registrationCoverButton =
                FindViewById<Button>(Resource.Id.infection_status_registration_button_relativeLayout_button);

            //ImageViews
            _notificationDot = FindViewById<ImageView>(Resource.Id.infection_status_message_bell_imageView);
            _fhiLogo = FindViewById<ImageView>(Resource.Id.infection_status_app_icon_imageView);

            //Text initialization
            _activityStatusText.Text = INFECTION_STATUS_ACTIVE_TEXT;
            _activityStatusDescription.Text = INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT;
            _messeageHeader.Text = INFECTION_STATUS_MESSAGE_HEADER_TEXT;
            _messageSubHeader.Text = INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT;
            _registrationHeader.Text = INFECTION_STATUS_REGISTRATION_HEADER_TEXT;
            _registrationSubheader.Text = INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;
            _menuText.Text = INFECTION_STATUS_MENU_TEXT;

            //Accessibility
            _messageSubHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _messeageHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _registrationHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            _menuIcon.ContentDescription = INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT;
            _messageCoverButton.ContentDescription =
                $"{INFECTION_STATUS_MESSAGE_HEADER_TEXT} {INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT}";
            _registrationCoverButton.ContentDescription =
                $"{INFECTION_STATUS_REGISTRATION_HEADER_TEXT} {INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT}";
            _fhiLogo.ContentDescription = SMITTESPORING_FHI_LOGO_ACCESSIBILITY;

            //Button click events
            _onOffButton.Click += new SingleClick(StartStopButton_Click, 500).Run;
            _messageRelativeLayout.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _messageCoverButton.Click += new SingleClick(MessageLayoutButton_Click, 500).Run;
            _registrationRelativeLayout.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _registrationCoverButton.Click += new SingleClick(RegistrationLayoutButton_Click, 500).Run;
            _menuIcon.Click += new SingleClick((sender, e) => NavigationHelper.GoToSettingsPage(this), 500).Run;
            _menuText.Click += new SingleClick((sender, e) => NavigationHelper.GoToSettingsPage(this), 500).Run;
            if (!await _viewModel.IsRunning())
            {
                _onOffButton.PerformClick();
            }

            UpdateUI();
            FlightModeHandlerBroadcastReceiver.OnFlightModeChange += UpdateUI;
        }

        void UpdateUI()
        {
            RunOnUiThread(async () =>
            {
                bool isRunning = await _viewModel.IsRunning();
                _activityStatusText.Text = await _viewModel.StatusTxt();
                _activityStatusDescription.Text = await _viewModel.StatusTxtDescription();
                
                Color enabledColor = new Color(ContextCompat.GetColor(this, Resource.Color.infectionStatusButtonOnGreen));
                Color disabledColor = new Color(ContextCompat.GetColor(this, Resource.Color.infectionStatusButtonOffRed));
                _statusLinearLayout.SetBackgroundColor(isRunning ? enabledColor : disabledColor);
                _toolbarLinearLayout.SetBackgroundColor(isRunning ? enabledColor : disabledColor);

                //Accessibility
                _activityStatusText.ContentDescription = SMITTESPORING_APP_TITLE_ACCESSIBILITY + await _viewModel.StatusTxt();

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

        void OnNewMessagesIconVisibilityChanged(object sender, EventArgs e)
        {
            UpdateMessagesStatus();
        }

        private async void StartStopButton_Click(object sender, EventArgs e)
        {
            bool isRunning = await _viewModel.IsRunning();

            if (isRunning)
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.OffDialogViewModel,
                    StopGoogleAPI);
            }
            else
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.OnDialogViewModel,
                    StartGoogleAPI);
            }

            UpdateUI();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                ShowPermissionsDialogIfTheyHavChangedWhileInIdle();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {
                if (resultCode == Result.Ok)
                {
                    Xamarin.ExposureNotifications.ExposureNotification.OnActivityResult(requestCode, resultCode, data);
                }
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
                await _viewModel.StartENService();
                bool isRunning = await _viewModel.IsRunning();
                if (isRunning)
                {
                    BackgroundFetchScheduler.ScheduleBackgroundFetch();
                }

                if (await _viewModel.IsEnabled() &&
                    !await _viewModel.IsRunning() &&
                    await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI) == BluetoothState.OFF)
                {
                    await _permissionUtils.HasPermissions();
                    // wait until BT state change will be completed
                    await BluetoothStateBroadcastReceiver.GetBluetoothState(UpdateUI);
                }
            }
            finally
            {
                UpdateUI();
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

        private void MessageLayoutButton_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(MessagesActivity)));
        }

        private async void RegistrationLayoutButton_Click(object sender, EventArgs e)
        {
            if (!await _viewModel.IsRunning())
            {
                await DialogUtils.DisplayDialogAsync(
                    this,
                    _viewModel.ReportingIllDialogViewModel);
                return;
            }

            Intent intent = new Intent(this, typeof(InformationAndConsentActivity));
            this.StartActivity(intent);
        }
    }
}
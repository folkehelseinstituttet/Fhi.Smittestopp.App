using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using NDB.Covid19.Droid.GoogleApi.HardwareServices;
using NDB.Covid19.Droid.Shared.Utils;
using NDB.Covid19.Droid.GoogleApi.Views.ErrorActivities;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Shared.Utils.StressUtils;
using Thread = Java.Lang.Thread;

namespace NDB.Covid19.Droid.GoogleApi.Views.ProximityStatus
{
    [Activity(Label = "", Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class ProximityStatusActivity : AppCompatActivity
    {
        LinearLayout _warningLocalLayout;
        RelativeLayout _counterOffLayout;
        RelativeLayout _counterOnLayout;
        TextView _counterOffTextView;
        TextView _numberCounterTextView;
        TextView _numberCounterTextTextView;
        TextView _lastUpdatedTextView;
        TextView _counterExplainedTextView;
        TextView _warningTextView;
        RelativeLayout _warningLayout;
        Button _errorButton;
        Button _onOffButton;
        ImageButton _explanationContactCounterButton;
        ImageButton _explanationDailyAverageImageButton;
        ImageButton _explanationDailyAverageAllUsersImageButton;
        ProximityStatusViewModel _viewModel;
        TextView _unsupportedTextView;
        TextView _dailyAverageTextView;
        TextView _dailyAverageNumberTextView;
        TextView _dailyAverageNumberAllUsersTextView;
        TextView _dailyAverageAllUsersTextView;
        private RelativeLayout _settingsIcon;
        LinearLayout _averageContactsRelativeLayout;
        View _horizontalDevider;

        private DroidBluetoothServiceConnection serviceConnection;
        private LocationServicesPermissionUtils _locationServicesPermissionUtils;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.proximity_status);
            _viewModel = new ProximityStatusViewModel();
            _viewModel.GetAndParseDevices();
            InitView();

            Thread thread = new Thread(ResizeDevider);
            thread.Start();

            StartBluetoothIfHasPermissions();
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetDailyAverage();
            GetDailyAverageAllUsers();
            _viewModel.OnUpdateUICount += OnUpdateUICount;
            StopBluetoothIfNotHasPermissions();
            UpdateUI();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            _locationServicesPermissionUtils?.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            LocationServicesPermissionUtils.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnPause()
        {
            _viewModel.OnUpdateUICount -= OnUpdateUICount;
            base.OnPause();
        }

        private void StartBluetoothIfHasPermissions()
        {
            if (!LocationServicesPermissionUtils.DoesNotHavePermissions() && !_viewModel.IsRunning && _viewModel.GetAutoStartScanner())
            {
                if (LocationServicesPermissionUtils.AdapterState == State.On)
                {
                    _viewModel.StartBluetooth();
                }
            }
        }

        private void StopBluetoothIfNotHasPermissions()
        {
            if ((LocationServicesPermissionUtils.DoesNotHavePermissions() || !_viewModel.GetAutoStartScanner()) && _viewModel.IsRunning)
            {
                _viewModel.StopBluetooth();

                UnbindService(serviceConnection);
            }
        }

        void InitView()
        {
            // Views related to the main content in the on-state.
            _numberCounterTextView = FindViewById<TextView>(Resource.Id.number_counter_textView);
            _numberCounterTextTextView = FindViewById<TextView>(Resource.Id.number_counter_text_textView);
            _lastUpdatedTextView = FindViewById<TextView>(Resource.Id.last_updated_textView);
            _counterExplainedTextView = FindViewById<TextView>(Resource.Id.counter_explained_text_textView);
            _onOffButton = FindViewById<Button>(Resource.Id.on_off_button);
            _explanationContactCounterButton =
                FindViewById<ImageButton>(Resource.Id.number_counter_text_help_imageButton);
            ConstraintLayout userDataLayout = FindViewById<ConstraintLayout>(Resource.Id.userData);
            ConstraintLayout allUsersDataLayout = FindViewById<ConstraintLayout>(Resource.Id.allUsersData);
            _explanationDailyAverageImageButton =
                userDataLayout.FindViewById<ImageButton>(Resource.Id.daily_average_text_help_imageButton);
            _explanationDailyAverageAllUsersImageButton =
                allUsersDataLayout.FindViewById<ImageButton>(Resource.Id.daily_average_text_help_imageButton);

            _dailyAverageTextView = userDataLayout.FindViewById<TextView>(Resource.Id.daily_average_text_textView);
            _dailyAverageNumberTextView = userDataLayout.FindViewById<TextView>(Resource.Id.daily_average_number_textView);
            _dailyAverageAllUsersTextView = allUsersDataLayout.FindViewById<TextView>(Resource.Id.daily_average_text_textView);
            _dailyAverageNumberAllUsersTextView =
                allUsersDataLayout.FindViewById<TextView>(Resource.Id.daily_average_number_textView);
            _settingsIcon = FindViewById<RelativeLayout>(Resource.Id.icon_settin);
            _averageContactsRelativeLayout = FindViewById<LinearLayout>(Resource.Id.average_contacts_relativeLayout);
            _horizontalDevider = FindViewById<View>(Resource.Id.horizontal_devider);

            // Update default text
            _lastUpdatedTextView.Text = "";
            _numberCounterTextView.Text = "";
            _numberCounterTextTextView.Text = ProximityStatusViewModel.PROXIMITY_NUMBER_OF_CONTACTS_TEXT;
            _counterExplainedTextView.Text = ProximityStatusViewModel.PROXIMITY_TODAY_TEXT;
            _counterExplainedTextView.ContentDescription = ProximityStatusViewModel.PROXIMITY_TODAY_TEXT_ACCESSIBILITY;
            _dailyAverageTextView.Text = ProximityStatusViewModel.PROXIMITY_DAILY_AVERAGE_TEXT;
            _dailyAverageAllUsersTextView.Text = ProximityStatusViewModel.PROXIMITY_DAILY_AVERAGE_ALL_USERS_TEXT;

            // OnClickListeners
            _explanationDailyAverageAllUsersImageButton.Click += ExplanationDailyAverageAllUsers_Click;
            _explanationContactCounterButton.Click += ExplanationCounter_Click;
            _explanationDailyAverageImageButton.Click += ExplanationDailyAverage_Click;
            _onOffButton.Click += new SingleClick(StartStopButton_Click, 500).Run;
            _settingsIcon.Click += (sender, e) => NavigationHelper.GoToSettingsPage(this);

            // Accessibility descriptions
            _explanationDailyAverageAllUsersImageButton.ContentDescription = ProximityStatusViewModel.PROXIMITY_ACCESIBILITY_DAILY_AVERAGE_ALL_USERS_DESCRIPTOR;
            _explanationContactCounterButton.ContentDescription = ProximityStatusViewModel.PROXIMITY_ACCESIBILITY_NUMBER_OF_CONTACTS_DESCRIPTOR;
            _explanationDailyAverageImageButton.ContentDescription = ProximityStatusViewModel.PROXIMITY_ACCESIBILITY_DAILY_AVERAGE_DESCRIPTOR;
            _settingsIcon.ContentDescription = ProximityStatusViewModel.MENU_ACCESS_TEXT;

            // Views related to errors and warnings
            _unsupportedTextView = FindViewById<TextView>(Resource.Id.unsupported_Transmit_text_textView);
            _counterOffTextView = FindViewById<TextView>(Resource.Id.counter_off_text_textView);
            _counterOffLayout =
                FindViewById<RelativeLayout>(Resource.Id.proximity_status_page_counter_off_relativeLayout);
            _counterOnLayout =
                FindViewById<RelativeLayout>(Resource.Id.proximity_status_page_counter_on_relativeLayout);
            _warningLocalLayout = FindViewById<LinearLayout>(Resource.Id.warningBar); // Included layout in proximity_activity.xml
            _warningLayout = FindViewById<RelativeLayout>(Resource.Id.warning_layout); // Part of warningbar.xml, sibling to _errorTextView
            _warningTextView = FindViewById<TextView>(Resource.Id.warning_textView); // Child of _warningLayout
            _errorButton = FindViewById<Button>(Resource.Id.button_error); // Part of warningbar.xml sibling to _warningLayout

            _errorButton.Text = WelcomeViewModel.TRANSMISSION_ERROR_MSG_NOTIFICATION_TEXT;
            _warningTextView.Text = ProximityStatusViewModel.PROXIMITY_COUNT_TURNED_OFF_WARNING_TEXT;
            _counterOffTextView.Text = ProximityStatusViewModel.PROXIMITY_COUNTER_OFF_TEXT;
            _unsupportedTextView.Text = ProximityStatusViewModel.PROXIMITY_UNSUPPORTED_TRANSMIT_TEXT;

            // Default invisible layouts
            _warningLocalLayout.Visibility = ViewStates.Gone;
            _counterOffLayout.Visibility = ViewStates.Gone;
            _unsupportedTextView.Visibility = ViewStates.Gone;

            
            if (_viewModel.IsBleAvailable())
            {
                if (!_viewModel.IsRunning && _viewModel.GetAutoStartScanner())
                {
                    _onOffButton.PerformClick();
                }
                UpdateUI();
            }
            else {
                DisplayUnsupportedLayout();
            }
        }

        private void OnUpdateUICount(object sender, BluetoothResultViewModel bluetoothResultViewModel)
        {
            RunOnUiThread(() =>
            {
                SetCount(bluetoothResultViewModel);
            });
        }

        private void SetCount(BluetoothResultViewModel bluetoothResultViewModel)
        {
            _numberCounterTextView.Text = bluetoothResultViewModel.NumberOfContacts;
            _lastUpdatedTextView.Text = ProximityStatusViewModel.PROXIMITY_LAST_UPDATED_TEXT;
        }

        public void UpdateUI()
        {
            if (_viewModel.IsRunning)
            {
                DisplayOnLayout();

                switch (_viewModel.IsBleAvailable())
                {
                    case true:
                        _unsupportedTextView.Visibility = ViewStates.Gone;
                        break;
                    case false:
                        DisplayUnsupportedLayout();
                        break;
                }
            }
            else
            {
                DisplayOffLayout();
            }
        }

        private void DisplayUnsupportedLayout()
        {
            // If bluetooth isn't supported hide the on button.
            _counterOffTextView.Visibility = ViewStates.Gone;
            _onOffButton.Visibility = ViewStates.Gone;

            // Update layout to show UI elements for the unsupported-state.
            _counterOnLayout.Visibility = ViewStates.Gone; 
            _counterOffLayout.Visibility = ViewStates.Gone;
            _warningLocalLayout.Visibility = ViewStates.Visible; // Layout in proximity_status.xml.
            
            _errorButton.Visibility = ViewStates.Visible; 
            _unsupportedTextView.Visibility = ViewStates.Visible;
            _warningLayout.Visibility = ViewStates.Gone; // not interested int the counter is shutdown info
            _lastUpdatedTextView.Visibility = ViewStates.Gone;
        }

        private void DisplayOffLayout()
        {
            // while NOT running, update button to show the on-state
            _onOffButton.Text = ProximityStatusViewModel.SWITCH_ON_TEXT;
            _onOffButton.SetBackgroundResource(Resource.Drawable.on_off_button_green);

            // Update layout to show UI elements for the stopped-state
            _lastUpdatedTextView.Visibility = ViewStates.Invisible;
            _warningLayout.Visibility = ViewStates.Visible;
            _errorButton.Visibility = ViewStates.Gone;
            _unsupportedTextView.Visibility = ViewStates.Gone;
            _warningLocalLayout.Visibility = ViewStates.Visible;
            _counterOnLayout.Visibility = ViewStates.Gone;
            _counterOffLayout.Visibility = ViewStates.Visible;
            _errorButton.Visibility = ViewStates.Invisible;
        }

        private void DisplayOnLayout()
        {
            // While running update the button to show the off-state
            _onOffButton.Text = ProximityStatusViewModel.SWITCH_OFF_TEXT;
            _onOffButton.SetBackgroundResource(Resource.Drawable.on_off_button);

            // Update layout to show UI elements for the running-state
            _lastUpdatedTextView.Visibility = ViewStates.Visible;
            _warningLayout.Visibility = ViewStates.Gone;
            _warningLocalLayout.Visibility = ViewStates.Gone;
            _counterOnLayout.Visibility = ViewStates.Visible;
            _counterOffLayout.Visibility = ViewStates.Gone;
        }

        private async void StartStopButton_Click(object sender, EventArgs e)
        {
            if (!_viewModel.IsRunning)
            {
                _locationServicesPermissionUtils = new LocationServicesPermissionUtils();
                (await _locationServicesPermissionUtils.CheckMyOwnPermissionsPromise())
                    .Then(result =>
                    {
                        HandleStartStop();
                    });
            }
            else
            {
                HandleStartStop();
            }
        }

        private void HandleStartStop()
        {
            if (_viewModel.IsRunning)
            {
                _viewModel.StopBluetooth();
                UnbindService(serviceConnection);
            }
            else
            {
                if (!LocationServicesPermissionUtils.DoesNotHavePermissions() && !_viewModel.IsRunning)
                {
                    _viewModel.StartBluetooth();
                    BindDroidBluetoothService();
                }
            }
            UpdateUI();
        }

        private void ErrorButtonClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TransmissionErrorActivity));
            StartActivity(intent);
        }

        private void ExplanationCounter_Click(object sender, EventArgs e)
        {
            DialogUtils.DisplayBubbleDialog(this,
                ProximityStatusViewModel.PROXIMITY_EXPLANATION_DAILY_ENCOUNTER_HELP_TEXT, ProximityStatusViewModel.PROXIMITY_DIALOG_HELP_CLOSE);
        }

        private void ExplanationDailyAverage_Click(object sender, EventArgs e)
        {
            DialogUtils.DisplayBubbleDialog(this,
                ProximityStatusViewModel.PROXIMITY_EXPLANATION_DAILY_AVERAGE_CONTENT_TEXT, ProximityStatusViewModel.PROXIMITY_DIALOG_HELP_CLOSE);
        }

        private void ExplanationDailyAverageAllUsers_Click(object sender, EventArgs e)
        {
            DialogUtils.DisplayBubbleDialog(this,
                ProximityStatusViewModel.PROXIMITY_EXPLANATION_DAILY_AVERAGE_ALL_USERS_CONTENT_TEXT, ProximityStatusViewModel.PROXIMITY_DIALOG_HELP_CLOSE);
        }

        private async void GetDailyAverage()
        {
            string average = await _viewModel.GetDailyAverage();

            if (this != null)
            {
                RunOnUiThread(() => 
                { 
                    _dailyAverageNumberTextView.Text = average; 
                });
            }
        }

        private async void GetDailyAverageAllUsers()
        {
            string average = await _viewModel.GetDailyAverageAllUsersAsync();

            if (this != null)
            {
                RunOnUiThread(() =>
                {
                    _dailyAverageNumberAllUsersTextView.Text = average;
                    _dailyAverageNumberAllUsersTextView.ContentDescription = average == "-" ? ProximityStatusViewModel.PROXIMITY_STATUS_PAGE_ACCESSIBILITY_DAILY_AVERAGE_ALL_USERS_NOT_AVAILABLE : average;
                });
            }
        }

        public void ResizeDevider()
        {
            RunOnUiThread(() =>
            {
                if (_horizontalDevider.Height < _averageContactsRelativeLayout.Height - 75)
                {
                    _horizontalDevider.SetMinimumHeight(_averageContactsRelativeLayout.Height - 75);
                }
            });
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_viewModel.GetAutoStartScanner())
            {
                BindDroidBluetoothService();
            }
        }

        private void BindDroidBluetoothService()
        {
            System.Diagnostics.Debug.WriteLine("ProximityStatusActivity - should start a service");

            if (serviceConnection == null)
            {
                this.serviceConnection = new DroidBluetoothServiceConnection(this);
            }

            System.Diagnostics.Debug.WriteLine($"ProximityStatusActivity - {serviceConnection.IsConnected}");

            Intent serviceToStart = new Intent(this, typeof(DroidBluetoothService));
            BindService(serviceToStart, this.serviceConnection, Bind.AutoCreate);

            System.Diagnostics.Debug.WriteLine($"ProximityStatusActivity - {serviceConnection.IsConnected}");
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }
    }
}
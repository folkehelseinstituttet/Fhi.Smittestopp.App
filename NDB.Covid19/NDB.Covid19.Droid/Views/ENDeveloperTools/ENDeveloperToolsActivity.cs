using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using static NDB.Covid19.Droid.Utils.StressUtils;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Droid.Utils;
using System;
using Android.Text;
using Android.Views;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace NDB.Covid19.Droid.Views.ENDeveloperTools
{
    [Activity(Label = "ENDeveloperToolsActivity", Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    public class ENDeveloperToolsActivity : AppCompatActivity
    {
        private ENDeveloperToolsViewModel _viewModel;

        private Button _buttonBack;
        private Button _buttonPullKeys;
        private Button _buttonPullKeysAndGetExposureInfo;
        private Button _buttonPushKeys;
        private Button _buttonSendExposureMessage;
        private Button _buttonSendExposureMessageIncrement;
        private Button _buttonSendExposureMessageDecrement;
        private Button _buttonFetchExposureConfiguration;
        private Button _buttonLastUsedExposureConfiguration;
        private Button _buttonResetLocalData;
        private Button _buttonToggleMessageRetentionLength;
        private Button _buttonPrintLastSymptomOnsetDate;
        private Button _buttonPrintLastKeysPulledAndTimestamp;
        private Button _buttonSendExposureMessageAfter10Sec;
        private Button _buttonShowLastSummary;
        private Button _buttonShowLastExposureInfo;
        private Button _buttonShowLatestPullKeysTimesAndStatuses;
        private TextView _textViewDevOutput;
        private Button _buttonNoConsents;
        private Button _buttonOnlyV1Consents;
        private Button _buttonAllConsents;
        private Button _buttonPrintActualPreferences;
        private Button _buttonFakeGateway;
        private Button _pullWithDelay;
        private Button _buttonNoConsentsNoRestart;
        private Button _buttonOnlyV1ConsentsNoRestart;
        private Button _buttonAllConsentNoRestarts;
        private Button _buttonGoToForceUpdate;
        private Button _buttonShowLastDailySummary;
        private Button _buttonShowLastExposureWindow;
        private Button _buttonPrintLastMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.en_developer_tools);
            _viewModel = new ENDeveloperToolsViewModel();
            InitLayout();
        }

        private void InitLayout()
        {
            _buttonBack = FindViewById<Button>(Resource.Id.enDeveloperTools_button_back);
            _buttonBack.Click += new SingleClick((sender, args) => Finish()).Run;

            _buttonPullKeys = FindViewById<Button>(Resource.Id.enDeveloperTools_button_pullKeys);
            _buttonPullKeys.Click += new SingleClick((sender, args) => PullKeys()).Run;

            _buttonPullKeysAndGetExposureInfo =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_pullKeysAndGetExposureInfo);
            _buttonPullKeysAndGetExposureInfo.Click +=
                new SingleClick((sender, args) => PullKeysAndGetExposureInfo()).Run;

            _buttonPushKeys = FindViewById<Button>(Resource.Id.enDeveloperTools_button_pushKeys);
            _buttonPushKeys.Click += new SingleClick((sender, args) => GetPushKeyInfo()).Run;

            _buttonSendExposureMessage = FindViewById<Button>(Resource.Id.enDeveloperTools_button_sendExposureMessage);
            _buttonSendExposureMessage.Click +=
                new SingleClick(async (sender, args) => await SendExposureMessage()).Run;

            _buttonSendExposureMessageIncrement =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_sendExposureMessage_increment);
            _buttonSendExposureMessageIncrement.Click +=
                new SingleClick((sender, args) => SendExposureMessageIncrement()).Run;

            _buttonSendExposureMessageDecrement =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_sendExposureMessage_decrement);
            _buttonSendExposureMessageDecrement.Click +=
                new SingleClick((sender, args) => SendExposureMessageDecrement()).Run;

            _buttonSendExposureMessageAfter10Sec =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_sendExposureMessage_after_10_sec);
            _buttonSendExposureMessageAfter10Sec.Click +=
                new SingleClick(async (sender, args) => await SendExposureMessageAfter10Sec()).Run;

            _buttonFetchExposureConfiguration =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_fetchExposureConfiguration);
            _buttonFetchExposureConfiguration.Click +=
                new SingleClick((sender, args) => FetchExposureConfiguration()).Run;

            _buttonLastUsedExposureConfiguration =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_lastUsedExposureConfiguration);
            _buttonLastUsedExposureConfiguration.Click +=
                new SingleClick((sender, args) => LastUsedExposureConfiguration()).Run;

            _buttonResetLocalData = FindViewById<Button>(Resource.Id.enDeveloperTools_button_resetLocalData);
            _buttonResetLocalData.Click += new SingleClick((sender, args) => ResetLocalData()).Run;

            _buttonToggleMessageRetentionLength =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_toggleMessageRetentionLength);
            _buttonToggleMessageRetentionLength.Click += new SingleClick((sender, args) => ToggleRetentionTime()).Run;

            _buttonPrintLastSymptomOnsetDate =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_printLastSymptomOnsetDate);
            _buttonPrintLastSymptomOnsetDate.Click +=
                new SingleClick((sender, args) => PrintLastSymptomsOnsetDate()).Run;

            _buttonPrintLastKeysPulledAndTimestamp =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_printLastKeysPulledAndTimestamp);
            _buttonPrintLastKeysPulledAndTimestamp.Click +=
                new SingleClick((sender, args) => PrintLastPulledKeysAndTimestamp()).Run;

            _buttonShowLastSummary = FindViewById<Button>(Resource.Id.enDeveloperTools_button_showLastSummary);
            _buttonShowLastSummary.Click += new SingleClick((sender, args) => PrintLastSummary()).Run;

            _buttonShowLastExposureInfo =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_showLastExposureInfo);
            _buttonShowLastExposureInfo.Click += new SingleClick((sender, args) => PrintLastExposureInfo()).Run;

            _buttonShowLastDailySummary = FindViewById<Button>(Resource.Id.enDeveloperTools_button_showLastDailySummary);
            _buttonShowLastDailySummary.Click += new SingleClick((sender, args) => PrintLastDailySummary()).Run;

            _buttonShowLastExposureWindow = FindViewById<Button>(Resource.Id.enDeveloperTools_button_showLastExposureWindow);
            _buttonShowLastExposureWindow.Click += new SingleClick((sender, args) => PrintLastExposureWindow()).Run;


            _buttonShowLatestPullKeysTimesAndStatuses =
                FindViewById<Button>(Resource.Id.enDeveloperTools_button_showLatestPullKeysTimesAndStatuses);
            _buttonShowLatestPullKeysTimesAndStatuses.Click +=
                new SingleClick((sender, args) => ShowLatestPullKeysTimesAndStatuses()).Run;

            _textViewDevOutput = FindViewById<TextView>(Resource.Id.enDeveloperTools_textView_devOutput);

            _buttonNoConsents = FindViewById<Button>(Resource.Id.no_consents);
            _buttonOnlyV1Consents = FindViewById<Button>(Resource.Id.only_v1);
            _buttonAllConsents = FindViewById<Button>(Resource.Id.all_consents);

            _buttonNoConsents.Click +=
                new SingleClick((o, args) => ChangeConsentsAndRestart(OnboardingStatus.NoConsentsGiven)).Run;
            _buttonOnlyV1Consents.Click += new SingleClick((o, args) =>
                ChangeConsentsAndRestart(OnboardingStatus.OnlyMainOnboardingCompleted)).Run;
            _buttonAllConsents.Click += new SingleClick((o, args) =>
                ChangeConsentsAndRestart(OnboardingStatus.CountriesOnboardingCompleted)).Run;

            _buttonPrintActualPreferences = FindViewById<Button>(Resource.Id.print_actual_preferences);
            _buttonPrintActualPreferences.Click += OnPrintActualPreferences().Run;

            _buttonGoToForceUpdate = FindViewById<Button>(Resource.Id.navigate_to_force_update);
            _buttonGoToForceUpdate.Click += new SingleClick((o, args) =>
            {
                GoToForceUpdate();
            }).Run;

            _buttonFakeGateway = FindViewById<Button>(Resource.Id.fake_gateway);
            _buttonFakeGateway.Click += OnFakeGateway().Run;

            _pullWithDelay = FindViewById<Button>(Resource.Id.pull_with_delay);
            _pullWithDelay.Click += OnPullWithDelay().Run;

            _buttonNoConsentsNoRestart = FindViewById<Button>(Resource.Id.no_consents_no_restart);
            _buttonOnlyV1ConsentsNoRestart = FindViewById<Button>(Resource.Id.only_v1_no_restart);
            _buttonAllConsentNoRestarts = FindViewById<Button>(Resource.Id.all_consents_no_restart);

            _buttonNoConsentsNoRestart.Click += new SingleClick((o, args) =>
                    OnboardingStatusHelper.Status = OnboardingStatus.NoConsentsGiven).Run;
            _buttonOnlyV1ConsentsNoRestart.Click += new SingleClick((o, args) =>
                OnboardingStatusHelper.Status = OnboardingStatus.OnlyMainOnboardingCompleted).Run;
            _buttonAllConsentNoRestarts.Click += new SingleClick((o, args) =>
                OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted).Run;

            _buttonPrintLastMessage = FindViewById<Button>(Resource.Id.print_last_fetched_message_response);
            _buttonPrintLastMessage.Click += new SingleClick((o, args) =>
            {
                PrintLastMessage();
            }).Run;
        }

        private SingleClick OnPullWithDelay() => new SingleClick((o, args) =>
            _viewModel.PullWithDelay(_viewModel.PullKeysFromServer));

        private SingleClick OnFakeGateway() => new SingleClick((o, args) =>
        {
            RunOnUiThread(() =>
            {
                EditText input = new EditText(this);
                InputFilterLengthFilter inputFilterLengthFilter = new InputFilterLengthFilter(2);
                input.SetFilters(new IInputFilter[]{inputFilterLengthFilter});
                input.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                new AlertDialog.Builder(this)
                    .SetMessage("Enter region code (2 chars country code)")
                    .SetTitle("Fake Gateway")
                    .SetView(input)
                    .SetPositiveButton("OK", async (sender, eventArgs) =>
                    {
                        ApiResponse response = await _viewModel.FakeGateway(input.Text);
                        if (response != null)
                        {
                            UpdateText($"Pushed keys to region: {input.Text}\n" +
                                       $"isSuccessful: {response.IsSuccessfull}\n" +
                                       $"StatusCode: {response.StatusCode}\n" +
                                       $"Error Message: {response.ErrorLogMessage}\n\n");
                        }
                        else
                        {
                            UpdateText($"Pushing of keys failed with unknown error");
                        }
                    })
                    .SetNegativeButton("Cancel", (sender, eventArgs) => { /* ignore */ })
                .Show();
            });
        });

        private void GoToForceUpdate()
        {
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }
        
        private SingleClick OnPrintActualPreferences() => new SingleClick(async (o, args) =>
        {
            UpdateText(await _viewModel.GetFormattedPreferences());
        });

        private void ChangeConsentsAndRestart(OnboardingStatus status)
        {
            OnboardingStatusHelper.Status = status;
            NavigationHelper.RestartApp(this);
        }

        private void SendExposureMessageDecrement()
        {
            UpdateText(_viewModel.DecrementExposureDate());
        }

        private void SendExposureMessageIncrement()
        {
            UpdateText(_viewModel.IncementExposureDate());
        }

        private void UpdateText(string text) {
            RunOnUiThread(() => _textViewDevOutput.Text = text);
        }

        private void ToggleRetentionTime() {
            UpdateText(_viewModel.ToggleMessageRetentionTime());
        }

        private void PrintLastSymptomsOnsetDate() {
            UpdateText(_viewModel.PrintLastSymptomOnsetDate());
        }

        private void PrintLastPulledKeysAndTimestamp() {
            UpdateText(_viewModel.PrintLastPulledKeysAndTimestamp());
        }

        private void PullKeys()
        {
            Task.Run(async () =>
            {
                await DiagnosisKeysDataMappingUtils.SetDiagnosisKeysDataMappingAsync();
                await _viewModel.PullKeysFromServer();
                UpdateText($"{ENDeveloperToolsViewModel.GetLastPullResult()}");
            });
        }

        private async void PullKeysAndGetExposureInfo()
        {
            await _viewModel.PullKeysFromServerAndGetExposureInfo();
            UpdateText($"{ENDeveloperToolsViewModel.GetLastPullResult()}");
        }

        private async void GetPushKeyInfo()
        {
            string text = await _viewModel.GetPushKeyInfoFromSharedPrefs();
            UpdateText("Copied to clipboard: \n" + text);
        }

        private async Task SendExposureMessage()
        {
            UpdateText("Sending Exposure Message");
            try
            {
                await _viewModel.SimulateExposureMessage();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                UpdateText("Test method: _viewModel.SimulateExposureMessage() failed on android");
            }
        }

        private async Task SendExposureMessageAfter10Sec()
        {
            UpdateText("Sending Exposure Message in 10 sec");
            try {
                await _viewModel.SimulateExposureMessageAfter10Sec();
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                UpdateText("Test method: _viewModel.SimulateExposureMessageAfter10Sec() failed on android");
            }
        }

        private void FetchExposureConfiguration()
        {
            Task.Run(async () => {
                string res = await _viewModel.FetchExposureConfigurationAsync();
                RunOnUiThread(() => {
                    UpdateText("Copied to clipboard:\n" + res);
                }); 
            });
        }
        private void LastUsedExposureConfiguration()
        {
            string res = _viewModel.LastUsedExposureConfigurationAsync();
            RunOnUiThread(() => {
                UpdateText("Copied to clipboard:\n" + res);
            }); 
        }

        private void ResetLocalData()
        {
            DeviceUtils.CleanDataFromDevice();
            UpdateText("Device cleaned");
            DialogUtils.DisplayDialogAsync(this, "Local data partially deleted", "Delete successful. You still need to reinstall the app to delete Exposure Notification history and to avoid bugs where the app doesn't know we have exceeded the 'provide keys' quota for the past 24 hours", "OK");
        }

        private void PrintLastSummary()
        {
            UpdateText(_viewModel.GetLastExposureSummary());
        }

        private void PrintLastExposureInfo()
        {
            UpdateText(_viewModel.GetExposureInfosFromLastPull());
        }

        private void ShowLatestPullKeysTimesAndStatuses()
        {
            UpdateText(_viewModel.GetPullHistory());
        }

        private void PrintLastExposureWindow()
        {
            UpdateText(_viewModel.GetExposureWindows());
        }

        private void PrintLastDailySummary()
        {
            UpdateText(_viewModel.GetDailySummaries());
        }

        private void PrintLastMessage()
        {
            UpdateText(_viewModel.GetLastFetchedImportantMessage());
        }
    }
}
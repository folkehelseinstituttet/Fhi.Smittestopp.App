using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Models;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.ENDeveloperTools
{
    public partial class ENDeveloperToolsViewController : BaseViewController
    {
        ENDeveloperToolsViewModel _enDeveloperViewModel;

        public ENDeveloperToolsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _enDeveloperViewModel = new ENDeveloperToolsViewModel();
            _enDeveloperViewModel.DevToolUpdateOutput += updateUI;
        }

        async partial void ENDevPullKeys_TouchUpInside(UIButton sender)
        {
            await _enDeveloperViewModel.PullKeysFromServer();
            ENDevOutput.Text = ENDeveloperToolsViewModel.GetLastPullResult();
        }

        async partial void ENDevPullKeysAndGetExposureInfo_TouchUpInside(UIButton sender)
        {
            await _enDeveloperViewModel.PullKeysFromServerAndGetExposureInfo();
            ENDevOutput.Text = ENDeveloperToolsViewModel.GetLastPullResult();
        }

        async partial void ENDevPushKeys_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = "Copied to clipboard:\n" + await _enDeveloperViewModel.GetPushKeyInfoFromSharedPrefs();
        }

        partial void ENDevSendExposureMessage_TouchUpInside(UIButton sender)
        {
            Task.Run(async () => {
                await _enDeveloperViewModel.SimulateExposureMessage();
            });
        }

        partial void ENDevToggleMessageRetentionBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.ToggleMessageRetentionTime();
        }

        partial void ENDevPrintLastSymptomOnsetDateBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.PrintLastSymptomOnsetDate();
        }

        partial void ENDevPrintLastKeysAndTimestampBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.PrintLastPulledKeysAndTimestamp();
        }

        partial void ENDevShowLatestPullKeysTimesAndStatusesBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetPullHistory();
        }

        partial void ENDevSenExposureMessageAfter10SecBtn_TouchUpInside(UIButton sender)
        {
            _enDeveloperViewModel.SimulateExposureMessageAfter10Sec();
            ENDevOutput.Text = "Sending Message After 10 Sec...";
        }

        partial void ENDevResetDeviceBtn_TouchUpInside(UIButton sender)
        {
            DeviceUtils.CleanDataFromDevice();
            ENDevOutput.Text = "Cleaned Device, consider restarting the app";
        }

        partial void ENDevSendExposureDateDecrease_TouchUpInside(UIButton sender)
        {
            string res = _enDeveloperViewModel.DecrementExposureDate();
            ENDevOutput.Text = res;
        }

        partial void ENDevSendExposureDateIncrease_TouchUpInside(UIButton sender)
        {
            string res = _enDeveloperViewModel.IncementExposureDate();
            ENDevOutput.Text = res;
        }

        partial void ENDevExposureHistoryBtn_TouchUpInside(UIButton sender)
        {
            Task.Run(async () => {
                string res = await _enDeveloperViewModel.FetchExposureConfigurationAsync();
                ENDevOutput.Text = "Copied to clipboard" + res;
                
            });
        }

        void updateUI()
        {
            InvokeOnMainThread(() => ENDevOutput.Text = _enDeveloperViewModel.DevToolsOutput);
            ServiceLocator.Current.GetInstance<IClipboard>().SetTextAsync(_enDeveloperViewModel.DevToolsOutput).GetAwaiter().GetResult();
        }

        partial void ENDevShowSummaryBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetLastExposureSummary();
        }

        partial void ENDevShowExposureInfoBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetExposureInfosFromLastPull();
        }

        partial void ENDevShowDailySummaryBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetDailySummaries();
        }

        partial void ENDevShowExposureWindowBtn_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetExposureWindows();
        }

        partial void ENDevLastUsedConfigurationBtn_TouchUpInside(UIButton sender)
        {
             string res = _enDeveloperViewModel.LastUsedExposureConfigurationAsync();
             ENDevOutput.Text = "Copied to clipboard:\n" + res;
        }

        partial void PrintLastMessageButton_TouchUpInside(UIButton sender)
        {
            ENDevOutput.Text = _enDeveloperViewModel.GetLastFetchedImportantMessage();           
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }

        partial void ENDevFakeGatewayBtn_TounchInside(UIButton sender)
        {
            ShowModalAlertView(
                "Fake gateway",
                "Enter region code (2 chars country code)");
        }

        void ShowModalAlertView(string title, string message)
        {
            UIAlertController alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            string region = "no";
            alert.AddTextField(textField => { region = textField.Text; });
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, obj => { }));
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, async obj =>
            {
                ApiResponse response = await _enDeveloperViewModel.FakeGateway(alert.TextFields[0].Text);
                ENDevOutput.Text =$"Pushed keys to region: {alert.TextFields[0].Text}\n" +
                                  $"isSuccessful: {response.IsSuccessfull}\n" +
                                  $"StatusCode: {response.StatusCode}\n" +
                                  $"Error Message: {response.ErrorLogMessage}\n\n";
            }));
            PresentViewController(alert, true, null);
        }

        async partial void ENDevPrintPreferencesBtn_TounchInside(UIButton sender)
        {
            ENDevOutput.Text = "Actual preferences:\n" + await _enDeveloperViewModel.GetFormattedPreferences();
        }

        partial void GoToForceUpdate_TouchUpInside(UIButton sender)
        {
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_FORCE_UPDATE);
        }

        partial void NoConsentButton_TouchUpInside(UIButton sender)
        {
            OnboardingStatusHelper.Status = OnboardingStatus.NoConsentsGiven;
        }

        partial void V1OnlyButton_TouchUpInside(UIButton sender)
        {
            OnboardingStatusHelper.Status = OnboardingStatus.OnlyMainOnboardingCompleted;
        }

        partial void AllConsentGivenButton_TouchUpInside(UIButton sender)
        {
            OnboardingStatusHelper.Status = OnboardingStatus.CountriesOnboardingCompleted;
        }

        partial void PullWithDelay_TouchUpInside(UIButton sender)
        {
            _enDeveloperViewModel.PullWithDelay(_enDeveloperViewModel.PullKeysFromServer);
        }
    }
}
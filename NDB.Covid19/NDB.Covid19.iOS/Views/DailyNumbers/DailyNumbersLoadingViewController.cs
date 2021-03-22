using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;
using NDB.Covid19.ViewModels;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19.iOS.Views.DailyNumbers
{
    public partial class DailyNumbersLoadingViewController : BaseViewController
    {
        public DailyNumbersLoadingViewController(IntPtr handle) : base(handle)
        {
        }

        DailyNumbersViewModel _diseaseRateOfTodayData;

        public static DailyNumbersLoadingViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("DailyNumbersLoading", null);
            DailyNumbersLoadingViewController vc = storyboard.InstantiateInitialViewController() as DailyNumbersLoadingViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        UIActivityIndicatorView _spinner;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _spinner = StyleUtil.ShowSpinner(View,
                AppDelegate.ShouldOperateIn12_5Mode ? UIActivityIndicatorViewStyle.WhiteLarge : UIActivityIndicatorViewStyle.Large);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FetchDataForDiseaseRateViewModel();
        }

        public async void FetchDataForDiseaseRateViewModel()
        {

            _diseaseRateOfTodayData = new DailyNumbersViewModel();

            try
            {
                bool isSuccess = await DailyNumbersViewModel.UpdateFHIDataAsync();
                if (!isSuccess && LocalPreferencesHelper.HasNeverSuccessfullyFetchedFHIData)
                {
                    OnError(new NullReferenceException("No FHI data"));
                    return;
                }
                LogUtils.LogMessage(LogSeverity.INFO, "Data for the disease rate of the day is loaded");

                OnSuccess();
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        void Cleanup()
        {
            _spinner?.RemoveFromSuperview();
        }

        void OnError(Exception e)
        {
            if (LocalPreferencesHelper.HasNeverSuccessfullyFetchedFHIData)
            {
                AuthErrorUtils.GoToTechnicalErrorFHINumbers(this, LogSeverity.ERROR, e, "Could not load data for disease rate of the day, showing technical error page");
            }
            else
            {
                LogUtils.LogException(LogSeverity.ERROR, e, "Could not load data for disease rate of the day, showing old data");
                NavigationController?.PushViewController(DailyNumbersViewController.Create(), true);
            }
        }

        void OnSuccess()
        {
            Cleanup();
            NavigationController?.PushViewController(DailyNumbersViewController.Create(), true);
        }
    }
}
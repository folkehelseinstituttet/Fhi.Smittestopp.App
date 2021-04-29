using Foundation;
using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
    public partial class LoadingPageViewController : BaseViewController
    {
        public LoadingPageViewController (IntPtr handle) : base (handle)
        {
        }

        bool _isRunning;

        public static LoadingPageViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("LoadingPage", null);
            LoadingPageViewController vc = storyboard.InstantiateInitialViewController() as LoadingPageViewController;
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        UIActivityIndicatorView _spinner;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _spinner = StyleUtil.ShowSpinner(View,
                AppDelegate.ShouldOperateIn12_5Mode ? UIActivityIndicatorViewStyle.WhiteLarge : UIActivityIndicatorViewStyle.Large);
            AddObservers();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
            if (!_isRunning)
            {
                RunBackgroundActivity();
                _isRunning = true;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        void AddObservers()
        {
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Loading Page", null, GetCorrelationId());
            });

            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE, (object _) =>
            {
                LogUtils.LogMessage(LogSeverity.INFO, "The user left Loading Page", null, GetCorrelationId());
            });
        }

        public async void RunBackgroundActivity()
        {
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.SubmitSelfDiagnosisAsync();
                LogUtils.LogMessage(LogSeverity.INFO, "The user agreed to share keys", null, GetCorrelationId());
                OnSuccess();
            }
            catch (Exception e)
            {
                if (e is NSErrorException nsErrorEx && nsErrorEx.Code == 4) 
                {
                    LogUtils.LogException(LogSeverity.INFO, e, "The user refused to share keys", null, GetCorrelationId());
                    NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
                }
                else
                {
                   
                    OnError(e);
                }
            }
        }

        void Cleanup()
        {
            _spinner?.RemoveFromSuperview();
        }

        void OnError(Exception e, bool isOnFail = false)
        {
            if(!isOnFail)
            {
                LogUtils.LogMessage(LogSeverity.INFO, "Something went wrong during key sharing", e.Message, GetCorrelationId());
            }
            Cleanup();
            AuthErrorUtils.GoToTechnicalError(this, LogSeverity.ERROR, e, "Pushing keys failed" );
        }

        void OnSuccess()
        {
            Cleanup();
            NavigationController?.PushViewController(UploadCompletedViewController.Create(), true);
        }
    }
}
using Foundation;
using System;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using UIKit;

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
            _spinner = StyleUtil.ShowSpinner(View, UIActivityIndicatorViewStyle.Large);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!_isRunning)
            {
                RunBackgroundActivity();
                _isRunning = true;
            }
        }

        public async void RunBackgroundActivity()
        {
            try
            {
                await Xamarin.ExposureNotifications.ExposureNotification.SubmitSelfDiagnosisAsync();
                LogUtils.LogMessage(LogSeverity.INFO, "Successfully pushed keys to server");
                OnSuccess();
            }
            catch (Exception e)
            {
                if (e is NSErrorException nsErrorEx && nsErrorEx.Code == 4) 
                {
                    LogUtils.LogException(LogSeverity.INFO, e, "Permission to push keys was declined");
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

        void OnError(Exception e)
        {
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
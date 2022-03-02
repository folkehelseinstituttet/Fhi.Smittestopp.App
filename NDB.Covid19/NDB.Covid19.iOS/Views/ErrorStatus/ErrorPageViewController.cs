using System;
using System.Diagnostics;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.AuthenticationFlow;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

namespace NDB.Covid19.iOS.Views.ErrorStatus
{
    public partial class ErrorPageViewController : BaseViewController
	{
		public string ErrorTitle = "errorTitle";
		public string ErrorMessage = "errorMessageText";
		public ErrorPageViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupStyling();
			AddObservers();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			LogUtils.LogMessage(Enums.LogSeverity.INFO, "The user is seeing General Error", null, GetCorrelationId());
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
				LogUtils.LogMessage(Enums.LogSeverity.INFO, "The user is seeing General Error", null, GetCorrelationId());
			});

			MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE, (object _) =>
			{
				LogUtils.LogMessage(Enums.LogSeverity.INFO, "The user left General Error", null, GetCorrelationId());
			});
		}

		public static ErrorPageViewController Create(String errorTitle = "", String errorMessageTxt = "")
		{
			UpdateCorrelationId(null);
			UIStoryboard storyboard = UIStoryboard.FromName("ErrorPage", null);
			ErrorPageViewController vc = (ErrorPageViewController)storyboard.InstantiateViewController(nameof(ErrorPageViewController));
			vc.ErrorTitle = errorTitle;
			vc.ErrorMessage = errorMessageTxt;
			vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

			return vc;
		}

		private void SetupStyling()
        {
			ErrorTitleLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 32, 36);
			ErrorTitleLabel.Text = ErrorTitle;
			ErrorTitleLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;

			ErrorTitleLabel.AccessibilityTraits = UIAccessibilityTrait.Header;

			NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes
			{
				DocumentType = NSDocumentType.HTML,
				StringEncoding = NSStringEncoding.UTF8
			};
			NSError error = null;
			NSAttributedString attributedString = new NSAttributedString(
				NSData.FromString(ErrorMessage, NSStringEncoding.UTF8),
				documentAttributes,
				ref error);
			if (error != null)
			{
				Debug.Print(error.LocalizedDescription);
			}
			ErrorMessageLabel.AttributedText = attributedString;
			ErrorMessageLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 20, 24);
			ErrorMessageLabel.Editable = false;
			ErrorMessageLabel.Selectable = true;
			ErrorMessageLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
		
			StyleUtil.InitButtonStyling(OkButton, ErrorViewModel.REGISTER_ERROR_DISMISS);

			BackButton.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
			BackButton.TintColor = ColorHelper.PRIMARY_COLOR;

			ErrorTitleLabel.IsAccessibilityElement = ErrorTitle != "";
			ErrorMessageLabel.IsAccessibilityElement = ErrorMessage != "";

            if (ErrorTitleLabel.IsAccessibilityElement && ErrorTitle == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_HEADER)
            {
				ErrorTitleLabel.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_HEADER;
            }

            if (ErrorMessageLabel.IsAccessibilityElement && ErrorMessage == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_DESCRIPTION)
            {
                ErrorMessageLabel.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_DESCRIPTION;
            }

			StyleUtil.InitButtonStyling(ContinueWithSelftestButton, ErrorViewModel.REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT);

			ContinueWithSelftestButton.Hidden = ErrorTitle != ErrorViewModel.REGISTER_ERROR_NOMATCH_HEADER;
			ContinueWithSelftestButton.TitleLabel.TextAlignment = UITextAlignment.Center;
		}
		
		private void AdjustTextHeight(UILabel label)
        {
			//Enable auto-layout to be resized programatically
			label.TranslatesAutoresizingMaskIntoConstraints = true;
			//Extend size of the label
			label.SizeToFit();
		}

        partial void BackButton_TouchUpInside(UIButton sender)
        {
			Close();
		}

        partial void DismissErrorBtn_TouchUpInside(UIButton sender)
        {
			Close();
		}

        partial void ContinueWithSelftestButton_TouchUpInside(UIButton sender)
        {
			IsReportingSelfTest = true;
            NavigationController?.PushViewController(QuestionnaireViewController.Create(), true);
		}

        void Close()
        {
			if (NavigationController != null)
			{
				// This is a special case where error on this screen has a NavigationController but should not
				// dismiss it as it is done by GoToResultPageFromAuthFlow() method. Otherwise, it will return user
				// to the Consent Page 1.
				if (ErrorTitle.Equals(ErrorViewModel.REGISTER_ERROR_FETCH_FHI_DATA_HEADER))
				{
					NavigationHelper.GoToResultPage(NavigationController, false);
					return;
				}
		    	NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
			}
			else
			{
				DismissViewController(true, null);
			}
		}
	}
}

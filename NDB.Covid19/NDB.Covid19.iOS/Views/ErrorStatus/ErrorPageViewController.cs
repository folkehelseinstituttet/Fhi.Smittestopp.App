using System;
using NDB.Covid19.ViewModels;
using NDB.Covid19.iOS.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views.ErrorStatus
{
    public partial class ErrorPageViewController : BaseViewController
	{
		public string ErrorTitle = "errorTitle";
		public string ErrorSubtitle = "errorSubtitle";
		public string ErrorMessage = "errorMessageText";
		public ErrorPageViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupStyling();
			
		}

		public static ErrorPageViewController Create(String errorTitle = "", String errorSubtitle = "", String errorMessageTxt = "")
		{
			UIStoryboard storyboard = UIStoryboard.FromName("ErrorPage", null);
			ErrorPageViewController vc = (ErrorPageViewController)storyboard.InstantiateViewController(nameof(ErrorPageViewController));
			vc.ErrorTitle = errorTitle;
			vc.ErrorSubtitle = errorSubtitle;
			vc.ErrorMessage = errorMessageTxt;
			vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

			return vc;
		}


		private void SetupStyling()
        {
			ErrorTitleLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 24, 26);
			ErrorTitleLabel.Text = ErrorTitle;

			ErrorSubtitleLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 16, 18);
			ErrorSubtitleLabel.Text = ErrorSubtitle;

			ErrorMessageLabel.SetAttributedText(ErrorMessage);
			ErrorMessageLabel.Editable = false;
			ErrorMessageLabel.Selectable = true;
			
			StyleUtil.InitButtonStyling(OkButton, ErrorViewModel.REGISTER_ERROR_DISMISS);

			BackButton.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_CLOSE_BUTTON_TEXT;

			ErrorTitleLabel.IsAccessibilityElement = ErrorTitle == "" ? false : true;
			ErrorSubtitleLabel.IsAccessibilityElement = ErrorSubtitle == "" ? false : true;
			ErrorMessageLabel.IsAccessibilityElement = ErrorMessage == "" ? false : true;

            if (ErrorTitleLabel.IsAccessibilityElement == true && ErrorTitle == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_HEADER)
            {
				ErrorTitleLabel.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_HEADER;
            }

            if (ErrorMessageLabel.IsAccessibilityElement == true && ErrorMessage == ErrorViewModel.REGISTER_ERROR_TOOMANYTRIES_DESCRIPTION)
            {
                ErrorMessageLabel.AccessibilityLabel = ErrorViewModel.REGISTER_ERROR_ACCESSIBILITY_TOOMANYTRIES_DESCRIPTION;
            }

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

        void Close()
        {
            if (NavigationController != null)
            {
				NavigationHelper.GoToResultPageFromAuthFlow(NavigationController);
			}
            else
            {
				DismissViewController(true, null);
            }
        }
	}
}

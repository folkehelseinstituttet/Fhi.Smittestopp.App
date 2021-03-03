using System;
using System.Diagnostics;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

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
			
		}

		public static ErrorPageViewController Create(String errorTitle = "", String errorMessageTxt = "")
		{
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

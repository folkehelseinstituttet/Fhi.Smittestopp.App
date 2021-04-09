using System;
using Foundation;
using NDB.Covid19.Configuration;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.ForceUpdate
{
    public partial class ForceUpdateViewController : BaseViewController
    {
        public ForceUpdateViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8
            };
            NSError error = null;
            NSAttributedString attributedString =
                new NSAttributedString(NSData.FromString(ForceUpdateViewModel.FORCE_UPDATE_MESSAGE, NSStringEncoding.UTF8), documentAttributes, ref error);

            if (error != null)
            {
                LogUtils.LogMessage(Enums.LogSeverity.ERROR, error.LocalizedDescription);
            }

            //Ensuring text is resiezed correctly when font size is increased
            StyleUtil.InitLabekWithSpacingAndHTMLFormatting(
                TextLabel, StyleUtil.FontType.FontBold, attributedString, 1.28, 24, 30, UITextAlignment.Center);
            TextLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;

            StyleUtil.InitButtonStyling(AppStoreLinkButton, ForceUpdateViewModel.FORCE_UPDATE_BUTTON_APPSTORE_IOS);
            AppStoreLinkButton.AccessibilityTraits = UIAccessibilityTrait.Link;

            FhiLogo.AccessibilityLabel = ForceUpdateViewModel.SMITTESPORING_FHI_LOGO_ACCESSIBILITY;
            TextLabel.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(ForceUpdateViewModel.FORCE_UPDATE_MESSAGE);
        }

        partial void AppStoreLinkButton_TouchUpInside(UIButton sender)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(Conf.IOSAppstoreAppLink), new NSDictionary() { }, null);
        }
    }
}
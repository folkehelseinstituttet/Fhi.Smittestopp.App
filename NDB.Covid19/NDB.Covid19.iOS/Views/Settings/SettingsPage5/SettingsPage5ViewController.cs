using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage5
{
    public partial class SettingsPage5ViewController : BaseViewController
    {
        public SettingsPage5ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            string contentText = SettingsPage5ViewModel.SETTINGS_PAGE_5_CONTENT + " " + SettingsPage5ViewModel.SETTINGS_PAGE_5_LINK;

            HeaderLabel.SetAttributedText(SettingsPage5ViewModel.SETTINGS_PAGE_5_HEADER);

            StyleUtil.InitTextViewWithSpacing(ContentText, FontType.FontRegular, contentText, 1.28, 16, 22);
            ContentText.TextColor = UIColor.White;

            //ForegroundColor sets the color of the links. UnderlineStyle determins if the link is underlined, 0 without underline 1 with underline.
            ContentText.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, "#FADC5D".ToUIColor(), UIStringAttributeKey.UnderlineStyle, new NSNumber(1));

            StyleUtil.InitLabelWithSpacing(BuildVersionLbl, FontType.FontRegular, SettingsPage5ViewModel.GetVersionInfo(), 1.14, 14, 16);
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            ContentText.AccessibilityIdentifier = "contentTextIdentifier";
            ContentText.IsAccessibilityElement = true;
            ContentText.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(contentText);
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}
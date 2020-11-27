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

            InitTextViewWithSpacing(ContentText, FontType.FontRegular, contentText, 1.28, 16, 22);

            ContentText.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
            
            //ForegroundColor sets the color of the links. UnderlineStyle determins if the link is underlined, 0 without underline 1 with underline.
            ContentText.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, ColorHelper.TEXT_COLOR_ON_BACKGROUND, UIStringAttributeKey.UnderlineStyle, new NSNumber(1));

            InitLabelWithSpacing(BuildVersionLbl, FontType.FontRegular, SettingsPage5ViewModel.GetVersionInfo(), 1.14, 14, 16);
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            ContentText.AccessibilityIdentifier = "contentTextIdentifier";
            ContentText.IsAccessibilityElement = true;
            ContentText.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(contentText);

            SetupStyling();
        }

        private void SetupStyling()
        {
            HeaderLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            ContentText.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            BuildVersionLbl.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}
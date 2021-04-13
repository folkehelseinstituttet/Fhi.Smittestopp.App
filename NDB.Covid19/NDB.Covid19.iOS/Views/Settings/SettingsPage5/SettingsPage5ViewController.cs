using System;
using CoreGraphics;
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
            HeaderLabel.AccessibilityTraits = UIAccessibilityTrait.Header;

            InitTextViewWithSpacing(ContentText, FontType.FontRegular, contentText, 1.28, 16, 22);

            /* Necessary to unify horizontal alignment due to bug in iOS
            ** https://stackoverflow.com/questions/746670/how-to-lose-margin-padding-in-uitextview
            */
            ContentText.ContentInset = UIEdgeInsets.Zero;
            ContentText.TextContainerInset = UIEdgeInsets.Zero;
            ContentText.TextContainer.LineFragmentPadding = 0;
            ContentText.AccessibilityTraits = UIAccessibilityTrait.Link;
            ContentText.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
            
            //ForegroundColor sets the color of the links. UnderlineStyle determins if the link is underlined, 0 without underline 1 with underline.
            ContentText.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, ColorHelper.LINK_COLOR, UIStringAttributeKey.UnderlineStyle, new NSNumber(1));

            InitLabelWithSpacing(BuildVersionLbl, FontType.FontRegular, SettingsPage5ViewModel.GetVersionInfo(), 1.14, 14, 16);
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            ContentText.IsAccessibilityElement = true;
            ContentText.ScrollEnabled = false;

            InitAccessibilityStatementButton();

            SetupStyling();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            PostAccessibilityNotificationAndReenableElement(BackButton, HeaderLabel);
        }

        private void InitAccessibilityStatementButton()
        {
            InitLinkButtonStyling(AccessibilityStatementButton, SettingsPage5ViewModel.SETTINGS_PAGE_5_ACCESSIBILITY_STATEMENT_BUTTON_TEXT);
            AccessibilityStatementButton.AccessibilityTraits = UIAccessibilityTrait.Link;
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

        partial void AccessibilityStatementButton_TouchUpInside(UIButton sender)
        {
            SettingsPage5ViewModel.OpenAccessibilityStatementLink();
        }
    }
}
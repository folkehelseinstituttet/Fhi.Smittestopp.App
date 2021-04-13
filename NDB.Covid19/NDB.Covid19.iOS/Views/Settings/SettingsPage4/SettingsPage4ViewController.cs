using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings.SettingsPage4
{
    public partial class SettingsPage4ViewController : BaseViewController
    {
        public SettingsPage4ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            /* Note:
            This functionality is not planned for release 1.0. Kept for future use.

            //Line with email and phone number
            //string mail = SettingsPage4ViewModel.EMAIL_TEXT + " " + SettingsPage4ViewModel.EMAIL;
            //string phoneNum = SettingsPage4ViewModel.PHONE_NUM_Text + " " + SettingsPage4ViewModel.PHONE_NUM;

            //Opening hours
            //string monThu = "•  " + SettingsPage4ViewModel.PHONE_OPEN_MON_THU;
            //string fri = "•  " + SettingsPage4ViewModel.PHONE_OPEN_FRE;
            //string sunHoly = SettingsPage4ViewModel.PHONE_OPEN_SAT_SUN_HOLY;
            //string openingHours = SettingsPage4ViewModel.PHONE_OPEN_TEXT + "<br><br>" + monThu + "<br>" + fri + "<br><br>" + sunHoly;

            //Contract info section
            //string contactsInfo = "<br><br>" + mail + "<br>" + phoneNum + ".<br><br>" + openingHours;

            */

            //Support url and associated text
            string urlStringAndText = SettingsPage4ViewModel.CONTENT_TEXT_BEFORE_SUPPORT_LINK + " " + "<a href=" + SettingsPage4ViewModel.SUPPORT_LINK + ">" + SettingsPage4ViewModel.SUPPORT_LINK_SHOWN_TEXT + "</a>";

            //Concatenation the content
            string content = urlStringAndText; // + contactsInfo;

            ContentText.SetAttributedText(content);
            ContentText.AccessibilityTraits = UIAccessibilityTrait.Link;
            ContentText.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
            ContentText.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, ColorHelper.LINK_COLOR, UIStringAttributeKey.UnderlineStyle, new NSNumber(1));

            //Ensuring text is resiezed correctly when font size is increased
            HeaderLabel.SetAttributedText(SettingsPage4ViewModel.HEADER);
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            HeaderLabel.AccessibilityTraits = UIAccessibilityTrait.Header;

            SetupStyling();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            PostAccessibilityNotificationAndReenableElement(BackButton, HeaderLabel);
        }

        public void SetupStyling()
        {
            HeaderLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            ContentText.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}
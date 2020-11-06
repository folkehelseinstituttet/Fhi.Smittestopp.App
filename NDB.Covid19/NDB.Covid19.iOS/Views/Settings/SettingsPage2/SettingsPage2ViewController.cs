using System;
using Foundation;
using NDB.Covid19.Interfaces;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using Xamarin.Essentials;
using static NDB.Covid19.iOS.Utils.StyleUtil;


namespace NDB.Covid19.iOS.Views.Settings.SettingsPage2
{
    public partial class SettingsPage2ViewController : BaseViewController
    {
        UITapGestureRecognizer _gestureRecognizer;
        ConsentViewModel _consentViewModel;
        public SettingsPage2ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _consentViewModel = new ConsentViewModel();
            SetTexts();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _gestureRecognizer = new UITapGestureRecognizer();
            _gestureRecognizer.AddTarget(() => OpenWebPageBtnTapped(_gestureRecognizer));
            ButtonView.AddGestureRecognizer(_gestureRecognizer);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ButtonView.RemoveGestureRecognizer(_gestureRecognizer);
        }

        void SetTexts()
        {
            string intro = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_INTRO;
            string par1Title = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_TITLE;
            string par1Content = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT;
            string par2Title = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_TITLE;
            string par2Content = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT;
            string par3Title = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_TITLE;
            string par3Content = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_CONTENT;
            string par4Title = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_TITLE;
            string par4Content = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT;

            string contentText = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_INTRO +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_TITLE +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_TITLE +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_TITLE +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_CONTENT +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_TITLE +
                                 SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT;

            HeaderLabel.SetAttributedText(SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER);
            ContentText.TextContainerInset = new UIEdgeInsets(0, 20, 20, 40);
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
            InitLabel(UrlLabel, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_LINK_TEXT, 16, 28);

            NSMutableAttributedString text = new NSMutableAttributedString();
            text.Append(ApplyStylingToText(intro, FontType.FontRegular, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par1Title, FontType.FontBold, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par1Content, FontType.FontRegular, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par2Title, FontType.FontBold, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par2Content, FontType.FontRegular, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par3Title, FontType.FontBold, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par3Content, FontType.FontRegular, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par4Title, FontType.FontBold, 1.28, 16, 22, false));
            text.Append(ApplyStylingToText(par4Content, FontType.FontRegular, 1.28, 16, 22, false));
            ContentText.AttributedText = text;

            //Accessibility
            HeaderLabel.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER);
            ContentText.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(contentText);
            ContentText.AccessibilityIdentifier = "contentTextIdentifier";
            ContentText.IsAccessibilityElement = true;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }

        void OpenWebPageBtnTapped(UITapGestureRecognizer recognizer)
        {
            CommonServiceLocator.ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_LINK, BrowserLaunchMode.SystemPreferred);
        }

        private NSMutableAttributedString ApplyStylingToText(string text, FontType fontType, double lineHeight, int fontSize, int maxFontSize, bool isLink)
        {
            //Wraps the string containing HTML, with styling tags inorder to ensure correct font, color, size.
            string html = HtmlWrapper.HtmlForLabelWithText(text, 16, false, false, "#ffffff");

            //Defining document type as HTML, set string encoding and formats an attributed string with these.
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(html, NSStringEncoding.UTF8), documentAttributes, ref error);
            
            NSMutableAttributedString attributedTextToAppend = new NSMutableAttributedString(attributedString);
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);

            NSRange range = new NSRange(0, attributedTextToAppend.Length);
            attributedTextToAppend.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            attributedTextToAppend.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);

            //Add a hyperlink to this text, makes text clickable
            if (isLink)
            {
               attributedTextToAppend.AddAttribute(UIStringAttributeKey.Link, new NSUrl("https://www." + text), range);
            }

            return attributedTextToAppend;
        }

    }
}
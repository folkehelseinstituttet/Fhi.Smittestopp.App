using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.iOS.Views.CustomSubclasses;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
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
            SetupStyling();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _gestureRecognizer = new UITapGestureRecognizer();

            PostAccessibilityNotificationAndReenableElement(BackButton, HeaderLabel);
        }

        void SetTexts()
        {
            HeaderLabel.SetAttributedText(SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER);
            InitLabel(IntroLabel, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_INTRO, 16, 20);
            InitLabel(Header1Label, FontType.FontBold, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_TITLE, 16, 20);
            InitLabel(Paragraph1Label1, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT, 16, 20);
            InitLabel(Paragraph1Label2, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT2, 16, 20);
            InitLabel(Header2Label, FontType.FontBold, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_TITLE, 16, 20);
            InitLabel(Paragraph2Label1, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT, 16, 20);
            InitLabel(Paragraph2Label2, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT2, 16, 20);
            InitLabel(Paragraph2Label3, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT3, 16, 20);
            InitLabel(Paragraph2Label4, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT4, 16, 20);
            InitLabel(Header3Label, FontType.FontBold, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_TITLE, 16, 20);
            InitLabel(Paragraph3Label1, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_CONTENT, 16, 20);
            InitLabel(Header4Label, FontType.FontBold, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_TITLE, 16, 20);
            InitLabel(Paragraph4Label1, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT, 16, 20);
            InitLabel(Paragraph4Label2, FontType.FontRegular, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT2, 16, 20);

            HeaderLabel.AccessibilityTraits = UIAccessibilityTrait.Header;
            Header1Label.AccessibilityTraits = UIAccessibilityTrait.Header;
            Header2Label.AccessibilityTraits = UIAccessibilityTrait.Header;
            Header3Label.AccessibilityTraits = UIAccessibilityTrait.Header;
            Header4Label.AccessibilityTraits = UIAccessibilityTrait.Header;

            NSMutableAttributedString text = new NSMutableAttributedString();
            text.Append(ApplyStylingToText(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT3, FontType.FontRegular));
            // Necessary to unify horizontal alignment with the rest of the text on the page
            Paragraph4Label3.TextContainerInset = UIEdgeInsets.Zero;
            Paragraph4Label3.TextContainer.LineFragmentPadding = 0;

            Paragraph4Label3.AccessibilityTraits = UIAccessibilityTrait.Link;
            Paragraph4Label3.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
            Paragraph4Label3.AttributedText = text;
            Paragraph4Label3.WeakLinkTextAttributes =
                new NSDictionary(
                    UIStringAttributeKey.ForegroundColor,
                    ColorHelper.LINK_COLOR,
                    UIStringAttributeKey.UnderlineStyle,
                    new NSNumber(1));

            //Accessibility
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;
            HeaderLabel.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER);
            Header1Label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_TITLE);
            Paragraph1Label1.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT);
            Paragraph1Label2.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT2);
            Header2Label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_TITLE);
            Paragraph2Label1.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT);
            Paragraph2Label2.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT2);
            Paragraph2Label3.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT3);
            Paragraph2Label4.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT4);
            Header3Label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_TITLE);
            Paragraph3Label1.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_CONTENT);
            Header4Label.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_TITLE);
            Paragraph4Label1.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT);
            Paragraph4Label2.AccessibilityLabel = AccessibilityUtils.RemovePoorlySpokenSymbolsString(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT2);
            Paragraph4Label3.AccessibilityIdentifier = "contentTextIdentifier";
            Paragraph4Label3.IsAccessibilityElement = true;
        }

        public void SetupStyling()
        {
            HeaderLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            IntroLabel.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header1Label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph1Label1.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph1Label2.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header2Label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph2Label1.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph2Label2.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph2Label3.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph2Label4.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header3Label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph3Label1.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Header4Label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph4Label1.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph4Label2.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            Paragraph4Label3.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }

        private NSMutableAttributedString ApplyStylingToText(string text, FontType fontType, double lineHeight = 1.28, int fontSize = 16, int maxFontSize = 22, bool isLink = false)
        {
            //Wraps the string containing HTML, with styling tags inorder to ensure correct font, color, size.
            string html = HtmlWrapper.HtmlForLabelWithText(text, 16, false, false, "#ffffff");

            //Defining document type as HTML, set string encoding and formats an attributed string with these.
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(html, NSStringEncoding.UTF8), documentAttributes, ref error);

            NSMutableAttributedString attributedTextToAppend = new NSMutableAttributedString(attributedString);
            attributedTextToAppend.DeleteRange(new NSRange(attributedTextToAppend.Length - 1, 1));
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle
            {
                LineHeightMultiple = new nfloat(lineHeight)
            };

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
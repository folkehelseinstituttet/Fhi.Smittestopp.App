using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public static class StyleUtil
    {
        public static string FontRegularName => "PTSans-Regular";
        public static string FontBoldName => "PTSans-Bold";
        public static string FontSemiBoldkName => "PTSans-Regular";
        public static string FontMediumkName => "PTSans-Caption";
        public static string FontMonospacedName => "Menlo-Regular";
        public static string FontItalicName => "PTSans-Italic";

        public enum FontType
        {
            FontRegular,
            FontBold,
            FontSemiBold,
            FontMedium,
            FontMonospaced,
            FontItalic
        }

        public static string GetFontName(FontType fontType)
        {
            switch (fontType)
            {
                case FontType.FontBold:
                    return FontBoldName;
                case FontType.FontMedium:
                    return FontMediumkName;
                case FontType.FontSemiBold:
                    return FontSemiBoldkName;
                case FontType.FontMonospaced:
                    return FontMonospacedName;
                case FontType.FontItalic:
                    return FontItalicName;
                default:
                    return FontRegularName;
            }
        }

        public static UIFont Font(FontType fontType = FontType.FontRegular, float fontSize = 14, float? maxFontSize = null)
        {
            UIFont font = UIFont.FromName(GetFontName(fontType), fontSize);

            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                return font;
            }

            if (maxFontSize == null)
            {
                return UIFontMetrics.DefaultMetrics.GetScaledFont(font);
            }

            return UIFontMetrics.DefaultMetrics.GetScaledFont(font, (System.nfloat)maxFontSize);
        }

        public static void InitButtonStyling(UIButton btn, string text)
        {
            btn.TitleLabel.AdjustsFontSizeToFitWidth = true;
            btn.SetTitle(text, UIControlState.Normal);
            btn.Superview.SetNeedsLayout();
            btn.BackgroundColor = ColorHelper.PRIMARY_COLOR;
            btn.Layer.CornerRadius = 10;
            btn.Font = Font(FontType.FontMedium, 18f, 24f);
            nfloat verticalSpacing = 8;
            nfloat horizontalSpacing = 24;
            btn.ContentEdgeInsets = new UIEdgeInsets(top: verticalSpacing, left: horizontalSpacing, bottom: verticalSpacing, right: horizontalSpacing);
            btn.SetTitleColor(ColorHelper.TEXT_COLOR_ON_PRIMARY, UIControlState.Normal);
        }

        public static void InitLanguageSelectionButtonStyling(UIButton btn)
        {
            btn.BackgroundColor = ColorHelper.DEFAULT_BACKGROUND_COLOR;
            btn.Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            btn.Layer.BorderWidth = 1;
            btn.SetTitleColor(ColorHelper.TEXT_COLOR_ON_BACKGROUND, UIControlState.Normal);
        }

        /// <summary>
        /// The button needs a height constraint of 30 in the storyboard.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="text"></param>
        public static void InitButtonSecondaryStyling(UIButton btn, string text)
        {
            btn.BackgroundColor = UIColor.Clear;
            btn.Font = Font(FontType.FontMedium, 18f, 24f);
            btn.SetTitleColor(ColorHelper.PRIMARY_COLOR, UIControlState.Normal);
            btn.SetTitle(text, UIControlState.Normal);
            btn.Layer.CornerRadius = 10;
            btn.BackgroundColor = UIColor.Clear;
            btn.Layer.BorderWidth = 1;
            btn.Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            btn.TitleLabel.Lines = 0;
            btn.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            
            btn.ContentEdgeInsets = new UIEdgeInsets(12, 12, 12, 12);
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle { HyphenationFactor = 1.0f };
            UIStringAttributes attributes = new UIStringAttributes { ParagraphStyle = paragraphStyle };
            btn.TitleLabel.AttributedText = new NSMutableAttributedString(text, attributes);
        }

        public static void InitLinkButtonStyling(UIButton btn, string text)
        {
            var attributedString = new NSMutableAttributedString(text);
            attributedString.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, attributedString.Length));
            btn.BackgroundColor = UIColor.Clear;
            btn.Font = Font(FontType.FontRegular, 16f, 22f);
            btn.SetTitleColor(ColorHelper.LINK_COLOR, UIControlState.Normal);
            btn.TintColor = ColorHelper.LINK_COLOR;
            btn.SetAttributedTitle(attributedString, UIControlState.Normal);
            btn.TitleLabel.Lines = 0;
            btn.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            btn.HorizontalAlignment = UIControlContentHorizontalAlignment.Leading;
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        public static void InitLabel(UILabel label, FontType fontType, string text, float fontSize, float maxFontSize)
        {
            label.Text = text;
            label.Font = Font(fontType, fontSize, maxFontSize);
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        public static void InitUnderlinedLabel(UILabel label, FontType fontType, string text, float fontSize, float maxFontSize)
        {
            var attributedString = new NSMutableAttributedString(text);
            attributedString.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, attributedString.Length));
            label.AttributedText = attributedString;
            label.Font = Font(fontType, fontSize, maxFontSize);
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="rawText"></param>
        /// <param name="lineHeight"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        /// <param name="alignment"></param>
        public static void InitLabelWithSpacing(UILabel label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize,bool centerAlignment = false, bool useHyphenation = false)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            if (centerAlignment)
            {
                paragraphStyle.Alignment = UITextAlignment.Center;
            }
            else
            {
                paragraphStyle.Alignment = LayoutUtils.GetTextAlignment();
            }
            if (useHyphenation)
            {
                paragraphStyle.HyphenationFactor = 1.0f;
            }
            NSMutableAttributedString text = new NSMutableAttributedString(rawText);
            NSRange range = new NSRange(0, rawText.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        public static void InitCenteredLabelWithSpacing(UILabel label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            paragraphStyle.Alignment = UITextAlignment.Center;
            NSMutableAttributedString text = new NSMutableAttributedString(rawText);
            NSRange range = new NSRange(0, rawText.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
        }

        public static void InitMonospacedLabel(UILabel label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize)
        {
            NSMutableAttributedString text = new NSMutableAttributedString(rawText);
            NSRange range = new NSRange(0, rawText.Length);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextAlignment = LayoutUtils.GetTextAlignment();
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="rawText"></param>
        /// <param name="lineHeight"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        public static void InitTextViewWithSpacing(UITextView label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            NSMutableAttributedString text = new NSMutableAttributedString(rawText);
            NSRange range = new NSRange(0, rawText.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextAlignment = LayoutUtils.GetTextAlignment();
        }

        public static void InitTextViewWithSpacingAndHTMLFormat(UITextView label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize)
        {
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(rawText, NSStringEncoding.UTF8), documentAttributes, ref error);
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            NSMutableAttributedString text = new NSMutableAttributedString(attributedString);
            NSRange range = new NSRange(0, attributedString.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            label.TextAlignment = LayoutUtils.GetTextAlignment();
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text and ensures embedded links are formatted correctly
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="AttributeText"></param>
        /// <param name="lineHeight"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        public static void InitTextViewWithSpacingAndUrl(UITextView label, FontType fontType, NSAttributedString AttributeText, double lineHeight, float fontSize, float maxFontSize)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            NSMutableAttributedString text = new NSMutableAttributedString(AttributeText);
            NSRange range = new NSRange(0, text.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;
            label.TextAlignment = LayoutUtils.GetTextAlignment();
        }

        /// <summary>
        /// Set maxFontSize for accessibility - large text and ensures HTML formatting is preserved
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="AttributeText"></param>
        /// <param name="lineHeight"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        /// <param name="alignment"></param>
        public static void InitLabekWithSpacingAndHTMLFormatting(UILabel label, FontType fontType, NSAttributedString AttributeText, double lineHeight, float fontSize, float maxFontSize, UITextAlignment alignment = UITextAlignment.Natural)
        {
            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            if (alignment == UITextAlignment.Natural)
            {
                paragraphStyle.Alignment = LayoutUtils.GetTextAlignment();
            }
            else
            {
                paragraphStyle.Alignment = alignment;
            }
            NSMutableAttributedString text = new NSMutableAttributedString(AttributeText);
            NSRange range = new NSRange(0, text.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;

        }

        /// <summary>
        /// Adds Accessibility label on the label using the raw text containing html
        /// </summary>
        /// <param name="label"></param>
        /// <param name="rawText"></param>
        public static void InitLabelAccessibilityTextWithHTMLFormat(UILabel label, string rawText)
        {
            //Defining attibutes inorder to format the embedded link
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(rawText, NSStringEncoding.UTF8), documentAttributes, ref error);

            NSMutableAttributedString text = new NSMutableAttributedString(attributedString);
            label.AccessibilityAttributedLabel = text;
        }

        /// <summary>
        /// Using HTML formating to style UILabel
        /// </summary>
        /// <param name="label"></param>
        /// <param name="rawText"></param>
        public static void InitLabelWithHTMLFormat(UILabel label, string rawText, FontType fontType = FontType.FontRegular)
        {
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(rawText, NSStringEncoding.UTF8), documentAttributes, ref error);

            //Ensuring text is resiezed correctly when font size is increased
            InitLabekWithSpacingAndHTMLFormatting(label, fontType, attributedString, 1.28, 16, 22);
            label.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            label.TextAlignment = LayoutUtils.GetTextAlignment();
        }


        /// <summary>
        /// Generates a running spinner and pins it to the center of the parentView.
        /// </summary>
        /// <param name="parentView"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static UIActivityIndicatorView ShowSpinner(UIView parentView, UIActivityIndicatorViewStyle style)
        {
            UIActivityIndicatorView spinner = AddSpinnerToView(parentView, style);
            CenterView(spinner, parentView);
            spinner.Color = ColorHelper.PRIMARY_COLOR;
            spinner.StartAnimating();

            return spinner;
        }

        /// <summary>
        /// Add a spinner to the parentView but without any constraints set yet.
        /// </summary>
        /// <param name="parentView"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static UIActivityIndicatorView AddSpinnerToView(UIView parentView, UIActivityIndicatorViewStyle style)
        {
            UIActivityIndicatorView spinner = new UIActivityIndicatorView(style);
            spinner.HidesWhenStopped = true;
            spinner.TranslatesAutoresizingMaskIntoConstraints = false;
            parentView.Add(spinner);

            return spinner;
        }

        public static void CenterView(UIView viewToCenter, UIView referenceView)
        {
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] {
                viewToCenter.CenterXAnchor.ConstraintEqualTo(referenceView.CenterXAnchor),
                viewToCenter.CenterYAnchor.ConstraintEqualTo(referenceView.CenterYAnchor)
            });
        }

        /// <summary>
        /// Embeds a UIView inside the UIbutton.
        /// </summary>
        /// <param name="viewToEmbed"></param>
        /// <param name="buttonToUse"></param>
        /// <returns></returns>
        public static void EmbedViewInsideButton(UIView viewToEmbed, UIButton buttonToUse)
        {
            UIStackView stackView = (viewToEmbed.Superview as UIStackView);

            stackView.AddArrangedSubview(buttonToUse);

            // Turn aff UserInteraction on the view, so the Button will catch the gestures
            viewToEmbed.UserInteractionEnabled = false;

            stackView.WillRemoveSubview(viewToEmbed);
            stackView.RemoveArrangedSubview(viewToEmbed);
            viewToEmbed.RemoveFromSuperview();
            viewToEmbed.WillMoveToSuperview(buttonToUse);
            buttonToUse.AddSubview(viewToEmbed);

            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] {
                viewToEmbed.LeadingAnchor.ConstraintEqualTo(viewToEmbed.Superview.LeadingAnchor),
                viewToEmbed.TrailingAnchor.ConstraintEqualTo(viewToEmbed.Superview.TrailingAnchor),
                viewToEmbed.TopAnchor.ConstraintEqualTo(viewToEmbed.Superview.TopAnchor),
                viewToEmbed.BottomAnchor.ConstraintEqualTo(viewToEmbed.Superview.BottomAnchor),
            });
            viewToEmbed.MovedToSuperview();

            stackView.SetNeedsLayout();
        }

        public static void FlashScrollIndicatorsInSubScrollViews(UIView[] subviews)
        {
            foreach (var view in subviews)
            {
                if (view is UIScrollView)
                {
                    var scrollView = view as UIScrollView;
                    scrollView.FlashScrollIndicators();
                }
            }
        }

        // <summary>
        /// Set maxFontSize for accessibility - large text
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fontType"></param>
        /// <param name="rawText"></param>
        /// <param name="lineHeight"></param>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        /// <param name="alignment"></param>
        public static void InitUITextViewWithSpacingAndUrl(UITextView label, FontType fontType, string rawText, double lineHeight, float fontSize, float maxFontSize, UITextAlignment alignment = UITextAlignment.Left)
        {
            //Defining attibutes inorder to format the embedded link
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(rawText, NSStringEncoding.UTF8), documentAttributes, ref error);

            NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineHeightMultiple = new nfloat(lineHeight);
            paragraphStyle.Alignment = alignment;
            NSMutableAttributedString text = new NSMutableAttributedString(attributedString);
            NSRange range = new NSRange(0, text.Length);
            text.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, range);
            text.AddAttribute(UIStringAttributeKey.Font, Font(fontType, fontSize, maxFontSize), range);
            label.AttributedText = text;

            label.TextColor = UIColor.White;
            label.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, "#FADC5D".ToUIColor(), UIStringAttributeKey.UnderlineStyle, new NSNumber(1));
        }
    } 
}

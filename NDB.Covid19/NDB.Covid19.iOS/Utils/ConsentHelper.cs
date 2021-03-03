using System.Collections.Generic;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.Models.ConsentViewModel;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Utils
{
    public static class ConsentHelper
    {

        private static void AddSpacerToStackView(UIStackView stackView)
        {
            UIView spacer = new UIView();
            spacer.HeightAnchor.ConstraintEqualTo(20).Active = true;
            stackView.AddArrangedSubview(spacer);
        }

        private static UITextView CreateTextViewWithLinks(string text, string accessibilityLabel)
        {
            UITextView textView = new UITextView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear
            };
            InitTextViewWithSpacingAndHTMLFormat(textView, FontType.FontRegular, text, 1.28, 16, 22);
            if (accessibilityLabel != null)
            {
                textView.AccessibilityLabel = accessibilityLabel;
            }
            textView.ContentInset = UIEdgeInsets.Zero;
            textView.TextContainerInset = UIEdgeInsets.Zero;
            textView.TextContainer.LineFragmentPadding = 0;
            textView.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;
            textView.ScrollEnabled = false;
            textView.Editable = false;
            textView.DataDetectorTypes = UIDataDetectorType.PhoneNumber | UIDataDetectorType.Link;

            return textView;
        }

        public static void SetConsentLabels(UIStackView stackView, List<ConsentSectionTexts> sections, UIButton privacyPolicyButton)
        {
            // We add each label manually to the stackView
            foreach (ConsentSectionTexts obj in sections)
            {
                if (obj.Title != null)
                {
                    UILabel titleLbl = new UILabel
                    {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Lines = 0,
                        TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND
                    };

                    InitLabel(titleLbl, FontType.FontBold, obj.Title, 16, 22);
                    titleLbl.AccessibilityTraits = UIAccessibilityTrait.Header;

                    stackView.AddArrangedSubview(titleLbl);
                }

                if (obj.Paragraph == ConsentViewModel.CONSENT_FOUR_PARAGRAPH)
                {
                    stackView.AddArrangedSubview(
                        CreateTextViewWithLinks(ConsentViewModel.CONSENT_FOUR_PARAGRAPH, obj.ParagraphAccessibilityText)
                    );
                    AddSpacerToStackView(stackView);
                }

                if (obj.Paragraph == ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_TWO)
                {
                    stackView.AddArrangedSubview(
                        CreateTextViewWithLinks(ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_TWO, obj.ParagraphAccessibilityText)
                    );
                    AddSpacerToStackView(stackView);
                }

                if (obj.Paragraph == ConsentViewModel.CONSENT_SIX_PARAGRAPH)
                {
                    UILabel italic_paragraph = new UILabel
                    {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Lines = 0,
                        TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND
                    };

                    InitLabel(italic_paragraph, FontType.FontItalic, obj.Paragraph, 16, 22);

                    stackView.AddArrangedSubview(italic_paragraph);

                    if (obj.ParagraphAccessibilityText != null)
                    {
                        InitLabelAccessibilityTextWithHTMLFormat(italic_paragraph, obj.ParagraphAccessibilityText);
                    }

                    AddSpacerToStackView(stackView);

                    if (obj.Paragraph == ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_THREE)
                    {
                        stackView.AddArrangedSubview(privacyPolicyButton);
                        AddSpacerToStackView(stackView);
                    }
                }


                if (obj.Paragraph != ConsentViewModel.CONSENT_SIX_PARAGRAPH
                    && obj.Paragraph != ConsentViewModel.CONSENT_FOUR_PARAGRAPH
                    && obj.Paragraph != ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_TWO)
                {
                    UILabel paragrapLbl = new UILabel
                    {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Lines = 0,
                        TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND
                    };
                    InitLabelWithHTMLFormat(paragrapLbl, obj.Paragraph);

                    if (obj.ParagraphAccessibilityText != null)
                    {
                        InitLabelAccessibilityTextWithHTMLFormat(paragrapLbl, obj.ParagraphAccessibilityText);
                    }

                    stackView.AddArrangedSubview(paragrapLbl);

                    AddSpacerToStackView(stackView);

                    if (obj.Paragraph == ConsentViewModel.CONSENT_FIVE_PARAGRAPH_SECTION_THREE)
                    {
                        stackView.AddArrangedSubview(privacyPolicyButton);
                        AddSpacerToStackView(stackView);
                    }
                }
            }
        }
    }
}

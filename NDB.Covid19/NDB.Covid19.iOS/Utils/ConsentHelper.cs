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

                    stackView.AddArrangedSubview(titleLbl);
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

                    stackView.AddArrangedSubview(italic_paragraph);

                    if (obj.Paragraph == ConsentViewModel.CONSENT_FIVE_PARAGRAPH)
                    {
                        stackView.AddArrangedSubview(privacyPolicyButton);
                        AddSpacerToStackView(stackView);
                    }
                }


                if (obj.Paragraph != ConsentViewModel.CONSENT_SIX_PARAGRAPH)
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

                    if (obj.Paragraph == ConsentViewModel.CONSENT_FIVE_PARAGRAPH)
                    {
                        stackView.AddArrangedSubview(privacyPolicyButton);
                        AddSpacerToStackView(stackView);
                    }
                }
            }
        }
    }
}

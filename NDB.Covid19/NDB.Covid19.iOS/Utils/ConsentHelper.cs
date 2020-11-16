using System.Collections.Generic;
using NDB.Covid19.ViewModels;
using UIKit;
using static NDB.Covid19.Models.ConsentViewModel;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Utils
{
    public static class ConsentHelper
    {


        public static void SetConsentLabels(UIStackView stackView, List<ConsentSectionTexts> sections, UIButton privacyPolicyButton)
        {
            // We add each label manually to the stackView
            foreach (ConsentSectionTexts obj in sections)
            {
                UILabel titleLbl = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Lines = 0,
                    TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND
                };

                InitLabel(titleLbl, FontType.FontBold, obj.Title, 16, 22);

                stackView.AddArrangedSubview(titleLbl);

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

                if (obj.Title == ConsentViewModel.CONSENT_SEVEN_TITLE)
                {
                    stackView.AddArrangedSubview(privacyPolicyButton);
                }
            }
        }
    }
}

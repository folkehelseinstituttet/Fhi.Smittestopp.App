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
                UILabel titleLbl = new UILabel();
                titleLbl.TranslatesAutoresizingMaskIntoConstraints = false;
                titleLbl.TextColor = UIColor.White;
                titleLbl.Lines = 0;

                InitLabel(titleLbl, FontType.FontBold, obj.Title, 16, 22);

                stackView.AddArrangedSubview(titleLbl);

                UILabel paragrapLbl = new UILabel();
                paragrapLbl.TranslatesAutoresizingMaskIntoConstraints = false;
                paragrapLbl.TextColor = UIColor.White;
                paragrapLbl.Lines = 100;
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

using System.Collections.Generic;
using UIKit;
using static NDB.Covid19.ViewModels.ConsentViewModel;
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

                StyleUtil.InitLabel(titleLbl, FontType.FontBold, obj.Title, 16, 22);

                stackView.AddArrangedSubview(titleLbl);

                UILabel paragrapLbl = new UILabel();
                paragrapLbl.TranslatesAutoresizingMaskIntoConstraints = false;
                paragrapLbl.TextColor = UIColor.White;
                paragrapLbl.Lines = 100;
                StyleUtil.InitLabelWithHTMLFormat(paragrapLbl, obj.Paragraph);

                if (obj.ParagraphAccessibilityText != null)
                {
                    StyleUtil.InitLabelAccessibilityTextWithHTMLFormat(paragrapLbl, obj.ParagraphAccessibilityText);
                }

                stackView.AddArrangedSubview(paragrapLbl);

                if (obj.Title == CONSENT_SEVEN_TITLE)
                {
                    stackView.AddArrangedSubview(privacyPolicyButton);
                }
            }
        }
    }
}

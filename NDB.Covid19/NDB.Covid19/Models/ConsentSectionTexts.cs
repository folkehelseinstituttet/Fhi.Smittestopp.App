namespace NDB.Covid19.Models
{
    public partial class ConsentViewModel
    {
        public class ConsentSectionTexts
        {
            public string Title { get; private set; }
            public string Paragraph { get; private set; }
            public string ParagraphAccessibilityText { get; private set; }

            public ConsentSectionTexts(string title, string paragraph, string paragraphAccessibilityText)
            {
                Title = title;
                Paragraph = paragraph;
                ParagraphAccessibilityText = paragraphAccessibilityText;
            }
        }

    }
}

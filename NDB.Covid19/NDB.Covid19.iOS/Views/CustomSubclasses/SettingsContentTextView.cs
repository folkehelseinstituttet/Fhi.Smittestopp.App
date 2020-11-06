using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class SettingsContentTextView : UITextView
    {
        public SettingsContentTextView (IntPtr handle) : base (handle)
        {
            BackgroundColor = UIColor.Clear;
        }

        void Setup()
        {
            //Defining attibutes inorder to format the embedded link
            NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
            documentAttributes.StringEncoding = NSStringEncoding.UTF8;
            NSError error = null;
            NSAttributedString attributedString = new NSAttributedString(NSData.FromString(Text, NSStringEncoding.UTF8), documentAttributes, ref error);

            //Ensuring text is resiezed correctly when font size is increased
            StyleUtil.InitTextViewWithSpacingAndUrl(this, FontType.FontRegular, attributedString, 1.28, 16, 22);
            TextColor = UIColor.White;
            //ForegroundColor sets the color of the links. UnderlineStyle determins if the link is underlined, 0 without underline 1 with underline.
            WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, "#FADC5D".ToUIColor(), UIStringAttributeKey.UnderlineStyle, new NSNumber(1));
            
        }

        /// <summary>
        /// Style the textview to
        /// </summary>
        /// <param name="text">The string needs to be formattet as HTML</param>
        /// <param name="scrollEnabled"></param>
        public void SetAttributedText(string text)
        {
            Text = text;
            Setup();
        }

    
    }
}
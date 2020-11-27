using CommonServiceLocator;
using Foundation;
using NDB.Covid19.Interfaces;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public class OpenTextViewUrlInWebviewDelegate : UITextViewDelegate
    {
        private UIViewController Owner { get; set; }

        public OpenTextViewUrlInWebviewDelegate(UIViewController owner)
        {
            Owner = owner;
        }

        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(URL);
            return false;
        }
    }
}
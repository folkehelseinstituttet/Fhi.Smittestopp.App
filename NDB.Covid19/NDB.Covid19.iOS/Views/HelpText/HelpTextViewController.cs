using System;
using NDB.Covid19.iOS.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views.HelpText
{
    public partial class HelpTextViewController : UIViewController
    {
        public string TitleText { get; set; }
        public string Content { get; set; }
        public string OkBtnText { get; set; }

        public HelpTextViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TitleLabel.Text = TitleText;
            BodyText.Text = Content;
            StyleUtil.InitButtonStyling(OkBtn, OkBtnText);
        }

        partial void OkBtn_TouchUpInside(UIButton sender)
        {
            DismissViewController(true, null);
        }
    }
}
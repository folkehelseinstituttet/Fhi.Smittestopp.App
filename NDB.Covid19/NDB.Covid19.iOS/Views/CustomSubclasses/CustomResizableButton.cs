using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    [Register("CustomResizableButton")]
    public class CustomResizableButton : UIButton
    {
        public CustomResizableButton(IntPtr handle) : base(handle) { }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                CGSize labelSize = TitleLabel.IntrinsicContentSize;
                CGSize buttonSize = new CGSize(labelSize.Width + TitleEdgeInsets.Left + TitleEdgeInsets.Right,
                    labelSize.Height + TitleEdgeInsets.Top + TitleEdgeInsets.Bottom + 10);

                return buttonSize;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            TitleLabel.PreferredMaxLayoutWidth = TitleLabel.Frame.Size.Width;
        }
    }
}

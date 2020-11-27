using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    [Register("CustomBackButton")]
    public class CustomBackButton : UIButton
    {

        public nfloat ExtendedArea { get; set; } = 34;

        public CustomBackButton(IntPtr handle) : base(handle)
        {
            var imageView = this.ImageView;
            imageView.UserInteractionEnabled = false;
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            var extendedArea = new CGRect
            {
                X = this.Bounds.Location.X - ExtendedArea,
                Y = this.Bounds.Location.Y - ExtendedArea,
                Width = this.Bounds.Size.Width + ExtendedArea * 2,
                Height = this.Bounds.Size.Width + ExtendedArea * 2
            };

            return extendedArea.Contains(point);
        }
    }
}

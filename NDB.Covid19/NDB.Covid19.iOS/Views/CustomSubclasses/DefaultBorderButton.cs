using System;
using CoreGraphics;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.iOS.Utils.StyleUtil;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class DefaultBorderButton : UIButton, IDisposable
    {
        public DefaultBorderButton(IntPtr handle) : base(handle)
        {
            Font = Font(FontType.FontSemiBold, 18f, 24f);
            SetTitleColor(UIColor.White, UIControlState.Normal);
            BackgroundColor = UIColor.Clear;
            TitleLabel.AdjustsFontSizeToFitWidth = true;
            SetTitleColor(UIColor.Clear, UIControlState.Selected);
            Layer.BorderWidth = 1;
            Layer.BorderColor = UIColor.White.CGColor;
            Layer.CornerRadius = Layer.Frame.Height / 2;
            TintColor = UIColor.Clear;
        }

        UIActivityIndicatorView _spinner;

        public override void SetTitle(string title, UIControlState forState)
        {
            base.SetTitle(title, forState);
            Superview.SetNeedsLayout();
            Layer.CornerRadius = Layer.Frame.Height / 2;
        }

        // Ensures that the button height scales with the amount of lines in the button text label
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
            this.TitleLabel.PreferredMaxLayoutWidth = this.TitleLabel.Frame.Size.Width;
        }

        public void ShowSpinner(UIView parentView, UIActivityIndicatorViewStyle style)
        {
            _spinner = StyleUtil.AddSpinnerToView(parentView, style);
            StyleUtil.CenterView(_spinner, this);
            
            Selected = true;
            _spinner.StartAnimating();
        }

        public void HideSpinner()
        {
            _spinner?.RemoveFromSuperview();
            _spinner = null;
            Selected = false;
        }

        public new void Dispose()
        {
            HideSpinner();
            base.Dispose();
        }
    }
}
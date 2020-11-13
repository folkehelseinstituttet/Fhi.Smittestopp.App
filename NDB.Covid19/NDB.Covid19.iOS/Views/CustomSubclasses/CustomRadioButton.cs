using System;
using CoreGraphics;
using Foundation;
using NDB.Covid19.iOS.Utils;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    [Register("CustomRadioButton")]
    public partial class CustomRadioButton : UIButton, IDisposable
    {
        const int _borderWidth = 2;
        int _padding => _borderWidth * 3;
        UIView _innerView;

        bool _selected;
        new public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateState();
            }
        }

        public CustomRadioButton(IntPtr handle) : base(handle)
        {
            BackgroundColor = UIColor.Clear;
            Layer.BorderWidth = _borderWidth;
            Layer.BorderColor = ColorHelper.PRIMARY_COLOR.CGColor;
            TouchUpInside += OnTouchUpInside;

            AddInnerView();
            UpdateState();
        }

        
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            SetTitle("", UIControlState.Normal);
            UpdateCornerRadius();
        }

        void UpdateCornerRadius()
        {
            Layer.CornerRadius = Layer.Frame.Height / 2;
            _innerView.Layer.CornerRadius = _innerView.Layer.Frame.Height / 2;
        }

        public new void Dispose()
        { 
            TouchUpInside -= OnTouchUpInside;
            _innerView.RemoveFromSuperview();
            _innerView = null;
            base.Dispose();
        }

        void OnTouchUpInside(object sender, EventArgs e)
        {
            Selected = !Selected;
        }

        void UpdateState()
        {
            _innerView.Hidden = !Selected;

            if (Selected)
            {
                AccessibilityTraits |= UIAccessibilityTrait.Selected;
            }
            else
            {
                AccessibilityTraits &= ~UIAccessibilityTrait.Selected;
            }
        }

        void AddInnerView()
        {
            _innerView = new UIView();
            
            _innerView.TranslatesAutoresizingMaskIntoConstraints = false;

            _innerView.BackgroundColor = ColorHelper.PRIMARY_COLOR;
            _innerView.UserInteractionEnabled = false;

            AddSubview(_innerView);
            SetInnerViewConstraints();
        }

        void SetInnerViewConstraints()
        {
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] {
                    _innerView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, _padding),
                    _innerView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -_padding),
                    _innerView.TopAnchor.ConstraintEqualTo(TopAnchor, _padding),
                    _innerView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -_padding),
                });

            SetNeedsLayout();
        }
    }
}
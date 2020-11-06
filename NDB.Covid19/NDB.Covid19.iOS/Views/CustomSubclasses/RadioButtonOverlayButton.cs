using System;
using UIKit;

namespace NDB.Covid19.iOS.Views.CustomSubclasses
{
    public partial class RadioButtonOverlayButton : UIButton, IDisposable
    {
        public RadioButtonOverlayButton(IntPtr handle) : base(handle)
        {
            TouchUpInside += OnTouchUpInside;

            UpdateState();
        }

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

        public new void Dispose()
        {
            TouchUpInside -= OnTouchUpInside;
            base.Dispose();
        }

        void OnTouchUpInside(object sender, EventArgs e)
        {
            Selected = !Selected;
        }

        void UpdateState()
        {
            if (Selected)
            {
                AccessibilityTraits |= UIAccessibilityTrait.Selected;
            }
            else
            {
                AccessibilityTraits &= ~UIAccessibilityTrait.Selected;
            }
        }
    }
}

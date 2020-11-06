using System;
using System.Linq;
using Foundation;
using NDB.Covid19.iOS.Utils;
using ObjCRuntime;
using UIKit;

namespace NDB.Covid19.iOS.Views.HelpCustomView
{
    public partial class HelpCustomView : UIView
    {
        public HelpCustomView(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        /// This method instantiates and add the HelpCustomView to the parentView provided.
        /// </summary>
        /// <param name="parentView"></param>
        /// <returns></returns>
        public static HelpCustomView Create(UIView parentView, string displayText, string buttonText, UIView initiaterView)
        {
            NSArray arr = NSBundle.MainBundle.LoadNib(nameof(HelpCustomView), null, null);
            HelpCustomView v = Runtime.GetNSObject<HelpCustomView>(arr.ValueAt(0));
            v._displayText = displayText;
            v._buttonText = buttonText;
            v._initiaterView = initiaterView;
            HelpCustomView.EmbedInParentView(v, parentView);

            return v;
        }

        static void EmbedInParentView(UIView childView, UIView parentView)
        {
            childView.WillMoveToSuperview(parentView);
            parentView.AddSubview(childView);
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] {
                childView.LeadingAnchor.ConstraintEqualTo(parentView.LeadingAnchor),
                childView.TrailingAnchor.ConstraintEqualTo(parentView.TrailingAnchor),
                childView.TopAnchor.ConstraintEqualTo(parentView.TopAnchor),
                childView.BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor),
            });
            childView.MovedToSuperview();
        }

        string _displayText;
        string _buttonText;
        UIView _initiaterView;
        bool _eventHandlersSet = false;
        bool _stylingAlreadySet;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TranslatesAutoresizingMaskIntoConstraints = false;

            BackgroundView.IsAccessibilityElement = true;
            BackgroundView.AccessibilityLabel = "Luk";
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (!_stylingAlreadySet)
            {
                SetupTextAndStyling();
            }
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);

            if (newsuper != null && !_eventHandlersSet)
            {
                SetupTapRecognizerOnBackgroundView();

                _eventHandlersSet = true;
            }
        }

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();

            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, TextLabel);
        }

        public override void RemoveFromSuperview()
        {
            base.RemoveFromSuperview();

            RemoveGestureRecognizers();
            _eventHandlersSet = false;
        }

        void Close()
        {
            Superview.WillRemoveSubview(this);
            RemoveFromSuperview();
            UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, _initiaterView);
        }

        void SetupTapRecognizerOnBackgroundView()
        {
            UITapGestureRecognizer tap = new UITapGestureRecognizer(Close);
            BackgroundView.AddGestureRecognizer(tap);
        }

        void RemoveGestureRecognizers()
        {
            if (BackgroundView.GestureRecognizers != null && Enumerable.Any<UIGestureRecognizer>(BackgroundView.GestureRecognizers))
            {
                foreach (UIGestureRecognizer gesture in BackgroundView.GestureRecognizers)
                {
                    BackgroundView.RemoveGestureRecognizer(gesture);
                }
            }
        }

        void SetupTextAndStyling()
        {
            TextContainerView.Layer.CornerRadius = 12;
            TextLabel.Text = _displayText;
            TextLabel.Font = StyleUtil.Font(StyleUtil.FontType.FontRegular, 17);

            CloseBtn.SetTitle(_buttonText, UIControlState.Normal);
            CloseBtn.Font = StyleUtil.Font(StyleUtil.FontType.FontBold, 17);

            _stylingAlreadySet = true;
        }

        partial void OnCloseBtnTapped(UIButton sender)
        {
            Close();
        }
    }
}
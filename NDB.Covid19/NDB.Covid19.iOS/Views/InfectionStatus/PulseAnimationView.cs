using UIKit;
using CoreGraphics;
using CoreAnimation;
using Foundation;

namespace NDB.Covid19.iOS.Views.InfectionStatus
{
    public class PulseAnimationView: UIView
    {
        private CAAnimationGroup _animationGroup;

        public PulseAnimationView()
        {
            _animationGroup = CreateAnimation();
            Layer.AddAnimation(_animationGroup, "pulse");
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillEnterForegroundNotification, AnimationWillEnterForeground);
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidEnterBackgroundNotification, AnimationDidEnterBackground);
        }

        public override void Draw(CGRect rect)
        {
            Layer.CornerRadius = Bounds.Height / 2;
            Layer.MasksToBounds = true;
        }

        private CAAnimationGroup CreateAnimation()
        {
            double duration = 3.5;
            CABasicAnimation scaleAnimation = CABasicAnimation.FromKeyPath("transform.scale.xy");
            scaleAnimation.SetFrom(NSNumber.FromFloat(1f));
            scaleAnimation.SetTo(NSNumber.FromFloat(1.4f));
            scaleAnimation.Duration = duration;

            CAKeyFrameAnimation opacityAnimation = CAKeyFrameAnimation.FromKeyPath("opacity");
            opacityAnimation.KeyTimes = new NSNumber[] { NSNumber.FromFloat(0), NSNumber.FromFloat(0.3f), NSNumber.FromFloat(1) };
            opacityAnimation.Values = new NSNumber[] { NSNumber.FromFloat(0.4f), NSNumber.FromFloat(0.8f), NSNumber.FromFloat(0) };
            opacityAnimation.Duration = duration;

            CAAnimationGroup group = new CAAnimationGroup();
            group.RepeatCount = float.MaxValue;
            group.Duration = duration;
            group.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
            group.Animations = new CAAnimation[] { scaleAnimation, opacityAnimation };
            return group;
        }

        public void RestartAnimation()
        {
            Layer.RemoveAnimation("pulse");
            Layer.AddAnimation(_animationGroup, "pulse");
        }

        private void AnimationWillEnterForeground(NSNotification notification)
        {
            Layer.AddAnimation(_animationGroup, "pulse");
        }

        private void AnimationDidEnterBackground(NSNotification notification)
        {
            Layer.RemoveAnimation("pulse");
        }

    }
}

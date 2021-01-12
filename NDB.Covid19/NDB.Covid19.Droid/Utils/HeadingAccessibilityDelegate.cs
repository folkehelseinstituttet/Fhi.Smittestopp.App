using System;
using Android.Views;
using Android.Views.Accessibility;

namespace NDB.Covid19.Droid.Utils
{
    public class HeadingAccessibilityDelegate : View.AccessibilityDelegate
    {
        public override void OnInitializeAccessibilityNodeInfo(View host, AccessibilityNodeInfo info)
        {
            base.OnInitializeAccessibilityNodeInfo(host, info);
            info.Heading = true;
        }
    }
}

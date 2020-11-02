using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.ViewPager.Widget;

namespace NDB.Covid19.Droid.Views.Welcome
{
    public class NonSwipeableViewPager : ViewPager
    {
        bool IsEnabled;
        public NonSwipeableViewPager(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer) { }

        public NonSwipeableViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            IsEnabled = true;
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            return IsEnabled
                ? base.OnTouchEvent(e)
                : false;
        }
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return IsEnabled
                ? base.OnInterceptTouchEvent(ev)
                : false;
        }
        public void SetPagingEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
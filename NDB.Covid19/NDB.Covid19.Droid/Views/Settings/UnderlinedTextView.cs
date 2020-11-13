using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using AndroidX.AppCompat.Widget;

namespace NDB.Covid19.Droid.Views.Settings
{
    public class UnderlinedTextView : AppCompatTextView
    {
        private Rect _lineBoundsRect;
        private Paint _underlinePaint;
        private float _strokeWidth;
        private Color _color;
        private float _margin;
        
        protected UnderlinedTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public UnderlinedTextView(Context context) : base(context)
        {
            init(context, null, 0);
        }

        public UnderlinedTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init(context, attrs, 0);
        }

        public UnderlinedTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {
            init(context, attrs, defStyleAttr);
        }

        private void init(Context context, IAttributeSet attributeSet, int defStyle)
        {
            if (attributeSet == null) return;
            float density = context.Resources.DisplayMetrics.Density;

            TypedArray typedArray = context.ObtainStyledAttributes(attributeSet, Resource.Styleable.UnderlinedTextView, defStyle, 0);
            _color = typedArray.GetColor(Resource.Styleable.UnderlinedTextView_underlineColor, new Color(0,0,0));
            _strokeWidth = typedArray.GetDimension(Resource.Styleable.UnderlinedTextView_underlineWidth, density * 2);
            _margin = typedArray.GetDimension(Resource.Styleable.UnderlinedTextView_underlineMarginTop, density * 2);
            typedArray.Recycle();

            _lineBoundsRect = new Rect();
            _underlinePaint = new Paint();
            _underlinePaint.SetStyle(Android.Graphics.Paint.Style.Stroke);
            _underlinePaint.Color = _color;
            _underlinePaint.StrokeWidth = _strokeWidth;
        }

        public int getUnderLineColor()
        {
            return _underlinePaint.Color;
        }

        public void setUnderLineColor(Color mColor)
        {
            _underlinePaint.Color = mColor;
            Invalidate();
        }

        public float getUnderlineWidth()
        {
            return _strokeWidth;
        }

        public void setUnderlineWidth(float width)
        {
            _strokeWidth = width;
            Invalidate();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int h = MeasuredHeight + (int)_margin;
            SetMeasuredDimension(widthMeasureSpec, h);
        }

        protected override void OnDraw(Canvas canvas)
        {
            int count = LineCount;

            Layout layout = Layout;
            float x_start, x_stop, x_diff;
            int firstCharInLine, lastCharInLine;

            for (int i = 0; i < count; i++)
            {
                int baseline = GetLineBounds(i, _lineBoundsRect);
                firstCharInLine = layout.GetLineStart(i);
                lastCharInLine = layout.GetLineEnd(i);

                x_start = layout.GetPrimaryHorizontal(firstCharInLine);
                x_diff = layout.GetPrimaryHorizontal(firstCharInLine + 1) - x_start;
                x_stop = layout.GetPrimaryHorizontal(lastCharInLine - 1) + x_diff;

                float y = baseline + _strokeWidth + _margin;
                canvas.DrawLine(x_start, y, x_stop, y, _underlinePaint);
            }

            base.OnDraw(canvas);
        }
    }
}
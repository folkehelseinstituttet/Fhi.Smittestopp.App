using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using Java.Lang;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.Settings
{
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    class SettingsHowItWorksActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER;
            SetContentView(Resource.Layout.settings_how_it_works);
            Init();
        }

        void Init()
        {
            ImageButton backButton = FindViewById<ImageButton>(Resource.Id.arrow_back);
            backButton.ContentDescription = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView textField = FindViewById<TextView>(Resource.Id.settings_how_it_works_text);
            TextView titleField = FindViewById<TextView>(Resource.Id.settings_how_it_works_title);

            titleField.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER;
            titleField.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            textField.TextFormatted = HtmlCompat.FromHtml(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT,
                HtmlCompat.FromHtmlModeLegacy);
            LinkUtil.LinkifyTextView(textField);
            FormatLink(textField);

            backButton.Click += new SingleClick((sender, args) => Finish()).Run;
        }

        private void FormatLink(TextView textView)
        {
            SpannableStringBuilder spannable = new SpannableStringBuilder(textView.TextFormatted);
            Object[] spans =
                spannable.GetSpans(
                    0,
                    spannable.Length(),
                    Class.FromType(typeof(URLSpan)));
            foreach (URLSpan span in spans)
            {
                int start = spannable.GetSpanStart(span);
                int end = spannable.GetSpanEnd(span);
                spannable.RemoveSpan(span);
                URLSpanWithUnderline newSpan = new URLSpanWithUnderline(span.URL);
                spannable.SetSpan(newSpan, start, end, 0);
            }

            textView.TextFormatted = spannable;
        }

        private class URLSpanWithUnderline : URLSpan
        {
            public URLSpanWithUnderline(string url) : base(url)
            {
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.UnderlineText = true;
                ds.Color = "#32345C".ToColor();
            }
        }
    }
}
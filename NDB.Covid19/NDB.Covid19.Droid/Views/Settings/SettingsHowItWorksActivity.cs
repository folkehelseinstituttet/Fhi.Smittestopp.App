using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Views;
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
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
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
            backButton.ContentDescription = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;

            TextView titleField = FindViewById<TextView>(Resource.Id.settings_how_it_works_title);
            TextView intro = FindViewById<TextView>(Resource.Id.settings_how_it_works_intro);
            TextView heading1 = FindViewById<TextView>(Resource.Id.settings_how_it_works_heading1);
            TextView paragraph1textView1 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph1textView1);
            TextView paragraph1textView2 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph1textView2);
            TextView heading2 = FindViewById<TextView>(Resource.Id.settings_how_it_works_heading2);
            TextView paragraph2textView1 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph2textView1);
            TextView paragraph2textView2 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph2textView2);
            TextView paragraph2textView3 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph2textView3);
            TextView paragraph2textView4 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph2textView4);
            TextView heading3 = FindViewById<TextView>(Resource.Id.settings_how_it_works_heading3);
            TextView paragraph3textView1 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph3textView1);
            TextView heading4 = FindViewById<TextView>(Resource.Id.settings_how_it_works_heading4);
            TextView paragraph4textView1 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph4textView1);
            TextView paragraph4textView2 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph4textView2);
            TextView paragraph4textView3 = FindViewById<TextView>(Resource.Id.settings_how_it_works_paragraph4textView3);

            titleField.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER;
            intro.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_INTRO;
            heading1.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_TITLE;
            paragraph1textView1.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT;
            paragraph1textView2.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_1_CONTENT2;
            heading2.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_TITLE;
            paragraph2textView1.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT;
            paragraph2textView2.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT2;
            paragraph2textView3.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT3;
            paragraph2textView4.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_2_CONTENT4;
            heading3.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_TITLE;
            paragraph3textView1.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_3_CONTENT;
            heading4.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_TITLE;
            paragraph4textView1.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT;
            paragraph4textView2.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT2;

            paragraph4textView3.TextFormatted = HtmlCompat.FromHtml(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_CONTENT3,
                HtmlCompat.FromHtmlModeLegacy);

            titleField.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            LinkUtil.LinkifyTextView(paragraph4textView3);
            FormatLink(paragraph4textView3);

            backButton.Click += new SingleClick((sender, args) => Finish()).Run;

            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();
            backButton.SetBackgroundResource(LayoutUtils.GetBackArrow());
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
            }
        }
    }
}
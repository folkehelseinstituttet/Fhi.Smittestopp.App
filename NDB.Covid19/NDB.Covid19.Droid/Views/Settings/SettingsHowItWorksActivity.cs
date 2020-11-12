using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using Java.Lang;
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
            Button backButton = FindViewById<Button>(Resource.Id.arrow_back);
            backButton.ContentDescription = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;

            TextView textField = FindViewById<TextView>(Resource.Id.settings_how_it_works_text);
            TextView titleField = FindViewById<TextView>(Resource.Id.settings_how_it_works_title);
            TextView linkField = FindViewById<TextView>(Resource.Id.settings_how_it_works_link);

            titleField.Text = SettingsPage2ViewModel.SETTINGS_PAGE_2_HEADER;
            textField.TextFormatted = HtmlCompat.FromHtml(SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT, HtmlCompat.FromHtmlModeLegacy);

            string linkString = $"<a href=\"{SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_LINK}\">{SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_LINK_TEXT}</a>";
            linkField.TextFormatted = HtmlCompat.FromHtml(linkString, HtmlCompat.FromHtmlModeLegacy);
            FormatLink(linkField);
            linkField.SetOnClickListener(new OnClickListener(this, SettingsPage2ViewModel.SETTINGS_PAGE_2_CONTENT_TEXT_PARAGRAPH_4_LINK));

            backButton.Click += new SingleClick((sender, args) => Finish()).Run;
        }

        private void FormatLink (TextView textView) {
            SpannableStringBuilder spannable = new SpannableStringBuilder (textView.TextFormatted);
            Object[] spans = spannable.GetSpans (0, spannable.Length (), Class.FromType (typeof (URLSpan)));
            foreach (URLSpan span in spans) {
                int start = spannable.GetSpanStart (span);
                int end = spannable.GetSpanEnd (span);
                spannable.RemoveSpan(span);
                URLSpanNoUnderline newSpan = new URLSpanNoUnderline (span.URL);
                spannable.SetSpan(newSpan, start, end, 0);
            }
            textView.TextFormatted = spannable;
        }

        class OnClickListener : Object, View.IOnClickListener
        {
            private SettingsHowItWorksActivity _self;
            private string _link;

            public OnClickListener(SettingsHowItWorksActivity self, string link)
            {
                _self = self;
                _link = link;
            }
            public void OnClick(View v)
            {
                Intent browserIntent = new Intent(Intent.ActionView);
                browserIntent.SetData(Uri.Parse(_link));
                _self.StartActivity(browserIntent);
            }
        }

        private class URLSpanNoUnderline : URLSpan {
            public URLSpanNoUnderline (string url) : base (url) {
            }

            public override void UpdateDrawState (TextPaint ds) {
                base.UpdateDrawState (ds);
                ds.UnderlineText = false;
                ds.Color = "#32345C".ToColor();
            }
        }
    }
}
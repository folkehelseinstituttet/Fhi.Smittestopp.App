using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow.ErrorActivities
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.FullUser,
        LaunchMode = LaunchMode.SingleTop)]
    class GeneralErrorActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.error_page);
            Init();
        }

        void Init()
        {
            Bundle textFieldsBundle = Intent.Extras;
            string titleText = textFieldsBundle.GetString("title");
            string descriptionText = textFieldsBundle.GetString("description");
            string buttonText = textFieldsBundle.GetString("button");

            TextView subtitleTextView = FindViewById<TextView>(Resource.Id.error_subtitle);
            if (textFieldsBundle.ContainsKey("subtitle"))
            {
                string subtitleText = textFieldsBundle.GetString("subtitle");
                subtitleTextView.Text = subtitleText;
                subtitleTextView.ContentDescription = subtitleText;
                subtitleTextView.Visibility = ViewStates.Visible;
            }
            else
            {
                subtitleTextView.Visibility = ViewStates.Gone;
            }

            TextView errorTitle = FindViewById<TextView>(Resource.Id.error_title);
            errorTitle.Text = titleText;
            errorTitle.ContentDescription = titleText;
            errorTitle.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());

            TextView errorDescription = FindViewById<TextView>(Resource.Id.error_description);
            ISpanned formattedDescription = HtmlCompat.FromHtml(descriptionText, HtmlCompat.FromHtmlModeLegacy);
            errorDescription.TextFormatted = formattedDescription;
            errorDescription.ContentDescriptionFormatted = formattedDescription;
            errorDescription.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

            ViewGroup close = FindViewById<ViewGroup>(Resource.Id.close_cross_btn);
            close.Click += new SingleClick((o, ev) => {
                NavigationHelper.GoToResultPageAndClearTop(this);
            }).Run;
            close.ContentDescription = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
            Button button = FindViewById<Button>(Resource.Id.error_button);
            button.Text = buttonText;
            button.ContentDescription = buttonText;
            button.Click += new SingleClick((o, ev) => {
                NavigationHelper.GoToResultPageAndClearTop(this);
            }).Run;

            this.Title = textFieldsBundle.GetString("title");
        }

        public override void OnBackPressed()
        {
            Intent intent = NavigationHelper.GetStartPageIntent(this);

            if (intent != null)
            {
                intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                StartActivity(intent);
            }
        }
    }
}
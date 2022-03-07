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
using NDB.Covid19.Enums;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;

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

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing General Error", null, GetCorrelationId());
        }

        protected override void OnPause()
        {
            base.OnPause();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is leaving General Error Page", null, GetCorrelationId());
        }

        void Init()
        {
            View rootView = Window.DecorView.RootView;
            rootView.LayoutDirection = LayoutUtils.GetLayoutDirection();

            Bundle textFieldsBundle = Intent.Extras;
            string titleText = textFieldsBundle.GetString("title");
            string descriptionText = textFieldsBundle.GetString("description");
            string buttonText = textFieldsBundle.GetString("button");
            string continueButtonText = textFieldsBundle.GetString("continuebutton");
            bool canContinueReportingInfected = false;

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

            if (textFieldsBundle.ContainsKey("canContinueReportingInfected"))
            {
                canContinueReportingInfected = textFieldsBundle.GetBoolean("canContinueReportingInfected");
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

            if (canContinueReportingInfected)
            {
                Button registerSelftestButton = FindViewById<Button>(Resource.Id.continue_button);
                registerSelftestButton.Visibility = ViewStates.Visible;
                registerSelftestButton.Text = continueButtonText;
                registerSelftestButton.ContentDescription = ErrorViewModel.REGISTER_CONTINUE_WITH_SELF_TEST_BUTTON_TEXT;
                registerSelftestButton.Click += new SingleClick((o, ev) =>
                {
                    IsReportingSelfTest = true;
                    Intent intent = new Intent(this, typeof(QuestionnairePageActivity));
                    StartActivity(intent);
                }).Run;
            }
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
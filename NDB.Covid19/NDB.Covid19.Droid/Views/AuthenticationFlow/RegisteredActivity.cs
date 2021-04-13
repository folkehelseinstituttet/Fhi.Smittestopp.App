using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using NDB.Covid19.Utils;
using NDB.Covid19.Interfaces;
using Xamarin.Essentials;
using static NDB.Covid19.ViewModels.QuestionnaireViewModel;
using static NDB.Covid19.Droid.Utils.StressUtils;
using static NDB.Covid19.PersistedData.LocalPreferencesHelper;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Enums;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow
{
    [Activity(Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.FullUser, LaunchMode = LaunchMode.SingleTop)]
    class RegisteredActivity : AppCompatActivity
    {
        private Button _closeButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = REGISTER_QUESTIONAIRE_RECEIPT_HEADER;
            SetContentView(Resource.Layout.registered_page);
            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            LogUtils.LogMessage(LogSeverity.INFO, "The user is seeing Registered", null, GetCorrelationId());
        }

        private void Init()
        {
            _closeButton = FindViewById<Button>(Resource.Id.close_cross_btn);
            _closeButton.ContentDescription = REGISTER_QUESTIONAIRE_ACCESSIBILITY_CLOSE_BUTTON_TEXT;
            _closeButton.Click +=
                new SingleClick((o, ev) => GoToInfectionStatusActivity()).Run;

            TextView registeredTitle = FindViewById<TextView>(Resource.Id.registered_title);
            TextView registeredTickText = FindViewById<TextView>(Resource.Id.registered_tick_text);
            TextView registeredDescription = FindViewById<TextView>(Resource.Id.registered_description);
            TextView recipeHeader = FindViewById<TextView>(Resource.Id.recipe_header);
            TextView recipeSmallText = FindViewById<TextView>(Resource.Id.recipe_small_text);
            
            registeredTitle.Text = REGISTER_QUESTIONAIRE_RECEIPT_HEADER;
            registeredTickText.Text = REGISTER_QUESTIONAIRE_RECEIPT_TEXT;
            registeredDescription.Text = REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION;
            recipeHeader.Text = REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER;
            recipeSmallText.Text = REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE;

            registeredTitle.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_HEADER;
            registeredTitle.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            registeredTickText.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_TEXT;
            registeredDescription.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_DESCRIPTION;
            recipeHeader.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_INNER_HEADER;
            recipeHeader.SetAccessibilityDelegate(AccessibilityUtils.GetHeadingAccessibilityDelegate());
            recipeSmallText.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_INNER_READ_MORE;

            Button button = FindViewById<Button>(Resource.Id.registered_button);
            button.Text = REGISTER_QUESTIONAIRE_RECEIPT_DISMISS;
            button.ContentDescription = REGISTER_QUESTIONAIRE_RECEIPT_DISMISS;
            button.Click += new SingleClick((o, ev) => GoToInfectionStatusActivity()).Run;

            FindViewById<ConstraintLayout>(Resource.Id.explanation_link).Click +=
                async (sender, args) =>
                    await CommonServiceLocator.ServiceLocator.Current.GetInstance<IBrowser>()
                        .OpenAsync(
                            REGISTER_QUESTIONAIRE_RECEIPT_LINK,
                            BrowserLaunchMode.SystemPreferred);
            LogUtils.LogMessage(LogSeverity.INFO, "The user has successfully shared their keys", null, GetCorrelationId());
            UpdateCorrelationId(null);
        }

        public override void OnBackPressed() => GoToInfectionStatusActivity();

        private void GoToInfectionStatusActivity() => NavigationHelper.GoToResultPageAndClearTop(this);
    }
}
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Droid.Utils;
using static NDB.Covid19.ViewModels.WelcomePageWhatIsNewViewModel;

namespace NDB.Covid19.Droid.Views.Welcome
{
    [Activity(Label = "WelcomePageWhatIsNewActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]

    public class WelcomePageWhatIsNewActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.welcome_what_is_new);

            TextView title = FindViewById<TextView>(Resource.Id.welcome_what_is_new_title);

            title.Text = WELCOME_PAGE_WHATS_NEW_TITLE;

            SetBulletText(Resource.Id.bullet_one, WELCOME_PAGE_WHATS_NEW_BULLET_ONE);

            Button button = FindViewById<Button>(Resource.Id.ok_button);
            TextView footer = FindViewById<TextView>(Resource.Id.footer);

            button.Text = WELCOME_PAGE_WHATS_NEW_BUTTON;
            footer.Text = WELCOME_PAGE_WHATS_NEW_FOOTER;

            button.Click += new StressUtils.SingleClick(((o, args) => NavigationHelper.GoToOnBoarding(this, true))).Run;
        }

        private void SetBulletText(int resourceId, string textContent)
        {
            LinearLayout bullet = FindViewById<LinearLayout>(resourceId);
            CheckBox bulletCheckBox = bullet.FindViewById<CheckBox>(Resource.Id.bulletText);
            if (bulletCheckBox != null)
            {
                bulletCheckBox.TextFormatted =  HtmlCompat.FromHtml(textContent, HtmlCompat.FromHtmlModeLegacy);
            }
        }
    }
}
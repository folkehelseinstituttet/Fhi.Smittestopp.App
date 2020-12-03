using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Text;
using NDB.Covid19.Configuration;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.ViewModels;
using Uri = Android.Net.Uri;

namespace NDB.Covid19.Droid.Views
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTop)]
    public class ForceUpdateActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.force_update);

            FindViewById<TextView>(Resource.Id.force_update_label).TextFormatted =
                HtmlCompat.FromHtml(ForceUpdateViewModel.FORCE_UPDATE_MESSAGE, HtmlCompat.FromHtmlModeLegacy);
            Button updateButton = FindViewById<Button>(Resource.Id.force_update_button);
            updateButton.Text = ForceUpdateViewModel.FORCE_UPDATE_BUTTON_GOOGLE_ANDROID;
            updateButton.ContentDescription = ForceUpdateViewModel.FORCE_UPDATE_BUTTON_GOOGLE_ANDROID;
            updateButton.Click += new StressUtils.SingleClick(GoToGooglePlay).Run;
        }

        private void GoToGooglePlay(object o, EventArgs eventArgs)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetData(Uri.Parse(Conf.GooglePlayAppLink));
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            // Do not allow user to go back as app should be updated.
        }
    }
}
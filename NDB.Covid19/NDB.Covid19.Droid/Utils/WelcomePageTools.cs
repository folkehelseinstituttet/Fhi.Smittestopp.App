using Android.Views;
using Android.Widget;
using NDB.Covid19.Droid.Views.Welcome;
using static Plugin.CurrentActivity.CrossCurrentActivity;

namespace NDB.Covid19.Droid.Utils
{
    public static class WelcomePageTools
    {
        public static void SetArrowVisibility(View view)
        {
            bool isOnBoarding = (Current.Activity as WelcomeActivity)?.IsOnBoarding ?? false;
            Button arrowBack = view.FindViewById<Button>(Resource.Id.arrow_back);
            arrowBack.ContentDescription = ViewModels.SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
            arrowBack.Visibility = isOnBoarding ? ViewStates.Gone : ViewStates.Visible;
            if (!isOnBoarding)
            {
                arrowBack.Click += new StressUtils.SingleClick(((o, args) => Current.Activity.Finish())).Run;
            }
        }
    }
}
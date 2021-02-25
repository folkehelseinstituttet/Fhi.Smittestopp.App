using Android.App;
using Android.Content;
using NDB.Covid19.Droid.Views;
using NDB.Covid19.Droid.Views.ENDeveloperTools;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.Droid.Views.Settings;
using NDB.Covid19.Droid.Views.Welcome;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

namespace NDB.Covid19.Droid.Utils
{
    public static class NavigationHelper
    {
        public static void GoToSettingsPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsActivity));
            parent.StartActivity(intent);
        }

        public static void GoToSettingsHowItWorksPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsHowItWorksActivity));
            parent.StartActivity(intent);
        }

        public static void GoToSettingsHelpPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsHelpActivity));
            parent.StartActivity(intent);
        }

        public static void GoToSettingsAboutPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsAbout));
            parent.StartActivity(intent);
        }

        public static void GoToDebugPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(ENDeveloperToolsActivity));
            parent.StartActivity(intent);
        }

        public static void GoToGenetalSettingsPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsGeneralActivity));
            parent.StartActivity(intent);
        }

        /// <summary>
        /// // ActivityFlags.ClearTop will make sure to use an existing activity in the stack and finish all other activities on top of it.
        // This way we can go back to InfectionStatusActivity and clean up on the way.
        /// </summary>
        public static void GoToResultPageAndClearTop(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(InfectionStatusActivity));
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            parent.StartActivity(intent);
        }

        public static void GoToWelcomeWhatsNewPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(WelcomePageWhatIsNewActivity));
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            parent.StartActivity(intent);
        }

        public static void RestartApp(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(InitializerActivity));
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            parent.StartActivity(intent);
        }

        public static void GoToConsentsWithdrawPage(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(SettingsWithdrawConsentsActivity));
            parent.StartActivity(intent);
        }

        public static void GoToOnBoarding(Activity parent, bool isOnBoarding)
        {
            Intent intent = new Intent(parent, typeof(WelcomeActivity));
            intent.PutExtra(DroidRequestCodes.isOnBoardinIntentExtra, isOnBoarding);
            parent.StartActivity(intent);
        }

        public static void GoToLanguageSelection(Activity parent)
        {
            Intent intent = new Intent(parent, typeof(LanguageSelectionActivity));
            parent.StartActivity(intent);
        }

        public static Intent GetStartPageIntent(Activity parent)
        {
            return LocalPreferencesHelper.IsOnboardingCompleted == false
            ? new Intent(parent, typeof(InitializerActivity))
            : new Intent(parent, typeof(InfectionStatusActivity));
        }

        public static void GoToStartPageIfIsOnboarded(Activity parent)
        {
            ConsentsHelper.DoActionWhenOnboarded(() => GoToResultPageAndClearTop(parent));
        }
    }
}
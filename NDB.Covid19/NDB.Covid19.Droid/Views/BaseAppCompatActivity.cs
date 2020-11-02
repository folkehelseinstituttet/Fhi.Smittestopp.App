using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using NDB.Covid19.Enums;

namespace NDB.Covid19.Droid.Views
{
    public class BaseAppCompatActivity : AppCompatActivity
    {
        protected bool? _areYouStillAlive;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (State(savedInstanceState) == AppState.IsDestroyed)
            {
                System.Diagnostics.Debug.Print("Restarting the app from LoginWithPinActivity");
                base.OnCreate(null);
                Intent intent = GetStartingNewIntent();

                if(intent != null)
                {
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    StartActivity(intent);
                }
                Finish();
                return;
            }

            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// Provide an intent to start with when the activity is destroyed
        /// </summary>
        /// <returns></returns>
        protected virtual Intent GetStartingNewIntent()
        {
            return null;
        }

        public AppState State(Bundle savedInstanceState)
        {
            if (savedInstanceState != null && savedInstanceState.GetInt("SavedInstance") > 0)
            {
                string logMessage = "AppState: An activity was destroyed, app is restarting";
                System.Diagnostics.Debug.WriteLine(logMessage);
                return AppState.IsDestroyed;
            }

            return AppState.IsAlive;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("SavedInstance", 1);
        }
    }
}

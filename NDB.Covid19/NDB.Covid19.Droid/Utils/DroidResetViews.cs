using NDB.Covid19.Interfaces;
using Plugin.CurrentActivity;

namespace NDB.Covid19.Droid.Utils
{
    public class DroidResetViews : IResetViews
    {
        public void ResetViews()
        {
            NavigationHelper.RestartApp(CrossCurrentActivity.Current.Activity);
        }
    }
}
using Android.App;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.WebServices.ErrorHandlers;
using static Plugin.CurrentActivity.CrossCurrentActivity;

namespace NDB.Covid19.Droid.Services
{
    public class DroidDialogService : IDialogService
    {
        public void ShowMessageDialog(string title, string message, string okBtn, PlatformDialogServiceArguments platformArguments = null)
        {
            Activity activity = platformArguments?.Context != null && platformArguments.Context is Activity act ? act : Current.Activity;
            DialogUtils.DisplayDialogAsync(activity, title, message, okBtn);
        }
    }
}
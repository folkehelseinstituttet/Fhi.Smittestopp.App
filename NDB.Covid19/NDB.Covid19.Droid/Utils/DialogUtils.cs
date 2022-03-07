using System;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using Android.Widget;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using static NDB.Covid19.Droid.Utils.StressUtils;

namespace NDB.Covid19.Droid.Utils
{
    public static class DialogUtils
    {
        static string _logPrefix = $"Android {nameof(DialogUtils)}: ";

        public static void DisplayBubbleDialog(Activity current, string message, string buttonText)
        {
            if (current == null || current.IsFinishing) return;

            View dialogView = LayoutInflater.From(current).Inflate(Resource.Layout.bubble_layout, null);
            TextView bubbleMessage = dialogView.FindViewById<TextView>(Resource.Id.bubble_message);
            Button bubbleButton = dialogView.FindViewById<Button>(Resource.Id.buttonBubble);
            bubbleMessage.Text = message;
            bubbleButton.Text = buttonText;
            AlertDialog dialog = new AlertDialog.Builder(current)
                .SetView(dialogView)
                .SetCancelable(true)
                .Create();
            bubbleButton.Click += new SingleClick((arg1, arg2) => dialog.Dismiss()).Run;
            dialog.Window.SetLayout(WindowManagerLayoutParams.WrapContent, WindowManagerLayoutParams.WrapContent);
            dialog.Show();
        }

        public static Task<bool> DisplayDialogAsync(
            Activity activity,
            string title,
            string message,
            string okBtnText = null,
            string noBtnText = null,
            int? okBtnTextResourceId = null,
            int? noBtnTextResourceId = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (activity == null || activity.IsFinishing)
            {
                tcs.TrySetResult(false);
                return tcs.Task;
            }

            var dialog = new AlertDialog.Builder(activity)
              .SetTitle(title)
              .SetMessage(message)
              .SetCancelable(false);

            string additionalInfo = AdditionalDialogInfoForLogging(activity, title, message, okBtnText, noBtnText);

            if (string.IsNullOrEmpty(okBtnText) == false || okBtnTextResourceId != null)
            {
                if (string.IsNullOrEmpty(okBtnText) == false)
                {
                    dialog
                        .SetPositiveButton(okBtnText, (sender, e) => { SetResultWithCatch(tcs, true, additionalInfo); });
                }

                if (okBtnTextResourceId != null)
                {
                    dialog
                        .SetPositiveButton(okBtnTextResourceId.Value, (sender, e) => { SetResultWithCatch(tcs, true, additionalInfo); });
                }
            }

            if (string.IsNullOrEmpty(noBtnText) == false || noBtnTextResourceId != null)
            {
                if (string.IsNullOrEmpty(noBtnText) == false)
                {
                    dialog
                        .SetNegativeButton(noBtnText, (sender, e) => { SetResultWithCatch(tcs, false, additionalInfo); });
                }

                if (noBtnTextResourceId != null)
                {
                    dialog
                        .SetNegativeButton(noBtnTextResourceId.Value, (sender, e) => { SetResultWithCatch(tcs, false, additionalInfo); });
                }
            }

            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
            {
                AlertDialog alertDialog = dialog.Create();
                alertDialog.Window.DecorView.LayoutDirection = LayoutUtils.GetLayoutDirection();
                alertDialog.Show();
            }
            else
            {
                dialog.Show();
            }

            return tcs.Task;
        }

        public static Task<bool> DisplayDialogAsync(
            Activity activity,
            DialogViewModel dialogViewModel) =>
            DisplayDialogAsync(
                activity,
                dialogViewModel.Title,
                dialogViewModel.Body,
                dialogViewModel.OkBtnTxt,
                dialogViewModel.CancelbtnTxt);

        private static string AdditionalDialogInfoForLogging(Activity current, string title, string message, string okBtnText, string noBtnText)
        {
            return $"Exception occured in Activity {current.Class.SimpleName}. " +
                $"dialog title: {title}, dialog message: {message}, " +
                $"dialog okBtnText: {okBtnText}, dialog noBtnText: {noBtnText}";
        }

        private static void SetResultWithCatch(TaskCompletionSource<bool> tcs, bool result, String additionalInfo)
        {
            try
            {
                tcs.SetResult(result);
            }
            catch (Exception e) {
                if (e is InvalidOperationException)
                {
                    LogUtils.LogException(Enums.LogSeverity.WARNING, e, _logPrefix + "InvalidOperationException, this indicates the task was already in a final state when SetResult was called, (User most likely pressed both button at the same time)", additionalInfo);
                }
                else if (e is ObjectDisposedException)
                {
                    LogUtils.LogException(Enums.LogSeverity.ERROR, e, _logPrefix + "ObjectDisposedException, this indicates the task was disposed when SetResult was called.", additionalInfo);
                }
                else
                {
                    LogUtils.LogException(Enums.LogSeverity.ERROR, e, _logPrefix + "Some Exception from SetResult operation.", additionalInfo);
                }
            }
        }

        public static async Task<bool> DisplayDialogAsync(
            Activity current,
            DialogViewModel viewModel,
            Action actionOk = null,
            Action actionNotOk = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (current == null || current.IsFinishing)
            {
                tcs.TrySetResult(false);
                return await tcs.Task;
            }

            bool result = await DisplayDialogAsync(
                current,
                viewModel.Title,
                viewModel.Body,
                viewModel.OkBtnTxt,
                viewModel.CancelbtnTxt);

            if (result)
            {
                actionOk?.Invoke();
                tcs.TrySetResult(true);
            }
            else
            {
                actionNotOk?.Invoke();
                tcs.TrySetResult(false);
            }
            return await tcs.Task;
        }
    }
}

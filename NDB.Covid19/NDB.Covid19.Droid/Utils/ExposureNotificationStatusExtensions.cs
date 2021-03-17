using System;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Android.Runtime;
using Java.Util;
namespace NDB.Covid19.Droid.Utils
{
    static class ExposureNotificationStatusExtensions
    {
        public static Task<AbstractCollection> CastTask(this Android.Gms.Tasks.Task androidTask)
        {
            TaskCompletionSource<AbstractCollection> tcs = new TaskCompletionSource<AbstractCollection>();
            androidTask.AddOnCompleteListener(new MyCompleteListener(t =>
            {
                if (t.Exception == null)
                    tcs.TrySetResult(result: t.Result.JavaCast<AbstractCollection>());
                else
                    tcs.TrySetException(t.Exception);
            }));
            return tcs.Task;
        }
        private class MyCompleteListener :
            Java.Lang.Object,
            IOnCompleteListener
        {
            public MyCompleteListener(Action<Android.Gms.Tasks.Task> onComplete) => OnCompleteHandler = onComplete;
            private Action<Android.Gms.Tasks.Task> OnCompleteHandler { get; }
            public void OnComplete(Android.Gms.Tasks.Task task)
            {
                OnCompleteHandler?.Invoke(task);
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using BackgroundTasks;
using ExposureNotifications;
using Foundation;
using NDB.Covid19.Utils;
using Xamarin.ExposureNotifications;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.iOS.Utils
{
    public class BackgroundServiceHandler
    {
        public static Task PlatformScheduleFetch()
        {
            string logPrefix = $"{nameof(BackgroundServiceHandler)}.{nameof(PlatformScheduleFetch)}: ";


            // iOS 12.5

            if (AppDelegate.ShouldOperateIn12_5Mode)
            {
                if (ObjCRuntime.Class.GetHandle("ENManager") == IntPtr.Zero)
                {
                    LogUtils.LogException(Enums.LogSeverity.ERROR,
                        new NullReferenceException("Pointer to ENManager is null"),
                        logPrefix + "Failed to retrieve ENManager instance [iOS 12.5 mode]");
                    return null;
                }

                ENManager manager = new ENManager();

                manager.SetLaunchActivityHandler(activityFlags =>
                {
                    if (activityFlags.HasFlag(ENActivityFlags.PeriodicRun))
                    {
                        Task.Run(async () =>
                        {
                            CancellationTokenSource cancelSrc = new CancellationTokenSource();
                            try
                            {
                                await Xamarin.ExposureNotifications.ExposureNotification.UpdateKeysFromServer(cancelSrc.Token);
                            }
                            catch (OperationCanceledException e)
                            {
                                LogUtils.LogException(Enums.LogSeverity.WARNING, e, logPrefix + "Background task took too long to complete [iOS 12.5 mode]");
                            }
                            catch (NSErrorException nserror)
                            {
                                if (nserror.Domain == "ENErrorDomain")
                                {
                                    LogUtils.LogException(Enums.LogSeverity.WARNING, nserror, logPrefix + $"Background task failed due to EN API Error Code={nserror.Code} [iOS 12.5 mode]");
                                }
                                else
                                {
                                    LogUtils.LogException(Enums.LogSeverity.WARNING, nserror, logPrefix + $"Background task failed due to NSError {nserror.Domain} {nserror.Code}. [iOS 12.5 mode]");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtils.LogException(Enums.LogSeverity.WARNING, ex, logPrefix + $"Background task: An error occurred inside the async task [iOS 12.5 mode]");
                            }
                        });
                    }
                });
                return manager.ActivateAsync();
            }

            // iOS 13+

            // This is a special ID suffix which iOS treats a certain way
            // we can basically request infinite background tasks
            // and iOS will throttle it sensibly for us.
            string id = CommonServiceLocator.ServiceLocator.Current.GetInstance<IAppInfo>().PackageName + ".exposure-notification";

            BGTaskScheduler.Shared.Register(id, null, async task =>
            {
                try
                {
                    if (ENManager.AuthorizationStatus != ENAuthorizationStatus.Authorized) // ENManager is not authorised yet
                    {
                        Debug.WriteLine(logPrefix + "ENManager AuthorizationStatus isn't authorized");
                        scheduleBgTask(0); // reschedule to try in the future
                        return;
                    }

                    bool shouldFetch = await Xamarin.ExposureNotifications.ExposureNotification.IsEnabledAsync();
                    if (!shouldFetch)
                    {
                        LogUtils.LogMessage(Enums.LogSeverity.WARNING, logPrefix + "Did not pull. EN was not enabled (iOS)");
                    }

                    System.Diagnostics.Debug.WriteLine($"Background Task is fetching: {shouldFetch}");
                    if (shouldFetch)
                    {
                        CancellationTokenSource cancelSrc = new CancellationTokenSource();
                        task.ExpirationHandler = cancelSrc.Cancel;

                        // Run the actual task on a background thread
                        await Task.Run(async () =>
                        {
                            try
                            {
                                await Xamarin.ExposureNotifications.ExposureNotification.UpdateKeysFromServer(cancelSrc.Token);
                                task.SetTaskCompleted(true);
                            }
                            catch (OperationCanceledException e)
                            {
                                LogUtils.LogException(Enums.LogSeverity.WARNING, e, logPrefix + "Background task took too long to complete. - BG Task is rescheduled");
                                task.SetTaskCompleted(false);
                            }
                            catch (NSErrorException nserror)
                            {
                                if (nserror.Domain == "ENErrorDomain")
                                {
                                    LogUtils.LogException(Enums.LogSeverity.WARNING, nserror, logPrefix + $"Background task failed due to EN API Error Code={nserror.Code} - BG Task is rescheduled");
                                    task.SetTaskCompleted(false);
                                }
                                else
                                {
                                    LogUtils.LogException(Enums.LogSeverity.WARNING, nserror, logPrefix + $"Background task failed due to NSError {nserror.Domain} {nserror.Code}. - BG Task is rescheduled");
                                    task.SetTaskCompleted(false);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtils.LogException(Enums.LogSeverity.WARNING, ex, logPrefix + $"Background task: An error occurred inside the async task - BG Task is rescheduled");
                                task.SetTaskCompleted(false);
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    LogUtils.LogException(Enums.LogSeverity.WARNING, e, logPrefix + $"Background task: An error occurred before executing the async task - BG Task is rescheduled");
                    task.SetTaskCompleted(false);
                }

                scheduleBgTask(0);
            });

            scheduleBgTask(0);

            return Task.CompletedTask;

            void scheduleBgTask(int numberOfRetries)
            {
                BGProcessingTaskRequest newBgTask = new BGProcessingTaskRequest(id);
                newBgTask.RequiresNetworkConnectivity = true;
                try
                {
                    BGTaskScheduler.Shared.Submit(newBgTask, out var error);
                    if (error != null)
                    {
                        if ((BGTaskSchedulerErrorCode)(int)error.Code == BGTaskSchedulerErrorCode.TooManyPendingTaskRequests && numberOfRetries < 5)
                        {
                            BGTaskScheduler.Shared.Cancel(id);
                            scheduleBgTask(++numberOfRetries);
                        }
                        else
                        {
                            if ((BGTaskSchedulerErrorCode)(int)error.Code == BGTaskSchedulerErrorCode.Unavailable)
                            {
                                //This will happen IF:
                                // - The EN-API permission is not granted/was removed
                                // AND
                                // - The “Background app refresh” setting is turned OFF.
                            }
                            throw new NSErrorException(error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.LogException(Enums.LogSeverity.ERROR, ex, logPrefix + $"Failed to schedule the background task (Tried 5 times). It will try again when the app is restarted.");
                }
            }
        }

    }
}

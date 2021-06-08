using System.ComponentModel;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Interfaces;
using NDB.Covid19.ViewModels;
using Xamarin.Essentials;

namespace NDB.Covid19.Enums
{
    public enum NotificationsEnum
    {
        
        NewMessageReceived,
        ApiDeprecated,
        ConsentNeeded,
        ReApproveConsents,
        BackgroundFetch,
        BluetoothAndLocationOff,
        BluetoothOff,
        LocationOff,
        NoNotification,
        TimedReminder,
        TimedReminderFinished
    }

    public static class NotificationsEnumExtensions 
    {
        public static NotificationViewModel Data(this NotificationsEnum notificationType) 
        {
            switch (notificationType)
            {
                
                case NotificationsEnum.NewMessageReceived:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.NewMessageReceived,
                        Title = "NOTIFICATION_HEADER".Translate(),
                        Body = "NOTIFICATION_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.ApiDeprecated:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.ApiDeprecated,
                        Title = "NOTIFICATION_UPDATE_HEADER".Translate(),
                        Body = (ServiceLocator.Current.GetInstance<IDeviceInfo>().Platform == DevicePlatform.iOS ?
                            "NOTIFICATION_iOS_UPDATE_DESCRIPTION" :
                            "NOTIFICATION_ANDROID_UPDATE_DESCRIPTION").Translate()
                    };
                case NotificationsEnum.ConsentNeeded:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.ConsentNeeded,
                        Title = "NOTIFICATION_UPDATE_HEADER".Translate(),
                        Body = "NOTIFICATION_CONSENT_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.ReApproveConsents:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.ReApproveConsents,
                        Title = "NOTIFICATION_CONSENT_HEADER".Translate(),
                        Body = "NOTIFICATION_CONSENT_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.BackgroundFetch:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.BackgroundFetch,
                        Title = "NOTIFICATION_BACKGROUND_FETCH_HEADER".Translate(),
                        Body = "NOTIFICATION_BACKGROUND_FETCH_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.LocationOff:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.LocationOff,
                        Title = "NOTIFICATION_LOCATION_OFF_TITLE".Translate(),
                        Body = "NOTIFICATION_LOCATION_OFF_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.BluetoothOff:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.BluetoothOff,
                        Title = "NOTIFICATION_BLUETOOTH_OFF_TITLE".Translate(),
                        Body = "NOTIFICATION_BLUETOOTH_OFF_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.BluetoothAndLocationOff:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.BluetoothAndLocationOff,
                        Title = "NOTIFICATION_BLUETOOTH_AND_LOCATION_OFF_TITLE".Translate(),
                        Body = "NOTIFICATION_BLUETOOTH_AND_LOCATION_OFF_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.TimedReminder:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.TimedReminder,
                        Title = "NOTIFICATION_TIMED_REMINDER_TITLE".Translate(),
                        Body = "NOTIFICATION_TIMED_REMINDER_DESCRIPTION".Translate()
                    };
                case NotificationsEnum.TimedReminderFinished:
                    return new NotificationViewModel
                    {
                        Type = NotificationsEnum.TimedReminderFinished,
                        Title = "NOTIFICATION_TIMED_REMINDER_FINISHED_TITLE".Translate(),
                        Body = "NOTIFICATION_TIMED_REMINDER_FINISHED_DESCRIPTION".Translate()
                    };
                default:
                    throw new InvalidEnumArgumentException("Notification type does not exist");
            }
        }
    }
}

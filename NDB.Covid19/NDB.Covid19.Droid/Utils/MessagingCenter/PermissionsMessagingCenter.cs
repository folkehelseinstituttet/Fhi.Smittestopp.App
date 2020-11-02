using System;
using NDB.Covid19.Utils;
using static NDB.Covid19.Utils.MessagingCenter;

namespace NDB.Covid19.Droid.Utils.MessagingCenter
{
    public class PermissionsMessagingCenter
    {
        public static bool PermissionsChanged { get; set; } = false; 

        public static void SubscribeForPermissionsChanged(object sender, Action<object> action)
        {
            Subscribe(sender, MessagingCenterKeys.KEY_PERMISSIONS_CHANGED, action);
        }

        public static void Unsubscribe(object sender)
        {
            Unsubscribe<object>(sender, MessagingCenterKeys.KEY_PERMISSIONS_CHANGED);
        }

        public static void NotifyPermissionsChanged(object sender)
        {
            PermissionsChanged = true;
            Send(sender, MessagingCenterKeys.KEY_PERMISSIONS_CHANGED);
        }
    }
}
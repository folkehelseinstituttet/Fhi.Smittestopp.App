using NDB.Covid19.Enums;

namespace NDB.Covid19.ViewModels
{
    public class NotificationViewModel
    {
        public NotificationsEnum Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}

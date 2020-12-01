using System;
using I18NPortable;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Utils;

namespace NDB.Covid19.ViewModels
{
    public class MessageItemViewModel
    {
        public static string MESSAGES_RECOMMENDATIONS => "MESSAGES_RECOMMENDATIONS_".Translate();

        private bool _isRead;
        public int ID { get; }
        public string Title { get; }
        public DateTime TimeStamp { get; }
        public string MessageLink { get; }
        public string DayAndMonthString => $"{DateUtils.GetDateFromDateTime(TimeStamp, "d. MMMMM yyyy")}";

        public bool IsRead
        {
            get => _isRead;
            set
            {
                MessageUtils.MarkAsRead(this, value);
                _isRead = value;
            }
        }

        public MessageItemViewModel(MessageSQLiteModel model)
        {
            ID = model.ID;
            Title = model.Title;
            TimeStamp = model.TimeStamp;
            MessageLink = model.MessageLink;
            IsRead = model.IsRead;
        }

        public MessageItemViewModel()
        {
        }
    }
}
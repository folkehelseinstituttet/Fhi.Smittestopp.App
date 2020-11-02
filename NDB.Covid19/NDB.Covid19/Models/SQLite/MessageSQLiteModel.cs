using System;
using NDB.Covid19.ViewModels;
using SQLite;

namespace NDB.Covid19.Models.SQLite
{
    public class MessageSQLiteModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MessageLink { get; set; }
        public bool IsRead { get; set; }

        public MessageSQLiteModel() {}
        public MessageSQLiteModel(MessageItemViewModel model)
        {
            ID = model.ID;
            Title = model.Title;
            TimeStamp = model.TimeStamp;
            MessageLink = model.MessageLink;
            IsRead = model.IsRead;
        }
    }
}

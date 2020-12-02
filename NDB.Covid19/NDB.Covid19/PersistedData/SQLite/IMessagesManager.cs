using System.Collections.Generic;
using System.Threading.Tasks;
using NDB.Covid19.Models.SQLite;

namespace NDB.Covid19.PersistedData.SQLite
{
    public interface IMessagesManager
    {
        Task<int> SaveNewMessage(MessageSQLiteModel log);
        Task<List<MessageSQLiteModel>> GetMessages();
        Task<List<MessageSQLiteModel>> GetUnreadMessages();
        Task DeleteMessages(List<MessageSQLiteModel> logs);
        Task DeleteAll();
        Task MarkAsRead(MessageSQLiteModel message, bool isRead);
        Task MarkAllAsRead();

    }
}
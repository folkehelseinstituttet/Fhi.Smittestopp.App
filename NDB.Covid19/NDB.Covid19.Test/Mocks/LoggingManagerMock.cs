using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData.SQLite;

namespace NDB.Covid19.Test.Mocks
{

    public class LoggingManagerMock : ILoggingManager
    {
        private List<LogSQLiteModel> _logList;

        public LoggingManagerMock()
        {
            _logList = new List<LogSQLiteModel>();
        }

        public async void SaveNewLog(LogSQLiteModel log)
        {

            _logList.Add(log);

        }

        public async Task<List<LogSQLiteModel>> GetLogs(int amount)
        {

            return _logList;

        }

        public async Task DeleteLogs(List<LogSQLiteModel> logs)
        {

            foreach (LogSQLiteModel log in logs)
            {
                _logList.Remove(log);
            }

        }

        public async Task DeleteAll()
        {

            _logList = new List<LogSQLiteModel>();

        }
    }
}

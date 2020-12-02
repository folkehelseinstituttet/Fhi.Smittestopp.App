using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using NDB.Covid19.Models.SQLite;
using SQLite;

namespace NDB.Covid19.PersistedData.SQLite
{

    public class MessagesManager : IMessagesManager
    {
        readonly SQLiteAsyncConnection _database;
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        public MessagesManager()
        {
            _database = new SQLiteAsyncConnection(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Conf.DB_NAME));
            _database.CreateTableAsync<MessageSQLiteModel>().Wait();
        }

        public async Task<int> SaveNewMessage(MessageSQLiteModel message)
        {
            await _syncLock.WaitAsync();
            try
            {
                return await _database.InsertAsync(message);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<List<MessageSQLiteModel>> GetMessages()
        {
            await _syncLock.WaitAsync();
            try
            {
                return await _database.Table<MessageSQLiteModel>().ToListAsync();
            }
            catch
            {
                return new List<MessageSQLiteModel>();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<List<MessageSQLiteModel>> GetUnreadMessages()
        {
            await _syncLock.WaitAsync();
            try
            {
                return await _database.Table<MessageSQLiteModel>()
                    .Where(message => !message.IsRead)
                    .ToListAsync();
            }
            catch
            {
                return new List<MessageSQLiteModel>();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteMessages(List<MessageSQLiteModel> messages)
        {
            await _syncLock.WaitAsync();
            try
            {
                foreach (MessageSQLiteModel message in messages)
                {
                    await _database.Table<MessageSQLiteModel>().DeleteAsync(it => it.ID == message.ID);
                }
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteAll()
        {
            await _syncLock.WaitAsync();
            try
            {
                await _database.DeleteAllAsync<MessageSQLiteModel>();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task MarkAsRead(MessageSQLiteModel message, bool isRead)
        {
            await _syncLock.WaitAsync();
            try
            {
                message.IsRead = isRead;
                await _database.UpdateAsync(message);
            }
            finally
            {
                _syncLock.Release();
            }
        }
        
        public async Task MarkAllAsRead()
        {
            await _syncLock.WaitAsync();
            try
            {
                List<MessageSQLiteModel> messages = await _database.Table<MessageSQLiteModel>()
                    .Where(message => !message.IsRead)
                    .ToListAsync();
                foreach (MessageSQLiteModel message in messages)
                {
                    message.IsRead = true;
                }
                await _database.UpdateAllAsync(messages);
            }
            finally
            {
                _syncLock.Release();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using NDB.Covid19.Enums;
using NDB.Covid19.ExposureNotifications.Helpers;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.PersistedData.SQLite;
using NDB.Covid19.PersistedData.SecureStorage;

namespace NDB.Covid19.Utils
{
    public static class MessageUtils
    {
        static string _logPrefix = $"{nameof(MessageUtils)}";
        static SecureStorageService _secureStorageService => ServiceLocator.Current.GetInstance<SecureStorageService>();

        static IMessagesManager _manager => ServiceLocator.Current.GetInstance<IMessagesManager>();

        static string DEFAULT_MESSAGE_TITLE => "MESSAGES_MESSAGE_HEADER";
        static string DEFAULT_MESSAGE_LINK => "MESSAGES_LINK";

        private static async Task CreateAndSaveNewMessage(object Sender, DateTime? customTime = null,
            int triggerAfterNSec = 0)
        {
            //When creating a message
            MessageSQLiteModel messageSqLiteModel = new MessageSQLiteModel()
            {
                IsRead = false,
                MessageLink = DEFAULT_MESSAGE_LINK,
                TimeStamp = customTime ?? SystemTime.Now(),
                Title = DEFAULT_MESSAGE_TITLE
            };


            if (!HasCreatedMessageAndNotificationToday())
            {
                await _manager.SaveNewMessage(messageSqLiteModel);
                MessagingCenter.Send<object>(Sender, MessagingCenterKeys.KEY_MESSAGE_RECEIVED);
                NotificationsHelper.CreateNotification(NotificationsEnum.NewMessageReceived, triggerAfterNSec);

                SaveDateTimeToSecureStorageForKey(
                    SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY,
                    customTime ?? SystemTime.Now(),
                    nameof(CreateAndSaveNewMessage));

                SaveDateTimeToSecureStorageForKey(
                    SecureStorageKeys.LAST_SENT_NOTIFICATION_UTC_KEY,
                    customTime ?? SystemTime.Now(),
                    nameof(CreateAndSaveNewMessage));
            }
        }

        public static async Task CreateMessage(object Sender, DateTime? customTime = null, int triggerAfterNSec = 0)
        {
            await CreateAndSaveNewMessage(Sender, customTime, triggerAfterNSec);
        }

        public static async Task<int> SaveMessages(MessageSQLiteModel message)
        {
            return await _manager.SaveNewMessage(message);
        }

        public static async Task<List<MessageSQLiteModel>> GetMessages()
        {
            IEnumerable<MessageSQLiteModel> messages =
                (await _manager.GetMessages())
                .OrderByDescending(x => x.TimeStamp);
            return messages.ToList();
        }

        public static async Task<List<MessageSQLiteModel>> GetAllUnreadMessages()
        {
            return await _manager.GetUnreadMessages();
        }

        public static void RemoveAll()
        {
            _manager.DeleteAll();
        }

        public static async Task RemoveMessages(List<MessageSQLiteModel> messages)
        {
            await _manager.DeleteMessages(messages);
        }

        public static async Task RemoveAllOlderThan(int minutes)
        {
            List<MessageSQLiteModel> messages = await GetMessages();
            List<MessageSQLiteModel> messagesToRemove = messages
                .FindAll(message => SystemTime.Now().Subtract(message.TimeStamp).TotalMinutes >= minutes).ToList();
            await RemoveMessages(messagesToRemove);
        }

        public static void MarkAsRead(MessageItemViewModel message, bool isRead)
        {
            _manager.MarkAsRead(new MessageSQLiteModel(message), isRead);
        }

        public static void MarkAllAsRead()
        {
            _manager.MarkAllAsRead();
        }

        public static List<MessageItemViewModel> ToMessageItemViewModelList(List<MessageSQLiteModel> list)
        {
            return list.Select(model => new MessageItemViewModel(model)).ToList();
        }

        public static bool HasCreatedMessageAndNotificationToday()
        {
            DateTime lastRiskAlertUtc = GetDateTimeFromSecureStorageForKey(
                SecureStorageKeys.LAST_HIGH_RISK_ALERT_UTC_KEY, nameof(HasCreatedMessageAndNotificationToday));
            return lastRiskAlertUtc.Date == SystemTime.Now().Date;
        }

        /// <summary>
        /// Helper method for saving DateTime assosiated with Messages in the SecureStorage.
        /// </summary>
        /// <param name="SecureStorageKey"></param>
        /// <param name="DateTimeToSave"></param>
        /// <param name="CallerMethodToLogError"></param>
        /// <returns></returns>
        public static void SaveDateTimeToSecureStorageForKey(string SecureStorageKey, DateTime DateTimeToSave,
            string CallerMethodToLogError)
        {
            try
            {
                _secureStorageService.SaveValue(SecureStorageKey, DateTimeToSave.ToString());
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e, $"{_logPrefix}.{CallerMethodToLogError}");
            }
        }

        /// <summary>
        /// Helper method for retrieving DateTime assosiated with Messages in the SecureStorage.
        /// </summary>
        /// <param name="SecureStorageKey"></param>
        /// <param name="CallerMethodToLogError"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromSecureStorageForKey(string SecureStorageKey,
            string CallerMethodToLogError)
        {
            DateTime DateTimeFromSecureStorage;
            try
            {
                if (_secureStorageService.KeyExists(SecureStorageKey))
                {
                    DateTimeFromSecureStorage = DateTime.Parse(_secureStorageService.GetValue(SecureStorageKey));
                }
                else
                {
                    DateTimeFromSecureStorage = SystemTime.Now().AddDays(-100);
                }
            }
            catch (Exception e)
            {
                LogUtils.LogException(Enums.LogSeverity.ERROR, e, $"{_logPrefix}.{CallerMethodToLogError}");
                DateTimeFromSecureStorage = DateTime.UtcNow.AddDays(-100); // Date far in the past
            }

            return DateTimeFromSecureStorage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.PersistedData;
using NDB.Covid19.Utils;

namespace NDB.Covid19.ViewModels
{
    public class MessagesViewModel
    {
        public static string MESSAGES_HEADER => "MESSAGES_HEADER".Translate();
        public static string MESSAGES_NEW_MESSAGES_HEADER => "MESSAGES_NEW_MESSAGES_HEADER".Translate();
        public static string MESSAGES_NO_NEW_MESSAGES_HEADER => "MESSAGES_NO_NEW_MESSAGES_HEADER".Translate();
        public static string MESSAGES_NEW_ITEM => "MESSAGES_NEW_ITEM".Translate();
        public static string MESSAGES_NO_ITEMS_TITLE => "MESSAGES_NOMESSAGES_HEADER".Translate();
        public static string MESSAGES_NO_ITEMS_DESCRIPTION => "MESSAGES_NOMESSAGES_LABEL".Translate();
        public static string MESSAGES_LAST_UPDATED_LABEL => "MESSAGES_LAST_UPDATED_LABEL".Translate();
        public static string MESSAGES_ACCESSIBILITY_CLOSE_BUTTON => "MESSAGES_ACCESSIBILITY_CLOSE_BUTTON".Translate();

        public static DateTime LastUpdateDateTime => LocalPreferencesHelper.GetUpdatedDateTime().ToLocalTime();

        public static string LastUpdateString => LastUpdateDateTime != DateTime.MinValue.ToLocalTime()
            ? string.Format(MESSAGES_LAST_UPDATED_LABEL, $"{DateUtils.GetDateFromDateTime(LastUpdateDateTime, "m")}", $"{DateUtils.GetDateFromDateTime(LastUpdateDateTime, "t")}")
            : "";

        private static readonly object Subscriber = new object();

        public static void SubscribeMessages(object subscriber, Action<List<MessageItemViewModel>> action)
        {
            MessagingCenter.Subscribe<object>(
                subscriber,
                MessagingCenterKeys.KEY_MESSAGE_RECEIVED, async obj =>
                {
                    action?.Invoke(await GetMessages());
                    MessagingCenter.Send(Subscriber, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
                });
        }

        public static void UnsubscribeMessages(object subscriber)
        {
            MessagingCenter.Unsubscribe<object>(subscriber, MessagingCenterKeys.KEY_MESSAGE_RECEIVED);
        }

        public static async Task<List<MessageItemViewModel>> GetMessages()
        {
            return MessageUtils.ToMessageItemViewModelList(await MessageUtils.GetMessages());
        }

        public static void MarkAllMessagesAsRead()
        {
            MessageUtils.MarkAllAsRead();
            MessagingCenter.Send(Subscriber, MessagingCenterKeys.KEY_MESSAGE_STATUS_UPDATED);
        }
    }
}
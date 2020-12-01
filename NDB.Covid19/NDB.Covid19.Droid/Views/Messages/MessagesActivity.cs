using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using CommonServiceLocator;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.Droid.Utils;
using NDB.Covid19.Droid.Views.InfectionStatus;
using NDB.Covid19.Interfaces;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using Xamarin.Essentials;
using XamarinShortcutBadger;
using static Plugin.CurrentActivity.CrossCurrentActivity;
using Object = Java.Lang.Object;

namespace NDB.Covid19.Droid.Views.Messages
{
    [Activity(
        Theme = "@style/AppTheme",
        ParentActivity = typeof(InfectionStatusActivity),
        ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class MessagesActivity : AppCompatActivity
    {
        private ListView _messagesList;
        private MessagesAdapter _adapterMessages;
        private TextView _noItemsTextView;
        private TextView _lastUpdatedTextView;
        private ImageView _closeButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = MessagesViewModel.MESSAGES_HEADER;
            SetContentView(Resource.Layout.messages_page);
            Init();
        }

        protected override void OnDestroy()
        {
            MessagesViewModel.UnsubscribeMessages(this);
            Task.Run(async () =>
                await MessagesViewModel.MarkAllMessagesAsRead());
            base.OnDestroy();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await MessageUtils.RemoveAllOlderThan(Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES);
            CloseLocalNotification();
            Update();
            RemoveBadgeCounterOnOlderPlatforms();
        }

        private void RemoveBadgeCounterOnOlderPlatforms()
        {
            bool isLowerVersion = Build.VERSION.SdkInt < BuildVersionCodes.O;
            bool isBadgeCounterSupported = ShortcutBadger.IsBadgeCounterSupported(ApplicationContext);

            // Use Plugin for badges on older platforms that support them
            if (isLowerVersion && isBadgeCounterSupported)
            {
                ShortcutBadger.RemoveCount(ApplicationContext);
            }
        }

        private void CloseLocalNotification()
        {
            NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(Current.Activity);
            notificationManagerCompat.Cancel(LocalNotificationsManager.NotificationId);
        }

        private async void Init()
        {
            MessagesViewModel.SubscribeMessages(this, ClearAndAddNewMessages);
            
            _messagesList = FindViewById<ListView>(Resource.Id.messages_list);
            _noItemsTextView = FindViewById<TextView>(Resource.Id.no_items_description);
            _lastUpdatedTextView = FindViewById<TextView>(Resource.Id.last_updated);

            _messagesList.Divider = null;
            _messagesList.DividerHeight = 0;
            
            FindViewById<TextView>(Resource.Id.messages_page_title).Text =
                MessagesViewModel.MESSAGES_HEADER;
            
            FindViewById<TextView>(Resource.Id.messages_page_sub_header).Text =
                (await MessageUtils.GetAllUnreadMessages()).Count > 0 ?
                    MessagesViewModel.MESSAGES_NEW_MESSAGES_HEADER :
                    MessagesViewModel.MESSAGES_NO_ITEMS_TITLE;

            string lastUpdatedString = MessagesViewModel.LastUpdateString;
            if (lastUpdatedString == "")
            {
                _lastUpdatedTextView.Visibility = ViewStates.Gone;
            }
            else
            {
                _lastUpdatedTextView.Visibility = ViewStates.Visible;
                _lastUpdatedTextView.Text = MessagesViewModel.LastUpdateString;
            }

            _noItemsTextView.Text = MessagesViewModel.MESSAGES_NO_ITEMS_DESCRIPTION;

            _closeButton = FindViewById<ImageView>(Resource.Id.arrow_back);
            _closeButton.Click +=new StressUtils.SingleClick(OnCloseBtnClicked).Run;
            _closeButton.ContentDescription = MessagesViewModel.MESSAGES_ACCESSIBILITY_CLOSE_BUTTON;

            _adapterMessages = new MessagesAdapter(this, new MessageItemViewModel[0]);
            _messagesList.Adapter = _adapterMessages;
            _messagesList.OnItemClickListener = new ItemClickListener(_adapterMessages);
            ShowList(false);
        }

        async Task HandleBeforeActivityClose()
        {
            await MessagesViewModel.MarkAllMessagesAsRead();
        }

        public override async void OnBackPressed()
        {
            await HandleBeforeActivityClose();
            base.OnBackPressed();
        }

        private async void OnCloseBtnClicked(object arg1, EventArgs arg2)
        {
            await HandleBeforeActivityClose();
            Finish();
        }

        class ItemClickListener : Object, AdapterView.IOnItemClickListener
        {
            private MessagesAdapter _adapterMessages;

            public ItemClickListener(MessagesAdapter adapterMessages)
            {
                _adapterMessages = adapterMessages;
            }

            public async void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                await ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(_adapterMessages[position].MessageLink.Translate(), BrowserLaunchMode.SystemPreferred);
                _adapterMessages[position].IsRead = true;
                _adapterMessages.NotifyDataSetChanged();
            }
        }

        public void ClearAndAddNewMessages(List<MessageItemViewModel> messages)
        {
            _adapterMessages.ClearList();
            ShowList(messages.Count > 0);
            _adapterMessages.AddItems(messages);
        }

        public async void Update()
        {
            ClearAndAddNewMessages(await MessagesViewModel.GetMessages());
        }

        private void ShowList(bool isShown)
        {
            _messagesList.Visibility = isShown ? ViewStates.Visible : ViewStates.Invisible;
            _noItemsTextView.Visibility = isShown ? ViewStates.Invisible : ViewStates.Visible;
        }
    }
}
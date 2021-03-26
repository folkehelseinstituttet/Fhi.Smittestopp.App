using NDB.Covid19.iOS.Utils;
using static NDB.Covid19.ViewModels.MessagesViewModel;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using NDB.Covid19.Configuration;
using UserNotifications;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19.iOS.Views.MessagePage
{
    public partial class MessagePageViewController : BaseViewController
    {
        public MessagePageViewController(IntPtr handle) : base(handle)
        {
        }

        public static MessagePageViewController Create()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("MessagePage", null);
            MessagePageViewController vc = (MessagePageViewController)storyboard.InstantiateInitialViewController();
            vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return vc;
        }

        public static UINavigationController GetMessagePageControllerInNavigationController()
        {
            UIViewController vc = MessagePageViewController.Create();
            UINavigationController navigationController = new UINavigationController(vc);
            navigationController.SetNavigationBarHidden(true, false);
            navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            return navigationController;
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetStyling();
            SetupTableView();
        }

        void OnAppReturnsFromBackground(object obj)
        {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
            Update();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //Subscribe to update table service
            MessagesViewModel.SubscribeMessages(this, ClearOrAddNewMessages);
            MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND, OnAppReturnsFromBackground);
            //MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_MESSAGE_RECEIVED);
            //remove all notifications if user opens messages view
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();

            UpdateMessagesOnViewWillAppear();

            PostAccessibilityNotificationAndReenableElement(BackButton, Label);
        }

        async void UpdateMessagesOnViewWillAppear()
        {
            await MessageUtils.RemoveAllOlderThan(Conf.MAX_MESSAGE_RETENTION_TIME_IN_MINUTES);
            InvokeOnMainThread(Update);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            Task.Run(MarkAllMessagesAsRead);
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        private async void SetStyling()
        {
            StyleUtil.InitLabelWithSpacing(Label, StyleUtil.FontType.FontBold, MESSAGES_HEADER, 0.8, 22, 34);
            Label.AccessibilityTraits = UIAccessibilityTrait.Header;
            StyleUtil.InitLabelWithSpacing(LabelLastUpdate, StyleUtil.FontType.FontRegular, LastUpdateString, 1.14, 15, 17);
            int unreadMessages = (await MessageUtils.GetAllUnreadMessages()).Count;
            int messages = (await MessageUtils.GetMessages()).Count;
            string headerText = MESSAGES_NO_ITEMS_TITLE;
            if (unreadMessages > 0)
            {
                headerText = MESSAGES_NEW_MESSAGES_HEADER;
            }
            else if (messages > 0)
            {
                headerText = MESSAGES_NO_NEW_MESSAGES_HEADER;
            }
            StyleUtil.InitLabelWithSpacing(NoItemsLabel1, StyleUtil.FontType.FontBold, headerText, 1, 32, 34);
            NoItemsLabel1.AccessibilityTraits = UIAccessibilityTrait.Header;
            StyleUtil.InitLabelWithSpacing(NoItemsLabel2, StyleUtil.FontType.FontRegular, MESSAGES_NO_ITEMS_DESCRIPTION, 1.25, 18, 20);
            BackButton.AccessibilityLabel = SettingsViewModel.BACK_BUTTON_ACCESSIBILITY_TEXT;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            UnsubscribeMessages(this);
            MessagingCenter.Unsubscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND);
        }

        void SetupTableView()
        {
            MessageTable.RegisterNibForCellReuse(MessagePageCell.Nib, MessagePageCell.Key);
            MessageTable.Source = new MessageTableViewSource();
        }

        public async void Update()
        {
            SetupTableView();
            ClearOrAddNewMessages(await GetMessages());
        }

        public void ClearOrAddNewMessages(List<MessageItemViewModel> list)
        {
            LabelLastUpdate.Text = LastUpdateString;
            List<MessageItemViewModel> listReversed = list;
            InvokeOnMainThread(() =>
            {   
                NoItemsView.Hidden = list.Count > 0;
                MessageTable.Hidden = list.Count <= 0;
                (MessageTable.Source as MessageTableViewSource).Update(listReversed);
                MessageTable.ReloadData();
            });
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            LeaveController();
        }
    }
}
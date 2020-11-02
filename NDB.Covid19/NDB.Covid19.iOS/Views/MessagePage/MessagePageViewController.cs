using NDB.Covid19.iOS.Utils;
using static NDB.Covid19.ViewModels.MessagesViewModel;
using NDB.Covid19.Utils;
using NDB.Covid19.ViewModels;
using System;
using System.Collections.Generic;
using UIKit;
using NDB.Covid19.Config;
using System.Threading.Tasks;
using NDB.Covid19.PersistedData;
using UserNotifications;

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

        private void SetStyling()
        {
            StyleUtil.InitLabelWithSpacing(Label, StyleUtil.FontType.FontBold, MESSAGES_HEADER, 1.14, 24, 36);
            StyleUtil.InitLabelWithSpacing(LabelLastUpdate, StyleUtil.FontType.FontRegular, LastUpdateString, 1.14, 12, 14, UITextAlignment.Left);
            StyleUtil.InitLabelWithSpacing(NoItemsLabel1, StyleUtil.FontType.FontRegular, MESSAGES_NO_ITEMS_TITLE, 1.14, 16, 18);
            StyleUtil.InitLabelWithSpacing(NoItemsLabel2, StyleUtil.FontType.FontRegular, MESSAGES_NO_ITEMS_DESCRIPTION, 1.14, 12, 14, UITextAlignment.Left);
            BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON;
            SetLogoBasedOnAppLanguage();
        }

        private void SetLogoBasedOnAppLanguage()
        {
            string appLanguage = LocalesService.GetLanguage();
            AuthorityImageView.Image = appLanguage != null && appLanguage.ToLower() == "en"
                ? UIImage.FromBundle("logo_SFP_en")
                : UIImage.FromBundle("logo_SFP_da");
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
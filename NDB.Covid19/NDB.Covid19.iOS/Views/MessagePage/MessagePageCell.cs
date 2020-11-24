using System;
using Foundation;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.ViewModels;
using UIKit;
using NDB.Covid19.Utils;

namespace NDB.Covid19.iOS.Views.MessagePage
{
	public partial class MessagePageCell : UITableViewCell
	{
        public static readonly NSString Key = new NSString("MessagePageCell");
        public static readonly UINib Nib;

        static MessagePageCell()
        {
            Nib = UINib.FromName("MessagePageCell", NSBundle.MainBundle);
        }

        protected MessagePageCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Update(MessageItemViewModel message)
        {
            StyleUtil.InitLabelWithSpacing(Label1, StyleUtil.FontType.FontMedium, message.Title, 1.14, 18, 28);
            StyleUtil.InitLabelWithSpacing(Label2, StyleUtil.FontType.FontRegular, DateUtils.GetDateFromDateTime(message.TimeStamp, "d. MMMMM"), 1.14, 15, 17);
            StyleUtil.InitLabelWithSpacing(Label3, StyleUtil.FontType.FontRegular, MessageItemViewModel.MESSAGES_RECOMMENDATIONS, 1.14, 15, 17);
            StyleUtil.InitLabelWithSpacing(UnreadLabel, StyleUtil.FontType.FontBold, MessagesViewModel.MESSAGES_NEW_ITEM, 1.14, 15, 17);
            if (message.IsRead)
            {
                IndicatorView.Hidden = true;
                UnreadLabel.Hidden = true;
                Label2.LeadingAnchor.ConstraintEqualTo(Label3.LeadingAnchor).Active = true;
            }
            this.BackgroundColor = message.IsRead ? new UIColor(new nfloat(1), new nfloat(0.1)) : new UIColor(new nfloat(1), new nfloat(0.25));
        }
	}
}

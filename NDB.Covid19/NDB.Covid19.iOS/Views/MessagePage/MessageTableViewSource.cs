using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Foundation;
using I18NPortable;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Interfaces;
using UIKit;
using Xamarin.Essentials;
using NDB.Covid19.iOS.Utils;

namespace NDB.Covid19.iOS.Views.MessagePage
{
    public class MessageTableViewSource : UITableViewSource
    { 
         List<MessageItemViewModel> _models = new List<MessageItemViewModel>();

        public MessageTableViewSource()
        {
        }

        public int ID { get; internal set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            MessagePageCell cell = tableView.DequeueReusableCell(MessagePageCell.Key, indexPath) as MessagePageCell;
            MessageItemViewModel model = _models[indexPath.Section];
            cell.ContentView.Subviews[0].Layer.CornerRadius = 14;
            cell.ContentView.Subviews[0].Layer.BorderColor = ColorHelper.MESSAGE_BORDER_COLOR.CGColor;
            cell.ContentView.Subviews[0].Layer.BorderWidth = 1;
            cell.ContentView.Subviews[0].ClipsToBounds = true;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.Update(model);
            cell.AccessibilityTraits = UIAccessibilityTrait.Link;
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _models.Count();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 1;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 0;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView view = new UIView();
            view.BackgroundColor = UIColor.Clear;
            return view;
        }

        internal void Update(List<MessageItemViewModel> models)
        {
            _models = models;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
           ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(_models[indexPath.Section].MessageLink.Translate(), BrowserLaunchMode.SystemPreferred);
           _models[indexPath.Section].IsRead = true;
            tableView.ReloadData();
        }
    }
}
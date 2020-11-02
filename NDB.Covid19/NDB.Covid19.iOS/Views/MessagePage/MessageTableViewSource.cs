using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Foundation;
using NDB.Covid19.ViewModels;
using NDB.Covid19.Interfaces;
using UIKit;
using Xamarin.Essentials;

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
            MessageItemViewModel model = _models[indexPath.Row];
            cell.Update(model);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _models.Count();
        }

        internal void Update(List<MessageItemViewModel> models)
        {
            _models = models;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
           ServiceLocator.Current.GetInstance<IBrowser>().OpenAsync(_models[indexPath.Row].MessageLink, BrowserLaunchMode.SystemPreferred);
           _models[indexPath.Row].IsRead = true;
            tableView.ReloadData();
        }
    }
}
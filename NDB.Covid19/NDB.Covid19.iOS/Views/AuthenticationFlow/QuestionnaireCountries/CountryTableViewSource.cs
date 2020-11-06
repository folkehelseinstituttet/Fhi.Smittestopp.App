using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using NDB.Covid19.ViewModels;
using UIKit;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow.QuestionnaireCountries
{
    public class CountryTableViewSource : UITableViewSource
    {
        List<CountryDetailsViewModel> _models = new List<CountryDetailsViewModel>();

        public CountryTableViewSource(List<CountryDetailsViewModel> items)
        {
            _models = items;
        }

        public int ID { get; internal set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CountryTableCell cell = tableView.DequeueReusableCell(CountryTableCell.Key, indexPath) as CountryTableCell;
            CountryDetailsViewModel model = _models[indexPath.Row];
            cell.UpdateCell(model);

            return cell;
        }

        internal void Update(List<CountryDetailsViewModel> models)
        {
            _models = models;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _models.Count();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return CountryTableCell.ROW_HEIGHT;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _models[indexPath.Row].Checked = !_models[indexPath.Row].Checked;
            tableView.ReloadData();
        }
    }
}

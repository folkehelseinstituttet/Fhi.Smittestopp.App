using System;
using System.Collections.Generic;
using Foundation;
using NDB.Covid19.Enums;
using NDB.Covid19.iOS.Utils;
using NDB.Covid19.Models;
using UIKit;

namespace NDB.Covid19.iOS.Views.Settings
{
    public class SettingsTableViewSource : UITableViewSource
    {
        List<SettingItem> settingItemList;

        public EventHandler<SettingItemType> OnCellTapped { get; set; }

        public SettingsTableViewSource(List<SettingItem> itemList)
        {
            settingItemList = itemList;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return settingItemList.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            SettingsItemCell cell = tableView.DequeueReusableCell("SettingsItemCell", indexPath) as SettingsItemCell;

            string text = settingItemList[indexPath.Row].Text;
            cell.SetData(text);
            cell.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(text);
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            OnCellTapped?.Invoke(this, settingItemList[indexPath.Row].Type);
        }
    }
}

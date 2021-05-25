using System;
using System.Collections.Generic;
using UIKit;

namespace NDB.Covid19.iOS.Views.InfectionStatus
{
    class HoursPickerModel : UIPickerViewModel
    {
        public EventHandler ValueChanged;
        public static nint SelectedOptionByUser;
        private readonly List<string> _optionList;

        public HoursPickerModel(List<string> optionList)
        {
            _optionList = optionList;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _optionList.Count;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return _optionList[(int)row];
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedOptionByUser = row;
        }
    }
}
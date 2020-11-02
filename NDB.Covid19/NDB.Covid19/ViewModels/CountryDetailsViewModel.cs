using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Utils;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ViewModels
{
    public class CountryDetailsViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Checked { get; set; }
    }
}
using System.Collections.Generic;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;
using I18NPortable;
using NDB.Covid19.Configuration;

namespace NDB.Covid19.ViewModels
{
    public class SettingsViewModel
    {
        public static string SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON => "SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON".Translate();
        public static string BACK_BUTTON_ACCESSIBILITY_TEXT => "BACK_BUTTON_ACCESSIBILITY_TEXT".Translate();

        public bool ShowDebugItem => Conf.UseDeveloperTools;

        public List<SettingItem> SettingItemList { get; private set; }

        public SettingsViewModel()
        {
            SettingItemList = new List<SettingItem>()
            {
                new SettingItem(SettingItemType.Intro),
                new SettingItem(SettingItemType.HowItWorks),
                new SettingItem(SettingItemType.Consent),
                new SettingItem(SettingItemType.Help),
                new SettingItem(SettingItemType.About),
                new SettingItem(SettingItemType.Settings)
            };

            if (Conf.UseDeveloperTools)
            {
                SettingItemList.Add(new SettingItem(SettingItemType.Debug));
            }
        }
    }
}

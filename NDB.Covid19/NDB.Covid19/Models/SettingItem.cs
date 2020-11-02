using I18NPortable;
using NDB.Covid19.Enums;

namespace NDB.Covid19.Models
{
    public class SettingItem
    {
        public SettingItemType Type { get; private set; }

        public string Text => GetFriendlyTextFromSettingItemType();


        public SettingItem(SettingItemType type)
        {
            Type = type;
        }

        private string GetFriendlyTextFromSettingItemType()
        {
            switch (Type)
            {
                case SettingItemType.About:
                    return "SETTINGS_ITEM_ABOUT".Translate();
                case SettingItemType.Consent:
                    return "SETTINGS_ITEM_CONSENT".Translate();
                case SettingItemType.Help:
                    return "SETTINGS_ITEM_HELP".Translate();
                case SettingItemType.HowItWorks:
                    return "SETTINGS_ITEM_HOW_IT_WORKS".Translate();
                case SettingItemType.Intro:
                    return "SETTINGS_ITEM_INTRO".Translate();
                case SettingItemType.Settings:
                    return "SETTINGS_ITEM_GENERAL".Translate();
                case SettingItemType.Messages:
                    return "SETTINGS_ITEM_MESSAGES".Translate();
                case SettingItemType.Debug:
                    return "Developer Tools";
                default:
                    return string.Empty;

            }
        }

    }
}
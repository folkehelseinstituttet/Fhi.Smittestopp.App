namespace NDB.Covid19.Enums
{
    public enum SettingsLanguageSelection
    {
        Bokmal,
        Nynorsk,
        English,
        Polish
    }

    public static class SettingsLanguageSelectionExtensions
    {
        public static string GetStringValue(SettingsLanguageSelection settingsLanguageSelection)
        {
            switch (settingsLanguageSelection)
            {
                case SettingsLanguageSelection.Bokmal:
                    return "nb";
                case SettingsLanguageSelection.Nynorsk:
                    return "nn";
                case SettingsLanguageSelection.English:
                    return "en";
                case SettingsLanguageSelection.Polish:
                    return "pl";
                default:
                    return "nb";
            }
        }
    }
}

namespace NDB.Covid19.Enums
{
    public enum SettingsLanguageSelection
    {
        Bokmal,
        Nynorsk,
        English,
        Polish,
        Somali,
        Tigrinya
    }

    public static class SettingsLanguageSelectionExtensions
    {
        public static string ToString(SettingsLanguageSelection settingsLanguageSelection)
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
                case SettingsLanguageSelection.Somali:
                    return "so";
                case SettingsLanguageSelection.Tigrinya:
                    return "ti";
                default:
                    return "nb";
            }
        }

        public static SettingsLanguageSelection FromString(string languageCode)
        {
            switch (languageCode)
            {
                case "nb":
                    return SettingsLanguageSelection.Bokmal;
                case "nn":
                    return SettingsLanguageSelection.Nynorsk;
                case "en":
                    return SettingsLanguageSelection.English;
                case "pl":
                    return SettingsLanguageSelection.Polish;
                case "so":
                    return SettingsLanguageSelection.Somali;
                case "ti":
                    return SettingsLanguageSelection.Tigrinya;
                default:
                    return SettingsLanguageSelection.Bokmal;
            }
        }
    }
}

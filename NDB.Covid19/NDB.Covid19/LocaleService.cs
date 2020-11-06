using System.Globalization;
using System.Linq;
using System.Reflection;
using I18NPortable;
using I18NPortable.JsonReader;
using NDB.Covid19.Configuration;
using NDB.Covid19.PersistedData;

namespace NDB.Covid19
{
    //See nuget https://github.com/xleon/I18N-Portable
    public static class LocalesService
    {
        public static void Initialize()
        {
            if (I18N.Current?.Locale == null)
            {
                I18N.Current
                ?.SetNotFoundSymbol("$") // Optional: when a key is not found, it will appear as $key$ (defaults to "$")
                .SetFallbackLocale(Conf.DEFAULT_LANGUAGE) // Optional but recommended: locale to load in case the system locale is not supported
                .AddLocaleReader(new JsonKvpReader(), ".json") //Use the json parser
                .Init(typeof(LocalesService).GetTypeInfo().Assembly);
            }

            SetInternationalization();
        }

        public static string GetLanguage()
        {
            if (LocalPreferencesHelper.GetAppLanguage() != null)
            {
                return LocalPreferencesHelper.GetAppLanguage();
            }

            bool currentCultureIsSupported = Conf.SUPPORTED_LANGUAGES.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                    
            return currentCultureIsSupported
                ? CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                : Conf.DEFAULT_LANGUAGE;
        }

        public static void SetInternationalization()
        {
            I18N.Current.Locale = GetLanguage();
        }
    }
}

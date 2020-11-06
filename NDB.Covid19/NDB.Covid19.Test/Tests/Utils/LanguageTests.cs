using System.Globalization;
using NDB.Covid19.Config;
using NDB.Covid19.Models.DTOsForServer;
using Xunit;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class LanguageTests
    {
        public LanguageTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public void GetLanguage_NotNullOrEmpty()
        {
            string language = LocalesService.GetLanguage();
            Assert.NotNull(language);
            Assert.NotEmpty(language);
        }

        [Theory]
        [InlineData("es-ES")]
        [InlineData("zh-CN")]
        [InlineData("fr-FR")]
        [InlineData("fr-LU")]
        [InlineData("hr-HR")]
        [InlineData("sl-SI")]
        public void GetLanguage_NotSupportedCulture_ReturnsDefault(string culture)
        {
            CultureInfo initialCulture = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = new CultureInfo(culture);

            string language = LocalesService.GetLanguage();
            Assert.Equal(Conf.DEFAULT_LANGUAGE, language);

            CultureInfo.CurrentCulture = initialCulture;
        }

        [Theory]
        [InlineData("nb")]
        [InlineData("nb-NO")]
        [InlineData("en")]
        [InlineData("en-US")]
        [InlineData("en-GB")]
        public void GetLanguage_SupportedCulture_ReturnsCorrect(string culture)
        {
            CultureInfo initialCulture = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = new CultureInfo(culture);

            string language = LocalesService.GetLanguage();
            Assert.Equal(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, language);

            CultureInfo.CurrentCulture = initialCulture;
        }

        [Fact]
        public void CountryDetailsDTO_GetName_NotNullOrEmpty()
        {
            CountryDetailsDTO country = new CountryDetailsDTO()
            {
                Name_NB = "Norge",
                Name_NN = "Noreg",
                Name_EN = "Norway",
                Code = "no"
            };

            Assert.NotNull(country.GetName());
            Assert.NotEmpty(country.GetName());
        }

        [Theory]
        [InlineData("nb", "Norge", "Noreg", "Norway", "Norge")]
        [InlineData("en", "England", "England", "England", "England")]
        [InlineData("nb", "Name_NB", "Name_NN", "Name_EN", "Name_NB")]
        [InlineData("nn", "Name_NB", "Name_NN", "Name_EN", "Name_NN")]
        [InlineData("en", "Name_NB", "Name_NN", "Name_EN", "Name_EN")]
        [InlineData("pl", "Name_NB", "Name_NN", "Name_EN", "Name_NB")]
        [InlineData("es", "Name_NB", "Name_NN", "Name_EN", "Name_NB")]
        public void CountryDetailsDTO_GetName_ReturnsCorrect(string code, string nb, string nn, string en, string result)
        {
            CultureInfo initialCulture = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = new CultureInfo(code);

            CountryDetailsDTO country = new CountryDetailsDTO()
            {
                Name_NB = nb,
                Name_NN = nn,
                Name_EN = en,
                Code = code
            };

            Assert.Equal(result, country.GetName());

            CultureInfo.CurrentCulture = initialCulture;
        }
    }
}

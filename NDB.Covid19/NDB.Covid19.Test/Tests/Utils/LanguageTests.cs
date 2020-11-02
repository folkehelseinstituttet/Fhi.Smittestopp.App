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
        [InlineData("da")]
        [InlineData("da-DK")]
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
                Code = "da",
                Name_DA = "Danmark",
                Name_EN = "Denmark"
            };

            Assert.NotNull(country.GetName());
            Assert.NotEmpty(country.GetName());
        }

        [Theory]
        [InlineData("da", "Danmark", "Denmark", "Danmark")]
        [InlineData("en", "England", "England", "England")]
        [InlineData("da", "Name_DA", "Name_EN", "Name_DA")]
        [InlineData("en", "Name_DA", "Name_EN", "Name_EN")]
        [InlineData("pl", "Name_DA", "Name_EN", "Name_DA")]
        [InlineData("es", "Name_DA", "Name_EN", "Name_DA")]
        public void CountryDetailsDTO_GetName_ReturnsCorrect(string code, string da, string en, string result)
        {
            CultureInfo initialCulture = CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = new CultureInfo(code);

            CountryDetailsDTO country = new CountryDetailsDTO()
            {
                Code = code,
                Name_DA = da,
                Name_EN = en
            };

            Assert.Equal(result, country.GetName());

            CultureInfo.CurrentCulture = initialCulture;
        }
    }
}

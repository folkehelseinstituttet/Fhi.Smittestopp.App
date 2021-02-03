using System.Globalization;
using NDB.Covid19.Configuration;
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
    }
}

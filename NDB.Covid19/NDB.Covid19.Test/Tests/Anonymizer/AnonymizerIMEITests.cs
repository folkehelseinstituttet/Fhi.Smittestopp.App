using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerIMEITests
    {
        private string replacementImei = "xxxxxxxxxxxxxxx";

        [Fact]
        public void ImeiShouldBeEmpty()
        {
            string imeiNumber = "";
            ReplaceIMEI(imeiNumber).Should().BeEmpty();
        }

        [Fact]
        public void CorrectImeiShouldBeHidden()
        {
            string imeiNumber = "490154203237518";
            ReplaceIMEI(imeiNumber).Should().Be(replacementImei);
        }

        [Fact]
        public void CorrectImeiChainShouldBeHidden()
        {
            string imeiNumber = "490154203237518";
            ReplaceIMEI($"{imeiNumber}{imeiNumber}").Should().Be($"{replacementImei}{replacementImei}");
            ReplaceIMEI($"{imeiNumber} {imeiNumber}").Should().Be($"{replacementImei} {replacementImei}");
        }

        [Fact]
        public void IncorrectImeiShouldBeShown()
        {
            string imeiNumber = "990000862471854";
            ReplaceIMEI(imeiNumber).Should().Be(imeiNumber);
        }

        [Fact]
        public void IncorrectImeiChainShouldBeShown()
        {
            string correctImeiNumber = "490154203237518";
            string incorrectImeiNumber = "990000862471854";
            ReplaceIMEI($"{incorrectImeiNumber}{incorrectImeiNumber}").Should().Be($"{incorrectImeiNumber}{incorrectImeiNumber}");
            ReplaceIMEI($"{incorrectImeiNumber} {incorrectImeiNumber}").Should().Be($"{incorrectImeiNumber} {incorrectImeiNumber}");

            ReplaceIMEI($"{correctImeiNumber}{incorrectImeiNumber}").Should().Be($"{replacementImei}{incorrectImeiNumber}");
            ReplaceIMEI($"{correctImeiNumber} {incorrectImeiNumber}").Should().Be($"{replacementImei} {incorrectImeiNumber}");

            ReplaceIMEI($"{incorrectImeiNumber}{correctImeiNumber}").Should().Be($"{incorrectImeiNumber}{replacementImei}");
            ReplaceIMEI($"{incorrectImeiNumber} {correctImeiNumber}").Should().Be($"{incorrectImeiNumber} {replacementImei}");
        }

        [Fact]
        public void CorrectImeiInStringShouldBeHidden()
        {
            string correctImeiNumber = "490154203237518";
            string testString1 = "Random test string";
            string testString2 = "Another random test string.";
            ReplaceIMEI($"{testString1}{correctImeiNumber}{testString2}").Should().Be($"{testString1}{replacementImei}{testString2}");
            ReplaceIMEI($"{testString1} {correctImeiNumber} {testString2}").Should().Be($"{testString1} {replacementImei} {testString2}");
        }
    }
}

using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerCprTests
    {
        private string replacementKebabCase = "xxxxxx-xxxx";
        private string replacementContinuousCase = "xxxxxxxxxx";
        private string replacementSpaceCase = "xxxxxx xxxx";

        [Fact]
        public void CprShouldBeEmpty()
        {
            string testCpr = "";
            ReplaceCpr(testCpr).Should().Be("");
        }

        [Fact]
        public void KebabCaseCprShouldBeHidden()
        {
            string testCpr = "010203-1234";
            ReplaceCpr(testCpr).Should().Be(replacementKebabCase);
        }

        [Fact]
        public void ContinuousCaseCprShouldBeHidden()
        {
            string testCpr = "0102031234";
            ReplaceCpr(testCpr).Should().Be(replacementContinuousCase);
        }

        [Fact]
        public void SpaceCaseCprShouldBeHidden()
        {
            string testCpr = "010203 1234";
            ReplaceCpr(testCpr).Should().Be(replacementSpaceCase);
        }

        [Fact]
        public void CprSpaceChainShouldBeHidden()
        {
            string testCpr = "010203 1234 010203 1234";
            ReplaceCpr(testCpr).Should().Be($"{replacementSpaceCase} {replacementSpaceCase}");
            testCpr = "010203 1234010203 1234";
            ReplaceCpr(testCpr).Should().Be($"{replacementSpaceCase}{replacementSpaceCase}");
        }

        [Fact]
        public void CprContinousChainShouldBeHidden()
        {
            string testCpr = "0102031234 0102031234";
            ReplaceCpr(testCpr).Should().Be($"{replacementContinuousCase} {replacementContinuousCase}");
            testCpr = "01020312340102031234";
            ReplaceCpr(testCpr).Should().Be($"{replacementContinuousCase}{replacementContinuousCase}");
        }

        [Fact]
        public void CprKebabChainShouldBeHidden()
        {
            string testCpr = "010203-1234 010203-1234";
            ReplaceCpr(testCpr).Should().Be($"{replacementKebabCase} {replacementKebabCase}");

            testCpr = "010203-1234010203-1234";
            ReplaceCpr(testCpr).Should().Be($"{replacementKebabCase}{replacementKebabCase}");
        }

        [Fact]
        public void CprCombinedChainShouldBeHidden()
        {
            string testCpr1 = "010203-1234";
            string testCpr2 = "0102031234";
            string testCpr3 = "010203 1234";

            ReplaceCpr(testCpr1 + testCpr2).Should().Be($"{replacementKebabCase}{replacementContinuousCase}");
            ReplaceCpr(testCpr1 + testCpr3).Should().Be($"{replacementKebabCase}{replacementSpaceCase}");
            ReplaceCpr(testCpr2 + testCpr3).Should().Be($"{replacementContinuousCase}{replacementSpaceCase}");
            ReplaceCpr(testCpr1 + " " + testCpr2).Should().Be($"{replacementKebabCase} {replacementContinuousCase}");
            ReplaceCpr(testCpr1 + " " + testCpr3).Should().Be($"{replacementKebabCase} {replacementSpaceCase}");
            ReplaceCpr(testCpr2 + " " + testCpr3).Should().Be($"{replacementContinuousCase} {replacementSpaceCase}");
        }

        [Fact]
        public void CprInRandomStringChainShouldBeHidden()
        {
            string testCpr1 = "010203-1234";
            string testString1 = "RanDoM StrInG";
            string testString2 = "An0Th3r RanDoM StrInG";

            ReplaceCpr(testString1+testCpr1+testString2).Should().Be($"{testString1}{replacementKebabCase}{testString2}");
            ReplaceCpr($"{testString1} {testCpr1} {testString2}").Should().Be($"{testString1} {replacementKebabCase} {testString2}");

            testString1 = "010203";
            testString2 = "1234";

            ReplaceCpr(testString1 + testCpr1 + testString2).Should().Be($"{testString1}{replacementKebabCase}{testString2}");
            ReplaceCpr($"{testString1} {testCpr1} {testString2}").Should().Be($"{testString1} {replacementKebabCase} {testString2}");
        }
    }
}
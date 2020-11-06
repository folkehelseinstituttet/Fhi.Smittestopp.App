using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerEmailTests
    {
        [Fact]
        public void EmailShouldBeEmpty()
        {
            string testEmail = "";
            ReplaceEmailAddress(testEmail).Should().Be("");
        }

        [Fact]
        public void EmailShouldBeHidden()
        {
            string testEmail = "test@gmail.com";
            ReplaceEmailAddress(testEmail).Should().Be($"****@*****.com");
        }

        [Fact]
        public void EmailTwoPartsShouldBeHidden()
        {
            string testEmail = "test@gmail.co.uk";
            ReplaceEmailAddress(testEmail).Should().Be($"****@*****.co.uk");
        }

        [Fact]
        public void ShortEmailShouldBeHidden()
        {
            string testEmail = "a@a.a";
            ReplaceEmailAddress(testEmail).Should().Be($"*@*.a");
        }

        [Fact]
        public void LongEmailShouldBeHidden()
        {
            string prefix = "very-long-and-complicate-email_address";
            string postfix =
                "and-here-we-have-long-and_complicated_domain_name.with_dots.and_basically.few_of.them.com";
            string mail = $"{prefix}@{postfix}";
            ReplaceEmailAddress(mail).Should()
                .Be(
                    $"{new string('*', prefix.Length)}@{new string('*', postfix.IndexOf('.'))}{postfix.Substring(postfix.IndexOf('.'))}");
        }

        [Fact]
        public void NorwegianEmailShouldBeHidden()
        {
            string testEmail = "test@denmark.gov.no";
            ReplaceEmailAddress(testEmail).Should().Be($"****@*******.gov.no");
        }

        [Fact]
        public void EmailChainShouldBeHidden()
        {
            string testEmail1 = "test@test.com";
            string testEmail2 = "test@test.com";
            ReplaceEmailAddress($"{testEmail1}{testEmail2}").Should().Be($"****@************@****.com");
            ReplaceEmailAddress($"{testEmail1} {testEmail2}").Should().Be($"****@****.com ****@****.com");
        }

        [Fact]
        public void EmailInStringShouldBeHidden()
        {
            string testString = "My email is:";
            string testEmail1 = "test@test.com";
            ReplaceEmailAddress($"{testString} {testEmail1}").Should().Be($"{testString} ****@****.com");
        }
    }
}
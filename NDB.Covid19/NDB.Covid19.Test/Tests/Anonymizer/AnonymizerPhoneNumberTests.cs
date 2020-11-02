using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerPhoneNumberTests
    {
        private string replacementShort = "xxxxxxxx";
        private string replacementFourSpaced = "xx-xx-xx-xx";
        private string replacementTwoSpaced = "xxxx-xxxx";
        private string replacementDirectional = "+xx";

        [Fact]
        public void PhoneNumberShouldBeEmpty()
        {
            string testPhoneNumberEmpty = "";
            ReplacePhoneNumber(testPhoneNumberEmpty).Should().Be(testPhoneNumberEmpty);
        }

        [Fact]
        public void PhoneNumberShouldBeHidden()
        {
            string directional = "";
            string testPhoneNumber1 = $"{directional}11111111";
            string testPhoneNumber2 = $"{directional}11 11 11 11";
            string testPhoneNumber3 = $"{directional}1111 1111";
            string testPhoneNumber4 = $"{directional}11-11-11-11";
            string testPhoneNumber5 = $"{directional}1111-1111";
            ReplacePhoneNumber(testPhoneNumber1).Should().Be($"{replacementShort}");
            ReplacePhoneNumber(testPhoneNumber2).Should().Be($"{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber3).Should().Be($"{replacementTwoSpaced}");
            ReplacePhoneNumber(testPhoneNumber4).Should().Be($"{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber5).Should().Be($"{replacementTwoSpaced}");

            directional = "+45";
            testPhoneNumber1 = $"{directional}11111111";
            testPhoneNumber2 = $"{directional}11 11 11 11";
            testPhoneNumber3 = $"{directional}1111 1111";
            testPhoneNumber4 = $"{directional}11-11-11-11";
            testPhoneNumber5 = $"{directional}1111-1111";
            ReplacePhoneNumber(testPhoneNumber1).Should().Be($"{replacementDirectional}{replacementShort}");
            ReplacePhoneNumber(testPhoneNumber2).Should()
                .Be($"{replacementDirectional}{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber3).Should()
                .Be($"{replacementDirectional}{replacementTwoSpaced}");
            ReplacePhoneNumber(testPhoneNumber4).Should()
                .Be($"{replacementDirectional}{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber5).Should()
                .Be($"{replacementDirectional}{replacementTwoSpaced}");

            directional = "0045";
            testPhoneNumber1 = $"{directional}11111111";
            testPhoneNumber2 = $"{directional}11 11 11 11";
            testPhoneNumber3 = $"{directional}1111 1111";
            testPhoneNumber4 = $"{directional}11-11-11-11";
            testPhoneNumber5 = $"{directional}1111-1111";
            ReplacePhoneNumber(testPhoneNumber1).Should().Be($"{replacementDirectional}{replacementShort}");
            ReplacePhoneNumber(testPhoneNumber2).Should()
                .Be($"{replacementDirectional}{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber3).Should()
                .Be($"{replacementDirectional}{replacementTwoSpaced}");
            ReplacePhoneNumber(testPhoneNumber4).Should()
                .Be($"{replacementDirectional}{replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber5).Should()
                .Be($"{replacementDirectional}{replacementTwoSpaced}");

            directional = "+45";
            testPhoneNumber1 = $"{directional} 11111111";
            testPhoneNumber2 = $"{directional} 11 11 11 11";
            testPhoneNumber3 = $"{directional} 1111 1111";
            testPhoneNumber4 = $"{directional} 11-11-11-11";
            testPhoneNumber5 = $"{directional} 1111-1111";
            ReplacePhoneNumber(testPhoneNumber1).Should().Be($"{replacementDirectional} {replacementShort}");
            ReplacePhoneNumber(testPhoneNumber2).Should()
                .Be($"{replacementDirectional} {replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber3).Should()
                .Be($"{replacementDirectional} {replacementTwoSpaced}");
            ReplacePhoneNumber(testPhoneNumber4).Should()
                .Be($"{replacementDirectional} {replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber5).Should()
                .Be($"{replacementDirectional} {replacementTwoSpaced}");

            directional = "0045";
            testPhoneNumber1 = $"{directional} 11111111";
            testPhoneNumber2 = $"{directional} 11 11 11 11";
            testPhoneNumber3 = $"{directional} 1111 1111";
            testPhoneNumber4 = $"{directional} 11-11-11-11";
            testPhoneNumber5 = $"{directional} 1111-1111";
            ReplacePhoneNumber(testPhoneNumber1).Should().Be($"{replacementDirectional} {replacementShort}");
            ReplacePhoneNumber(testPhoneNumber2).Should()
                .Be($"{replacementDirectional} {replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber3).Should()
                .Be($"{replacementDirectional} {replacementTwoSpaced}");
            ReplacePhoneNumber(testPhoneNumber4).Should()
                .Be($"{replacementDirectional} {replacementFourSpaced}");
            ReplacePhoneNumber(testPhoneNumber5).Should()
                .Be($"{replacementDirectional} {replacementTwoSpaced}");
        }

        [Fact]
        public void PhoneNumberInStringChainShouldBeHidden()
        {
            string testString1 = "RanDoM StrInG";
            string testString2 = "An0Th3r RanDoM StrInG";
            string directional = "+45";
            string phoneNumber = $"{directional} 11-11-11-11";
            ReplacePhoneNumber($"{testString1}{phoneNumber}{testString2}").Should()
                .Be($"{testString1}{replacementDirectional} {replacementFourSpaced}{testString2}");
            ReplacePhoneNumber($"{testString1} {phoneNumber} {testString2}").Should()
                .Be($"{testString1} {replacementDirectional} {replacementFourSpaced} {testString2}");
        }
    }
}
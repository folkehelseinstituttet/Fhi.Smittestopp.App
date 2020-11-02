using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonyimzerAllTests
    {
        private string replacementCpr = "xxxxxx-xxxx";
        private string replacementEmail = "****@*****.com";
        private string replacementImei = "xxxxxxxxxxxxxxx";
        string replacementMacAddress = "xx:xx:xx:xx";
        private string replacementPhoneNumber = "&amp;#43;xxxxxxxxxx";

        [Fact]
        public void AllShouldBeEmpty()
        {
            string testString = "";
            RedactText(testString).Should().Be("");
        }

        [Fact]
        public void AllShouldBeHidden()
        {
            string testCpr = "010203-1234";
            string testEmail = "test@gmail.com";
            string imeiNumber = "490154203237518";
            string macAddress = "00-22-64-a6-c4-f0";
            string phoneNumber = "+4511111111";
            string testString = $"{testCpr}\n" +
                                $"{testEmail}\n" +
                                $"{imeiNumber}\n" +
                                $"{macAddress}\n" +
                                $"{phoneNumber}";

            string resultString = $"{replacementCpr} " +
                                  $"{replacementEmail} " +
                                  $"{replacementImei} " +
                                  $"{replacementMacAddress} " +
                                  $"{replacementPhoneNumber}";

            RedactText(testString).Should().Be(resultString);
        }

        [Fact]
        public void AllWithStringsShouldBeHidden()
        {
            string testCpr = "010203-1234";
            string testEmail = "test@gmail.com";
            string imeiNumber = "490154203237518";
            string macAddress = "00-22-64-a6-c4-f0";
            string phoneNumber = "+4511111111";
            string testString = $"Cpr: {testCpr}\n" +
                                $"Email: {testEmail}\n" +
                                $"IMEI: {imeiNumber}\n" +
                                $"MacAddress: {macAddress}\n" +
                                $"Phone: {phoneNumber}";

            string resultString = $"Cpr: {replacementCpr} " +
                                  $"Email: {replacementEmail} " +
                                  $"IMEI: {replacementImei} " +
                                  $"MacAddress: {replacementMacAddress} " +
                                  $"Phone: {replacementPhoneNumber}";

            RedactText(testString).Should().Be(resultString);

            string testString2 = $"Cpr:{testCpr}\n" +
                                $"Email:{testEmail}\n" +
                                $"IMEI:{imeiNumber}\n" +
                                $"MacAddress:{macAddress}\n" +
                                $"Phone:{phoneNumber}";

            string resultString2 = $"Cpr:{replacementCpr} " +
                                  $"Email:{replacementEmail} " +
                                  $"IMEI:{replacementImei} " +
                                  $"MacAddress:{replacementMacAddress} " +
                                  $"Phone:{replacementPhoneNumber}";

            RedactText(testString2).Should().Be(resultString2);
        }
    }
}
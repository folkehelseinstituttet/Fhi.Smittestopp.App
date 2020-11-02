using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using static NDB.Covid19.Utils.Anonymizer;

namespace NDB.Covid19.Test.Tests.Anonymizer
{
    public class AnonymizerMacAddressTests
    {
        private string replacementMacAddress = "xx:xx:xx:xx";
        [Fact]
        public void MacAddressShouldBeEmpty()
        {
            string macAddress = "";
            ReplaceMacAddress(macAddress).Should().BeEmpty();
        }

        [Fact]
        public void CorrectMacAddressShouldBeHidden()
        {
            List<string> macAddresses = new List<string>
            {
                "00-22-64-a6-c4-f0",
                "00:22:64:a6:c4:f0",
                "00.22.64.a6.c4.f0",
                "002-264-a6c-4f0",
                "002:264:a6c:4f0",
                "002.264.a6c.4f0"
            };

            foreach (var address in macAddresses)
            {
                ReplaceMacAddress(address).Should().Be(replacementMacAddress);
            }
        }

        [Fact]
        public void IncorrectMacAddressShouldBeShown()
        {
            List<string> macAddresses = new List<string>
            {
                "00-22-64-a6-c4-fg",
                "00:22:64:a6:c4:fg",
                "00.22.64.a6.c4.fg",
                "002-264-a6c-4fg",
                "002:264:a6c:4fg",
                "002.264.a6c.4fg"
            };

            for (var index = 0; index < macAddresses.Count; index++)
            {
                var address = macAddresses[index];
                ReplaceMacAddress(address).Should().Be(address);
            }
        }

        [Fact]
        public void MacAddressChainShouldBeHidden()
        {
            List<string> macAddresses = new List<string>
            {
                "00-22-64-a6-c4-f0",
                "00:22:64:a6:c4:f0",
                "00.22.64.a6.c4.f0",
                "002-264-a6c-4f0",
                "002:264:a6c:4f0",
                "002.264.a6c.4f0"
            };

            ReplaceMacAddress(macAddresses.Aggregate("", (s, s1) => s + s1))
                .Should().Be(string.Concat(Enumerable.Repeat(replacementMacAddress, 6)));
        }
    }
}

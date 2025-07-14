using CryptoSuite48.Extensions;
using System;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.Extensions
{
    public class ByteExtensionsTests
    {
        [Fact]
        public void ToBase64_ShouldEncodeCorrectly()
        {
            var input = Encoding.UTF8.GetBytes("test123");
            Assert.Equal(Convert.ToBase64String(input), input.ToBase64());
        }

        [Fact]
        public void ToHex_ShouldConvertCorrectly()
        {
            var input = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            Assert.Equal("DEADBEEF", input.ToHex());
        }

        [Fact]
        public void ToUtf8String_ShouldDecodeCorrectly()
        {
            var input = Encoding.UTF8.GetBytes("測試");
            Assert.Equal("測試", input.ToUtf8String());
        }
    }
}
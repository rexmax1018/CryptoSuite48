using CryptoSuite48.Extensions;
using System;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void FromBase64_ShouldDecodeCorrectly()
        {
            var str = "dGVzdDEyMw=="; // "test123"
            Assert.Equal("test123", Encoding.UTF8.GetString(str.FromBase64()));
        }

        [Fact]
        public void FromHex_ShouldConvertCorrectly()
        {
            var hex = "414243";
            Assert.Equal("ABC", Encoding.UTF8.GetString(hex.FromHex()));
        }

        [Fact]
        public void FromHex_ShouldThrowIfOddLength()
        {
            Assert.Throws<FormatException>(() => "ABC".FromHex());
        }

        [Fact]
        public void ToBytes_ShouldConvertUtf8ByDefault()
        {
            var str = "hello";
            Assert.Equal(Encoding.UTF8.GetBytes(str), str.ToBytes());
        }

        [Fact]
        public void JsonSerialization_ShouldWorkCorrectly()
        {
            var obj = new { Name = "Alice", Age = 30 };
            var json = obj.ToJson();
            var restored = json.FromJson<dynamic>();
            Assert.Equal("Alice", (string)restored.Name);
        }
    }
}
using CryptoSuite48.Helpers;
using System;
using Xunit;

namespace CryptoSuite48.Tests.Helpers
{
    /// <summary>
    /// 測試 Base64Utils 編碼與解碼功能。
    /// </summary>
    public class Base64UtilsTests
    {
        [Fact(DisplayName = "標準 Base64 編碼與解碼應一致")]
        public void EncodeDecode_ShouldMatchOriginalData()
        {
            // Arrange
            byte[] originalData = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            // Act
            string base64 = Base64Utils.Encode(originalData);
            byte[] decoded = Base64Utils.Decode(base64);

            // Assert
            Assert.Equal(originalData, decoded);
        }

        [Fact(DisplayName = "URL Safe Base64 編碼與解碼應一致")]
        public void UrlSafeEncodeDecode_ShouldMatchOriginalData()
        {
            // Arrange
            byte[] originalData = new byte[] { 250, 200, 100, 50 };

            // Act
            string urlSafe = Base64Utils.EncodeUrlSafe(originalData);
            byte[] decoded = Base64Utils.DecodeUrlSafe(urlSafe);

            // Assert
            Assert.Equal(originalData, decoded);
        }

        [Theory(DisplayName = "Decode 應丟出例外 when Base64 為 null 或空字串")]
        [MemberData(nameof(TestStringData.NullOrWhitespaceStrings), MemberType = typeof(TestStringData))]
        public void Decode_InvalidBase64_ShouldThrow(string base64)
        {
            Assert.Throws<ArgumentException>(() => Base64Utils.Decode(base64));
        }

        [Theory(DisplayName = "DecodeUrlSafe 應丟出例外 when URL Safe Base64 為 null 或空字串")]
        [MemberData(nameof(TestStringData.NullOrWhitespaceStrings), MemberType = typeof(TestStringData))]
        public void DecodeUrlSafe_InvalidInput_ShouldThrow(string urlSafeBase64)
        {
            Assert.Throws<ArgumentException>(() => Base64Utils.DecodeUrlSafe(urlSafeBase64));
        }

        [Fact(DisplayName = "Encode 應丟出例外 when 資料為 null 或空陣列")]
        public void Encode_InvalidData_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => Base64Utils.Encode(null));
            Assert.Throws<ArgumentException>(() => Base64Utils.Encode(Array.Empty<byte>()));
        }
    }
}
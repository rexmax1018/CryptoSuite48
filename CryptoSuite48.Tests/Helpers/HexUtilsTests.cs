using CryptoSuite48.Helpers;
using System;
using Xunit;

namespace CryptoSuite48.Tests.Helpers
{
    /// <summary>
    /// 測試 HexUtils 編碼與解碼功能。
    /// </summary>
    public class HexUtilsTests
    {
        [Fact(DisplayName = "Hex Encode / Decode 應與原始資料一致")]
        public void EncodeDecode_ShouldMatchOriginal()
        {
            // Arrange
            byte[] data = new byte[] { 0xAB, 0xCD, 0xEF, 0x01 };

            // Act
            string hex = HexUtils.Encode(data); // 預設為大寫
            byte[] result = HexUtils.Decode(hex);

            // Assert
            Assert.Equal(data, result);
        }

        [Fact(DisplayName = "Hex Encode 輸出應為大寫格式")]
        public void Encode_ShouldReturnUppercaseHex()
        {
            // Arrange
            byte[] data = new byte[] { 0x12, 0xAF };

            // Act
            string hex = HexUtils.Encode(data);

            // Assert
            Assert.Equal("12AF", hex);
        }

        [Fact(DisplayName = "Hex Encode 輸出應為小寫格式")]
        public void Encode_ShouldReturnLowercaseHex_WhenLowercaseOptionIsTrue()
        {
            // Arrange
            byte[] data = new byte[] { 0x12, 0xAF };

            // Act
            string hex = HexUtils.Encode(data, upperCase: false);

            // Assert
            Assert.Equal("12af", hex);
        }

        [Theory(DisplayName = "Hex Decode 應能處理大小寫混合輸入")]
        [InlineData("A1B2C3")]
        [InlineData("a1b2c3")]
        [InlineData("A1b2C3")]
        public void Decode_MixedCaseHex_ShouldWork(string hex)
        {
            // Arrange
            byte[] expected = new byte[] { 0xA1, 0xB2, 0xC3 };

            // Act
            byte[] result = HexUtils.Decode(hex);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory(DisplayName = "Decode 應拋出例外 when 輸入為 null 或空白")]
        [MemberData(nameof(TestStringData.NullOrWhitespaceStrings), MemberType = typeof(TestStringData))]
        public void Decode_InvalidInput_ShouldThrowArgumentException(string hex)
        {
            Assert.Throws<ArgumentException>(() => HexUtils.Decode(hex));
        }

        [Fact(DisplayName = "Decode 應拋出例外 when Hex 長度為奇數")]
        public void Decode_OddLengthHex_ShouldThrowFormatException()
        {
            Assert.Throws<FormatException>(() => HexUtils.Decode("ABC"));
        }

        [Fact(DisplayName = "Decode 應拋出例外 when Hex 字串包含無效字元")]
        public void Decode_InvalidCharacters_ShouldThrowFormatException()
        {
            Assert.Throws<FormatException>(() => HexUtils.Decode("ZXY1")); // Z 不是合法 hex
        }

        [Fact(DisplayName = "Encode 應拋出例外 when 資料為 null 或空陣列")]
        public void Encode_InvalidInput_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => HexUtils.Encode(null));
            Assert.Throws<ArgumentException>(() => HexUtils.Encode(Array.Empty<byte>()));
        }
    }
}
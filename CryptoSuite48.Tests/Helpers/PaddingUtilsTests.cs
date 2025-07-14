using CryptoSuite48.Helpers;
using System;
using Xunit;

namespace CryptoSuite48.Tests.Helpers
{
    /// <summary>
    /// 測試 PaddingUtils 的補齊與去除功能。
    /// </summary>
    public class PaddingUtilsTests
    {
        [Theory(DisplayName = "ApplyPadding 產生正確的補齊資料")]
        [InlineData(16)]
        [InlineData(8)]
        public void ApplyPadding_ShouldReturnCorrectLength(int blockSize)
        {
            byte[] data = new byte[] { 0x01, 0x02, 0x03 };

            byte[] padded = PaddingUtils.ApplyPadding(data, blockSize);

            Assert.Equal(0, padded.Length % blockSize); // 補齊後長度應為區塊大小倍數
        }

        [Fact(DisplayName = "RemovePadding 應還原為原始資料")]
        public void RemovePadding_ShouldReturnOriginal()
        {
            byte[] original = new byte[] { 0x10, 0x20, 0x30 };
            byte[] padded = PaddingUtils.ApplyPadding(original, 8);

            byte[] result = PaddingUtils.RemovePadding(padded);

            Assert.Equal(original, result);
        }

        [Fact(DisplayName = "RemovePadding 應拋出例外 when padding 格式不合法")]
        public void RemovePadding_InvalidFormat_ShouldThrow()
        {
            byte[] invalid = new byte[] { 0x01, 0x02, 0x03, 0x05 };

            Assert.Throws<FormatException>(() => PaddingUtils.RemovePadding(invalid));
        }

        [Fact(DisplayName = "RemovePadding 應拋出例外 when padding 長度超過資料長度")]
        public void RemovePadding_InvalidPaddingLength_ShouldThrow()
        {
            // 長度為 2，但 padding byte 為 16（超過上限）
            byte[] invalid = new byte[] { 0x00, 0x10 };

            Assert.Throws<FormatException>(() => PaddingUtils.RemovePadding(invalid));
        }

        [Fact(DisplayName = "ApplyPadding 應拋出例外 when blockSize 非法")]
        public void ApplyPadding_InvalidBlockSize_ShouldThrow()
        {
            byte[] data = new byte[] { 0x01, 0x02 };

            Assert.Throws<ArgumentOutOfRangeException>(() => PaddingUtils.ApplyPadding(data, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => PaddingUtils.ApplyPadding(data, 300));
        }

        [Fact(DisplayName = "ApplyPadding / RemovePadding 應拋出例外 when 輸入為 null")]
        public void NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => PaddingUtils.ApplyPadding(null, 16));
            Assert.Throws<ArgumentException>(() => PaddingUtils.RemovePadding(null));
        }
    }
}
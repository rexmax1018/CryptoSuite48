using System;

namespace CryptoSuite48.Helpers
{
    /// <summary>
    /// 提供區塊加密中使用的 Padding 與 Unpadding 工具。
    /// 預設使用 PKCS#7 規則。
    /// </summary>
    public static class PaddingUtils
    {
        /// <summary>
        /// 對資料進行 PKCS#7 Padding，使其長度為區塊大小的整數倍。
        /// </summary>
        /// <param name="data">原始資料。</param>
        /// <param name="blockSize">區塊大小（以 byte 為單位）。</param>
        /// <returns>補齊後的資料。</returns>
        public static byte[] ApplyPadding(byte[] data, int blockSize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (blockSize <= 0 || blockSize > 255)
                throw new ArgumentOutOfRangeException(nameof(blockSize), "區塊大小必須介於 1~255 之間");

            int paddingLength = blockSize - (data.Length % blockSize);
            byte paddingByte = (byte)paddingLength;

            byte[] padded = new byte[data.Length + paddingLength];
            Buffer.BlockCopy(data, 0, padded, 0, data.Length);
            for (int i = data.Length; i < padded.Length; i++)
                padded[i] = paddingByte;

            return padded;
        }

        /// <summary>
        /// 去除資料末端的 PKCS#7 Padding。
        /// </summary>
        /// <param name="paddedData">已補齊的資料。</param>
        /// <returns>去除補齊後的原始資料。</returns>
        public static byte[] RemovePadding(byte[] paddedData)
        {
            if (paddedData == null || paddedData.Length == 0)
                throw new ArgumentException("資料不可為空", nameof(paddedData));

            byte paddingLength = paddedData[paddedData.Length - 1];

            if (paddingLength <= 0 || paddingLength > paddedData.Length)
                throw new FormatException("Padding 資料不合法");

            for (int i = paddedData.Length - paddingLength; i < paddedData.Length; i++)
            {
                if (paddedData[i] != paddingLength)
                    throw new FormatException("Padding 格式錯誤");
            }

            byte[] original = new byte[paddedData.Length - paddingLength];
            Buffer.BlockCopy(paddedData, 0, original, 0, original.Length);
            return original;
        }
    }
}
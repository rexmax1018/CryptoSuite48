using System;
using System.Globalization;
using System.Text;

namespace CryptoSuite48.Helpers
{
    /// <summary>
    /// 提供 Hex（十六進位）編碼與解碼的工具方法。
    /// </summary>
    public static class HexUtils
    {
        /// <summary>
        /// 將位元組陣列轉換為十六進位字串。
        /// </summary>
        /// <param name="data">要編碼的位元組陣列。</param>
        /// <param name="upperCase">是否輸出大寫格式（預設為 true）。</param>
        /// <returns>十六進位字串。</returns>
        public static string Encode(byte[] data, bool upperCase = true)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("輸入資料不可為空", nameof(data));

            var format = upperCase ? "X2" : "x2";
            var builder = new StringBuilder(data.Length * 2);

            foreach (var b in data)
            {
                builder.Append(b.ToString(format));
            }

            return builder.ToString();
        }

        /// <summary>
        /// 將十六進位字串轉換為位元組陣列。
        /// </summary>
        /// <param name="hex">要解碼的十六進位字串。</param>
        /// <returns>解碼後的位元組陣列。</returns>
        public static byte[] Decode(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Hex 字串不可為空", nameof(hex));

            if (hex.Length % 2 != 0)
                throw new FormatException("Hex 字串長度必須為偶數");

            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = byte.Parse(hex.Substring(i, 2), NumberStyles.HexNumber);
            }

            return bytes;
        }
    }
}
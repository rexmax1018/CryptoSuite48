using System;

namespace CryptoSuite48.Helpers
{
    /// <summary>
    /// 提供 Base64 編碼與解碼的工具方法。
    /// </summary>
    public static class Base64Utils
    {
        /// <summary>
        /// 將位元組陣列編碼為 Base64 字串。
        /// </summary>
        /// <param name="data">要編碼的資料。</param>
        /// <returns>Base64 編碼字串。</returns>
        public static string Encode(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("輸入資料不可為空", nameof(data));

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 將 Base64 字串解碼為位元組陣列。
        /// </summary>
        /// <param name="base64">Base64 編碼字串。</param>
        /// <returns>解碼後的位元組陣列。</returns>
        public static byte[] Decode(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 字串不可為空", nameof(base64));

            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// 將位元組陣列編碼為 URL Safe 的 Base64 字串（移除 padding 並替換 + /）。
        /// </summary>
        /// <param name="data">要編碼的資料。</param>
        /// <returns>URL Safe Base64 編碼字串。</returns>
        public static string EncodeUrlSafe(byte[] data)
        {
            var base64 = Encode(data);

            return base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        /// <summary>
        /// 將 URL Safe 的 Base64 字串還原為位元組陣列。
        /// </summary>
        /// <param name="urlSafeBase64">URL Safe Base64 字串。</param>
        /// <returns>解碼後的位元組陣列。</returns>
        public static byte[] DecodeUrlSafe(string urlSafeBase64)
        {
            if (string.IsNullOrWhiteSpace(urlSafeBase64))
                throw new ArgumentException("URL Safe Base64 字串不可為空", nameof(urlSafeBase64));

            string base64 = urlSafeBase64.Replace('-', '+').Replace('_', '/');

            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Decode(base64);
        }
    }
}
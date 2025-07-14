using Newtonsoft.Json;
using System;
using System.Text;

namespace CryptoSuite48.Extensions
{
    /// <summary>
    /// 提供 string 常用的擴充方法，包括 Base64/Hex 解碼、JSON 反序列化與轉 byte[]。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 將 Base64 字串還原為 byte[]。
        /// </summary>
        /// <param name="base64">Base64 字串</param>
        /// <returns>還原後的 byte 陣列</returns>
        public static byte[] FromBase64(this string base64) =>
            Convert.FromBase64String(base64);

        /// <summary>
        /// 將十六進位字串還原為 byte[]。
        /// </summary>
        /// <param name="hex">Hex 字串（必須為偶數長度）</param>
        /// <returns>還原後的 byte 陣列</returns>
        public static byte[] FromHex(this string hex)
        {
            if (hex.Length % 2 != 0)
                throw new FormatException("Hex string must have even length.");

            var result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);

            return result;
        }

        /// <summary>
        /// 將字串轉換為 byte[]，預設使用 UTF8 編碼。
        /// </summary>
        /// <param name="text">輸入字串</param>
        /// <param name="encoding">可選編碼（預設 UTF8）</param>
        /// <returns>對應的 byte 陣列</returns>
        public static byte[] ToBytes(this string text, Encoding encoding = null) =>
            (encoding ?? Encoding.UTF8).GetBytes(text);

        /// <summary>
        /// 將 JSON 字串反序列化為指定型別。
        /// </summary>
        /// <typeparam name="T">目標物件型別</typeparam>
        /// <param name="json">JSON 字串</param>
        /// <returns>反序列化後的物件</returns>
        public static T FromJson<T>(this string json)
        {
            T result = JsonConvert.DeserializeObject<T>(json);
            if (result == null)
            {
                throw new JsonException("反序列化失敗");
            }
            return result;
        }

        /// <summary>
        /// 將物件序列化為 JSON 字串。
        /// </summary>
        /// <param name="obj">要序列化的物件</param>
        /// <param name="indented">是否使用縮排格式</param>
        /// <returns>序列化後的 JSON 字串</returns>
        public static string ToJson(this object obj, bool indented = false) =>
            JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
    }
}
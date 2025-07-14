using System;
using System.Text;

namespace CryptoSuite48.Extensions
{
    /// <summary>
    /// 提供 byte[] 常用的擴充方法，包括編碼轉換與字串表示。
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// 將 byte[] 編碼為 Base64 字串。
        /// </summary>
        /// <param name="data">要轉換的 byte 陣列</param>
        /// <returns>Base64 編碼的字串</returns>
        public static string ToBase64(this byte[] data) =>
            Convert.ToBase64String(data);

        /// <summary>
        /// 將 byte[] 轉換為十六進位字串（大寫，不含連字號）。
        /// </summary>
        /// <param name="data">要轉換的 byte 陣列</param>
        /// <returns>Hex 字串</returns>
        public static string ToHex(this byte[] data) =>
            BitConverter.ToString(data).Replace("-", "");

        /// <summary>
        /// 將 byte[] 轉為 UTF8 編碼的字串。
        /// </summary>
        /// <param name="data">要轉換的 byte 陣列</param>
        /// <returns>對應的 UTF8 字串</returns>
        public static string ToUtf8String(this byte[] data) =>
            Encoding.UTF8.GetString(data);
    }
}
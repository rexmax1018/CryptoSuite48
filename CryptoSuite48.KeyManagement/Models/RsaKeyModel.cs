using System;

namespace CryptoSuite48.KeyManagement.Models
{
    /// <summary>
    /// RSA 金鑰模型，儲存 PEM 格式的公鑰與私鑰內容。
    /// </summary>
    public class RsaKeyModel
    {
        /// <summary>
        /// 公鑰（PEM 格式字串）
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// 私鑰（PEM 格式字串）
        /// </summary>
        public string PrivateKey { get; set; } = string.Empty;

        /// <summary>
        /// 金鑰長度（bits）
        /// </summary>
        public int KeySize { get; set; }

        /// <summary>
        /// 金鑰建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
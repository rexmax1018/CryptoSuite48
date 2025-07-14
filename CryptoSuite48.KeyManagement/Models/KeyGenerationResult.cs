using CryptoSuite48.KeyManagement.Enums;
using System;

namespace CryptoSuite48.KeyManagement.Models
{
    /// <summary>
    /// 表示一筆金鑰產生結果的資訊。
    /// </summary>
    public class KeyGenerationResult
    {
        /// <summary>
        /// 使用的金鑰演算法類型（AES、RSA、ECC）。
        /// </summary>
        public CryptoAlgorithmType Algorithm { get; set; }

        /// <summary>
        /// 儲存的金鑰檔案名稱（含副檔名）。
        /// </summary>
        public string KeyFileName { get; set; } = string.Empty;

        /// <summary>
        /// 儲存的金鑰完整路徑。
        /// </summary>
        public string KeyFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 金鑰產生的 UTC 時間。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 建立結果物件。
        /// </summary>
        public static KeyGenerationResult Create(CryptoAlgorithmType algorithm, string fileName, string fullPath)
        {
            return new KeyGenerationResult
            {
                Algorithm = algorithm,
                KeyFileName = fileName,
                KeyFilePath = fullPath,
                CreatedAt = DateTime.UtcNow
            };
        }

        public override string ToString()
        {
            return $"[{Algorithm}] {KeyFileName} @ {KeyFilePath} (UTC {CreatedAt:yyyy-MM-dd HH:mm:ss})";
        }
    }
}
using System;

namespace CryptoSuite48.KeyManagement.Models
{
    /// <summary>
    /// 表示對稱加密的金鑰與初始向量 (IV)。
    /// 用於 AES 加解密操作。
    /// </summary>
    public class SymmetricKeyModel
    {
        /// <summary>
        /// 對稱加密金鑰（以 byte[] 儲存）。
        /// </summary>
        public byte[] Key { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// 初始向量（IV），通常配合 CBC 模式使用。
        /// </summary>
        public byte[] IV { get; set; } = Array.Empty<byte>();
    }
}
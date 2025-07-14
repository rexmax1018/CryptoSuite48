using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.Services.Interfaces;

namespace CryptoSuite48.Extensions
{
    /// <summary>
    /// 提供對 CryptoService 的語法糖擴充，包括 Encrypt、Decrypt、Sign、Verify。
    /// 必須搭配 ICryptoService 實例使用。
    /// </summary>
    public static class CryptoExtensions
    {
        /// <summary>
        /// 使用指定演算法與金鑰模型對資料進行加密。
        /// </summary>
        /// <typeparam name="T">金鑰模型類型（例如 SymmetricKeyModel）</typeparam>
        /// <param name="data">原始資料</param>
        /// <param name="alg">加密演算法</param>
        /// <param name="keyModel">金鑰模型</param>
        /// <param name="service">CryptoService 實例</param>
        /// <returns>加密後資料</returns>
        public static byte[] EncryptWith<T>(this byte[] data, CryptoAlgorithmType alg, T keyModel, ICryptoService service)
            where T : class =>
            service.Encrypt(data, alg, keyModel);

        /// <summary>
        /// 使用指定演算法與金鑰模型對資料進行解密。
        /// </summary>
        public static byte[] DecryptWith<T>(this byte[] encrypted, CryptoAlgorithmType alg, T keyModel, ICryptoService service)
            where T : class =>
            service.Decrypt(encrypted, alg, keyModel);

        /// <summary>
        /// 使用指定演算法與私鑰模型對資料進行簽章。
        /// </summary>
        public static byte[] SignWith<T>(this byte[] data, CryptoAlgorithmType alg, T privateKey, ICryptoService service)
            where T : class =>
            service.Sign(data, alg, privateKey);

        /// <summary>
        /// 使用指定演算法與公鑰模型驗證簽章。
        /// </summary>
        public static bool VerifyWith<T>(this byte[] data, byte[] signature, CryptoAlgorithmType alg, T publicKey, ICryptoService service)
            where T : class =>
            service.Verify(data, signature, alg, publicKey);
    }
}
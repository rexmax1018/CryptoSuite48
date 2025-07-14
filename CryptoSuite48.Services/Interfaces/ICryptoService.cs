using CryptoSuite48.KeyManagement.Enums;

namespace CryptoSuite48.Services.Interfaces
{
    /// <summary>
    /// 提供加密、解密、簽章與驗章等整合功能的泛型介面。
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// 加密資料。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="data">原始資料</param>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="keyModel">金鑰模型</param>
        /// <returns>加密後的資料</returns>
        byte[] Encrypt<TKeyModel>(byte[] data, CryptoAlgorithmType algorithm, TKeyModel keyModel) where TKeyModel : class;

        /// <summary>
        /// 解密資料。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="encrypted">加密資料</param>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="keyModel">金鑰模型</param>
        /// <returns>解密後的原始資料</returns>
        byte[] Decrypt<TKeyModel>(byte[] encrypted, CryptoAlgorithmType algorithm, TKeyModel keyModel) where TKeyModel : class;

        /// <summary>
        /// 使用私鑰簽章。
        /// </summary>
        /// <typeparam name="TKeyModel">私鑰模型型別</typeparam>
        /// <param name="data">待簽名資料</param>
        /// <param name="algorithm">演算法類型</param>
        /// <param name="privateKeyModel">私鑰模型</param>
        /// <returns>簽章結果</returns>
        byte[] Sign<TKeyModel>(byte[] data, CryptoAlgorithmType algorithm, TKeyModel privateKeyModel) where TKeyModel : class;

        /// <summary>
        /// 驗證簽章。
        /// </summary>
        /// <typeparam name="TKeyModel">公鑰模型型別</typeparam>
        /// <param name="data">原始資料</param>
        /// <param name="signature">簽章資料</param>
        /// <param name="algorithm">演算法類型</param>
        /// <param name="publicKeyModel">公鑰模型</param>
        /// <returns>驗章是否成功</returns>
        bool Verify<TKeyModel>(byte[] data, byte[] signature, CryptoAlgorithmType algorithm, TKeyModel publicKeyModel) where TKeyModel : class;
    }
}
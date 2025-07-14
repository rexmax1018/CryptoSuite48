using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Models;

namespace CryptoSuite48.Services.Interfaces
{
    /// <summary>
    /// 提供加密金鑰的產生與載入等功能，支援多種演算法與模型類型。
    /// </summary>
    public interface ICryptoKeyService
    {
        /// <summary>
        /// 僅產生金鑰模型（不儲存）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">指定的加密演算法</param>
        /// <returns>產生的金鑰模型</returns>
        TModel GenerateKeyOnly<TModel>(CryptoAlgorithmType algorithm) where TModel : class;

        /// <summary>
        /// 產生金鑰並儲存為 JSON 檔案（同步）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="filePath">指定儲存路徑（可為 null 使用預設）</param>
        /// <returns>產生結果，包含檔案路徑與演算法類型</returns>
        KeyGenerationResult GenerateAndSaveKey<TModel>(CryptoAlgorithmType algorithm, string filePath = null) where TModel : class;

        /// <summary>
        /// 從檔案載入金鑰（同步）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="path">檔案路徑</param>
        /// <returns>載入的金鑰模型</returns>
        TModel LoadFromFile<TModel>(CryptoAlgorithmType algorithm, string path) where TModel : class;

        /// <summary>
        /// 從 Base64 字串還原金鑰模型
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">演算法類型</param>
        /// <param name="base64">Base64 編碼內容</param>
        /// <returns>還原的金鑰模型</returns>
        TModel LoadFromBase64<TModel>(CryptoAlgorithmType algorithm, string base64) where TModel : class;
    }
}
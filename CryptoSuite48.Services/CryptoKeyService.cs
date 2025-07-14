using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services.Interfaces;

namespace CryptoSuite48.Services
{
    /// <summary>
    /// 提供金鑰產生與載入服務，統一包裝各演算法產生器與載入器。
    /// </summary>
    public class CryptoKeyService : ICryptoKeyService
    {
        private readonly IKeyGeneratorFactory _generatorFactory;
        private readonly IKeyLoaderFactory _loaderFactory;

        /// <summary>
        /// 建構子注入 Factory 相依性。
        /// </summary>
        /// <param name="generatorFactory">金鑰產生器工廠</param>
        /// <param name="loaderFactory">金鑰載入器工廠</param>
        public CryptoKeyService(IKeyGeneratorFactory generatorFactory, IKeyLoaderFactory loaderFactory)
        {
            _generatorFactory = generatorFactory;
            _loaderFactory = loaderFactory;
        }

        /// <summary>
        /// 僅產生金鑰模型（不儲存）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法</param>
        /// <returns>產生的金鑰模型</returns>
        public TModel GenerateKeyOnly<TModel>(CryptoAlgorithmType algorithm) where TModel : class
        {
            var generator = _generatorFactory.Create<TModel>(algorithm);
            return generator.GenerateKeyOnly();
        }

        /// <summary>
        /// 產生金鑰並儲存為 JSON 檔案（同步）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="filePath">指定儲存路徑（可為 null 使用預設）</param>
        /// <returns>產生結果，包含檔案路徑與演算法類型</returns>
        public KeyGenerationResult GenerateAndSaveKey<TModel>(CryptoAlgorithmType algorithm, string filePath = null) where TModel : class
        {
            var generator = _generatorFactory.Create<TModel>(algorithm);
            return generator.GenerateAndSaveKey(filePath);
        }

        /// <summary>
        /// 從檔案載入金鑰（同步）
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="path">檔案路徑</param>
        /// <returns>載入的金鑰模型</returns>
        public TModel LoadFromFile<TModel>(CryptoAlgorithmType algorithm, string path) where TModel : class
        {
            var loader = _loaderFactory.Create<TModel>(algorithm);
            return loader.LoadFromFile(path);
        }

        /// <summary>
        /// 從 Base64 字串還原金鑰模型
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">演算法類型</param>
        /// <param name="base64">Base64 編碼內容</param>
        /// <returns>還原的金鑰模型</returns>
        public TModel LoadFromBase64<TModel>(CryptoAlgorithmType algorithm, string base64) where TModel : class
        {
            var loader = _loaderFactory.Create<TModel>(algorithm);
            return loader.LoadFromBase64(base64);
        }
    }
}
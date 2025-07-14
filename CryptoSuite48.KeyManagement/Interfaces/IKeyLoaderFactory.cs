using CryptoSuite48.KeyManagement.Enums;

namespace CryptoSuite48.KeyManagement.Interfaces
{
    /// <summary>
    /// 金鑰載入器工廠介面。
    /// </summary>
    public interface IKeyLoaderFactory
    {
        /// <summary>
        /// 建立對應演算法的金鑰載入器實例。
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">演算法類型</param>
        /// <returns>對應的金鑰載入器</returns>
        IKeyLoader<TModel> Create<TModel>(CryptoAlgorithmType algorithm) where TModel : class;
    }
}
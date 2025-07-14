using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.KeyGenerators;
using CryptoSuite48.KeyManagement.Models;
using System;

namespace CryptoSuite48.KeyManagement.Factories
{
    /// <summary>
    /// 根據指定加密演算法產生對應的金鑰產生器實例。
    /// </summary>
    public class KeyGeneratorFactory : IKeyGeneratorFactory
    {
        /// <summary>
        /// 建立對應演算法的金鑰產生器實例。
        /// </summary>
        /// <typeparam name="TModel">金鑰模型類型</typeparam>
        /// <param name="algorithm">加密演算法類型</param>
        /// <returns>對應的 IKeyGenerator 實例</returns>
        /// <exception cref="NotSupportedException">若不支援的演算法則拋出</exception>
        public IKeyGenerator<TModel> Create<TModel>(CryptoAlgorithmType algorithm) where TModel : class
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.AES:
                    if (typeof(TModel) == typeof(SymmetricKeyModel))
                    {
                        return (IKeyGenerator<TModel>)new AesKeyGenerator();
                    }
                    break; // 如果類型不匹配，則跳出 switch 讓其進入 default 處理

                case CryptoAlgorithmType.RSA:
                    if (typeof(TModel) == typeof(RsaKeyModel))
                    {
                        return (IKeyGenerator<TModel>)new RsaKeyGenerator();
                    }
                    break;

                case CryptoAlgorithmType.ECC:
                    if (typeof(TModel) == typeof(EccKeyModel))
                    {
                        return (IKeyGenerator<TModel>)new EccKeyGenerator();
                    }
                    break;

                default:
                    break; // 讓不支援的演算法或類型不匹配的情況進入此處的 throw
            }

            throw new NotSupportedException($"不支援的演算法或模型類型：{algorithm} → {typeof(TModel).Name}");
        }
    }
}
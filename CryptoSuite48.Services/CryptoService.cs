using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services.Interfaces;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptoSuite48.Services
{
    /// <summary>
    /// 提供 AES、RSA、ECC 的加解密、簽章與驗章等整合性功能。
    /// </summary>
    public class CryptoService : ICryptoService
    {
        /// <summary>
        /// 加密資料。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="data">要加密的資料</param>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="keyModel">金鑰模型</param>
        /// <returns>加密後資料</returns>
        public byte[] Encrypt<TKeyModel>(byte[] data, CryptoAlgorithmType algorithm, TKeyModel keyModel) where TKeyModel : class
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.AES:
                    if (keyModel is SymmetricKeyModel) // 檢查類型
                    {
                        SymmetricKeyModel aes = (SymmetricKeyModel)(object)keyModel; // 先轉為 object，再轉為目標類型
                        return AesEncrypt(data, aes);
                    }
                    break;

                case CryptoAlgorithmType.RSA:
                    if (keyModel is RsaKeyModel) // 檢查類型
                    {
                        RsaKeyModel rsa = (RsaKeyModel)(object)keyModel; // 先轉為 object，再轉為目標類型
                        return RsaEncrypt(data, rsa);
                    }
                    break;

                default:
                    break; // 讓不支援的演算法或類型不匹配的情況進入此處的 throw
            }

            throw new NotSupportedException($"Encrypt 不支援的演算法或金鑰類型: {algorithm}");
        }

        /// <summary>
        /// 解密資料。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="encrypted">已加密的資料</param>
        /// <param name="algorithm">加密演算法</param>
        /// <param name="keyModel">金鑰模型</param>
        /// <returns>解密後資料</returns>
        public byte[] Decrypt<TKeyModel>(byte[] encrypted, CryptoAlgorithmType algorithm, TKeyModel keyModel) where TKeyModel : class
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.AES:
                    if (keyModel is SymmetricKeyModel) // 檢查類型
                    {
                        SymmetricKeyModel aes = (SymmetricKeyModel)(object)keyModel; // 顯式轉換
                        return AesDecrypt(encrypted, aes);
                    }
                    break; // 如果類型不匹配，則跳出 switch 讓其進入 default 處理

                case CryptoAlgorithmType.RSA:
                    if (keyModel is RsaKeyModel) // 檢查類型
                    {
                        RsaKeyModel rsa = (RsaKeyModel)(object)keyModel; // 顯式轉換
                        return RsaDecrypt(encrypted, rsa);
                    }
                    break;

                default:
                    break; // 讓不支援的演算法或類型不匹配的情況進入此處的 throw
            }

            throw new NotSupportedException($"Decrypt 不支援的演算法或金鑰類型: {algorithm}");
        }

        /// <summary>
        /// 使用私鑰對資料進行簽章。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="data">原始資料</param>
        /// <param name="algorithm">簽章演算法</param>
        /// <param name="privateKeyModel">私鑰模型</param>
        /// <returns>簽章結果</returns>
        public byte[] Sign<TKeyModel>(byte[] data, CryptoAlgorithmType algorithm, TKeyModel privateKeyModel) where TKeyModel : class
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.RSA:
                    if (privateKeyModel is RsaKeyModel) // 檢查類型
                    {
                        RsaKeyModel rsa = (RsaKeyModel)(object)privateKeyModel; // 顯式轉換
                        return RsaSign(data, rsa);
                    }
                    break; // 如果類型不匹配，則跳出 switch 讓其進入 default 處理

                case CryptoAlgorithmType.ECC:
                    if (privateKeyModel is EccKeyModel) // 檢查類型
                    {
                        EccKeyModel ecc = (EccKeyModel)(object)privateKeyModel; // 顯式轉換
                        return EccSign(data, ecc);
                    }
                    break;

                default:
                    break; // 讓不支援的演算法或類型不匹配的情況進入此處的 throw
            }

            throw new NotSupportedException($"Sign 不支援的演算法或金鑰類型: {algorithm}");
        }

        /// <summary>
        /// 驗證簽章正確性。
        /// </summary>
        /// <typeparam name="TKeyModel">金鑰模型型別</typeparam>
        /// <param name="data">原始資料</param>
        /// <param name="signature">簽章資料</param>
        /// <param name="algorithm">驗章演算法</param>
        /// <param name="publicKeyModel">公鑰模型</param>
        /// <returns>是否驗證成功</returns>
        public bool Verify<TKeyModel>(byte[] data, byte[] signature, CryptoAlgorithmType algorithm, TKeyModel publicKeyModel) where TKeyModel : class
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.RSA:
                    if (publicKeyModel is RsaKeyModel) // 檢查類型
                    {
                        RsaKeyModel rsa = (RsaKeyModel)(object)publicKeyModel; // 顯式轉換
                        return RsaVerify(data, signature, rsa);
                    }
                    break; // 如果類型不匹配，則跳出 switch 讓其進入 default 處理

                case CryptoAlgorithmType.ECC:
                    if (publicKeyModel is EccKeyModel) // 檢查類型
                    {
                        EccKeyModel ecc = (EccKeyModel)(object)publicKeyModel; // 顯式轉換
                        return EccVerify(data, signature, ecc);
                    }
                    break;

                default:
                    break; // 讓不支援的演算法或類型不匹配的情況進入此處的 throw
            }

            throw new NotSupportedException($"Verify 不支援的演算法或金鑰類型: {algorithm}");
        }

        #region AES

        /// <summary>
        /// 使用 AES 對資料進行對稱加密。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="keyModel">對稱金鑰與 IV</param>
        /// <returns>加密後資料</returns>
        private byte[] AesEncrypt(byte[] data, SymmetricKeyModel keyModel)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = keyModel.Key;
                aes.IV = keyModel.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// 使用 AES 解密資料。
        /// </summary>
        /// <param name="encrypted">加密後資料</param>
        /// <param name="keyModel">對稱金鑰與 IV</param>
        /// <returns>解密後資料</returns>
        private byte[] AesDecrypt(byte[] encrypted, SymmetricKeyModel keyModel)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = keyModel.Key;
                aes.IV = keyModel.IV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                }
            }
        }

        #endregion AES

        #region RSA

        /// <summary>
        /// 使用 RSA 私鑰對資料簽章。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="keyModel">RSA 私鑰 PEM 格式</param>
        /// <returns>簽章資料</returns>
        private byte[] RsaSign(byte[] data, RsaKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入私鑰
            AsymmetricCipherKeyPair keyPair;
            using (var stringReader = new StringReader(keyModel.PrivateKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 會返回 AsymmetricCipherKeyPair 或 AsymmetricKeyParameter
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }

            // 2. 取得 RSA 私鑰參數
            RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;

            // 3. 創建簽章器
            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA"); // 使用 SHA256withRSA 簽章演算法
            signer.Init(true, privateKeyParams); // 初始化簽章器 (true 表示簽章)
            signer.BlockUpdate(data, 0, data.Length); // 提供要簽章的資料

            // 4. 生成簽章
            return signer.GenerateSignature();
        }

        /// <summary>
        /// 使用 RSA 公鑰驗證簽章。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="signature">簽章資料</param>
        /// <param name="keyModel">RSA 公鑰 PEM 格式</param>
        /// <returns>驗章是否成功</returns>
        private bool RsaVerify(byte[] data, byte[] signature, RsaKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入公鑰
            AsymmetricKeyParameter publicKeyParams;
            using (var stringReader = new StringReader(keyModel.PublicKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 會返回 AsymmetricKeyParameter (RsaKeyParameters 或 ECPublicKeyParameters)
                publicKeyParams = (AsymmetricKeyParameter)pemReader.ReadObject();
            }

            // 2. 創建簽章驗證器
            // "SHA256withRSA" 自動包含了 SHA256 雜湊和 PKCS1 填充
            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(false, publicKeyParams); // 初始化驗證器 (false 表示驗證)
            signer.BlockUpdate(data, 0, data.Length); // 提供原始資料

            // 3. 驗證簽章
            return signer.VerifySignature(signature);
        }

        /// <summary>
        /// 使用 RSA 公鑰加密資料（僅適合短資料）。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="keyModel">RSA 公鑰 PEM 格式</param>
        /// <returns>加密資料</returns>
        private byte[] RsaEncrypt(byte[] data, RsaKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入公鑰
            AsymmetricKeyParameter publicKeyParams;
            using (var stringReader = new StringReader(keyModel.PublicKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 會返回 AsymmetricKeyParameter (RsaKeyParameters 或 ECPublicKeyParameters)
                publicKeyParams = (AsymmetricKeyParameter)pemReader.ReadObject();
            }

            // 2. 創建 RSA 加密引擎並應用 PKCS#1 填充
            // Pkcs1Encoding 類別提供了 PKCS#1 v1.5 padding
            IAsymmetricBlockCipher cipher = new Pkcs1Encoding(new RsaEngine());
            cipher.Init(true, publicKeyParams); // 初始化為加密模式 (true 表示加密)

            // 3. 執行加密
            // RSA 加密有資料長度限制，如果原始資料超過金鑰長度減去填充位元組，需要分塊加密。
            // 這個範例假設資料長度在單次加密範圍內。
            return cipher.ProcessBlock(data, 0, data.Length);
        }

        /// <summary>
        /// 使用 RSA 私鑰解密資料。
        /// </summary>
        /// <param name="encrypted">加密後資料</param>
        /// <param name="keyModel">RSA 私鑰 PEM 格式</param>
        /// <returns>解密後資料</returns>
        private byte[] RsaDecrypt(byte[] encrypted, RsaKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入私鑰
            AsymmetricCipherKeyPair keyPair;
            using (var stringReader = new StringReader(keyModel.PrivateKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 通常會返回 AsymmetricCipherKeyPair 或 PrivateKeyInfo
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }

            // 2. 取得 RSA 私鑰參數
            RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;

            // 3. 創建 RSA 解密引擎並應用 PKCS#1 填充
            IAsymmetricBlockCipher cipher = new Pkcs1Encoding(new RsaEngine());
            cipher.Init(false, privateKeyParams); // 初始化為解密模式 (false 表示解密)

            // 4. 執行解密
            // RSA 解密後資料長度與金鑰長度相關，通常會得到原始資料
            return cipher.ProcessBlock(encrypted, 0, encrypted.Length);
        }

        #endregion RSA

        #region ECC

        /// <summary>
        /// 使用 ECC 私鑰簽章。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="keyModel">ECC 私鑰 PEM 格式</param>
        /// <returns>簽章資料</returns>
        private byte[] EccSign(byte[] data, EccKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入私鑰
            AsymmetricCipherKeyPair keyPair;
            using (var stringReader = new StringReader(keyModel.PrivateKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 通常會返回 AsymmetricCipherKeyPair 或 PrivateKeyInfo
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }

            // 2. 取得 ECC 私鑰參數
            ECPrivateKeyParameters privateKeyParams = (ECPrivateKeyParameters)keyPair.Private;

            // 3. 創建簽章器
            // "SHA256withECDSA" 表示使用 SHA256 雜湊演算法和 ECDSA 簽章演算法
            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(true, privateKeyParams); // 初始化簽章器 (true 表示簽章)
            signer.BlockUpdate(data, 0, data.Length); // 提供要簽章的資料

            // 4. 生成簽章
            return signer.GenerateSignature();
        }

        /// <summary>
        /// 使用 ECC 公鑰驗章。
        /// </summary>
        /// <param name="data">原始資料</param>
        /// <param name="signature">簽章資料</param>
        /// <param name="keyModel">ECC 公鑰 PEM 格式</param>
        /// <returns>驗章是否成功</returns>
        private bool EccVerify(byte[] data, byte[] signature, EccKeyModel keyModel)
        {
            // 1. 使用 Bouncy Castle 載入公鑰
            AsymmetricKeyParameter publicKeyParams;
            using (var stringReader = new StringReader(keyModel.PublicKey))
            {
                var pemReader = new PemReader(stringReader);
                // PemReader.ReadObject() 通常會返回 AsymmetricKeyParameter (ECPublicKeyParameters)
                publicKeyParams = (AsymmetricKeyParameter)pemReader.ReadObject();
            }

            // 2. 取得 ECC 公鑰參數
            ECPublicKeyParameters ecPublicKeyParams = (ECPublicKeyParameters)publicKeyParams;

            // 3. 創建簽章驗證器
            // "SHA256withECDSA" 表示使用 SHA256 雜湊演算法和 ECDSA 簽章演算法
            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(false, ecPublicKeyParams); // 初始化驗證器 (false 表示驗證)
            signer.BlockUpdate(data, 0, data.Length); // 提供原始資料

            // 4. 驗證簽章
            return signer.VerifySignature(signature);
        }

        #endregion ECC
    }
}
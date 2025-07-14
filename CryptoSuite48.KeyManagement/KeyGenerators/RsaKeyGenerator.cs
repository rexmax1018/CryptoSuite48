using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using ConfigRoot = CryptoSuite48.Config.CryptoConfig;

namespace CryptoSuite48.KeyManagement.KeyGenerators
{
    /// <summary>
    /// RSA 金鑰產生器，產生 PEM 格式的公私鑰並儲存至檔案。
    /// </summary>
    public class RsaKeyGenerator : IKeyGenerator<RsaKeyModel>
    {
        /// <summary>
        /// 僅產生 RSA 金鑰模型（包含 PEM 公私鑰）
        /// </summary>
        public RsaKeyModel GenerateKeyOnly()
        {
            var keySize = ConfigRoot.Current.RSA.KeySize;

            // 1. 使用 Bouncy Castle 生成 RSA 金鑰對
            var generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            // 2. 取得公鑰和私鑰參數
            RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
            RsaKeyParameters publicKeyParams = (RsaKeyParameters)keyPair.Public;

            // 3. 呼叫修改後的匯出方法
            var privateKeyPem = ExportPrivateKeyPem(privateKeyParams);
            var publicKeyPem = ExportPublicKeyPem(publicKeyParams);

            return new RsaKeyModel
            {
                PrivateKey = privateKeyPem,
                PublicKey = publicKeyPem,
                KeySize = keySize,
                CreatedAt = DateTime.UtcNow
            };
        }

        public KeyGenerationResult GenerateAndSaveKey(string filePath = null)
        {
            var model = GenerateKeyOnly();
            var fileName = filePath ?? ConfigRoot.GenerateKeyFileName(".pem");

            var fullPath = ConfigRoot.GetKeyPath("RSA", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(fullPath, json);

            return new KeyGenerationResult
            {
                KeyFilePath = fullPath,
                Algorithm = CryptoAlgorithmType.RSA
            };
        }

        /// <summary>
        /// 匯出私鑰為 PEM 格式
        /// </summary>
        private static string ExportPrivateKeyPem(RsaPrivateCrtKeyParameters privateKeyParams)
        {
            var pkcs8 = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParams);

            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter);
                pemWriter.WriteObject(new PemObject("PRIVATE KEY", pkcs8.GetEncoded()));
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// 匯出公鑰為 PEM 格式
        /// </summary>
        private static string ExportPublicKeyPem(RsaKeyParameters publicKeyParams)
        {
            var pubInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKeyParams);

            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter);
                pemWriter.WriteObject(new PemObject("PUBLIC KEY", pubInfo.GetEncoded()));
                return stringWriter.ToString();
            }
        }
    }
}
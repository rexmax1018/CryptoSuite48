using CryptoSuite48.Config.Enums;
using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using ConfigRoot = CryptoSuite48.Config.CryptoConfig;
using Org.BouncyCastle.Utilities.IO.Pem; // 確保此引用已存在

namespace CryptoSuite48.KeyManagement.KeyGenerators
{
    /// <summary>
    /// ECC 金鑰產生器，產生 PEM 格式的 ECDsa 公私鑰。
    /// </summary>
    public class EccKeyGenerator : IKeyGenerator<EccKeyModel>
    {
        /// <summary>
        /// 僅產生 ECC 金鑰模型（PEM 公私鑰 + 曲線）
        /// </summary>
        public EccKeyModel GenerateKeyOnly()
        {
            var curveType = ConfigRoot.Current.ECC.Curve;

            // 1. 取得 Bouncy Castle 對應的曲線參數
            X9ECParameters ecP;
            string oid; // 曲線的 OID 字串
            switch (curveType)
            {
                case EccCurveType.NistP256:
                    ecP = NistNamedCurves.GetByName("P-256");
                    oid = NistNamedCurves.GetOid("P-256").Id;
                    break;

                case EccCurveType.NistP384:
                    ecP = NistNamedCurves.GetByName("P-384");
                    oid = NistNamedCurves.GetOid("P-384").Id;
                    break;

                case EccCurveType.NistP521:
                    ecP = NistNamedCurves.GetByName("P-521");
                    oid = NistNamedCurves.GetOid("P-521").Id;
                    break;

                case EccCurveType.Secp256k1:
                    ecP = SecNamedCurves.GetByName("secp256k1");
                    oid = SecNamedCurves.GetOid("secp256k1").Id;
                    break;

                default:
                    throw new NotSupportedException($"不支援的橢圓曲線：{curveType}");
            }

            // 2. 創建曲線域參數
            ECDomainParameters ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());

            // 3. 使用 Bouncy Castle 生成金鑰對
            var generator = new ECKeyPairGenerator();
            var keyGenerationParameters = new ECKeyGenerationParameters(ecSpec, new SecureRandom());
            generator.Init(keyGenerationParameters);
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            // 4. 取得公鑰和私鑰參數
            ECPrivateKeyParameters privateKeyParams = (ECPrivateKeyParameters)keyPair.Private;
            ECPublicKeyParameters publicKeyParams = (ECPublicKeyParameters)keyPair.Public;

            // 5. 呼叫修改後的匯出方法
            var privateKeyPem = ExportPrivateKeyPem(privateKeyParams);
            var publicKeyPem = ExportPublicKeyPem(publicKeyParams);

            return new EccKeyModel
            {
                PrivateKey = privateKeyPem,
                PublicKey = publicKeyPem,
                Curve = curveType,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 同步產生並儲存 ECC 金鑰 JSON 檔案
        /// </summary>
        public KeyGenerationResult GenerateAndSaveKey(string filePath = null)
        {
            var model = GenerateKeyOnly();
            var fileName = filePath ?? ConfigRoot.GenerateKeyFileName(".json");
            var fullPath = ConfigRoot.GetKeyPath("ECC", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(fullPath, json);

            return new KeyGenerationResult
            {
                KeyFilePath = fullPath,
                Algorithm = CryptoAlgorithmType.ECC
            };
        }

        /// <summary>
        /// 匯出私鑰為 PEM 格式（PKCS#8）
        /// </summary>
        private static string ExportPrivateKeyPem(ECPrivateKeyParameters privateKeyParams)
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
        /// 匯出公鑰為 PEM 格式（SubjectPublicKeyInfo）
        /// </summary>
        private static string ExportPublicKeyPem(ECPublicKeyParameters publicKeyParams)
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
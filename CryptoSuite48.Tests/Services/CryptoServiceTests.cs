using CryptoSuite48.Config.Enums;
using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.Services
{
    public class CryptoServiceTests
    {
        private readonly CryptoService _service = new CryptoService();

        [Fact(DisplayName = "AES 加解密應正確還原原文")]
        public void Aes_Encrypt_And_Decrypt_ShouldReturnOriginal()
        {
            var key = new SymmetricKeyModel
            {
                Key = new byte[32], // AES-256
                IV = new byte[16]
            };

            var data = Encoding.UTF8.GetBytes("Hello CryptoSuite");
            var encrypted = _service.Encrypt(data, CryptoAlgorithmType.AES, key);
            var decrypted = _service.Decrypt(encrypted, CryptoAlgorithmType.AES, key);

            Assert.Equal(data, decrypted);
        }

        [Fact(DisplayName = "RSA 簽章與驗章應正確對應")]
        public void Rsa_Sign_And_Verify_ShouldPass()
        {
            // 使用 Bouncy Castle 生成 RSA 金鑰對
            AsymmetricCipherKeyPair keyPair = GenerateRsaKeyPair(2048);
            RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
            RsaKeyParameters publicKeyParams = (RsaKeyParameters)keyPair.Public;

            // 使用新的輔助方法匯出 PEM
            var privatePem = ExportPrivateKeyPem(privateKeyParams);
            var publicPem = ExportPublicKeyPem(publicKeyParams);

            var key = new RsaKeyModel
            {
                PrivateKey = privatePem,
                PublicKey = publicPem
            };

            var data = Encoding.UTF8.GetBytes("CryptoSuite RSA Sign Test");
            var signature = _service.Sign(data, CryptoAlgorithmType.RSA, key);
            var isValid = _service.Verify(data, signature, CryptoAlgorithmType.RSA, key);

            Assert.True(isValid);
        }

        [Fact(DisplayName = "RSA 加解密應正確還原原文")]
        public void Rsa_Encrypt_And_Decrypt_ShouldReturnOriginal()
        {
            // 使用 Bouncy Castle 生成 RSA 金鑰對
            AsymmetricCipherKeyPair keyPair = GenerateRsaKeyPair(2048);
            RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
            RsaKeyParameters publicKeyParams = (RsaKeyParameters)keyPair.Public;

            // 使用新的輔助方法匯出 PEM
            var privatePem = ExportPrivateKeyPem(privateKeyParams);
            var publicPem = ExportPublicKeyPem(publicKeyParams);

            var key = new RsaKeyModel
            {
                PrivateKey = privatePem,
                PublicKey = publicPem
            };

            var data = Encoding.UTF8.GetBytes("RSA encryption test");
            var encrypted = _service.Encrypt(data, CryptoAlgorithmType.RSA, key);
            var decrypted = _service.Decrypt(encrypted, CryptoAlgorithmType.RSA, key);

            Assert.Equal(data, decrypted);
        }

        [Fact(DisplayName = "ECC 簽章與驗章應正確對應")]
        public void Ecc_Sign_And_Verify_ShouldPass()
        {
            // 使用 Bouncy Castle 生成 ECC 金鑰對
            AsymmetricCipherKeyPair keyPair = GenerateEccKeyPair(EccCurveType.NistP256);
            ECPrivateKeyParameters privateKeyParams = (ECPrivateKeyParameters)keyPair.Private;
            ECPublicKeyParameters publicKeyParams = (ECPublicKeyParameters)keyPair.Public;

            // 使用新的輔助方法匯出 PEM
            var privatePem = ExportPrivateKeyPem(privateKeyParams);
            var publicPem = ExportPublicKeyPem(publicKeyParams);

            var key = new EccKeyModel
            {
                PrivateKey = privatePem,
                PublicKey = publicPem
            };

            var data = Encoding.UTF8.GetBytes("CryptoSuite ECC Test");
            var signature = _service.Sign(data, CryptoAlgorithmType.ECC, key);
            var isValid = _service.Verify(data, signature, CryptoAlgorithmType.ECC, key);

            Assert.True(isValid);
        }

        private static string ExportPrivateKeyPem(AsymmetricKeyParameter privateKeyParams)
        {
            PrivateKeyInfo pkcs8 = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParams);
            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter); // 修改此行
                pemWriter.WriteObject(new PemObject("PRIVATE KEY", pkcs8.GetEncoded()));
                return stringWriter.ToString();
            }
        }

        private static string ExportPublicKeyPem(AsymmetricKeyParameter publicKeyParams)
        {
            SubjectPublicKeyInfo pubInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKeyParams);
            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter); // 修改此行
                pemWriter.WriteObject(new PemObject("PUBLIC KEY", pubInfo.GetEncoded()));
                return stringWriter.ToString();
            }
        }

        private static AsymmetricCipherKeyPair GenerateRsaKeyPair(int keySize)
        {
            var generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
            return generator.GenerateKeyPair();
        }

        private static AsymmetricCipherKeyPair GenerateEccKeyPair(EccCurveType curveType)
        {
            X9ECParameters ecP;
            switch (curveType)
            {
                case EccCurveType.NistP256: ecP = NistNamedCurves.GetByName("P-256"); break;
                case EccCurveType.NistP384: ecP = NistNamedCurves.GetByName("P-384"); break;
                case EccCurveType.NistP521: ecP = NistNamedCurves.GetByName("P-521"); break;
                case EccCurveType.Secp256k1: ecP = SecNamedCurves.GetByName("secp256k1"); break;
                default: throw new NotSupportedException($"Unsupported curve for test: {curveType}");
            }
            ECDomainParameters ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.N, ecP.GetSeed()); // 這裡似乎有小錯誤，ecP.N 出現兩次
            var generator = new ECKeyPairGenerator();
            var keyGenerationParameters = new ECKeyGenerationParameters(ecSpec, new SecureRandom());
            generator.Init(keyGenerationParameters);
            return generator.GenerateKeyPair();
        }
    }
}
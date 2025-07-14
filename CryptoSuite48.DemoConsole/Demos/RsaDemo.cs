using CryptoSuite48.Config;
using CryptoSuite48.Config.Enums;
using CryptoSuite48.Extensions;
using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services.Interfaces;
using System;
using System.IO;

namespace CryptoSuite48.DemoConsole.Demos
{
    public class RsaDemo
    {
        private readonly ICryptoService _cryptoService;
        private readonly ICryptoKeyService _keyService;

        public RsaDemo(ICryptoService cryptoService, ICryptoKeyService keyService)
        {
            _cryptoService = cryptoService;
            _keyService = keyService;

            // 初始化 RSA 設定（2048 bits）
            CryptoConfig.Override(new CryptoConfigModel
            {
                KeyDirectory = Path.GetTempPath(),
                RSA = new RsaConfig
                {
                    KeySize = 2048,
                    Encoding = TextEncodingType.UTF8
                }
            });
        }

        public void Run()
        {
            Console.WriteLine("=== [RSA DEMO] 加解密 + 簽章驗章範例 ===");

            var key = _keyService.GenerateKeyOnly<RsaKeyModel>(CryptoAlgorithmType.RSA);

            Console.WriteLine("\n[RSA] 公鑰（PEM）：\n" + key.PublicKey);
            Console.WriteLine("[RSA] 私鑰（PEM）：\n" + key.PrivateKey);

            var plaintext = "Hello RSA!";
            var data = plaintext.ToBytes();

            Console.WriteLine($"\n原文：{plaintext}");

            // ===== 使用 CryptoService 加解密 =====
            var encryptedByService = _cryptoService.Encrypt(data, CryptoAlgorithmType.RSA, key);
            var decryptedByService = _cryptoService.Decrypt(encryptedByService, CryptoAlgorithmType.RSA, key);

            Console.WriteLine("\n[CryptoService] 加密（Base64）：");
            Console.WriteLine(encryptedByService.ToBase64());
            Console.WriteLine("[CryptoService] 解密還原：");
            Console.WriteLine(decryptedByService.ToUtf8String());

            // ===== 使用 Extension 加解密 =====
            var encryptedByExt = data.EncryptWith(CryptoAlgorithmType.RSA, key, _cryptoService);
            var decryptedByExt = encryptedByExt.DecryptWith(CryptoAlgorithmType.RSA, key, _cryptoService);

            Console.WriteLine("\n[Extensions] 加密（Base64）：");
            Console.WriteLine(encryptedByExt.ToBase64());
            Console.WriteLine("[Extensions] 解密還原：");
            Console.WriteLine(decryptedByExt.ToUtf8String());

            Console.WriteLine($"\n[驗證] 解密一致：{decryptedByService.ToUtf8String() == decryptedByExt.ToUtf8String()}");

            // ===== 使用 CryptoService 簽章驗章 =====
            var signatureByService = _cryptoService.Sign(data, CryptoAlgorithmType.RSA, key);
            var verifiedByService = _cryptoService.Verify(data, signatureByService, CryptoAlgorithmType.RSA, key);

            Console.WriteLine("\n[CryptoService] 簽章（Base64）：");
            Console.WriteLine(signatureByService.ToBase64());
            Console.WriteLine("[CryptoService] 驗章結果：" + (verifiedByService ? "✔ 通過" : "✘ 失敗"));

            // ===== 使用 Extension 簽章驗章 =====
            var signatureByExt = data.SignWith(CryptoAlgorithmType.RSA, key, _cryptoService);
            var verifiedByExt = data.VerifyWith(signatureByExt, CryptoAlgorithmType.RSA, key, _cryptoService);

            Console.WriteLine("\n[Extensions] 簽章（Base64）：");
            Console.WriteLine(signatureByExt.ToBase64());
            Console.WriteLine("[Extensions] 驗章結果：" + (verifiedByExt ? "✔ 通過" : "✘ 失敗"));

            Console.WriteLine($"\n[驗證] 簽章一致：{signatureByService.ToBase64() == signatureByExt.ToBase64()}");
        }
    }
}
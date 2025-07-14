using CryptoSuite48.Config;
using CryptoSuite48.Extensions;
using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services.Interfaces;
using System;
using System.IO;
using System.Text;

namespace CryptoSuite48.DemoConsole.Demos
{
    public class AesDemo
    {
        private readonly ICryptoService _cryptoService;
        private readonly ICryptoKeyService _keyService;

        public AesDemo(ICryptoService cryptoService, ICryptoKeyService keyService)
        {
            _cryptoService = cryptoService;
            _keyService = keyService;

            // 初始化 AES 金鑰設定
            CryptoConfig.Override(new CryptoConfigModel
            {
                KeyDirectory = Path.GetTempPath(),
                AES = new AesConfig { KeySize = 256 }
            });
        }

        public void Run()
        {
            Console.WriteLine("=== [AES DEMO] 加解密範例 ===");

            var key = _keyService.GenerateKeyOnly<SymmetricKeyModel>(CryptoAlgorithmType.AES);

            // 顯示金鑰資訊
            Console.WriteLine("\n[AES] 金鑰內容：");
            Console.WriteLine($"Key: {key.Key.ToBase64()}");
            Console.WriteLine($"IV : {key.IV.ToBase64()}");

            var plaintext = "Hello from CryptoSuite!";
            var data = plaintext.ToBytes();

            Console.WriteLine($"\n原文：{plaintext}");

            // ===== 使用 CryptoService =====
            var encryptedByService = _cryptoService.Encrypt(data, CryptoAlgorithmType.AES, key);
            var decryptedByService = _cryptoService.Decrypt(encryptedByService, CryptoAlgorithmType.AES, key);

            Console.WriteLine("\n[CryptoService] 加密後（Base64）：");
            Console.WriteLine(encryptedByService.ToBase64());
            Console.WriteLine("[CryptoService] 解密後還原：");
            Console.WriteLine(decryptedByService.ToUtf8String());

            // ===== 使用 Extension 語法糖 =====
            var encryptedByExtension = data.EncryptWith(CryptoAlgorithmType.AES, key, _cryptoService);
            var decryptedByExtension = encryptedByExtension.DecryptWith(CryptoAlgorithmType.AES, key, _cryptoService);

            Console.WriteLine("\n[Extensions] 加密後（Base64）：");
            Console.WriteLine(encryptedByExtension.ToBase64());
            Console.WriteLine("[Extensions] 解密後還原：");
            Console.WriteLine(decryptedByExtension.ToUtf8String());

            // 結果驗證
            Console.WriteLine($"\n[驗證] 解密一致：{Encoding.UTF8.GetString(decryptedByService) == Encoding.UTF8.GetString(decryptedByExtension)}");
        }
    }
}
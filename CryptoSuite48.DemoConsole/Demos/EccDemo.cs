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
    public class EccDemo
    {
        private readonly ICryptoService _cryptoService;
        private readonly ICryptoKeyService _keyService;

        public EccDemo(ICryptoService cryptoService, ICryptoKeyService keyService)
        {
            _cryptoService = cryptoService;
            _keyService = keyService;

            // 初始化 ECC 設定
            CryptoConfig.Override(new CryptoConfigModel
            {
                KeyDirectory = Path.GetTempPath(),
                ECC = new EccConfig
                {
                    Curve = EccCurveType.NistP256
                }
            });
        }

        public void Run()
        {
            Console.WriteLine("=== [ECC DEMO] 簽章驗章範例 ===");

            var key = _keyService.GenerateKeyOnly<EccKeyModel>(CryptoAlgorithmType.ECC);

            Console.WriteLine("\n[ECC] 公鑰（PEM）：\n" + key.PublicKey);
            Console.WriteLine("[ECC] 私鑰（PEM）：\n" + key.PrivateKey);

            var plaintext = "Hello ECC!";
            var data = plaintext.ToBytes();

            Console.WriteLine($"\n原文：{plaintext}");

            // ===== 使用 CryptoService =====
            var signatureByService = _cryptoService.Sign(data, CryptoAlgorithmType.ECC, key);
            var verifiedByService = _cryptoService.Verify(data, signatureByService, CryptoAlgorithmType.ECC, key);

            Console.WriteLine("\n[CryptoService] 簽章（Base64）：");
            Console.WriteLine(signatureByService.ToBase64());
            Console.WriteLine("[CryptoService] 驗章結果：" + (verifiedByService ? "✔ 通過" : "✘ 失敗"));

            // ===== 使用 Extension 語法 =====
            var signatureByExt = data.SignWith(CryptoAlgorithmType.ECC, key, _cryptoService);
            var verifiedByExt = data.VerifyWith(signatureByExt, CryptoAlgorithmType.ECC, key, _cryptoService);

            Console.WriteLine("\n[Extensions] 簽章（Base64）：");
            Console.WriteLine(signatureByExt.ToBase64());
            Console.WriteLine("[Extensions] 驗章結果：" + (verifiedByExt ? "✔ 通過" : "✘ 失敗"));

            // 比對一致性
            Console.WriteLine($"\n[驗證] 簽章一致：{signatureByExt.ToBase64() == signatureByService.ToBase64()}");
        }
    }
}
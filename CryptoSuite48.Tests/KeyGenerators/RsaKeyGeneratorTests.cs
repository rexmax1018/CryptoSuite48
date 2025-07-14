using CryptoSuite48.Config;
using CryptoSuite48.KeyManagement.KeyGenerators;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace CryptoSuite48.Tests.KeyGenerators
{
    public class RsaKeyGeneratorTests
    {
        public RsaKeyGeneratorTests()
        {
            try
            {
                _ = CryptoConfig.Current;
            }
            catch (InvalidOperationException)
            {
                CryptoConfig.Override(new CryptoConfigModel
                {
                    KeyDirectory = Path.GetTempPath(),
                    RSA = new RsaConfig { KeySize = 2048 }
                });
            }
        }

        [Fact(DisplayName = "GenerateKeyOnly 應產生合法的 RSA 公私鑰 PEM 字串")]
        public void GenerateKeyOnly_ShouldReturnValidRsaKeyModel()
        {
            var generator = new RsaKeyGenerator();
            var model = generator.GenerateKeyOnly();

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);
            Assert.Equal(2048, model.KeySize);
        }

        [Fact(DisplayName = "GenerateAndSaveKey 應建立合法的 JSON 檔案")]
        public void GenerateAndSaveKey_ShouldCreateJsonFile()
        {
            var generator = new RsaKeyGenerator();
            var result = generator.GenerateAndSaveKey();

            Assert.True(File.Exists(result.KeyFilePath));

            var json = File.ReadAllText(result.KeyFilePath);
            var model = JsonConvert.DeserializeObject<RsaKeyModel>(json);

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);

            File.Delete(result.KeyFilePath);
        }

        [Fact(DisplayName = "GenerateAndSaveKey 應支援自訂儲存路徑")]
        public void GenerateAndSaveKey_WithCustomPath_ShouldWriteFile()
        {
            var path = Path.Combine(Path.GetTempPath(), $"rsa_test_{Guid.NewGuid()}.json");
            var generator = new RsaKeyGenerator();
            var result = generator.GenerateAndSaveKey(path);

            Assert.True(File.Exists(path));
            Assert.Equal(path, result.KeyFilePath);

            var json = File.ReadAllText(path);
            var model = JsonConvert.DeserializeObject<RsaKeyModel>(json);

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);

            File.Delete(path);
        }
    }
}
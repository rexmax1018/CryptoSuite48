using CryptoSuite48.Config;
using CryptoSuite48.Config.Enums;
using CryptoSuite48.KeyManagement.KeyGenerators;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace CryptoSuite48.Tests.KeyGenerators
{
    public class EccKeyGeneratorTests
    {
        private void InitTestConfig()
        {
            CryptoConfig.Override(new CryptoConfigModel
            {
                KeyDirectory = Path.GetTempPath(),
                ECC = new EccConfig { Curve = EccCurveType.NistP256 }
            });
        }

        [Fact(DisplayName = "GenerateKeyOnly 應產生合法的 ECC 金鑰模型")]
        public void GenerateKeyOnly_ShouldReturnValidEccKeyModel()
        {
            InitTestConfig();
            var generator = new EccKeyGenerator();
            var model = generator.GenerateKeyOnly();

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);
            Assert.Equal(EccCurveType.NistP256, model.Curve);
        }

        [Fact(DisplayName = "GenerateAndSaveKey 應產生並寫入 JSON 檔案")]
        public void GenerateAndSaveKey_ShouldCreateValidJsonFile()
        {
            InitTestConfig();
            var generator = new EccKeyGenerator();
            var result = generator.GenerateAndSaveKey();

            Assert.True(File.Exists(result.KeyFilePath));

            var json = File.ReadAllText(result.KeyFilePath);
            var model = JsonConvert.DeserializeObject<EccKeyModel>(json);

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
            Assert.Equal(EccCurveType.NistP256, model.Curve);

            File.Delete(result.KeyFilePath);
        }

        [Fact(DisplayName = "使用自訂路徑儲存應正確產生金鑰檔")]
        public void GenerateAndSaveKey_WithCustomPath_ShouldWriteFile()
        {
            InitTestConfig();
            var path = Path.Combine(Path.GetTempPath(), $"ecc_test_{Guid.NewGuid()}.json");
            var generator = new EccKeyGenerator();
            var result = generator.GenerateAndSaveKey(path);

            Assert.True(File.Exists(path));
            Assert.Equal(path, result.KeyFilePath);

            var json = File.ReadAllText(path);
            var model = JsonConvert.DeserializeObject<EccKeyModel>(json);

            Assert.NotNull(model);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);

            File.Delete(path);
        }
    }
}
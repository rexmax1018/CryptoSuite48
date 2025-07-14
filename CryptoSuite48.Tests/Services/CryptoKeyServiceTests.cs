using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using CryptoSuite48.Services;
using Moq;
using System;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.Services
{
    public class CryptoKeyServiceTests
    {
        private readonly Mock<IKeyGeneratorFactory> _mockGenFactory = new Mock<IKeyGeneratorFactory>();
        private readonly Mock<IKeyLoaderFactory> _mockLoadFactory = new Mock<IKeyLoaderFactory>();

        private readonly Mock<IKeyGenerator<SymmetricKeyModel>> _mockGenerator = new Mock<IKeyGenerator<SymmetricKeyModel>>();
        private readonly Mock<IKeyLoader<SymmetricKeyModel>> _mockLoader = new Mock<IKeyLoader<SymmetricKeyModel>>();

        private readonly CryptoKeyService _service;

        public CryptoKeyServiceTests()
        {
            _mockGenFactory.Setup(f => f.Create<SymmetricKeyModel>(CryptoAlgorithmType.AES))
                .Returns(_mockGenerator.Object);

            _mockLoadFactory.Setup(f => f.Create<SymmetricKeyModel>(CryptoAlgorithmType.AES))
                .Returns(_mockLoader.Object);

            _service = new CryptoKeyService(_mockGenFactory.Object, _mockLoadFactory.Object);
        }

        [Fact(DisplayName = "GenerateKeyOnly 應正確回傳模型")]
        public void GenerateKeyOnly_ShouldReturnModel()
        {
            var expected = new SymmetricKeyModel();
            _mockGenerator.Setup(g => g.GenerateKeyOnly()).Returns(expected);

            var result = _service.GenerateKeyOnly<SymmetricKeyModel>(CryptoAlgorithmType.AES);

            Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "GenerateAndSaveKey 應呼叫對應產生器並回傳結果")]
        public void GenerateAndSaveKey_ShouldReturnResult()
        {
            var result = new KeyGenerationResult { KeyFilePath = "test.json" };
            _mockGenerator.Setup(g => g.GenerateAndSaveKey(null)).Returns(result);

            var output = _service.GenerateAndSaveKey<SymmetricKeyModel>(CryptoAlgorithmType.AES);

            Assert.Equal(result.KeyFilePath, output.KeyFilePath);
        }

        [Fact(DisplayName = "LoadFromFile 應載入模型")]
        public void LoadFromFile_ShouldReturnModel()
        {
            var model = new SymmetricKeyModel();
            _mockLoader.Setup(l => l.LoadFromFile("test.json")).Returns(model);

            var result = _service.LoadFromFile<SymmetricKeyModel>(CryptoAlgorithmType.AES, "test.json");

            Assert.Equal(model, result);
        }

        [Fact(DisplayName = "LoadFromBase64 應正確還原模型")]
        public void LoadFromBase64_ShouldReturnModel()
        {
            var model = new SymmetricKeyModel();
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("json"));

            _mockLoader.Setup(l => l.LoadFromBase64(base64)).Returns(model);

            var result = _service.LoadFromBase64<SymmetricKeyModel>(CryptoAlgorithmType.AES, base64);

            Assert.Equal(model, result);
        }
    }
}
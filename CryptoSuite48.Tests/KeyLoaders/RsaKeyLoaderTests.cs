using CryptoSuite48.KeyManagement.KeyLoaders;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CryptoSuite48.Tests.KeyLoaders
{
    public class RsaKeyLoaderTests
    {
        private readonly RsaKeyModel _sampleModel = new RsaKeyModel()
        {
            PublicKey = "-----BEGIN PUBLIC KEY-----\nMIIBIjAN...\n-----END PUBLIC KEY-----",
            PrivateKey = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADAN...\n-----END PRIVATE KEY-----",
            KeySize = 2048,
            CreatedAt = DateTime.UtcNow
        };

        private string SerializeSampleModel() => JsonConvert.SerializeObject(_sampleModel);

        [Fact(DisplayName = "應可從 JSON 檔案載入 RSA 金鑰模型")]
        public void LoadFromFile_ShouldReturnModel()
        {
            var path = Path.Combine(Path.GetTempPath(), $"rsa_{Guid.NewGuid()}.json");
            File.WriteAllText(path, SerializeSampleModel());

            var loader = new RsaKeyLoader();
            var model = loader.LoadFromFile(path);

            Assert.NotNull(model);
            Assert.Equal(2048, model.KeySize);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);

            File.Delete(path);
        }

        [Fact(DisplayName = "應可從 Base64 載入 RSA 金鑰模型")]
        public void LoadFromBase64_ShouldReturnModel()
        {
            var json = SerializeSampleModel();
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            var loader = new RsaKeyLoader();
            var model = loader.LoadFromBase64(base64);

            Assert.NotNull(model);
            Assert.Equal(2048, model.KeySize);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
        }

        [Fact(DisplayName = "無效 Base64 應拋出 InvalidDataException")]
        public void LoadFromInvalidBase64_ShouldThrow()
        {
            var loader = new RsaKeyLoader();
            Assert.Throws<InvalidDataException>(() => loader.LoadFromBase64("@@invalid@@"));
        }

        [Fact(DisplayName = "應可從 JSON 字串載入 RSA 金鑰模型")]
        public void LoadFromString_ShouldReturnModel()
        {
            var json = SerializeSampleModel();
            var loader = new RsaKeyLoader();
            var model = loader.LoadFromString(json);

            Assert.NotNull(model);
            Assert.Equal(_sampleModel.KeySize, model.KeySize);
        }

        [Fact(DisplayName = "應可從 Stream 載入 RSA 金鑰模型")]
        public void LoadFromStream_ShouldReturnModel()
        {
            var json = SerializeSampleModel();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var loader = new RsaKeyLoader();
                var model = loader.LoadFromStream(stream);

                Assert.NotNull(model);
                Assert.Equal(_sampleModel.KeySize, model.KeySize);
            }
        }
    }
}
using CryptoSuite48.Config.Enums;
using CryptoSuite48.KeyManagement.KeyLoaders;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.KeyLoaders
{
    public class EccKeyLoaderTests
    {
        private readonly EccKeyModel _sampleModel = new EccKeyModel()
        {
            PublicKey = "-----BEGIN PUBLIC KEY-----\nMFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAE...\n-----END PUBLIC KEY-----",
            PrivateKey = "-----BEGIN PRIVATE KEY-----\nMIGHAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBG0wawIBAQQg...\n-----END PRIVATE KEY-----",
            Curve = EccCurveType.NistP256,
            CreatedAt = DateTime.UtcNow
        };

        private string SerializeSample() => JsonConvert.SerializeObject(_sampleModel);

        [Fact(DisplayName = "LoadFromFile 應正確還原 EccKeyModel")]
        public void LoadFromFile_ShouldReturnModel()
        {
            var path = Path.Combine(Path.GetTempPath(), $"ecc_test_{Guid.NewGuid()}.json");
            File.WriteAllText(path, SerializeSample());

            var loader = new EccKeyLoader();
            var model = loader.LoadFromFile(path);

            Assert.NotNull(model);
            Assert.Equal(_sampleModel.Curve, model.Curve);
            Assert.Contains("BEGIN PUBLIC KEY", model.PublicKey);

            File.Delete(path);
        }

        [Fact(DisplayName = "LoadFromBase64 應正確還原 EccKeyModel")]
        public void LoadFromBase64_ShouldReturnModel()
        {
            var json = SerializeSample();
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            var loader = new EccKeyLoader();
            var model = loader.LoadFromBase64(base64);

            Assert.NotNull(model);
            Assert.Equal(EccCurveType.NistP256, model.Curve);
            Assert.Contains("BEGIN PRIVATE KEY", model.PrivateKey);
        }

        [Fact(DisplayName = "無效 Base64 字串應拋出 InvalidDataException")]
        public void LoadFromInvalidBase64_ShouldThrow()
        {
            var loader = new EccKeyLoader();
            Assert.Throws<InvalidDataException>(() => loader.LoadFromBase64("%%%invalid%%%"));
        }

        [Fact(DisplayName = "LoadFromString 應從 JSON 字串還原模型")]
        public void LoadFromString_ShouldReturnModel()
        {
            var json = SerializeSample();
            var loader = new EccKeyLoader();
            var model = loader.LoadFromString(json);

            Assert.NotNull(model);
            Assert.Equal(_sampleModel.Curve, model.Curve);
        }

        [Fact(DisplayName = "LoadFromStream 應從資料流還原模型")]
        public void LoadFromStream_ShouldReturnModel()
        {
            var json = SerializeSample();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var loader = new EccKeyLoader();
                var model = loader.LoadFromStream(stream);

                Assert.NotNull(model);
                Assert.Equal(_sampleModel.Curve, model.Curve);
            }
        }
    }
}
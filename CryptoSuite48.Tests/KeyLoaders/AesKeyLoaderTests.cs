using CryptoSuite48.KeyManagement.KeyLoaders;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace CryptoSuite48.Tests.KeyLoaders
{
    public class AesKeyLoaderTests
    {
        private const string ValidJson = "{\"Key\": [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16], \"IV\": [16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1]}";
        private const string ValidBase64 = "eyJLZXkiOiBbMSwgMiwgMywgNCwgNSwgNiwgNywgOCwgOSwgMTAsIDExLCAxMiwgMTMsIDE0LCAxNSwgMTZdLCAiSVYiOiBbMTYsIDE1LCAxNCwgMTMsIDEyLCAxMSwgMTAsIDksIDgsIDcsIDYsIDUsIDQsIDMsIDIsIDFdfQ==";

        [Fact(DisplayName = "LoadFromString 應正確還原 Key/IV")]
        public void LoadFromString_ShouldReturnModel()
        {
            var loader = new AesKeyLoader();
            var model = loader.LoadFromString(ValidJson);

            Assert.Equal(16, model.Key.Length);
            Assert.Equal(16, model.IV.Length);
            Assert.Equal(1, model.Key[0]);
            Assert.Equal(16, model.IV[0]);
        }

        [Fact(DisplayName = "LoadFromBase64 應正確還原 Key/IV")]
        public void LoadFromBase64_ShouldReturnModel()
        {
            var loader = new AesKeyLoader();
            var model = loader.LoadFromBase64(ValidBase64);

            Assert.Equal(16, model.Key.Length);
            Assert.Equal(16, model.IV.Length);
            Assert.Equal(1, model.Key[0]);
            Assert.Equal(16, model.IV[0]);
        }

        [Fact(DisplayName = "LoadFromStream 應正確還原 Key/IV")]
        public void LoadFromStream_ShouldReturnModel()
        {
            var loader = new AesKeyLoader();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ValidJson)))
            {
                var model = loader.LoadFromStream(stream);

                Assert.Equal(16, model.Key.Length);
                Assert.Equal(16, model.IV.Length);
            }
        }

        [Fact(DisplayName = "LoadFromFile 應正確讀取 JSON 檔")]
        public void LoadFromFile_ShouldReturnModel()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            File.WriteAllText(tempFile, ValidJson);

            var loader = new AesKeyLoader();
            var model = loader.LoadFromFile(tempFile);

            Assert.Equal(16, model.Key.Length);
            Assert.Equal(16, model.IV.Length);

            File.Delete(tempFile);
        }

        [Fact(DisplayName = "遇到無效 JSON 應拋出例外")]
        public void LoadFromInvalidString_ShouldThrow()
        {
            var loader = new AesKeyLoader();
            Assert.Throws<InvalidDataException>(() =>
            {
                loader.LoadFromString("INVALID_JSON");
            });
        }
    }
}
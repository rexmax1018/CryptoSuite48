using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace CryptoSuite48.KeyManagement.KeyLoaders
{
    /// <summary>
    /// ECC 金鑰載入器，支援從檔案、字串、Base64、Stream 還原 ECC 金鑰模型。
    /// </summary>
    public class EccKeyLoader : IKeyLoader<EccKeyModel>
    {
        /// <summary>
        /// 從指定檔案路徑載入 JSON 並反序列化為 EccKeyModel。
        /// </summary>
        /// <param name="path">金鑰 JSON 檔案路徑</param>
        /// <returns>還原後的 EccKeyModel</returns>
        public EccKeyModel LoadFromFile(string path)
        {
            var json = File.ReadAllText(path);
            return Deserialize(json);
        }

        /// <summary>
        /// 從原始 JSON 字串載入 ECC 金鑰模型。
        /// </summary>
        /// <param name="content">JSON 字串</param>
        /// <returns>還原後的 EccKeyModel</returns>
        public EccKeyModel LoadFromString(string content)
        {
            return Deserialize(content);
        }

        /// <summary>
        /// 從 Base64 編碼字串還原 JSON 並反序列化為 EccKeyModel。
        /// </summary>
        /// <param name="base64">Base64 編碼字串</param>
        /// <returns>還原後的 EccKeyModel</returns>
        /// <exception cref="InvalidDataException">當 base64 解碼或 JSON 無法解析時</exception>
        public EccKeyModel LoadFromBase64(string base64)
        {
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                return Deserialize(json);
            }
            catch (FormatException ex)
            {
                throw new InvalidDataException("無效的 Base64 字串格式", ex);
            }
        }

        /// <summary>
        /// 從資料流（stream）載入 JSON 並反序列化為 EccKeyModel。
        /// </summary>
        /// <param name="stream">資料流（如 MemoryStream 或 FileStream）</param>
        /// <returns>還原後的 EccKeyModel</returns>
        public EccKeyModel LoadFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                return Deserialize(json);
            }
        }

        /// <summary>
        /// 將 JSON 字串反序列化為 EccKeyModel，若失敗則拋出 InvalidDataException。
        /// </summary>
        /// <param name="json">JSON 格式字串</param>
        /// <returns>反序列化後的 EccKeyModel</returns>
        /// <exception cref="InvalidDataException">當反序列化失敗時</exception>
        private static EccKeyModel Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<EccKeyModel>(json)
                ?? throw new InvalidDataException("無法解析 ECC 金鑰資料");
        }
    }
}
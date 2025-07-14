using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.KeyManagement.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace CryptoSuite48.KeyManagement.KeyLoaders
{
    /// <summary>
    /// AES 金鑰載入器。
    /// 支援從檔案、字串、Base64、Stream 載入對稱金鑰。
    /// </summary>
    public class AesKeyLoader : IKeyLoader<SymmetricKeyModel>
    {
        /// <summary>
        /// 從指定的檔案路徑載入 AES 金鑰與 IV。
        /// </summary>
        /// <param name="path">JSON 金鑰檔案的完整路徑</param>
        /// <returns>還原後的 SymmetricKeyModel 物件</returns>
        public SymmetricKeyModel LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);
            return Deserialize(json);
        }

        /// <summary>
        /// 從 JSON 格式字串載入 AES 金鑰與 IV。
        /// </summary>
        /// <param name="content">JSON 格式字串</param>
        /// <returns>還原後的 SymmetricKeyModel</returns>
        public SymmetricKeyModel LoadFromString(string content)
        {
            return Deserialize(content);
        }

        /// <summary>
        /// 從 Base64 編碼的 JSON 字串還原 AES 金鑰與 IV。
        /// </summary>
        /// <param name="base64">Base64 編碼字串</param>
        /// <returns>還原後的 SymmetricKeyModel</returns>
        public SymmetricKeyModel LoadFromBase64(string base64)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            return Deserialize(json);
        }

        /// <summary>
        /// 從資料流中讀取 JSON 並還原 AES 金鑰與 IV。
        /// </summary>
        /// <param name="stream">輸入資料流（例如 MemoryStream、FileStream）</param>
        /// <returns>還原後的 SymmetricKeyModel</returns>
        public SymmetricKeyModel LoadFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                return Deserialize(json);
            }
        }

        /// <summary>
        /// 將 JSON 字串反序列化為 SymmetricKeyModel。
        /// 若格式錯誤，將封裝為 InvalidDataException 拋出。
        /// </summary>
        /// <param name="json">JSON 字串內容</param>
        /// <returns>反序列化結果</returns>
        private static SymmetricKeyModel Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<SymmetricKeyModel>(json)
                       ?? throw new InvalidDataException("JSON 內容無效：無法轉換為金鑰物件。");
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("JSON 格式解析錯誤。", ex);
            }
        }
    }
}
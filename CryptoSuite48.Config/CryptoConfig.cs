using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CryptoSuite48.Config
{
    /// <summary>
    /// 提供加密套件的設定載入與統一存取。
    /// 設定來源可為 JSON 檔案，包含 AES/RSA/ECC 等加密參數與金鑰儲存目錄。
    /// </summary>
    public static class CryptoConfig
    {
        private static CryptoConfigModel _cachedConfig;

        /// <summary>
        /// 取得目前載入的設定實體。
        /// 若尚未載入，會丟出例外。
        /// </summary>
        public static CryptoConfigModel Current =>
            _cachedConfig ?? throw new InvalidOperationException("尚未載入 CryptoConfig，請先呼叫 Load 或 Override");

        /// <summary>
        /// 載入設定檔（JSON 格式），可指定路徑。
        /// </summary>
        /// <param name="jsonPath">設定檔路徑（預設為 appsettings.json）</param>
        public static void Load(string jsonPath = "appsettings.json")
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"找不到設定檔：{jsonPath}");

            var json = File.ReadAllText(jsonPath, Encoding.UTF8);
            var jObject = JObject.Parse(json);

            var configSection = jObject["CryptoSuite"]?.ToString();
            if (string.IsNullOrWhiteSpace(configSection))
                throw new InvalidDataException("設定檔中缺少 CryptoSuite 節點");

            var settings = new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            _cachedConfig = JsonConvert.DeserializeObject<CryptoConfigModel>(configSection, settings)
                ?? throw new InvalidDataException("無法解析 CryptoSuite 設定");
        }

        /// <summary>
        /// 在單元測試中可用此方法直接指定設定，不經過讀檔。
        /// </summary>
        public static void Override(CryptoConfigModel model)
        {
            _cachedConfig = model;
        }

        /// <summary>
        /// 產生隨機金鑰檔名（包含副檔名），使用 8 碼英數字（大小寫）。
        /// </summary>
        /// <param name="extension">副檔名（包含 .）</param>
        public static string GenerateKeyFileName(string extension = ".key")
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var name = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return name + extension;
        }

        /// <summary>
        /// 根據指定演算法與檔名，取得金鑰完整路徑。
        /// 自動使用設定中的 KeyDirectory 並分類。
        /// </summary>
        /// <param name="algorithm">子資料夾名稱（例如 AES / RSA / ECC）</param>
        /// <param name="fileName">檔案名稱（需包含副檔名）</param>
        public static string GetKeyPath(string algorithm, string fileName)
        {
            return Path.Combine(Current.KeyDirectory, algorithm, fileName);
        }
    }
}
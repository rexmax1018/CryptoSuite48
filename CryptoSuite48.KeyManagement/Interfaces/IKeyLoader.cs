using System.IO;
using System.Threading.Tasks;

namespace CryptoSuite48.KeyManagement.Interfaces
{
    /// <summary>
    /// 定義從各種來源載入金鑰的標準泛型介面。
    /// 支援從檔案、字串、Base64、Stream 載入，並還原為指定的金鑰模型類型。
    /// </summary>
    /// <typeparam name="T">金鑰類型，例如 RSA、ECDsa、SymmetricKeyModel 等</typeparam>
    public interface IKeyLoader<T> where T : class
    {
        /// <summary>
        /// 從指定的檔案路徑載入金鑰（同步）。
        /// 適用於 JSON 或純文字 PEM 格式的金鑰內容。
        /// </summary>
        /// <param name="path">金鑰檔案完整路徑</param>
        /// <returns>還原後的金鑰模型</returns>
        T LoadFromFile(string path);

        /// <summary>
        /// 從原始字串內容（例如 JSON 或 PEM）還原金鑰模型。
        /// 適用於記憶體中動態資料或直接從網路回應中解析。
        /// </summary>
        /// <param name="content">原始金鑰字串內容</param>
        /// <returns>還原後的金鑰模型</returns>
        T LoadFromString(string content);

        /// <summary>
        /// 從 Base64 字串還原金鑰。
        /// 通常是序列化後的 JSON 字串經 Base64 編碼。
        /// </summary>
        /// <param name="base64">Base64 編碼字串</param>
        /// <returns>還原後的金鑰模型</returns>
        T LoadFromBase64(string base64);

        /// <summary>
        /// 從資料流中還原金鑰。
        /// 適用於從記憶體串流或檔案串流載入金鑰。
        /// </summary>
        /// <param name="stream">包含金鑰內容的資料流</param>
        /// <returns>還原後的金鑰模型</returns>
        T LoadFromStream(Stream stream);
    }
}
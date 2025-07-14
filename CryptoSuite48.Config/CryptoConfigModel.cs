using CryptoSuite48.Config.Enums;

namespace CryptoSuite48.Config
{
    /// <summary>
    /// AES 加密設定。
    /// </summary>
    public class AesConfig
    {
        /// <summary>
        /// 對稱金鑰長度（單位為 bit，建議為 128 / 192 / 256）。
        /// </summary>
        public int KeySize { get; set; } = 256;

        /// <summary>
        /// 加密文字所使用的編碼格式。
        /// </summary>
        public TextEncodingType Encoding { get; set; } = TextEncodingType.UTF8;
    }

    /// <summary>
    /// RSA 加密設定。
    /// </summary>
    public class RsaConfig
    {
        /// <summary>
        /// 公私鑰長度（單位為 bit，建議至少 2048）。
        /// </summary>
        public int KeySize { get; set; } = 2048;

        /// <summary>
        /// 加密文字所使用的編碼格式。
        /// </summary>
        public TextEncodingType Encoding { get; set; } = TextEncodingType.UTF8;

        /// <summary>
        /// RSA 金鑰將儲存在此資料夾下（由 Config 控制）。
        /// </summary>
        public string Directory => "RSA";
    }

    /// <summary>
    /// ECC 加密設定。
    /// </summary>
    public class EccConfig
    {
        /// <summary>
        /// 所選用的橢圓曲線類型。
        /// </summary>
        public EccCurveType Curve { get; set; } = EccCurveType.NistP256;

        /// <summary>
        /// 加密文字所使用的編碼格式。
        /// </summary>
        public TextEncodingType Encoding { get; set; } = TextEncodingType.UTF8;

        /// <summary>
        /// ECC 金鑰將儲存在此資料夾下（由 Config 控制）。
        /// </summary>
        public string Directory => "ECC";
    }

    /// <summary>
    /// CryptoSuite 設定模型。
    /// </summary>
    public class CryptoConfigModel
    {
        /// <summary>
        /// 所有金鑰儲存的根資料夾（系統會依演算法自動分類子目錄）。
        /// </summary>
        public string KeyDirectory { get; set; } = "Key";

        public AesConfig AES { get; set; } = new AesConfig();
        public RsaConfig RSA { get; set; } = new RsaConfig();
        public EccConfig ECC { get; set; } = new EccConfig();

        /// <summary>
        /// 是否啟用 URL 安全型 Base64 編碼（適用於 JWT 等）。
        /// </summary>
        public bool UseUrlSafeBase64 { get; set; } = true;
    }
}
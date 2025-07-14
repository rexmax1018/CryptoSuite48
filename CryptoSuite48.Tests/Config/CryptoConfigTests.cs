using CryptoSuite48.Config;
using CryptoSuite48.Config.Enums;
using System.IO;
using Xunit;
using ConfigRoot = CryptoSuite48.Config.CryptoConfig;

namespace CryptoSuite48.Tests.Config
{
    public class CryptoConfigTests
    {
        private void ResetConfigToDefault()
        {
            ConfigRoot.Override(new CryptoConfigModel
            {
                KeyDirectory = "Key",
                AES = new AesConfig { KeySize = 256 },
                RSA = new RsaConfig { KeySize = 2048, Encoding = TextEncodingType.UTF8 },
                ECC = new EccConfig { Curve = EccCurveType.Secp256k1 }
            });
        }

        [Fact(DisplayName = "應正確載入 AES/RSA/ECC 設定值")]
        public void Load_ShouldLoadAllValues()
        {
            ResetConfigToDefault();

            var config = ConfigRoot.Current;

            Assert.Equal("Key", config.KeyDirectory);
            Assert.Equal(256, config.AES.KeySize);
            Assert.Equal(2048, config.RSA.KeySize);
            Assert.Equal(TextEncodingType.UTF8, config.RSA.Encoding);
            Assert.Equal(EccCurveType.Secp256k1, config.ECC.Curve);
        }

        [Fact(DisplayName = "Load_ShouldBindEnumsCorrectly")]
        public void Load_ShouldBindEnumsCorrectly()
        {
            ResetConfigToDefault();

            var rsaEncoding = ConfigRoot.Current.RSA.Encoding;
            var eccCurve = ConfigRoot.Current.ECC.Curve;

            Assert.Equal(TextEncodingType.UTF8, rsaEncoding);
            Assert.Equal(EccCurveType.Secp256k1, eccCurve);
        }

        [Fact(DisplayName = "Override 應允許手動注入設定")]
        public void Override_ShouldSetConfigManually()
        {
            var temp = Path.GetTempPath();

            var model = new CryptoConfigModel
            {
                KeyDirectory = temp,
                AES = new AesConfig { KeySize = 192 },
                RSA = new RsaConfig { KeySize = 1024, Encoding = TextEncodingType.ASCII },
                ECC = new EccConfig { Curve = EccCurveType.NistP256 }
            };

            ConfigRoot.Override(model);

            Assert.Equal(temp, ConfigRoot.Current.KeyDirectory);
            Assert.Equal(192, ConfigRoot.Current.AES.KeySize);
            Assert.Equal(TextEncodingType.ASCII, ConfigRoot.Current.RSA.Encoding);
            Assert.Equal(EccCurveType.NistP256, ConfigRoot.Current.ECC.Curve);
        }
    }
}
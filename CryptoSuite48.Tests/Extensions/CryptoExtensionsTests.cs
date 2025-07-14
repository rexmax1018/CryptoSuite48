using CryptoSuite48.Extensions;
using CryptoSuite48.KeyManagement.Enums;
using CryptoSuite48.Services.Interfaces;
using Moq;
using Xunit;

namespace CryptoSuite48.Tests.Extensions
{
    public class CryptoExtensionsTests
    {
        private readonly Mock<ICryptoService> _mockService = new Mock<ICryptoService>();

        [Fact]
        public void EncryptWith_ShouldDelegateToService()
        {
            var data = "abc".ToBytes();
            var key = new object();
            var expected = new byte[] { 1, 2, 3 };

            _mockService.Setup(s => s.Encrypt(data, CryptoAlgorithmType.AES, key))
                        .Returns(expected);

            var result = data.EncryptWith(CryptoAlgorithmType.AES, key, _mockService.Object);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SignWith_ShouldDelegateToService()
        {
            var data = "hello".ToBytes();
            var key = new object();
            var sig = new byte[] { 0x99 };

            _mockService.Setup(s => s.Sign(data, CryptoAlgorithmType.RSA, key)).Returns(sig);

            var result = data.SignWith(CryptoAlgorithmType.RSA, key, _mockService.Object);
            Assert.Equal(sig, result);
        }

        [Fact]
        public void VerifyWith_ShouldReturnTrue_WhenMatch()
        {
            var data = "xyz".ToBytes();
            var sig = new byte[] { 0xAA };
            var key = new object();

            _mockService.Setup(s => s.Verify(data, sig, CryptoAlgorithmType.ECC, key)).Returns(true);

            var verified = data.VerifyWith(sig, CryptoAlgorithmType.ECC, key, _mockService.Object);
            Assert.True(verified);
        }
    }
}
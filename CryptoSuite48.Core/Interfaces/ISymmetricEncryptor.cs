namespace CryptoSuite48.Core.Interfaces
{
    /// <summary>
    /// 對稱式加密介面
    /// </summary>
    public interface ISymmetricEncryptor : IEncryptor
    {
        void SetKey(byte[] key, byte[] iv);
    }
}
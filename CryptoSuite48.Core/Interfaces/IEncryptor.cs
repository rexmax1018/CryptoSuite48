﻿namespace CryptoSuite48.Core.Interfaces
{
    /// <summary>
    /// 通用加解密介面
    /// </summary>
    public interface IEncryptor
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] encryptedData);
    }
}
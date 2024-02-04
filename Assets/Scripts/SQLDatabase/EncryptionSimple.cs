using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EncryptionSimple : MonoBehaviour
{

    private static readonly string Key = "EncryptionKey";

    public static string Encrypt(string plainText)
    {
        Aes aesAlg = Aes.Create();
        MemoryStream msEncrypt = new MemoryStream();
        CryptoStream csEncrypt = null;
        StreamWriter swEncrypt = null;

        try
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.IV = GenerateRandomIV(aesAlg);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(plainText);
            swEncrypt.Flush();
            csEncrypt.FlushFinalBlock();

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        finally
        {
            csEncrypt?.Dispose();
            swEncrypt?.Dispose();
            msEncrypt?.Dispose();
            aesAlg?.Dispose();
        }
    }

    public static string Decrypt(string cipherText)
    {
        Aes aesAlg = Aes.Create();
        MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        CryptoStream csDecrypt = null;
        StreamReader srDecrypt = null;
        try
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.IV = GenerateRandomIV(aesAlg);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            
            csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
        finally
        {
            csDecrypt?.Dispose();
            srDecrypt?.Dispose();
            msDecrypt?.Dispose();
            aesAlg?.Dispose();
        }
    }

    private static byte[] GenerateRandomIV(SymmetricAlgorithm algorithm)
    {
        byte[] iv = new byte[algorithm.BlockSize / 8];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(iv);
        }

        return iv;
    }
}

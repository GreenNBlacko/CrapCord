using System.Security.Cryptography;

namespace CrapCord_Services;

/// <summary>
///     Class to manage AES en/decryption
/// </summary>
class AESUtils {
    /// <summary>
    ///     Function to encrypt text using AES with the CBC algorithm
    /// </summary>
    /// <param name="plainText">Text to encrypt</param>
    /// <param name="key">AES key</param>
    /// <param name="iv">Initialization vector</param>
    /// <returns><see cref="string"/></returns>
    /// <seealso cref="DecryptAES"/>
    public string EncryptAES(string plainText, byte[] key, byte[] iv) {
        using (Aes aes = Aes.Create()) {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream()) {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cs)) {
                    writer.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    /// <summary>
    ///     Function to decrypt text encrypted using AES with the CBC algorithm
    /// </summary>
    /// <param name="cipherText">Text to decrypt</param>
    /// <param name="key">AES key</param>
    /// <param name="iv">Initialization vector</param>
    /// <returns><see cref="string"/></returns>
    /// <seealso cref="EncryptAES"/>
    public string DecryptAES(string cipherText, byte[] key, byte[] iv) {
        using (Aes aes = Aes.Create()) {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs)) {
                return reader.ReadToEnd();
            }
        }
    }
}
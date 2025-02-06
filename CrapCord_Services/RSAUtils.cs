using System.Security.Cryptography;
using System.Text;
using CrapCord_Entities;

namespace CrapCord_Services;

public class RSAUtils {
    public RSAKeyPair GenerateRSAKeys() {
        using (var rsa = new RSACryptoServiceProvider(2048)) {
            return new(rsa.ToXmlString(false), rsa.ToXmlString(true));
        }
    }

    public string EncryptRSA(string data, string publicKey) {
        using (var rsa = new RSACryptoServiceProvider()) {
            rsa.FromXmlString(publicKey);
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
            return Convert.ToBase64String(encryptedData);
        }
    }

    public string DecryptRSA(string encryptedData, string privateKey) {
        using (var rsa = new RSACryptoServiceProvider()) {
            rsa.FromXmlString(privateKey);
            byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedData), false);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
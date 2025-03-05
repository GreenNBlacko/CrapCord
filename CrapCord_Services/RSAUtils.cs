using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using CrapCord_Entities;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace CrapCord_Services;

public class RSAUtils {
    public RSAKeyPair GenerateRSAKeypair(string pasphrase) {
        var keys = GenerateRSAKeys(pasphrase);
        var keypair = new RSAKeyPair(ExportPublicKey(keys.Public), ExportPrivateKey(keys.Private));
        File.WriteAllText("data.bin",JsonConvert.SerializeObject(keypair, Formatting.Indented));
        return keypair;
    }

    // Derives a 256-bit key from the passphrase
    private byte[] DeriveKey(string passphrase, byte[] salt) {
        using (var pbkdf2 = new Rfc2898DeriveBytes(passphrase, salt, 100000, HashAlgorithmName.SHA256)) {
            return pbkdf2.GetBytes(32); // 256-bit key
        }
    }

    // Generate a deterministic RSA key pair using a passphrase
    private AsymmetricCipherKeyPair GenerateRSAKeys(string passphrase) {
        byte[] salt = Encoding.UTF8.GetBytes("CrapCord");
        byte[] seed = DeriveKey(passphrase, salt);

        var random =
            new SecureRandom(
                new Org.BouncyCastle.Crypto.Prng.DigestRandomGenerator(
                    new Org.BouncyCastle.Crypto.Digests.Sha256Digest()));
        random.SetSeed(seed);

        var keyGen = new RsaKeyPairGenerator();
        keyGen.Init(new KeyGenerationParameters(random, 2048));
        return keyGen.GenerateKeyPair();
    }

    // Convert the public key to PEM format
    private string ExportPublicKey(AsymmetricKeyParameter publicKey) {
        var sw = new System.IO.StringWriter();
        var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
        pemWriter.WriteObject(publicKey);
        pemWriter.Dispose();
        return sw.ToString();
    }

    // Convert the private key to PEM format
    private string ExportPrivateKey(AsymmetricKeyParameter privateKey) {
        var sw = new System.IO.StringWriter();
        var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
        pemWriter.WriteObject(privateKey);
        pemWriter.Dispose();
        return sw.ToString();
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
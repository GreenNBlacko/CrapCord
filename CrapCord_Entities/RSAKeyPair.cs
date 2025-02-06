namespace CrapCord_Entities;

public class RSAKeyPair {
    public readonly string PublicKey;
    public readonly string PrivateKey;

    public RSAKeyPair(string publicKey, string privateKey) {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
}
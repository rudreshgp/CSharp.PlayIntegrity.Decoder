namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public interface IPlayIntegrityTokenDecryptor
    {
        PlayIntegrityData? DecryptAndVerifyToken(string encryptedToken);
    }
}

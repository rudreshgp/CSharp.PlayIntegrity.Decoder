using Microsoft.IdentityModel.Tokens;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public interface ISignatureKeyService
    {
        ECDsaSecurityKey? GetSignatureKey(string base64SignatureKey);
    }
}

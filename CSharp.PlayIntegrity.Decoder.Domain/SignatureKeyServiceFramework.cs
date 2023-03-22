using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    /// <summary>
    /// solution source <see href="https://stackoverflow.com/a/44527439/1343684"/> 
    /// </summary>
    public class SignatureKeyServiceFramework : ISignatureKeyService
    {
        private static byte[] s_secp256r1Prefix =
          Convert.FromBase64String("MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE");

        private static byte[] s_cngBlobPrefix = { 0x45, 0x43, 0x53, 0x31, 0x20, 0, 0, 0 };
        private readonly ILogger<SignatureKeyServiceFramework> _logger;

        public SignatureKeyServiceFramework(ILogger<SignatureKeyServiceFramework> logger)
        {
            _logger = logger;
        }

        public ECDsaSecurityKey? GetSignatureKey(string base64SignatureKey)
        {
            try
            {

                byte[] subjectPublicKeyInfo = Convert.FromBase64String(base64SignatureKey);

                if (subjectPublicKeyInfo.Length != 91)
                    throw new InvalidOperationException();

                if (!subjectPublicKeyInfo.Take(s_secp256r1Prefix.Length).SequenceEqual(s_secp256r1Prefix))
                    throw new InvalidOperationException();

                byte[] cngBlob = new byte[s_cngBlobPrefix.Length + 64];
                Buffer.BlockCopy(s_cngBlobPrefix, 0, cngBlob, 0, s_cngBlobPrefix.Length);

                Buffer.BlockCopy(
                    subjectPublicKeyInfo,
                    s_secp256r1Prefix.Length,
                    cngBlob,
                    s_cngBlobPrefix.Length,
                    64);

                return new ECDsaSecurityKey(new ECDsaCng(CngKey.Import(cngBlob, CngKeyBlobFormat.EccPublicBlob)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error in building the security key");
                return default;
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public class SignatureKeyServiceCore : ISignatureKeyService
    {
        private readonly ILogger<SignatureKeyServiceCore> _logger;

        public SignatureKeyServiceCore(ILogger<SignatureKeyServiceCore> logger)
        {
            _logger = logger;
        }
        public ECDsaSecurityKey? GetSignatureKey(string base64SignatureKey)
        {
            try
            {
                var verificationKeyByteArray = Convert.FromBase64String(base64SignatureKey);
                var ecdsa = ECDsa.Create();
                ecdsa.ImportSubjectPublicKeyInfo(verificationKeyByteArray, out var _);
                return new ECDsaSecurityKey(ecdsa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error in building the sinature key");
                return default;
            }
        }
    }
}

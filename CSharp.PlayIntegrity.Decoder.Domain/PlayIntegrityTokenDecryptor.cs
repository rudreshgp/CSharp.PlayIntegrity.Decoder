using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public class PlayIntegrityTokenDecryptor : IPlayIntegrityTokenDecryptor
    {
        private readonly SymmetricSecurityKey? _decryptionKey;
        private readonly ECDsaSecurityKey? _verificationKey;
        private readonly ISecurityTokenValidator _tokenValidator = default!;
        private readonly TokenValidationParameters _validationParameters = default!;


        public PlayIntegrityTokenDecryptor(IOptions<ApiKeys> apiKeys,
            ISignatureKeyService signatureKeyService,
            ISecurityTokenValidator tokenValidator)
        {
            _decryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(apiKeys.Value.DecryptionKey));
            _verificationKey = signatureKeyService.GetSignatureKey(apiKeys.Value.VerificationKey);
            if (_decryptionKey == default || _verificationKey == default)
            {
                return;
            }
            _validationParameters = CreateTokenValidationParameters();
            _tokenValidator = tokenValidator;
        }
        public PlayIntegrityData? DecryptAndVerifyToken(string signedAttestationStatement)
        {
            if (_decryptionKey == default || _verificationKey == default)
            {
                return default;
            }

            var claimsPrincipal = _tokenValidator.ValidateToken(signedAttestationStatement, _validationParameters, out var token);
            if (claimsPrincipal?.Claims == default)
            {
                return default;
            }

            return new PlayIntegrityDataBuilder(claimsPrincipal.Claims.ToDictionary(x => x.Type, x => x.Value, StringComparer.OrdinalIgnoreCase))
                                        .WithDeviceIntegrity()
                                        .WithAppIntegrity()
                                        .WithAccountDetails()
                                        .WithRequestDetails()
                                        .Build();

        }
        private TokenValidationParameters CreateTokenValidationParameters()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return new TokenValidationParametersBuilder()
                .WithDecryptionKey(_decryptionKey)
                .WithEcDsa256IssuerSigningKey(_verificationKey)
                .WithValidateIssuer(false)
                .WithValidateLifetime(false)
                .WithValidateAudience(false)
                .WithValidateIssuerSigningKey(true)
                .Build();
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}

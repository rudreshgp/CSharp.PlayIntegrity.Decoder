using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public class TokenValidationParametersBuilder
    {
        private readonly TokenValidationParameters _instance;

        public TokenValidationParametersBuilder()
        {
            _instance = new TokenValidationParameters();
        }

        public TokenValidationParametersBuilder WithValidateIssuerSigningKey(bool validateIssuerSigningKey)
        {
            _instance.ValidateIssuerSigningKey = validateIssuerSigningKey;
            return this;
        }

        public TokenValidationParametersBuilder WithValidateAudience(bool validateAudience)
        {
            _instance.ValidateAudience = validateAudience;
            return this;
        }

        public TokenValidationParametersBuilder WithValidateLifetime(bool validateLifetime)
        {
            _instance.ValidateLifetime = validateLifetime;
            return this;
        }

        public TokenValidationParametersBuilder WithValidateIssuer(bool validateIssuer)
        {
            _instance.ValidateIssuer = validateIssuer;
            return this;
        }

        public TokenValidationParametersBuilder WithDecryptionKey(SymmetricSecurityKey decryptionKey)
        {
            if (decryptionKey == default)
            {
                throw new ArgumentNullException(nameof(decryptionKey), "Decryption key cann't be empty");
            }
            _instance.TokenDecryptionKey = decryptionKey;
            return this;
        }

        public TokenValidationParametersBuilder WithEcDsa256IssuerSigningKey(ECDsaSecurityKey signatureKey)
        {
            if (signatureKey == default)
            {
                throw new ArgumentNullException(nameof(signatureKey), "Decryption key cann't be empty");
            }
            _instance.IssuerSigningKey = signatureKey;
            return this;
        }

        public TokenValidationParameters Build()
        {
            return _instance;
        }
    }
}

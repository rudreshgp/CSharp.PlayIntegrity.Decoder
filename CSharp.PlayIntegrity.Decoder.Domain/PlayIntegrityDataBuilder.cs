using Google.Apis.PlayIntegrity.v1.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public class PlayIntegrityDataBuilder
    {
        private readonly IReadOnlyDictionary<string, string> _claims;
        private readonly PlayIntegrityData _instance;
        private bool _isValid = true;

        public PlayIntegrityDataBuilder(IReadOnlyDictionary<string, string> claims)
        {
            if (claims == default || claims.Count == 0)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            _claims = claims;
            _instance = new PlayIntegrityData();
        }


        public PlayIntegrityDataBuilder WithAccountDetails()
        {
            _instance.AccountDetails = GetValue<AccountDetails>();
            return this;
        }

        public PlayIntegrityDataBuilder WithAppIntegrity()
        {
            _instance.AppIntegrity = GetValue<AppIntegrity>();
            return this;
        }

        public PlayIntegrityDataBuilder WithDeviceIntegrity()
        {
            _instance.DeviceIntegrity = GetValue<DeviceIntegrity>();
            return this;
        }

        public PlayIntegrityDataBuilder WithRequestDetails()
        {
            _instance.RequestDetails = GetValue<RequestDetails>();
            return this;
        }

        public PlayIntegrityData Build() => _instance;

        private T GetValue<T>() where T : class
        {
            if (_isValid)
            {
                var valueType = typeof(T);
                string value;
                if (_claims.TryGetValue(valueType.Name, out value))
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                _isValid = false;
            }
            return null;
        }
    }
}

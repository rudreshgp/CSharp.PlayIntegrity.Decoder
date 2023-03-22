using Google.Apis.PlayIntegrity.v1.Data;

namespace CSharp.PlayIntegrity.Decoder.Domain
{
    public class PlayIntegrityData
    {
        public RequestDetails RequestDetails { get; set; } = default!;
        public AppIntegrity AppIntegrity { get; set; } = default!;
        public DeviceIntegrity DeviceIntegrity { get; set; } = default!;
        public AccountDetails AccountDetails { get; set; } = default!;
    }
}

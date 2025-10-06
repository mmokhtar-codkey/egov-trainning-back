using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Identity.Sts
{
    // STS Stands For Secure Token Service ( it is a common name in world of oauth2 and open id connect )
    [ExcludeFromCodeCoverage]
    public class StsConfig
    {
        public string Audience { get; set; }

        public string Authority { get; set; }

        public ApiClient ApiClient { get; set; }

    }

    public class ApiClient
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public List<string> AllowedScopes { get; set; } = new();

        public string GrantType { get; set; }
    }
}

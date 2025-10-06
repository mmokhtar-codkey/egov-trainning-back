using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Identity.Sts
{
    [ExcludeFromCodeCoverage]
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}

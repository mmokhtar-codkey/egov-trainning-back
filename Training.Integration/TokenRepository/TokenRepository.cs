using Training.Common.Configurations.Http;
using Training.Common.DTO.Identity.Sts;
using Training.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Training.Integration.TokenRepository
{
    public class TokenRepository(MicroServicesUrls microServicesUrls, IHttpClientFactory httpClientFactory, IOptions<StsConfig> stsConfig) : ITokenRepository
    {
        public async Task<TokenResponse> GenerateTokenAsync()
        {
            var url = microServicesUrls.GenerateClientCredentialToken;
            var identityServerClient = httpClientFactory.CreateClient(ClientNames.IdentityServerApiClient);
            var parameters = new Dictionary<string, string>()
            {

                {"grant_type", stsConfig.Value.ApiClient.GrantType},
                {"client_id", stsConfig.Value.ApiClient.ClientId},
                {"client_secret", stsConfig.Value.ApiClient.ClientSecret},
                {"scope", string.Join(" " ,stsConfig.Value.ApiClient.AllowedScopes)}
            };
            var content = new FormUrlEncodedContent(parameters);

            var response = await identityServerClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null)
                throw new Exception(MessagesConstants.ErrorGettingTokenResponse);

            return tokenResponse;
        }
    }
}

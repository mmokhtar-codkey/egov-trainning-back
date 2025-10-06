using Training.Integration.TokenRepository;
using System.Net.Http.Headers;

namespace Training.Api.HttpDelegationHandlers;

/// <summary>
/// Http client handler that adds an authorization token to outgoing requests.
/// </summary>
public class AuthorizedHttpClientHandler : DelegatingHandler
{
    private readonly ITokenRepository _tokenRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tokenRepository"></param>
    public AuthorizedHttpClientHandler(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    /// <summary>
    /// Send Request After Appending Header
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken ct)
    {
        var token = await _tokenRepository.GenerateTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return await base.SendAsync(request, ct);
    }
}
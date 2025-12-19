using System.Net.Http.Headers;

namespace ReadmoreMobile.Services;

public sealed class AuthHandler : DelegatingHandler
{
    private readonly TokenStore _store;

    public AuthHandler(TokenStore store) => _store = store;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var (token, exp) = await _store.GetAsync();
        if (!string.IsNullOrWhiteSpace(token) && exp is not null && exp > DateTime.UtcNow)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}

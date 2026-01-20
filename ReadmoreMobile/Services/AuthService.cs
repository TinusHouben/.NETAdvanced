using System.Net.Http.Json;

namespace ReadmoreMobile.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _factory;
    private readonly TokenStore _tokenStore;

    public AuthService(IHttpClientFactory factory, TokenStore tokenStore)
    {
        _factory = factory;
        _tokenStore = tokenStore;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var http = _factory.CreateClient("api");
        var payload = new { Email = email, Password = password };

        var resp = await http.PostAsJsonAsync("api/Auth/login", payload);
        if (!resp.IsSuccessStatusCode)
            return null;

        var dto = await resp.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (dto == null || string.IsNullOrWhiteSpace(dto.Token))
            return null;

        await _tokenStore.SaveAsync(dto.Token);
        return dto.Token;
    }

    public Task<string?> GetTokenAsync()
    {
        return _tokenStore.GetTokenAsync();
    }

    public Task LogoutAsync()
    {
        _tokenStore.Clear();
        return Task.CompletedTask;
    }

    private sealed class LoginResponseDto
    {
        public string? Token { get; set; }
    }
}

using System.Net.Http.Json;

namespace ReadmoreMobile.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private const string TokenKey = "jwt_token";

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var payload = new { Email = email, Password = password };

        var resp = await _http.PostAsJsonAsync("/api/Auth/login", payload);
        if (!resp.IsSuccessStatusCode)
            return null;

        var dto = await resp.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (dto == null || string.IsNullOrWhiteSpace(dto.Token))
            return null;

        await SecureStorage.SetAsync(TokenKey, dto.Token);
        return dto.Token;
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(TokenKey);
        }
        catch
        {
            return null;
        }
    }

    public Task LogoutAsync()
    {
        SecureStorage.Remove(TokenKey);
        return Task.CompletedTask;
    }

    private sealed class LoginResponseDto
    {
        public string? Token { get; set; }
    }
}

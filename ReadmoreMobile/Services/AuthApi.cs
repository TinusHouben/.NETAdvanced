using System.Net.Http.Json;
using ReadmoreMobile.Models;

namespace ReadmoreMobile.Services;

public sealed class AuthApi
{
    private readonly IHttpClientFactory _factory;

    public AuthApi(IHttpClientFactory factory) => _factory = factory;

    public async Task<(LoginResponseDto? Data, string? Error)> LoginAsync(LoginRequestDto req)
    {
        var http = _factory.CreateClient("api");

        HttpResponseMessage resp;
        try
        {
            resp = await http.PostAsJsonAsync("api/auth/login", req);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }

        if (!resp.IsSuccessStatusCode)
            return (null, $"{(int)resp.StatusCode} {resp.ReasonPhrase}");

        try
        {
            var data = await resp.Content.ReadFromJsonAsync<LoginResponseDto>();
            return (data, data is null ? "Empty response" : null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReadmoreMobile.Services;
using ReadmoreWeb.Data.Models;

namespace ReadmoreMobile.Services;

public sealed class BooksApi
{
    private readonly IHttpClientFactory _factory;
    private readonly TokenStore _store;

    public BooksApi(IHttpClientFactory factory, TokenStore store)
    {
        _factory = factory;
        _store = store;
    }

    public async Task<List<Book>> GetAsync()
    {
        var http = _factory.CreateClient("api");

        var (token, _) = await _store.GetAsync();
        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var resp = await http.GetAsync("api/BooksApi");

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            throw new Exception("401 Unauthorized (JWT ontbreekt/ongeldig)");

        var ct = resp.Content.Headers.ContentType?.MediaType ?? "";
        var body = await resp.Content.ReadAsStringAsync();

        if (!ct.Contains("json"))
            throw new Exception($"Geen JSON ontvangen. Content-Type={ct}. Eerste 200 chars: {body.Substring(0, Math.Min(200, body.Length))}");

        return await resp.Content.ReadFromJsonAsync<List<Book>>() ?? [];
    }
}

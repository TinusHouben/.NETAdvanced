using System.Net.Http.Json;
using ReadmoreWeb.Data.Models;

namespace ReadmoreMobile.Services;

public sealed class BooksApi
{
    private readonly IHttpClientFactory _factory;

    public BooksApi(IHttpClientFactory factory) => _factory = factory;

    public async Task<List<Book>> GetAsync()
    {
        var http = _factory.CreateClient("api");
        return await http.GetFromJsonAsync<List<Book>>("api/books") ?? [];
    }
}

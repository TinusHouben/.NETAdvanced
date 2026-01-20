using System.Net;
using System.Net.Http.Json;

namespace ReadmoreMobile.Services;

public class BooksApi
{
    private readonly IHttpClientFactory _factory;

    public BooksApi(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<BookDto>> GetBooksAsync()
    {
        var http = _factory.CreateClient("api");
        var resp = await http.GetAsync("api/BooksApi");

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            return new List<BookDto>();

        resp.EnsureSuccessStatusCode();

        var data = await resp.Content.ReadFromJsonAsync<List<BookDto>>();
        return data ?? new List<BookDto>();
    }
}

public class BookDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public decimal Price { get; set; }
    public DateTime? PublishedDate { get; set; }
}

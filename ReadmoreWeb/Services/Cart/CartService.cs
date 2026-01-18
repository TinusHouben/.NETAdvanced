using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Models.Cart;

namespace ReadmoreWeb.Services.Cart;

public class CartService : ICartService
{
    private const string Key = "cart";
    private readonly IHttpContextAccessor _http;
    private readonly ReadmoreDbContext _db;

    public CartService(IHttpContextAccessor http, ReadmoreDbContext db)
    {
        _http = http;
        _db = db;
    }

    private ISession Session => _http.HttpContext!.Session;

    public Task<CartViewModel> GetAsync()
    {
        var cart = Session.GetJson<CartViewModel>(Key) ?? new CartViewModel();
        return Task.FromResult(cart);
    }

    public async Task AddAsync(int bookId, int quantity = 1)
    {
        if (quantity < 1) quantity = 1;

        var cart = Session.GetJson<CartViewModel>(Key) ?? new CartViewModel();
        var existing = cart.Items.FirstOrDefault(i => i.BookId == bookId);

        if (existing != null)
        {
            existing.Quantity += quantity;
            Session.SetJson(Key, cart);
            return;
        }

        var book = await _db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == bookId);
        if (book == null) return;

        cart.Items.Add(new CartItem
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            Quantity = quantity
        });

        Session.SetJson(Key, cart);
    }

    public Task UpdateAsync(int bookId, int quantity)
    {
        var cart = Session.GetJson<CartViewModel>(Key) ?? new CartViewModel();
        var item = cart.Items.FirstOrDefault(i => i.BookId == bookId);
        if (item == null) return Task.CompletedTask;

        if (quantity <= 0)
            cart.Items.Remove(item);
        else
            item.Quantity = quantity;

        Session.SetJson(Key, cart);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(int bookId)
    {
        var cart = Session.GetJson<CartViewModel>(Key) ?? new CartViewModel();
        cart.Items.RemoveAll(i => i.BookId == bookId);
        Session.SetJson(Key, cart);
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        Session.Remove(Key);
        return Task.CompletedTask;
    }
}

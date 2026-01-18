using ReadmoreWeb.Models.Cart;

namespace ReadmoreWeb.Services.Cart;

public interface ICartService
{
    Task<CartViewModel> GetAsync();
    Task AddAsync(int bookId, int quantity = 1);
    Task UpdateAsync(int bookId, int quantity);
    Task RemoveAsync(int bookId);
    Task ClearAsync();
}

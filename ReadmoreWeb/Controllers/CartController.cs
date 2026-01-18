using Microsoft.AspNetCore.Mvc;
using ReadmoreWeb.Services.Cart;

namespace ReadmoreWeb.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cart;

    public CartController(ICartService cart)
    {
        _cart = cart;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = await _cart.GetAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int bookId, int quantity = 1, string? returnUrl = null)
    {
        await _cart.AddAsync(bookId, quantity);
        if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int bookId, int quantity)
    {
        await _cart.UpdateAsync(bookId, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int bookId)
    {
        await _cart.RemoveAsync(bookId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        await _cart.ClearAsync();
        return RedirectToAction("Index");
    }
}

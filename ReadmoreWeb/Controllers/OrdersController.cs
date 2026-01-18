using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;
using ReadmoreWeb.Models.Cart;
using ReadmoreWeb.Services.Cart;

namespace ReadmoreWeb.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ReadmoreDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICartService _cart;

    public OrdersController(ReadmoreDbContext db, UserManager<ApplicationUser> userManager, ICartService cart)
    {
        _db = db;
        _userManager = userManager;
        _cart = cart;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var vm = await _cart.GetAsync();
        if (vm.Items.Count == 0)
        {
            TempData["Error"] = "Je winkelmand is leeg.";
            return RedirectToAction("Index", "Cart");
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckoutConfirm()
    {
        var cart = await _cart.GetAsync();
        if (cart.Items.Count == 0)
        {
            TempData["Error"] = "Je winkelmand is leeg.";
            return RedirectToAction("Index", "Cart");
        }

        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Forbid();
        }

        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            TotalAmount = cart.Total,
            Status = "Created"
        };

        foreach (var item in cart.Items)
        {
            order.Items.Add(new OrderItem
            {
                BookId = item.BookId,
                Quantity = item.Quantity,
                UnitPrice = item.Price,
                LineTotal = item.Price * item.Quantity
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _cart.ClearAsync();

        TempData["Success"] = "Bestelling geplaatst.";
        return RedirectToAction("My");
    }

    [HttpGet]
    public async Task<IActionResult> My()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Forbid();

        var orders = await _db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.Book)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return View(orders);
    }
}

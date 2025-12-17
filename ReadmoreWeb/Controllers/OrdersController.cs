using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;
using ReadmoreWeb.Models;

namespace ReadmoreWeb.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ReadmoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ReadmoreDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> My()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Ongeldige bestelling.";
                return RedirectToAction("Index", "Books");
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == model.BookId);
                if (book == null)
                {
                    TempData["Error"] = "Boek niet gevonden.";
                    return RedirectToAction("Index", "Books");
                }

                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                order.Items.Add(new OrderItem
                {
                    BookId = book.Id,
                    Quantity = model.Quantity,
                    UnitPrice = book.Price
                });

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Bestelling geplaatst.";
                return RedirectToAction(nameof(My));
            }
            catch
            {
                TempData["Error"] = "Er ging iets mis bij het plaatsen van je bestelling.";
                return RedirectToAction("Index", "Books");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

namespace ReadmoreWeb.Controllers;

[Authorize(Roles = "Admin")]
public class OrdersAdminController : Controller
{
    private readonly ReadmoreDbContext _db;

    public OrdersAdminController(ReadmoreDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? q, string sort = "new")
    {
        var query = _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Book)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();

            query = query.Where(o =>
                (o.User != null && o.User.Email != null && o.User.Email.Contains(q)) ||
                (o.User != null && o.User.FirstName != null && o.User.FirstName.Contains(q)) ||
                (o.User != null && o.User.LastName != null && o.User.LastName.Contains(q)) ||
                o.Status.Contains(q));
        }

        query = sort == "old"
            ? query.OrderBy(o => o.CreatedAt)
            : query.OrderByDescending(o => o.CreatedAt);

        var orders = await query.ToListAsync();

        ViewBag.Q = q;
        ViewBag.Sort = sort;

        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }
}

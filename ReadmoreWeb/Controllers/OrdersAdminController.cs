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

    public async Task<IActionResult> Index(string? q, string sort = "new", string? status = null)
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

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(o => o.Status == status);
        }

        query = sort == "old"
            ? query.OrderBy(o => o.CreatedAt)
            : query.OrderByDescending(o => o.CreatedAt);

        var orders = await query.ToListAsync();

        ViewBag.Q = q;
        ViewBag.Sort = sort;
        ViewBag.Status = status ?? "All";

        return View(orders);
    }

    public async Task<IActionResult> Details(int id, string? returnUrl = null)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        ViewBag.AllowedStatuses = new List<string> { "Pending", "Completed", "Cancelled" };
        ViewBag.ReturnUrl = returnUrl;

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkCompleted(int id, string? returnUrl = null)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        order.Status = "Completed";
        await _db.SaveChangesAsync();

        TempData["Success"] = "Bestelling werd afgerond.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status, string? returnUrl = null)
    {
        var allowed = new HashSet<string> { "Pending", "Completed", "Cancelled" };
        if (!allowed.Contains(status))
        {
            TempData["Error"] = "Ongeldige status.";
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        order.Status = status;
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Status aangepast naar {status}.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }
}

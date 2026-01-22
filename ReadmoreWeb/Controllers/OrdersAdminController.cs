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

    private static string NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status)) return "Pending";
        return status == "Created" ? "Pending" : status;
    }

    private async Task NormalizeCreatedToPendingAsync()
    {
        var createdOrders = await _db.Orders
            .Where(o => o.Status == "Created")
            .ToListAsync();

        if (createdOrders.Count == 0) return;

        foreach (var o in createdOrders)
        {
            o.Status = "Pending";
        }

        await _db.SaveChangesAsync();
    }

    public async Task<IActionResult> Index(string? q, string sort = "new", string? status = null)
    {
        await NormalizeCreatedToPendingAsync();

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
            status = NormalizeStatus(status);
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

    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        var normalized = NormalizeStatus(order.Status);
        if (order.Status != normalized)
        {
            order.Status = normalized;
            await _db.SaveChangesAsync();
        }

        ViewBag.AllowedStatuses = new List<string>
        {
            "Pending",
            "Completed",
            "Cancelled"
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkCompleted(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        order.Status = "Completed";
        await _db.SaveChangesAsync();

        TempData["Message"] = "Bestelling werd succesvol afgerond.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        status = NormalizeStatus(status);

        var allowed = new HashSet<string> { "Pending", "Completed", "Cancelled" };
        if (!allowed.Contains(status))
        {
            TempData["Error"] = "Ongeldige status.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        order.Status = status;
        await _db.SaveChangesAsync();

        TempData["Message"] = $"Status aangepast naar {status}.";
        return RedirectToAction(nameof(Details), new { id });
    }
}

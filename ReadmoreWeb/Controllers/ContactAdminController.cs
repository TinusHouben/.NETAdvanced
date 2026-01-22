using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

namespace ReadmoreWeb.Controllers;

[Authorize(Roles = "Admin")]
public class ContactAdminController : Controller
{
    private readonly ReadmoreDbContext _db;

    public ContactAdminController(ReadmoreDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string status = "All")
    {
        var q = _db.ContactMessages
            .Include(m => m.User)
            .AsQueryable();

        if (status != "All")
            q = q.Where(m => m.Status == status);

        var messages = await q
            .OrderBy(m => m.Status == "Resolved")
            .ThenByDescending(m => m.CreatedAt)
            .ToListAsync();

        ViewBag.Status = status;
        return View(messages);
    }

    public async Task<IActionResult> Details(int id)
    {
        var msg = await _db.ContactMessages
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (msg == null) return NotFound();
        return View(msg);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetStatus(int id, string status)
    {
        var allowed = new HashSet<string> { "New", "InProgress", "Resolved" };
        if (!allowed.Contains(status))
        {
            TempData["Error"] = "Ongeldige status.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var msg = await _db.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
        if (msg == null) return NotFound();

        msg.Status = status;
        msg.ResolvedAt = status == "Resolved" ? DateTime.UtcNow : null;

        await _db.SaveChangesAsync();

        TempData["Success"] = $"Status aangepast naar {status}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var msg = await _db.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
        if (msg == null) return NotFound();

        _db.ContactMessages.Remove(msg);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Contactbericht verwijderd.";
        return RedirectToAction(nameof(Index));
    }
}

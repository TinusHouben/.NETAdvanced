using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;

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
            .AsNoTracking()
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
            .Include(m => m.Replies.OrderBy(r => r.CreatedAt))
            .FirstOrDefaultAsync(m => m.Id == id);

        if (msg == null) return NotFound();

        var unread = msg.Replies.Where(r => r.Sender == "User" && !r.SeenByAdmin).ToList();
        if (unread.Count > 0)
        {
            foreach (var r in unread) r.SeenByAdmin = true;
            await _db.SaveChangesAsync();
        }

        return View(msg);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(int id, string text)
    {
        text = (text ?? "").Trim();
        if (text.Length == 0)
        {
            TempData["Error"] = "Je bericht is leeg.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var msg = await _db.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
        if (msg == null) return NotFound();

        if (msg.Status == "Resolved")
        {
            TempData["Error"] = "Dit ticket is al opgelost.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _db.ContactReplies.Add(new ContactReply
        {
            ContactMessageId = id,
            Text = text,
            Sender = "Admin",
            CreatedAt = DateTime.UtcNow,
            SeenByAdmin = true,
            SeenByUser = false
        });

        if (msg.Status == "New") msg.Status = "InProgress";

        await _db.SaveChangesAsync();

        TempData["Success"] = "Antwoord verstuurd.";
        return RedirectToAction(nameof(Details), new { id });
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
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Controllers;

[Authorize]
public class ContactController : Controller
{
    private readonly ReadmoreDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ContactController(ReadmoreDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactMessage());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Subject,Message,Phone")] ContactMessage model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        model.UserId = user.Id;
        model.Email = user.Email ?? user.UserName ?? "";
        model.Status = "New";
        model.CreatedAt = DateTime.UtcNow;

        ModelState.Remove(nameof(ContactMessage.Email));
        ModelState.Remove(nameof(ContactMessage.UserId));
        ModelState.Remove(nameof(ContactMessage.Status));
        ModelState.Remove(nameof(ContactMessage.CreatedAt));
        ModelState.Remove(nameof(ContactMessage.ResolvedAt));

        if (!ModelState.IsValid)
            return View(model);

        _db.ContactMessages.Add(model);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Je bericht werd verzonden.";
        return RedirectToAction(nameof(My));
    }

    [HttpGet]
    public async Task<IActionResult> My()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Forbid();

        var list = await _db.ContactMessages
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Forbid();

        var msg = await _db.ContactMessages
            .Include(m => m.Replies.OrderBy(r => r.CreatedAt))
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

        if (msg == null) return NotFound();

        var unread = msg.Replies.Where(r => r.Sender == "Admin" && !r.SeenByUser).ToList();
        if (unread.Count > 0)
        {
            foreach (var r in unread) r.SeenByUser = true;
            await _db.SaveChangesAsync();
        }

        return View(msg);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(int id, string text)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Forbid();

        text = (text ?? "").Trim();
        if (text.Length == 0)
        {
            TempData["Error"] = "Je bericht is leeg.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var msg = await _db.ContactMessages.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
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
            Sender = "User",
            SenderUserId = userId,
            CreatedAt = DateTime.UtcNow,
            SeenByUser = true,
            SeenByAdmin = false
        });

        if (msg.Status == "New") msg.Status = "InProgress";

        await _db.SaveChangesAsync();

        TempData["Success"] = "Bericht verzonden.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
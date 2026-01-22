using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Index([Bind("Subject,Message,Email,Phone")] ContactMessage model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        model.UserId = user?.Id;
        model.Status = "New";
        model.CreatedAt = DateTime.UtcNow;

        _db.ContactMessages.Add(model);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Je bericht werd verzonden. We nemen zo snel mogelijk contact op.";
        return RedirectToAction(nameof(Index));
    }
}

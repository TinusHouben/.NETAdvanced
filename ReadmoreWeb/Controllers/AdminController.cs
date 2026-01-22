using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Users()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = _roleManager.Roles.Select(r => r.Name!).ToList();
        var userRoles = await _userManager.GetRolesAsync(user);

        ViewBag.Roles = roles;
        ViewBag.UserRoles = userRoles;

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> SetRole(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _roleManager.RoleExistsAsync(role))
        {
            TempData["Message"] = "Rol bestaat niet.";
            return RedirectToAction(nameof(EditUser), new { id });
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        await _userManager.AddToRoleAsync(user, role);

        TempData["Message"] = $"Rol gezet naar {role}.";
        return RedirectToAction(nameof(EditUser), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleLock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var isLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;

        if (isLocked)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            TempData["Message"] = "Gebruiker gedeblokkeerd.";
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            TempData["Message"] = "Gebruiker geblokkeerd.";
        }

        return RedirectToAction(nameof(EditUser), new { id });
    }
}

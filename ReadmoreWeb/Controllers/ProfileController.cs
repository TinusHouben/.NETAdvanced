using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadmoreWeb.Data.Models;
using ReadmoreWeb.Models.Profile;

namespace ReadmoreWeb.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? returnUrl = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var vm = new ProfileVm
        {
            FirstName = user.FirstName ?? "",
            LastName = user.LastName ?? "",
            PhoneNumber = user.PhoneNumber,
            Street = user.Street ?? "",
            City = user.City ?? "",
            PostalCode = user.PostalCode ?? ""
        };

        ViewBag.ReturnUrl = returnUrl;
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileVm vm, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        user.FirstName = vm.FirstName;
        user.LastName = vm.LastName;
        user.PhoneNumber = vm.PhoneNumber;
        user.Street = vm.Street;
        user.City = vm.City;
        user.PostalCode = vm.PostalCode;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        TempData["Success"] = "Profiel opgeslagen.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
}

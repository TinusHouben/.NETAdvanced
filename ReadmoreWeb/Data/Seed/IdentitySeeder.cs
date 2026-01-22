using Microsoft.AspNetCore.Identity;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User", "Moderator" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@readmore.be";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,

                    Street = "Lindelaan 9",
                    City = "Heverlee",
                    PostalCode = "3001"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                var changed = false;

                if (string.IsNullOrWhiteSpace(adminUser.Street))
                {
                    adminUser.Street = "Lindelaan 844";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(adminUser.City))
                {
                    adminUser.City = "Heverlee";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(adminUser.PostalCode))
                {
                    adminUser.PostalCode = "3001";
                    changed = true;
                }

                if (changed)
                {
                    await userManager.UpdateAsync(adminUser);
                }

                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

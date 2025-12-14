using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

var builder = WebApplication.CreateBuilder(args);

// Voeg DbContext toe met SQLite
builder.Services.AddDbContext<ReadmoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ReadmoreDb"))
);

// Voeg Identity toe
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // tijdelijk voor testen
})
.AddEntityFrameworkStores<ReadmoreDbContext>();

// Configureer cookie opties voor login redirect
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";             // route voor login
    options.LogoutPath = "/Account/Logout";           // route voor logout
    options.AccessDeniedPath = "/Account/AccessDenied"; // optioneel
});

// Voeg MVC controllers en views toe
builder.Services.AddControllersWithViews();

// Nodig voor Identity Razor Pages (Login/Register)
builder.Services.AddRazorPages();

var app = builder.Build();

// Database automatisch migreren of aanmaken
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ReadmoreDbContext>();
    db.Database.Migrate();
}

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity middleware
app.UseAuthentication();
app.UseAuthorization();

// Default MVC route: standaard naar loginpagina
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Nodig voor Identity Razor Pages
app.MapRazorPages();

app.Run();

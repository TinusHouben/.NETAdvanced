using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Voeg DbContext toe met SQLite
builder.Services.AddDbContext<ReadmoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ReadmoreDb"))
);

var app = builder.Build();

// Database automatisch migreren of create
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ReadmoreDbContext>();
    db.Database.Migrate(); // Voert migrations uit of maakt database aan
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

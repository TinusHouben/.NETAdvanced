using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Data
{
    // IdentityDbContext MET ApplicationUser
    public class ReadmoreDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ReadmoreDbContext(DbContextOptions<ReadmoreDbContext> options)
            : base(options)
        {
        }

        // Applicatie-tabellen
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optioneel: expliciete tabelnamen
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Customer>().ToTable("Customers");
        }
    }
}

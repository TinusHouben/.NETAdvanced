using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Data
{
    // Erft van IdentityDbContext om Identity-tabellen toe te voegen
    public class ReadmoreDbContext : IdentityDbContext
    {
        public ReadmoreDbContext(DbContextOptions<ReadmoreDbContext> options)
            : base(options)
        {
        }

        // Tabellen voor je app
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Optioneel: configureer tabelnamen en relaties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // belangrijk voor Identity

            // Expliciete tabelnamen
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Customer>().ToTable("Customers");

            // Voorbeeld: configureer een relatie (optioneel)
            // modelBuilder.Entity<Customer>()
            //     .HasMany(c => c.Orders)
            //     .WithOne(o => o.Customer)
            //     .HasForeignKey(o => o.CustomerId);
        }
    }
}

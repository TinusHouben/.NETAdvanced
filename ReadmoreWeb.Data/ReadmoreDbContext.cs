using Microsoft.EntityFrameworkCore;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Data
{
    public class ReadmoreDbContext : DbContext
    {
        public ReadmoreDbContext(DbContextOptions<ReadmoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }    // Tabel voor boeken
        public DbSet<Customer> Customers { get; set; }  // Tabel voor klanten

        // Optional: Configure table names or relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bijvoorbeeld: stel tabelnaam expliciet in
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Customer>().ToTable("Customers");
        }
    }
}

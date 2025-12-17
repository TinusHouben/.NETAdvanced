using ReadmoreWeb.Data;
using ReadmoreWeb.Data.Models;

namespace ReadmoreWeb.Data.Seed
{
    public static class BookSeeder
    {
        public static async Task SeedAsync(ReadmoreDbContext context)
        {
            if (context.Books.Any())
                return;

            var books = new List<Book>
            {
                new Book
                {
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Price = 39.99m,
                    PublishedDate = new DateTime(2008, 8, 1)
                },
                new Book
                {
                    Title = "The Pragmatic Programmer",
                    Author = "Andrew Hunt",
                    Price = 42.50m,
                    PublishedDate = new DateTime(1999, 10, 30)
                },
                new Book
                {
                    Title = "Design Patterns",
                    Author = "Erich Gamma",
                    Price = 45.00m,
                    PublishedDate = new DateTime(1994, 10, 21)
                }
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }
    }
}

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
                    PublishedDate = new DateTime(2008, 8, 1),
                    Description =
                        "Clean Code beschrijft hoe je leesbare, onderhoudbare en herbruikbare code schrijft. " +
                        "Het boek bevat principes, patronen en praktijkvoorbeelden die helpen om professionele software te bouwen."
                },
                new Book
                {
                    Title = "The Pragmatic Programmer",
                    Author = "Andrew Hunt & David Thomas",
                    Price = 42.50m,
                    PublishedDate = new DateTime(1999, 10, 30),
                    Description =
                        "Een absolute klassieker voor softwareontwikkelaars. Dit boek focust op pragmatisch denken, " +
                        "professionele houding en het maken van robuuste, duurzame software."
                },
                new Book
                {
                    Title = "Design Patterns",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                    Price = 45.00m,
                    PublishedDate = new DateTime(1994, 10, 21),
                    Description =
                        "Dit boek introduceert 23 herbruikbare ontwerppatronen voor objectgeoriënteerde software. " +
                        "Het helpt ontwikkelaars om flexibele en onderhoudbare systemen te bouwen."
                },
                new Book
                {
                    Title = "Refactoring",
                    Author = "Martin Fowler",
                    Price = 44.99m,
                    PublishedDate = new DateTime(1999, 7, 8),
                    Description =
                        "Refactoring legt uit hoe je bestaande code kan verbeteren zonder het gedrag te veranderen. " +
                        "Het boek toont technieken om technische schuld te verminderen en codekwaliteit te verhogen."
                },
                new Book
                {
                    Title = "Domain-Driven Design",
                    Author = "Eric Evans",
                    Price = 49.99m,
                    PublishedDate = new DateTime(2003, 8, 30),
                    Description =
                        "Domain-Driven Design helpt ontwikkelaars complexe software te structureren rond het domein. " +
                        "Het boek introduceert strategische en tactische patronen voor schaalbare applicaties."
                },
                new Book
                {
                    Title = "Head First Design Patterns",
                    Author = "Eric Freeman & Elisabeth Robson",
                    Price = 41.50m,
                    PublishedDate = new DateTime(2004, 10, 25),
                    Description =
                        "Een toegankelijke en visuele introductie tot design patterns. " +
                        "Ideaal voor ontwikkelaars die patronen willen begrijpen en toepassen in de praktijk."
                },
                new Book
                {
                    Title = "C# in Depth",
                    Author = "Jon Skeet",
                    Price = 47.00m,
                    PublishedDate = new DateTime(2019, 3, 23),
                    Description =
                        "C# in Depth gaat diep in op de C# taal en het .NET platform. " +
                        "Het boek verklaart geavanceerde concepten en evoluties van de taal doorheen de jaren."
                }
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }
    }
}

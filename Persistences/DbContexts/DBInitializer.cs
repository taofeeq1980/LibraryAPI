using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Persistence.DbContexts
{
    public static class DBInitializer
    {
        public static async Task SeedBookData(this IHost host)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var context = serviceProvider.GetRequiredService<LibraryDbContext>();
            // Create a list of Book objects
            List<Book> books =
            [
                new Book
                {
                    ISBN = "978-0131103627",
                    Title = "The C Programming Language",
                    Author = "Brian W. Kernighan, Dennis M. Ritchie",
                    IsAvailable = true,
                    IsReserved = false,
                    ReturnedDate = DateTime.Now.AddDays(-1),
                },
                new Book
                {
                    ISBN = "978-0201633610",
                    Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                    IsAvailable = true,
                    IsReserved = true,
                    ReturnedDate = DateTime.Now.AddDays(-4),
                },
                new Book
                {
                    ISBN = "978-0132350884",
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Author = "Robert C. Martin",
                    IsAvailable = false,
                    IsReserved = true,
                    ReturnedDate = DateTime.Now.AddDays(-2),
                },
                new Book
                {
                    ISBN = "978-0201633760",
                    Title = "Design Patterns: Using Clean Architecture",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                    IsAvailable = true,
                    IsReserved = true,
                    ReturnedDate = DateTime.Now.AddDays(-6),
                },
                new Book
                {
                    ISBN = "978-0132350874",
                    Title = "C#: A Handbook of .NET 8",
                    Author = "Robert C. Martin",
                    IsAvailable = false,
                    IsReserved = true,
                    ReturnedDate = DateTime.Now.AddDays(-1),
                }
            ];

            if (!context.Books.Any())
            {
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }
    }
}
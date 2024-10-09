using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Domain.Interfaces.Infrastructure
{
    public interface ILibraryDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<Loan> Loans { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}

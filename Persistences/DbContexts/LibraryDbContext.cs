using Domain.Entities;
using Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Persistence.Extensions;

namespace Persistence.DbContexts
{
    public class LibraryDbContext : DbContext, ILibraryDbContext
    {
        private readonly ILogger<LibraryDbContext> _logger;
        public LibraryDbContext(
            DbContextOptions<LibraryDbContext> options,
            ILogger<LibraryDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public override DatabaseFacade Database => base.Database;
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                bool hasChanges = ChangeTracker.HasChanges();

                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Saving Record Exception {e.Message}-{e.StackTrace}");
                return 0;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

            //makes sure that all items returned are not deleted items
            modelBuilder.ApplyGlobalFilters<bool>("IsDeleted", false);
        }
    }
}
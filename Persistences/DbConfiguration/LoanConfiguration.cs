using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Persistence.DbConfiguration.BaseConfiguration;

namespace Persistence.DbConfiguration
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ConfigureBaseEntity();

            _ = builder.HasIndex(x => x.BookId).IsUnique();
            _ = builder.HasIndex(x => x.CustomerId).IsUnique();

            builder.HasOne(x => x.Customer) // Each Book has one Customer
                  .WithMany(x => x.Loans)    // Each Customer can have many loans of books
                  .HasForeignKey(x => x.CustomerId)  // Foreign key in the Book entity
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
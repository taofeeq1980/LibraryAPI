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

            builder.HasOne(x => x.Customer)
                  .WithMany(x => x.Loans)
                  .HasForeignKey(x => x.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
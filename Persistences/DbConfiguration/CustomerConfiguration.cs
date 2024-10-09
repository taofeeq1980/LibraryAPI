using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Persistence.DbConfiguration.BaseConfiguration;

namespace Persistence.DbConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ConfigureBaseEntity();

            _ = builder.Property(x => x.PhoneNumber).HasColumnType("varchar(50)");
            _ = builder.Property(x => x.Username).HasColumnType("varchar(50)");
            _ = builder.Property(x => x.Email).HasColumnType("varchar(50)");
            _ = builder.Property(x => x.PasswordHash).HasColumnType("varchar(max)");
            _ = builder.Property(x => x.Name).HasColumnType("varchar(100)");
            _ = builder.HasIndex(x => x.Username).IsUnique();
            _ = builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
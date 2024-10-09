using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Persistence.DbConfiguration.BaseConfiguration;

namespace Persistence.DbConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ConfigureBaseEntity();

            _ = builder.Property(x => x.ISBN).HasColumnType("varchar(50)");
            _ = builder.Property(x => x.Title).HasColumnType("varchar(100)");
            _ = builder.Property(x => x.Author).HasColumnType("varchar(500)");
            _ = builder.HasIndex(x => x.ISBN).IsUnique();
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DbConfiguration.BaseConfiguration
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : Entity
        {
            _ = builder.HasKey(x => x.Id);

            _ = builder.Property(a => a.DateAdded)
                .HasColumnType("datetime2")
                   .IsRequired();

            _ = builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Persistence.DbConfiguration.BaseConfiguration;
using Domain.Enum;

namespace Persistence.DbConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ConfigureBaseEntity();

            _ = builder.Property(x => x.NotifyBy).HasColumnType("varchar(50)");
            _ = builder.HasIndex(x => x.CustomerId).IsUnique();

            builder.Property(x => x.NotifyBy)
                 .HasConversion(
                     v => Enum.GetName(v),
                     v => Enum.Parse<NotificationChannel>(v, true))
                 .HasColumnType("varchar(50)").IsRequired();
        }
    }
}
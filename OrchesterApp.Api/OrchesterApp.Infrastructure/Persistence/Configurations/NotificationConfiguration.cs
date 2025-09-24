using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            ConfigureNotificationTable(builder);
        }

        private void ConfigureNotificationTable(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => NotificationId.Create(value)
                );

            builder.Property(n => n.Type)
                .HasConversion<string>();

            builder.Property(n => n.Category)
                .HasConversion<string>();

            builder.Property(n => n.Urgency)
                .HasConversion<string>();

            builder.Property(n => n.TerminId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? TerminId.Create(value.Value) : null
                );

            builder.Property(n => n.Data);

            builder.Property(n => n.CreatedAt)
                .IsRequired();
        }
    }
}
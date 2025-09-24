using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace OrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            ConfigureUserNotificationTable(builder);
        }

        private void ConfigureUserNotificationTable(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotifications");

            builder.HasKey(un => un.Id);

            builder.Property(un => un.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => UserNotificationId.Create(value)
                );

            builder.Property(un => un.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value)
                );

            builder.Property(un => un.NotificationId)
                .HasConversion(
                    id => id.Value,
                    value => NotificationId.Create(value)
                );

            builder.Property(un => un.SendStatus)
                .HasConversion<string>();

            builder.Property(un => un.NotificationSink)
                .HasConversion<string>();

            builder.Property(un => un.SendAt);

            builder.HasOne<Notification>()
                .WithMany()
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
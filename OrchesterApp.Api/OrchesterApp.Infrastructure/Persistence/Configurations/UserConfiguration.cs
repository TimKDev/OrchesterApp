using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchesterApp.Domain.OrchesterMitgliedAggregate;
using OrchesterApp.Domain.UserAggregate;

namespace OrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureUserTable(builder);
        }

        private void ConfigureUserTable(EntityTypeBuilder<User> builder)
        {
            builder.HasOne<OrchesterMitglied>().WithOne().HasForeignKey<User>(u => u.OrchesterMitgliedsId).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.AbstimmungsIds);
        }
    }
}

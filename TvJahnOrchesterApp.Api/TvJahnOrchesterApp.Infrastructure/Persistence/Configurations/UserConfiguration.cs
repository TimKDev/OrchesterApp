using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations
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

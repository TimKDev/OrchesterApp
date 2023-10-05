using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class MitgliedsStatusConfiguration : IEntityTypeConfiguration<MitgliedsStatus>
    {
        public void Configure(EntityTypeBuilder<MitgliedsStatus> builder)
        {
            builder.ToTable("MitgliedsStatus");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                MitgliedsStatus.Create((int)MitgliedsStatusEnum.aktiv, "aktiv"),
                MitgliedsStatus.Create((int)MitgliedsStatusEnum.passiv, "passiv"),
                MitgliedsStatus.Create((int)MitgliedsStatusEnum.ausgetreten, "ausgetreten"),
            } );
        }
    }
}

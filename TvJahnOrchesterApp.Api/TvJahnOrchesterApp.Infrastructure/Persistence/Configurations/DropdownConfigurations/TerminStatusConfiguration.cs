using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class TerminStatusConfiguration : IEntityTypeConfiguration<TerminStatus>
    {
        public void Configure(EntityTypeBuilder<TerminStatus> builder)
        {
            builder.ToTable("TerminStatus");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                TerminStatus.Create((int)TerminStatusEnum.Angefragt, "Angefragt"),
                TerminStatus.Create((int)TerminStatusEnum.Zugesagt, "Zugesagt"),
                TerminStatus.Create((int)TerminStatusEnum.Abgesagt, "Abgesagt"),
            });
        }
    }
}

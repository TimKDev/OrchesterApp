using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.Common.Enums;

namespace OrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class TerminArtConfiguration : IEntityTypeConfiguration<TerminArt>
    {
        public void Configure(EntityTypeBuilder<TerminArt> builder)
        {
            builder.ToTable("TerminArten");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                TerminArt.Create((int)TerminArtEnum.Auftritt, "Auftritt"),
                TerminArt.Create((int)TerminArtEnum.Konzert, "Konzert"),
                TerminArt.Create((int)TerminArtEnum.Probe, "Probe"),
                TerminArt.Create((int)TerminArtEnum.Freizeit, "Freizeit"),
                TerminArt.Create((int)TerminArtEnum.Reise, "Reise"),
            });
        }
    }
}

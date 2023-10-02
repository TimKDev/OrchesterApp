using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class ArtInstrumentConfiguration : IEntityTypeConfiguration<ArtInstrument>
    {
        public void Configure(EntityTypeBuilder<ArtInstrument> builder)
        {
            builder.ToTable("ArtInstrument");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ArtInstrumentId.Create(value)
                );
            // Seed Data
            builder.HasData(new[]
            {
                ArtInstrument.Create((int)ArtInstrumentEnum.Holz, "Holz"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Blech, "Blech"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Schlagwerk, "Schlagwerk"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Dirigent, "Dirigent"),
            } );
        }
    }
}

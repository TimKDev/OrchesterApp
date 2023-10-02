using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.ToTable("Instrumente");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => InstrumentId.Create(value)
                );

            builder.HasOne<ArtInstrument>().WithMany().HasForeignKey(m => m.ArtInstrumentId);

            builder.HasData(new[]
            {
                Instrument.Create(1, "Saxophon", ArtInstrumentId.Create(1)),
                Instrument.Create(2, "Trompete", ArtInstrumentId.Create(2)),
                Instrument.Create(3, "Trommel", ArtInstrumentId.Create(3)),
                Instrument.Create(4, "Dirigent", ArtInstrumentId.Create(4))
            });
        }
    }
}
